using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Request;
using Northwind.Business.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminCategoryController : ControllerBase
    {
        private readonly IGenericService<AdminCategoryRequestDTO> _adminCategoryService;

        public AdminCategoryController(IGenericService<AdminCategoryRequestDTO> adminCategoryService)
        {
            _adminCategoryService = adminCategoryService;
        }

        // GET: api/admincategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminCategoryRequestDTO>>> GetAdminCategories()
        {
            var adminCategories = await _adminCategoryService.GetAllAsync();
            return Ok(adminCategories);
        }

        // GET: api/admincategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminCategoryRequestDTO>> GetAdminCategory(int id)
        {
            try
            {
                var adminCategory = await _adminCategoryService.GetByIdAsync(id);
                return Ok(adminCategory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/admincategory
        [HttpPost]
        public async Task<ActionResult<AdminCategoryRequestDTO>> PostAdminCategory(AdminCategoryRequestDTO adminCategoryDto)
        {
            var createdAdminCategory = await _adminCategoryService.CreateAsync(adminCategoryDto);
            return CreatedAtAction(nameof(GetAdminCategory), new { id = createdAdminCategory.CategoryId }, createdAdminCategory);
        }

        // PUT: api/admincategory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminCategory(int id, AdminCategoryRequestDTO adminCategoryDto)
        {
            if (id != adminCategoryDto.CategoryId)
            {
                return BadRequest("Category ID mismatch");
            }

            try
            {
                await _adminCategoryService.UpdateAsync(id, adminCategoryDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/admincategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminCategory(int id)
        {
            try
            {
                await _adminCategoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
