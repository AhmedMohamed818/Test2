using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookieRisePortalPanal.Services.CompanyService;
using RookieRisePortalPanal.Services.CompanyService.DTO;

namespace RookieRisePortalPanal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // 📋 GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _companyService.GetAllAsync();

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        // 🔍 GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _companyService.GetByIdAsync(id);

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        // ➕ CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto dto)
        {
            var result = await _companyService.CreateAsync(dto);

            return Ok(new
            {
                success = true,
                message = "Company created successfully",
                data = result
            });
        }

        // ✏️ UPDATE
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyDto dto)
        {
            await _companyService.UpdateAsync(dto);

            return Ok(new
            {
                success = true,
                message = "Company updated successfully"
            });
        }

        // ❌ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _companyService.DeleteAsync(id);

            return Ok(new
            {
                success = true,
                message = "Company deleted successfully"
            });
        }

        // ♻️ RESTORE
        [HttpPost("{id}/restore")]
        public async Task<IActionResult> Restore(Guid id)
        {
            await _companyService.RestoreAsync(id);

            return Ok(new
            {
                success = true,
                message = "Company restored successfully"
            });
        }
    }
}