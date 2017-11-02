namespace Memery.Console
{
    public class MemeryConsoleOptions
    {
        public string Server {get;set;} = "http://localhost:5000";
        public bool DisableSSLVerification {get;set;}
    }
}