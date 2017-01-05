using Clients;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleCoreHybridFlowWithPkce
{
    public class Program
    {
        const string authority = Constants.Authority;
        const string clientId = "console.hybrid.pkce";
        const int callbackPort= 7890;
        const string callbackPath = "/";
        const string callbackBase = "http://127.0.0.1:";
        const string scope = "openid profile api1";
        const string apiUrl = Constants.SampleApi;

        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            Console.Title = "Console Hybrid Flow with PKCE";

            Log.Logger = new LoggerConfiguration()
                           .MinimumLevel.Debug()
                           .WriteTo.LiterateConsole()
                           .CreateLogger();

            Console.WriteLine("+-----------------------+");
            Console.WriteLine("|  Sign in with OIDC    |");
            Console.WriteLine("+-----------------------+");
            Console.WriteLine("");
            Console.WriteLine("Press any key to sign in...");
            Console.ReadKey();

            var token = await RequestTokenAsync();

            Console.WriteLine();
            Console.WriteLine("Press any key to call the api...");
            Console.ReadLine();

            await CallServiceAsync(token);
        }

        static async Task<string> RequestTokenAsync()
        {
            var webView = new LoopbackWebView(callbackPort, callbackPath);

            var loggerFactory = new LoggerFactory();
            // uncomment this line for logging inside OidcClient
            //loggerFactory.AddSerilog(Log.Logger);

            var opts = new OidcClientOptions(authority,
                scope,
                callbackBase + callbackPort + callbackPath,
                clientId,
                webView: webView,
                loggerFactory: loggerFactory)
            {
                UseFormPost = true
            };

            var client = new OidcClient(opts);
            var result = await client.LoginAsync();
            if (!result.Success)
            {
                Console.WriteLine($"Error: {result.Error}.");
                return null;
            }

            Console.WriteLine($"Access token: {result.AccessToken}");
            return result.AccessToken;
        }

        static async Task CallServiceAsync(string token)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(apiUrl)
            };

            client.SetBearerToken(token);
            var response = await client.GetStringAsync("identity");

            "\n\nService claims:".ConsoleGreen();
            Console.WriteLine(JArray.Parse(response));
        }
    }
}
