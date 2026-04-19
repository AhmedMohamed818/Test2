using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Services.CurrentUserService
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? UserName { get; }
    }
}
