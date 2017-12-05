using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace MvcHybrid
{
    public class CookieEventHandler : CookieAuthenticationEvents
    {
        public CookieEventHandler(LogoutSessionManager logoutSessions)
        {
            LogoutSessions = logoutSessions;
        }

        public LogoutSessionManager LogoutSessions { get; }

        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            if (context.Principal.Identity.IsAuthenticated)
            {
                var sub = context.Principal.FindFirst("sub")?.Value;
                var sid = context.Principal.FindFirst("sid")?.Value;

                if (LogoutSessions.IsLoggedOut(sub, sid))
                {
                    context.RejectPrincipal();
                    return Task.CompletedTask;
                }
            }

            return base.ValidatePrincipal(context);
        }
    }
}