using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace SampleApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Sample API";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://localhost:3721")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
