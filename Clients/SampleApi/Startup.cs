using Microsoft.AspNet.Authentication.JwtBearer;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
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

            //var scopePolicy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .RequireClaim("scope", "calendar.read", "calendar.readwrite")
            //    .Build();

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new AuthorizeFilter(scopePolicy));
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("read",
            //        policy => policy.RequireClaim("scope", "calendar.read"));
            //    options.AddPolicy("readwrite",
            //        policy => policy.RequireClaim("scope", "calendar.readwrite"));
            //});
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
            //    options.AutomaticChallenge = true;
            //    options.Authority = Clients.Constants.BaseAddress;
            //    options.TokenValidationParameters.ValidateAudience = false;
            //    options.RequireHttpsMetadata = false;
            //});

            app.UseOAuth2IntrospectionAuthentication(options =>
            {
                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;

                options.ScopeName = "api1";
                options.ScopeSecret = "secret";
                options.Authority = Clients.Constants.BaseAddress;

                // use introspection endpoint for both JWTs and reference tokens (see above)
                options.SkipTokensWithDots = false;
            });

            //app.AllowScopes("calendar.read", "calendar.readwrite");

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}