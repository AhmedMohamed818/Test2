using Microsoft.AspNetCore.Http;
using RookieRisePortalPanal.Data.Context;
using System.Security.Claims;

namespace RookieRisePortalPanal.Services.CurrentUserService
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RookieRiseDbContext _context;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            RookieRiseDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public Guid? UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return userId != null ? Guid.Parse(userId) : null;
            }
        }

        public string? UserName =>
            _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}