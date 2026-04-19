using RookieRisePortalPanal.Data.Entities;
using RookieRisePortalPanal.Services.CompanyService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Services.CompanyService
{
    public interface ICompanyService
    {
        Task<List<CompanyDto>> GetAllAsync();
        Task<CompanyDto> GetByIdAsync(Guid id);

        Task<CompanyDto> CreateAsync(CreateCompanyDto dto);
        Task UpdateAsync(UpdateCompanyDto dto);
        Task DeleteAsync(Guid id);

        Task RestoreAsync(Guid id);
    }
}