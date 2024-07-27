using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Services;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private readonly IGenericService<ShipperRequestDTO> _shipperService;

        public ShipperController(IGenericService<ShipperRequestDTO> shipperService)
        {
            _shipperService = shipperService;
        }

        // GET: api/shipper
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShipperRequestDTO>>> GetShippers()
        {
            var shippers = await _shipperService.GetAllAsync();
            return Ok(shippers);
        }

        // GET: api/shipper/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShipperRequestDTO>> GetShipper(int id)
        {
            try
            {
                var shipper = await _shipperService.GetByIdAsync(id);
                return Ok(shipper);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/shipper
        [HttpPost]
        public async Task<ActionResult<ShipperRequestDTO>> PostShipper(ShipperRequestDTO shipperDto)
        {
            var createdShipper = await _shipperService.CreateAsync(shipperDto);
            return CreatedAtAction(nameof(GetShipper), new { id = createdShipper.ShipperId }, createdShipper);
        }

        // PUT: api/shipper/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipper(int id, ShipperRequestDTO shipperDto)
        {
            if (id != shipperDto.ShipperId)
            {
                return BadRequest("Shipper ID mismatch");
            }

            try
            {
                await _shipperService.UpdateAsync(id, shipperDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/shipper/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipper(int id)
        {
            try
            {
                await _shipperService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
