using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Request;
using Northwind.Business.Response;
using Northwind.Business.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IGenericService<CustomerRequestDTO> _customerService;

        public CustomerController(IGenericService<CustomerRequestDTO> customerService)
        {
            _customerService = customerService;
        }

        // GET: api/customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerRequestDTO>>> GetCustomers()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        // GET: api/customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerRequestDTO>> GetCustomer(int id)
        {
            try
            {
                var customer = await _customerService.GetByIdAsync(id);
                return Ok(customer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/customer
        [HttpPost]
        public async Task<ActionResult<CustomerRequestDTO>> PostCustomer(CustomerRequestDTO customerDto)
        {
            var createdCustomer = await _customerService.CreateAsync(customerDto);
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CustomerId }, createdCustomer);
        }

        // PUT: api/customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerRequestDTO customerDto)
        {
            if (id != customerDto.CustomerId)
            {
                return BadRequest("Customer ID mismatch");
            }

            try
            {
                await _customerService.UpdateAsync(id, customerDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
