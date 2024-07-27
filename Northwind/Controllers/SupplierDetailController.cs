using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Request;
using Northwind.Business.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierDetailController : ControllerBase
    {
        private readonly IGenericService<SupplierDetailRequestDTO> _supplierDetailService;

        public SupplierDetailController(IGenericService<SupplierDetailRequestDTO> supplierDetailService)
        {
            _supplierDetailService = supplierDetailService;
        }

        // GET: api/SupplierDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDetailRequestDTO>>> GetAllSupplierDetails()
        {
            var supplierDetails = await _supplierDetailService.GetAllAsync();
            return Ok(supplierDetails);
        }

        // GET: api/SupplierDetail/{id} 
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDetailRequestDTO>> GetSupplierDetail(int id)
        {
            var supplierDetail = await _supplierDetailService.GetByIdAsync(id);
            if (supplierDetail == null)
            {
                return NotFound();
            }
            return Ok(supplierDetail);
        }

        // POST: api/SupplierDetail
        [HttpPost]
        public async Task<ActionResult<SupplierDetailRequestDTO>> CreateSupplierDetail(SupplierDetailRequestDTO supplierDetailDto)
        {
            var createdSupplierDetail = await _supplierDetailService.CreateAsync(supplierDetailDto);
            return CreatedAtAction(nameof(GetSupplierDetail), new { id = createdSupplierDetail.SupplierID }, createdSupplierDetail);
        }

        // PUT: api/SupplierDetail/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplierDetail(int id, SupplierDetailRequestDTO supplierDetailDto)
        {
            if (id != supplierDetailDto.SupplierID)
            {
                return BadRequest("Supplier ID mismatch");
            }

            await _supplierDetailService.UpdateAsync(id, supplierDetailDto);
            return NoContent();
        }

        // DELETE: api/SupplierDetail/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplierDetail(int id)
        {
            await _supplierDetailService.DeleteAsync(id);
            return NoContent();
        }
    }
}
