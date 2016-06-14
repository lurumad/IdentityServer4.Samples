using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;

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
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Func<string, LogLevel, bool> filter = (scope, level) => 
                scope.StartsWith("Microsoft.AspNetCore.Authentication") || 
                scope.StartsWith("IdentityServer") ||
                scope.StartsWith("IdentityModel");

            loggerFactory.AddConsole(filter);
            loggerFactory.AddDebug(filter);

            app.UseCors(policy =>
            {
                policy.WithOrigins(
                    "http://localhost:28895", 
                    "http://localhost:7017");

                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:1941",
                RequireHttpsMetadata = false,

                ScopeName = "api1",
                ScopeSecret = "secret",
                AutomaticAuthenticate = true
            });

            app.UseMvc();
        }
    }
}