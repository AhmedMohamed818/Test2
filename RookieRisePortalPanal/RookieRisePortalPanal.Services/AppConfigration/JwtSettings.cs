using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Services.AppConfigration
{
    namespace RookieRisePortalPanal.Services.AppConfigration
    {
        public class JwtSettings
        {
            public string SecretKey { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public double DurationInDays { get; set; }
        }
    }
    
}
