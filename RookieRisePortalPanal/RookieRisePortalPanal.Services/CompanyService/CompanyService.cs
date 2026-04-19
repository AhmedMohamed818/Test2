using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RookieRisePortalPanal.Data.Context;
using RookieRisePortalPanal.Data.Entities;
using RookieRisePortalPanal.Repositories.CompaniesRepository;
using RookieRisePortalPanal.Repositories.Exceptions;
using RookieRisePortalPanal.Services.CompanyService.DTO;
using RookieRisePortalPanal.Services.CurrentUserService;

namespace RookieRisePortalPanal.Services.CompanyService
{
    public class CompanyService(RookieRiseDbContext dbContext,
            ICompanyRepository companyRepository,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CompanyService> logger) : ICompanyService
    {
       

        // ✅ helper method
        private void SetAuditInfo()
        {
            dbContext.CurrentUserId = currentUserService.UserId;

            dbContext.CurrentIpAddress =
                httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            dbContext.CurrentUserAgent =
                httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();
        }

        public async Task<List<CompanyDto>> GetAllAsync()
        {
            try
            {
                logger.LogInformation("Getting all companies");

                var companies = await companyRepository.GetAllAsync();

                var result = companies.Select(c => new CompanyDto
                {
                    CompanyId = c.CompanyId,
                    NameEn = c.NameEn,
                    NameAr = c.NameAr,
                    WebsiteUrl = c.WebsiteUrl,
                    LogoPath = c.LogoPath,
                }).ToList();

                logger.LogInformation("Returned {Count} companies", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting all companies");
                throw;
            }
        }

        public async Task<CompanyDto> GetByIdAsync(Guid id)
        {
            try
            {
                logger.LogInformation("Getting company by id {Id}", id);

                var c = await companyRepository.GetByIdAsync(id);

                if (c == null)
                {
                    logger.LogWarning("Company not found {Id}", id);
                    throw new UserNotFoundException("Company not found"); // ✅ بدل UserNotFound
                }

                return new CompanyDto
                {
                    CompanyId = c.CompanyId,
                    NameEn = c.NameEn,
                    NameAr = c.NameAr,
                    WebsiteUrl = c.WebsiteUrl,
                    LogoPath = c.LogoPath,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting company {Id}", id);
                throw;
            }
        }

        public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto)
        {
            try
            {
                logger.LogInformation("Creating company {Name}", dto.NameEn);
                    
                SetAuditInfo(); // 🔥

                var company = new Company
                {
                    NameEn = dto.NameEn,
                    NameAr = dto.NameAr,
                    WebsiteUrl = dto.WebsiteUrl
                };

                await companyRepository.AddAsync(company);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("Company created with id {Id}", company.CompanyId);

                return new CompanyDto
                {
                    CompanyId = company.CompanyId,
                    NameEn = company.NameEn,
                    NameAr = company.NameAr,
                    WebsiteUrl = company.WebsiteUrl,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating company {Name}", dto.NameEn);
                throw;
            }
        }

        public async Task UpdateAsync(UpdateCompanyDto dto)
        {
            try
            {
                logger.LogInformation("Updating company {Id}", dto.CompanyId);

                SetAuditInfo(); // 🔥

                var company = await companyRepository.GetByIdAsync(dto.CompanyId);

                if (company == null)
                {
                    logger.LogWarning("Company not found {Id}", dto.CompanyId);
                    throw new UserNotFoundException("Company not found");
                }

                company.NameEn = dto.NameEn;
                company.NameAr = dto.NameAr;
                company.WebsiteUrl = dto.WebsiteUrl;


                var affected = await companyRepository.UpdateAsync(
                    company,
                    currentUserService.UserId ?? throw new Exception("User not authenticated")
                );
                if (affected == 0)
                {
                    logger.LogWarning("Company not found {Id}", dto.CompanyId);
                    throw new UserNotFoundException("Company not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating company {Id}", dto.CompanyId);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                logger.LogInformation("Deleting company {Id}", id);

                SetAuditInfo(); // 🔥

                var company = await companyRepository.GetByIdAsync(id);

                if (company == null)
                {
                    logger.LogWarning("Company not found {Id}", id);
                    throw new UserNotFoundException("Company not found");
                }

                companyRepository.Delete(company);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("Company deleted {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting company {Id}", id);
                throw;
            }
        }

        public async Task RestoreAsync(Guid id)
        {
            try
            {
                logger.LogInformation("Restoring company {Id}", id);

                SetAuditInfo(); // 🔥

                await companyRepository.RestoreAsync(id);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("Company restored {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error restoring company {Id}", id);
                throw;
            }
        }

       
    }
}