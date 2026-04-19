using RookieRisePortalPanal.Services.AccountService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Services.AccountService
{
    public interface IAccountService
    {
        Task<UserResultDto> LoginAsync(LoginDto loginDto);
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> SetPasswordAsync(SetPasswordDto setPasswordDto);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<UserResultDto> GetCurrentUserAsync(string email);
    }
}
