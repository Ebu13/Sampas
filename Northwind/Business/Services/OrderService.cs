using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Business.Services
{
    public class OrderService : IGenericService<OrderRequestDTO>
    {
        private readonly NorthwindContext _context;

        public OrderService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderRequestDTO>> GetAllAsync()
        {
            return await _context.Orders
                .Select(o => new OrderRequestDTO
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    EmployeeId = o.EmployeeId,
                    OrderDate = o.OrderDate,
                    ShipperId = o.ShipperId
                })
                .ToListAsync();
        }

        public async Task<OrderRequestDTO> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Where(o => o.OrderId == id)
                .Select(o => new OrderRequestDTO
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    EmployeeId = o.EmployeeId,
                    OrderDate = o.OrderDate,
                    ShipperId = o.ShipperId
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            return order;
        }

        public async Task<OrderRequestDTO> CreateAsync(OrderRequestDTO orderDto)
        {
            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                EmployeeId = orderDto.EmployeeId,
                OrderDate = orderDto.OrderDate,
                ShipperId = orderDto.ShipperId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // DTO'ya ID'yi ekleyin
            orderDto.OrderId = order.OrderId;

            return orderDto;
        }

        public async Task UpdateAsync(int id, OrderRequestDTO orderDto)
        {
            if (id != orderDto.OrderId)
            {
                throw new ArgumentException("Order ID mismatch");
            }

            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            order.CustomerId = orderDto.CustomerId;
            order.EmployeeId = orderDto.EmployeeId;
            order.OrderDate = orderDto.OrderDate;
            order.ShipperId = orderDto.ShipperId;

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
