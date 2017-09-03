using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SampleApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Sample API";

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}