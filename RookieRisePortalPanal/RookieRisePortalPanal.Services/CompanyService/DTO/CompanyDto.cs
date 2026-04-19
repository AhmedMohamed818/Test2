using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RookieRisePortalPanal.Services.CompanyService.DTO
{
    public class CompanyDto
    {
        public Guid CompanyId { get; set; } // 🔥 كان Guid

        public string NameEn { get; set; }
        public string NameAr { get; set; }

        public string? WebsiteUrl { get; set; }
        public string? LogoPath { get; set; }

        // 🔥 من العلاقة مع User
        public string? UserEmail { get; set; }

        // لو One-to-One
        public bool HasUser => !string.IsNullOrEmpty(UserEmail);
    }
}