using AutoMapper;
using RookieRisePortalPanal.Data.Entities;
using RookieRisePortalPanal.Services.CompanyService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Services.MappingProfile
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {  //  Company → DTO
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.UserEmail,
                    opt => opt.MapFrom(src => src.User != null ? src.User.Email : null));

            //  Create DTO → Company
            CreateMap<CreateCompanyDto, Company>()
                .ForMember(dest => dest.LogoPath, opt => opt.Ignore())    
                .ForMember(dest => dest.User, opt => opt.Ignore());

            //  Update DTO → Company
            CreateMap<UpdateCompanyDto, Company>()
                .ForMember(dest => dest.LogoPath, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
