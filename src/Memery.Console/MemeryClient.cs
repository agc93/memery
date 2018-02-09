using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Memery.Console
{
    public class MemeryClient : HttpClient
    {
        public MemeryClient(IOptions<MemeryConsoleOptions> options, HttpMessageHandler handler) : base(handler)
        {
            this.BaseAddress = new Uri(options.Value.Server);
            if (options.Value.DisableSSLVerification)
            {
                
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;
            }
        }

        public async Task<ImageResponse> UploadImage(FileInfo filePath)
        {

            using (var content =
                // new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture))
                new MultipartFormDataContent()
            )
            {
                content.Add(new StreamContent(filePath.OpenRead()), "file", filePath.Name);

                using (
                   var message =
                       await PostAsync("/images", content))
                {
                    var json = await message.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ImageResponse>(json);
                }
            }
        }

        internal static HttpMessageHandler GetHandler(System.IServiceProvider provider) {
            var opts = provider.GetService<IOptions<MemeryConsoleOptions>>();
            if (opts.Value.DisableSSLVerification) {
                System.Console.WriteLine("WARNING: SSL Verification Disabled! This is not recommended and can lead to a heap of bad shit...");
                return new HttpClientHandler { 
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            }
            return new HttpClientHandler();

        }

        public async Task<ImageResponse> UploadImage(FileInfo filePath, string name)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StreamContent(filePath.OpenRead()), "file", filePath.Name);
                var resp = await PutAsync($"/images/{name}", content);
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ImageResponse>(json);
            }
        }

        public async Task<ImageResponse> AddImage(Uri uri, string name = null)
        {
            var path = new Url("")
                .AppendPathSegment("/images")
                .SetQueryParam("url", uri.AbsoluteUri);
            HttpResponseMessage response;
            if (string.IsNullOrWhiteSpace(name))
            {
                response = await PostAsync(path.ToString(), null);
            }
            else
            {
                path.AppendPathSegment(name);
                response = await PutAsync(path.ToString(), null);
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ImageResponse>(json);
        }

        public async Task<IEnumerable<ImageResponse>> GetImages()
        {
            var json = await GetStringAsync("/images");
            var dict = JsonConvert.DeserializeObject<Dictionary<string, ImageResponse>>(json);
            return dict.Values.ToList();
        }
    }
}