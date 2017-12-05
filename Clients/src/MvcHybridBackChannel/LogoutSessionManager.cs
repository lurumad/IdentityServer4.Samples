using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcHybrid
{
    public class LogoutSessionManager
    {
        // yes - that needs to be thread-safe, distributed etc (it's a sample)
        List<Session> _sessions = new List<Session>();

        public void Add(string sub, string sid)
        {
            _sessions.Add(new Session { Sub = sub, Sid = sid }); 
        }

        public bool IsLoggedOut(string sub, string sid)
        {
            var session = _sessions.Where(s => s.Sid == sid && s.Sub == sub);
            return session.Any();
        }

        private class Session
        {
            public string Sub { get; set; }
            public string Sid { get; set; }
        }
    }
}
