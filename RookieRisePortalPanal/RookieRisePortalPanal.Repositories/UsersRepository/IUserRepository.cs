using RookieRisePortalPanal.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RookieRisePortalPanal.Repositories.UsersRepository
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(Guid id);
        Task<AppUser?> GetByEmailAsync(string email);
        Task<AppUser?> GetByEmailWithCompanyAsync(string email);

        
    }
}
