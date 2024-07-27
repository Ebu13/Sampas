using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Services;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IGenericService<OrderDetailRequestDTO> _orderDetailService;

        public OrderDetailController(IGenericService<OrderDetailRequestDTO> orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        // GET: api/orderdetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailRequestDTO>>> GetOrderDetails()
        {
            var orderDetails = await _orderDetailService.GetAllAsync();
            return Ok(orderDetails);
        }

        // GET: api/orderdetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailRequestDTO>> GetOrderDetail(int id)
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

        // POST: api/orderdetail
        [HttpPost]
        public async Task<ActionResult<OrderDetailRequestDTO>> PostOrderDetail(OrderDetailRequestDTO orderDetailDto)
        {
            var createdOrderDetail = await _orderDetailService.CreateAsync(orderDetailDto);
            return CreatedAtAction(nameof(GetOrderDetail), new { id = createdOrderDetail.OrderDetailId }, createdOrderDetail);
        }

        // PUT: api/orderdetail/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetailRequestDTO orderDetailDto)
        {
            if (id != orderDetailDto.OrderDetailId)
            {
                return BadRequest("OrderDetail ID mismatch");
            }

            try
            {
                await _orderDetailService.UpdateAsync(id, orderDetailDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/orderdetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            try
            {
                await _orderDetailService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
