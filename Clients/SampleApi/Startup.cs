using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SampleApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            // can use one or the other right now due to bug in Jwt MW RC1
            //app.UseJwtBearerAuthentication(options =>
            //{
            //    options.AutomaticAuthenticate = true;
            //    options.Authority = Clients.Constants.BaseAddress;
            //    options.TokenValidationParameters.ValidateAudience = false;
            //    options.RequireHttpsMetadata = false;
            //});

            app.UseOAuth2IntrospectionAuthentication(options =>
            {
                options.AutomaticAuthenticate = true;
                options.ScopeName = "api1";
                options.ScopeSecret = "secret";
                options.Authority = Clients.Constants.BaseAddress;

                // use introspection endpoint for both JWTs and reference tokens (see above)
                options.SkipTokensWithDots = false;
            });

            //app.AllowScopes("api2");

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}