using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Request;
using Northwind.Business.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<OrderRequestDTO> _orderService;

        public OrderController(IGenericService<OrderRequestDTO> orderService)
        {
            _orderService = orderService;
        }

        // GET: api/order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderRequestDTO>>> GetOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        // GET: api/order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderRequestDTO>> GetOrder(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                return Ok(order);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/order
        [HttpPost]
        public async Task<ActionResult<OrderRequestDTO>> PostOrder(OrderRequestDTO orderDto)
        {
            var createdOrder = await _orderService.CreateAsync(orderDto);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.OrderId }, createdOrder);
        }

        // PUT: api/order/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderRequestDTO orderDto)
        {
            if (id != orderDto.OrderId)
            {
                return BadRequest("Order ID mismatch");
            }

            try
            {
                await _orderService.UpdateAsync(id, orderDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
