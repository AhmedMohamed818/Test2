using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RookieRisePortalPanal.Services.CompanyService.DTO
{
    

    
        public class CreateCompanyDto
        {
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


