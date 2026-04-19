using RookieRisePortalPanal.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Services.JwtService
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(AppUser user);

    }
}
