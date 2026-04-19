using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RookieRisePortalPanal.Services.CompanyService.DTO
{
    public class UpdateCompanyDto
    {
        [Required]
        public Guid CompanyId { get; set; } // 🔥 كان Guid وخليته int عشان يوافق الـ Entity

        [Required]
        [MaxLength(200)]
        public string NameEn { get; set; }

        [Required]
        [MaxLength(200)]
        public string NameAr { get; set; }

        [Url]
        public string WebsiteUrl { get; set; }

        public string? LogoName { get; set; }
        public IFormFile? LogoPath { get; set; }
    }
}