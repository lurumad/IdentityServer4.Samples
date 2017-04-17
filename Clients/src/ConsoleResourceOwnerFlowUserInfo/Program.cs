using Clients;
using IdentityModel.Client;
using System;
using System.Threading.Tasks;

namespace ConsoleResourceOwnerFlowUserInfo
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            Console.Title = "Console ResourceOwner Flow UserInfo";

            var response = await RequestTokenAsync();
            response.Show();

            await GetClaimsAsync(response.AccessToken);
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var disco = await DiscoveryClient.GetAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new TokenClient(
                disco.TokenEndpoint,
                "roclient",
                "secret");

            return await client.RequestResourceOwnerPasswordAsync("bob", "bob", "openid custom.profile");
        }

        static async Task GetClaimsAsync(string token)
        {
            var disco = await DiscoveryClient.GetAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new UserInfoClient(disco.UserInfoEndpoint);

            var response = await client.GetAsync(token);
            if (response.IsError) throw new Exception(response.Error);

            "\n\nUser claims:".ConsoleGreen();
            foreach (var claim in response.Claims)
            {
                Console.WriteLine("{0}\n {1}", claim.Type, claim.Value);
            }
        }
    }
}