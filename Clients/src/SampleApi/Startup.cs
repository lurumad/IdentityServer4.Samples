using Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace SampleApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddJsonFormatters()
                .AddAuthorization();

            services.AddWebEncoders();
            services.AddCors();
            services.AddDistributedMemoryCache();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            Func<string, LogLevel, bool> filter = (scope, level) => 
                scope.StartsWith("Microsoft.AspNetCore.Authentication") || 
                scope.StartsWith("Microsoft.AspNetCore.Authorization") ||
                scope.StartsWith("IdentityServer") ||
                scope.StartsWith("IdentityModel") ||
                level == LogLevel.Error ||
                level == LogLevel.Critical;

            loggerFactory.AddConsole(filter);
            loggerFactory.AddDebug(filter);

            app.UseCors(policy =>
            {
                policy.WithOrigins(
                    "http://localhost:28895", 
                    "http://localhost:7017");

                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithExposedHeaders("WWW-Authenticate");
            });

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = Constants.Authority,
                RequireHttpsMetadata = false,

                EnableCaching = false,

                ApiName = "api1",
                ApiSecret = "secret"
            });

            app.UseMvc();
        }
    }
}