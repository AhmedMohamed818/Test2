using RookieRisePortalPanal.Data.Entities;
using RookieRisePortalPanal.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Repositories.TokenRepository
{
    public interface IUserTokenRepository
    {
         
      
        Task AddAsync(UserToken token);
        Task<UserToken?> GetValidTokenAsync(string token, Guid userId, TokenType type);
        void MarkAsUsed(UserToken token);
        
    }
}
