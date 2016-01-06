using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

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

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseIdentityServerAuthentication(options =>
            {
                options.Authority = Clients.Constants.BaseAddress;
                options.ScopeName = "api1";
                options.ScopeSecret = "secret";

                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;

                options.SaveTokenAsClaim = true;
            });

            //app.AllowScopes("calendar.read", "calendar.readwrite");

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}