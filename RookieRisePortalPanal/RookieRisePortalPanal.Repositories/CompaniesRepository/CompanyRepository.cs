using Microsoft.EntityFrameworkCore;
using RookieRisePortalPanal.Data.Context;
using RookieRisePortalPanal.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieRisePortalPanal.Repositories.CompaniesRepository
{
    public class CompanyRepository(RookieRiseDbContext context) : ICompanyRepository
    {
       
        public async Task<List<Company>> GetAllAsync()
        {
            return await context.Companies
                .AsNoTracking()
                .Select(c => new Company
                {
                    CompanyId = c.CompanyId,
                    NameEn = c.NameEn,
                    NameAr = c.NameAr,
                    WebsiteUrl = c.WebsiteUrl,
                    LogoPath = c.LogoPath,

                    User = new AppUser
                    {
                        Id = c.User.Id,
                        Email = c.User.Email
                    }
                })
                .ToListAsync();
        }

        public async Task<Company?> GetByIdAsync(Guid id)
        {
            return await context.Companies
                .AsNoTracking()
                .Where(c => c.CompanyId == id)
                .Select(c => new Company
                {
                    CompanyId = c.CompanyId,
                    NameEn = c.NameEn,
                    NameAr = c.NameAr,
                    WebsiteUrl = c.WebsiteUrl,
                    LogoPath = c.LogoPath,

                    User = new AppUser
                    {
                        Id = c.User.Id,
                        Email = c.User.Email
                    }
                })
                .FirstOrDefaultAsync();
        }
        public async Task<Company?> GetByUserEmailAsync(string email)
        {
            return await context.Users
    .Where(u => u.Email == email)
    .Select(u => u.Company)
    .Select(c => new Company
    {
        CompanyId = c.CompanyId,
        NameEn = c.NameEn,
        NameAr = c.NameAr,
        WebsiteUrl = c.WebsiteUrl,
        LogoPath = c.LogoPath
    })
    .FirstOrDefaultAsync();
        }

        public async Task RestoreAsync(Guid id)
        {
            var company = await context.Companies
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.CompanyId == id);

            if (company == null) return;

            company.IsDeleted = false;
            company.DeletedAt = null;
            company.DeletedBy = null;

            context.Companies.Update(company);
        }

        public async Task AddAsync(Company company)
        {
            await context.Companies.AddAsync(company);
        }

        public async Task<int> UpdateAsync(Company entity, Guid userId)
        {
            var affected = await context.Companies
                .Where(c => c.CompanyId == entity.CompanyId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.NameEn, entity.NameEn)
                    .SetProperty(c => c.NameAr, entity.NameAr)
                    .SetProperty(c => c.WebsiteUrl, entity.WebsiteUrl)
                    .SetProperty(c => c.LogoPath, entity.LogoPath)
                    .SetProperty(c => c.UpdatedBy, userId)
                    .SetProperty(c => c.UpdatedAt, DateTime.UtcNow)
                );

            return affected;
        }

        public void Delete(Company company)
        {
            context.Companies.Remove(company);
        }

      
    }
}
