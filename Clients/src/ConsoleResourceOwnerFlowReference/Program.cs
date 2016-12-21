using Clients;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleResourceOwnerFlowReference
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            Console.Title = "Console ResourceOwner Flow Reference";

            var response = await RequestTokenAsync();
            response.Show();

            Console.ReadLine();
            await CallServiceAsync(response.AccessToken);
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new TokenClient(
                Constants.TokenEndpoint,
                "roclient.reference",
                "secret");

            return await client.RequestResourceOwnerPasswordAsync("bob", "bob", "api1");
        }

        static async Task CallServiceAsync(string token)
        {
            var baseAddress = Constants.AspNetWebApiSampleApi;

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            client.SetBearerToken(token);

            while (true)
            {
                var response = await client.GetStringAsync("identity");

                "\n\nService claims:".ConsoleGreen();
                Console.WriteLine(JArray.Parse(response));

                Console.ReadLine();
            }
        }
    }
}