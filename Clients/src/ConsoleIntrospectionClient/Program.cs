using Clients;
using IdentityModel.Client;
using System;
using System.Linq;

namespace ConsoleIntrospectionClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            var response = RequestToken();
            Introspection(response.AccessToken);
        }

        static TokenResponse RequestToken()
        {
            var client = new TokenClient(
                Constants.TokenEndpoint,
                "roclient.reference",
                "secret");

            return client.RequestResourceOwnerPasswordAsync("bob", "bob", "api1 api2").Result;
        }

        private static void Introspection(string accessToken)
        {
            var client = new IntrospectionClient(
                Constants.IntrospectionEndpoint,
                "api1",
                "secret");

            var request = new IntrospectionRequest
            {
                Token = accessToken
            };

            var result = client.SendAsync(request).Result;

            if (result.IsError)
            {
                Console.WriteLine(result.Error);
            }
            else
            {
                if (result.IsActive)
                {
                    result.Claims.ToList().ForEach(c => Console.WriteLine("{0}: {1}",
                        c.Item1, c.Item2));
                }
                else
                {
                    Console.WriteLine("token is not active");
                }
            }
        }
    }
}