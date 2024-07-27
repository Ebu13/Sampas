using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Services;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IGenericService<SupplierRequestDTO> _supplierService;

        public SupplierController(IGenericService<SupplierRequestDTO> supplierService)
        {
            _supplierService = supplierService;
        }

        // GET: api/supplier
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierRequestDTO>>> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllAsync();
            return Ok(suppliers);
        }

        // GET: api/supplier/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierRequestDTO>> GetSupplier(int id)
        {
            try
            {
                var supplier = await _supplierService.GetByIdAsync(id);
                return Ok(supplier);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/supplier
        [HttpPost]
        public async Task<ActionResult<SupplierRequestDTO>> PostSupplier(SupplierRequestDTO supplierDto)
        {
            var createdSupplier = await _supplierService.CreateAsync(supplierDto);
            return CreatedAtAction(nameof(GetSupplier), new { id = createdSupplier.SupplierId }, createdSupplier);
        }

        // PUT: api/supplier/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, SupplierRequestDTO supplierDto)
        {
            if (id != supplierDto.SupplierId)
            {
                return BadRequest("Supplier ID mismatch");
            }

            try
            {
                await _supplierService.UpdateAsync(id, supplierDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/supplier/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                await _supplierService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
