using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Services.AccountService.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = " CompanyName Is Required")]

        public string NameEn { get; set; }
        [Required(ErrorMessage = " اسم الشركه مطلوب")]

        public string NameAr { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = " Eamil Is Not Valid")]
        public string Email { get; set; }
        
        public string WebsiteUrl { get; set; }
        public string? LogoName { get; set; }
        public IFormFile? LogoPath { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
