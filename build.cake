#tool "nuget:?package=GitVersion.CommandLine"
#load "build/helpers.cake"
#tool nuget:?package=docfx.console
#addin nuget:?package=Cake.DocFx
#addin nuget:?package=Cake.Docker
// #addin nuget:?package=Cake.AzCopy&prerelease

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var framework = Argument("framework", "netcoreapp2.1");
var fallbackVersion = Argument<string>("force-version", EnvironmentVariable("FALLBACK_VERSION") ?? "0.1.0");

///////////////////////////////////////////////////////////////////////////////
// VERSIONING
///////////////////////////////////////////////////////////////////////////////

var packageVersion = string.Empty;
#load "build/version.cake"

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutionPath = File("./src/Memery.sln");
var solution = ParseSolution(solutionPath);
var projects = GetProjects(solutionPath, configuration);
var artifacts = "./dist/";
var testResultsPath = MakeAbsolute(Directory(artifacts + "./test-results"));
// var runtimes = new List<string> { "win-x64", "osx.10.12-x64", "linux-x64" };

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Running tasks...");
	packageVersion = BuildVersion(fallbackVersion);
	if (FileExists("./build/.dotnet/dotnet.exe")) {
		Information("Using local install of `dotnet` SDK!");
		Context.Tools.RegisterFile("./build/.dotnet/dotnet.exe");
	}
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
	// Clean solution directories.
	foreach(var path in projects.AllProjectPaths)
	{
		Information("Cleaning {0}", path);
		CleanDirectories(path + "/**/bin/" + configuration);
		CleanDirectories(path + "/**/obj/" + configuration);
	}
	Information("Cleaning common files...");
	CleanDirectory(artifacts);
});

Task("Restore")
	.Does(() =>
{
	// Restore all NuGet packages.
	Information("Restoring solution...");
	foreach (var project in projects.AllProjectPaths) {
		DotNetCoreRestore(project.FullPath);
	}
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() =>
{
	Information("Building solution...");
	var settings = new DotNetCoreBuildSettings {
		Configuration = configuration,
		NoIncremental = true,
	};
	DotNetCoreBuild(solutionPath, settings);
});

Task("Generate-Docs")
.IsDependentOn("Post-Build") //since we're now using assemblies because docfx broke Linux compat
.WithCriteria(DirectoryExists("./docfx"))
	.Does(() => 
{
	DocFxMetadata("./docfx/docfx.json");
	DocFxBuild("./docfx/docfx.json");
	Zip("./docfx/_site/", artifacts + "/docfx.zip");
})
.OnError(ex => 
{
	Warning("Error generating documentation!");
});

Task("Post-Build")
	.IsDependentOn("Build")
	.Does(() =>
{
	CreateDirectory(artifacts + "build");
	CreateDirectory(artifacts + "build/Memery");
	var frameworkDir = $"{artifacts}build/Memery/";
	CreateDirectory(frameworkDir);
	var files = GetFiles($"./src/Memery/bin/{configuration}/{framework}/*.*");
	CopyFiles(files, frameworkDir);
	CopyFiles(GetFiles("./build/Dockerfile*"), artifacts);
	CopyFileToDirectory("./template/openshift.yaml", artifacts);
});

Task("Publish-Runtime")
	.IsDependentOn("Post-Build")
	.Does(() =>
{
	CreateDirectory(artifacts + "publish/");
	var projectDir = $"{artifacts}publish";
	CreateDirectory(projectDir);
	var runtime = "linux-x64";
	var runtimeDir = $"{projectDir}/{runtime}";
	var consoleDir = $"{projectDir}/console";
	CreateDirectory(runtimeDir);
	CreateDirectory(consoleDir);
	Information("Publishing for {0} runtime", runtime);
	var rSettings = new DotNetCoreRestoreSettings {
		ArgumentCustomization = args => args.Append("-r " + runtime)
	};
	DotNetCoreRestore(solutionPath, rSettings);
	var settings = new DotNetCorePublishSettings {
		ArgumentCustomization = args => args.Append("-r " + runtime),
		Configuration = configuration
	};
	DotNetCorePublish(solutionPath, settings);
	var publishDir = $"./src/Memery/bin/{configuration}/{framework}/{runtime}/publish/";
	var conPublishDir = $"./src/Memery.Console/bin/{configuration}/{framework}/{runtime}/publish/";
	CopyDirectory(publishDir, runtimeDir);
	CopyDirectory(conPublishDir, consoleDir);
	CopyFiles(GetFiles("./scripts/*.sh"), consoleDir);
	CreateDirectory($"{artifacts}archive");
	Zip(runtimeDir, $"{artifacts}archive/memery-linux-x64.zip");
	// CopyFiles(GetFiles("./scripts/*.ps1"), runtimeDir);
});

Task("Build-Docker-Image")
	.WithCriteria(IsRunningOnUnix())
	.IsDependentOn("Publish-Runtime")
	.Does(() =>
{
	Information("Building Docker image...");
	var bSettings = new DockerImageBuildSettings { Tag = new[] { $"agc93/memery:{packageVersion}", "agc93/memery:latest"}};
	DockerBuild(bSettings, artifacts);
});

Task("Build-Downlink-Image")
	.WithCriteria(IsRunningOnUnix())
	.IsDependentOn("Publish-Runtime")
	.IsDependentOn("Build-Console-Package")
	.Does(() => 
{
	Information("Building Downlink image...");
	var bSettings = new DockerImageBuildSettings {
		File = $"{artifacts}Dockerfile.cli",
		Tag = new[] { $"agc93/memery-dl:{packageVersion}", "agc93/memery-dl:latest" }
	};
	DockerBuild(bSettings, artifacts);
});

Task("Build-Console-Package")
	.IsDependentOn("Publish-Runtime")
	.WithCriteria(IsRunningOnUnix())
	.Does(() => 
{
	Information("Building packages in fpm containers");
	CreateDirectory($"{artifacts}/packages/");
	Information("Packaging {0}", "Memery.Console");
	var runtime = "linux-x64";
	var publishDir = $"{artifacts}publish/console";
	var sourceDir = MakeAbsolute(Directory(publishDir));
	var packageDir = MakeAbsolute(Directory($"{artifacts}packages"));
	var runSettings = new DockerContainerRunSettings {
		Name = "docker-fpm-linux-x64",
		Volume = new[] { $"{sourceDir}:/src:ro", $"{packageDir}:/out:rw"},
		Workdir = "/out",
		Rm = true,
		User = "1000"
	};
	var opts = "-s dir -a x86_64 -f -n memery-cli -m \"Alistair Chapman <alistair@agchapman.com>\" --after-install /src/post-install.sh --before-remove /src/pre-remove.sh";
	Information("Starting {0} for {1} ({2})", runSettings.Name, runtime, "linux-x64");
	var fullCmd = $"{opts} -v {packageVersion} -t rpm -d libunwind -d libicu /src/=/usr/lib/memery-cli/";
	DockerRun(runSettings, "tenzer/fpm", fullCmd);
});

Task("Default")
    .IsDependentOn("Post-Build");

Task("Docker")
	.IsDependentOn("Build-Docker-Image")
	.IsDependentOn("Build-Downlink-Image");

Task("Publish")
	.IsDependentOn("Docker")
	.IsDependentOn("Build-Console-Package");

RunTarget(target);