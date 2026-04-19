using Microsoft.EntityFrameworkCore;
using RookieRisePortalPanal.Data.Context;
using RookieRisePortalPanal.Data.Entities.Enums;

namespace RookieRisePortalPanal.Repositories.TokenRepository
{
    public class UserTokenRepository(RookieRiseDbContext context) : IUserTokenRepository
    {
      

        public async Task AddAsync(UserToken token)
        {
            await context.AppUserTokens.AddAsync(token);
        }

        public async Task<UserToken?> GetValidTokenAsync(string token, Guid userId, TokenType type)
        {
            return await context.AppUserTokens
                .Where(t =>
                    t.Token == token &&
                    t.UserId == userId &&
                    t.Type == type &&
                    !t.IsUsed &&
                    t.ExpirationTime > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        public void MarkAsUsed(UserToken token)
        {
            token.IsUsed = true;
        }
    }
}