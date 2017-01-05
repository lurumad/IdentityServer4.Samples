using Clients;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleCustomGrant
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            Console.Title = "Console Custom Grant";

            var response = await RequestTokenAsync();
            response.Show();

            Console.ReadLine();
            await CallServiceAsync(response.AccessToken);
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var disco = await DiscoveryClient.GetAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new TokenClient(
                disco.TokenEndpoint,
                "client.custom",
                "secret");

            var customParameters = new Dictionary<string, string>
                {
                    { "custom_credential", "custom credential"}
                };

            return await client.RequestCustomGrantAsync("custom", "api1", customParameters);
        }

        static async Task CallServiceAsync(string token)
        {
            var baseAddress = Constants.SampleApi;

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            client.SetBearerToken(token);
            var response = await client.GetStringAsync("identity");

            "\n\nService claims:".ConsoleGreen();
            Console.WriteLine(JArray.Parse(response));
        }
    }
}