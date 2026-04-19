using AutoMapper;
using Microsoft.Extensions.Configuration;
using RookieRisePortalPanal.Data.Entities;
using RookieRisePortalPanal.Services.AccountService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Services.MappingProfile
{
    public class PictureUrlResolver(IConfiguration configuration) : IValueResolver<AppUser, RegisterDto, string>
    {
        public string Resolve(AppUser source, RegisterDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Company.LogoPath))
            {
                return $"{configuration["BaseUrl"]}/{source.Company.LogoPath}";
            }
            return string.Empty;
        }


    }
}
