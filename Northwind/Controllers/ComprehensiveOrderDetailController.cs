using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Request;
using Northwind.Business.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprehensiveOrderDetailController : ControllerBase
    {
        private readonly ComprehensiveOrderDetailService _orderDetailService;

        public ComprehensiveOrderDetailController(ComprehensiveOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        // GET: api/comprehensiveorderdetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComprehensiveOrderDetailRequestDTO>>> GetAll()
        {
            var orderDetails = await _orderDetailService.GetAllAsync();
            return Ok(orderDetails);
        }

        // GET: api/comprehensiveorderdetail/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ComprehensiveOrderDetailRequestDTO>> GetById(int id)
        {
            try
            {
                var orderDetail = await _orderDetailService.GetByIdAsync(id);
                return Ok(orderDetail);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/comprehensiveorderdetail
        [HttpPost]
        public async Task<ActionResult<ComprehensiveOrderDetailRequestDTO>> Create(ComprehensiveOrderDetailRequestDTO orderDetailDto)
        {
            var createdOrderDetail = await _orderDetailService.CreateAsync(orderDetailDto);
            return CreatedAtAction(nameof(GetById), new { id = createdOrderDetail.OrderID }, createdOrderDetail);
        }

        // PUT: api/comprehensiveorderdetail/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ComprehensiveOrderDetailRequestDTO orderDetailDto)
        {
            if (id != orderDetailDto.OrderID)
            {
                return BadRequest();
            }

            try
            {
                await _orderDetailService.UpdateAsync(id, orderDetailDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/comprehensiveorderdetail/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderDetailService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
