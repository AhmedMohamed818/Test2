using RookieRisePortalPanal.Services.EmailService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(SendEmailDto dto);

    }
}
