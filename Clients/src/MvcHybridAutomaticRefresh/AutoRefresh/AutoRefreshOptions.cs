using System;
using System.Net.Http;

namespace MvcHybrid
{
    public class AutoRefreshOptions
    {
        public string Scheme { get; set; }
        public TimeSpan RefreshBeforeExpiration { get; set; } = TimeSpan.FromMinutes(1);
    }
}