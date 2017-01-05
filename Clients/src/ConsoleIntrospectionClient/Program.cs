using Clients;
using IdentityModel.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleIntrospectionClient
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            Console.Title = "Console Introspection Client";

            var response = await RequestTokenAsync();
            await IntrospectAsync(response.AccessToken);
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var disco = await DiscoveryClient.GetAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new TokenClient(
                disco.TokenEndpoint,
                "roclient.reference",
                "secret");

            return await client.RequestResourceOwnerPasswordAsync("bob", "bob", "api1 api2.read_only");
        }

        private static async Task IntrospectAsync(string accessToken)
        {
            var disco = await DiscoveryClient.GetAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new IntrospectionClient(
                disco.IntrospectionEndpoint,
                "api1",
                "secret");

            var request = new IntrospectionRequest
            {
                Token = accessToken
            };

            var result = await client.SendAsync(request);

            if (result.IsError)
            {
                Console.WriteLine(result.Error);
            }
            else
            {
                if (result.IsActive)
                {
                    result.Claims.ToList().ForEach(c => Console.WriteLine("{0}: {1}",
                        c.Type, c.Value));
                }
                else
                {
                    Console.WriteLine("token is not active");
                }
            }
        }
    }
}