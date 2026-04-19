using Microsoft.EntityFrameworkCore;
using RookieRisePortalPanal.Data.Context;
using RookieRisePortalPanal.Data.Entities;

namespace RookieRisePortalPanal.Repositories.UsersRepository
{
    public class UserRepository (RookieRiseDbContext context) : IUserRepository
    {
        

        public async Task<List<AppUser>> GetAllAsync()
        {
            return await context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AppUser?> GetByIdAsync(Guid id)
        {
            return await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            var normalizedEmail = email.ToUpperInvariant();

            return await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        }

        public async Task<AppUser?> GetByEmailWithCompanyAsync(string email)
        {
            var normalizedEmail = email.ToUpperInvariant();

            return await context.Users
                .AsNoTracking()
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        }

      
    }
}