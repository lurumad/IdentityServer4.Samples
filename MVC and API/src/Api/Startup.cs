using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Api
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IHostingEnvironment hostEnv)
        {   
            _config = new ConfigurationBuilder()
                .SetBasePath(hostEnv.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables("MVCAndAPISample_")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddJsonFormatters()
                .AddAuthorization();

            services.Configure<IdentityServerSettings>(_config.GetSection("IdentityServer"));
        }

        public void Configure(IApplicationBuilder app, IOptions<IdentityServerSettings> settings)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = settings.Value.Authority,
                RequireHttpsMetadata = false,

                ScopeName = "api1",
                AutomaticAuthenticate = true
            });

            app.UseMvc();
        }
    }
}