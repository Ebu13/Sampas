using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Business.Services
{
    public class OrderDetailService : IGenericService<OrderDetailRequestDTO>
    {
        private readonly NorthwindContext _context;

        public OrderDetailService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDetailRequestDTO>> GetAllAsync()
        {
            return await _context.OrderDetails
                .Select(od => new OrderDetailRequestDTO
                {
                    OrderDetailId = od.OrderDetailId,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity
                })
                .ToListAsync();
        }

        public async Task<OrderDetailRequestDTO> GetByIdAsync(int id)
        {
            var orderDetail = await _context.OrderDetails
                .Where(od => od.OrderDetailId == id)
                .Select(od => new OrderDetailRequestDTO
                {
                    OrderDetailId = od.OrderDetailId,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity
                })
                .FirstOrDefaultAsync();

            if (orderDetail == null)
            {
                throw new KeyNotFoundException("OrderDetail not found");
            }

            return orderDetail;
        }

        public async Task<OrderDetailRequestDTO> CreateAsync(OrderDetailRequestDTO orderDetailDto)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = orderDetailDto.OrderId,
                ProductId = orderDetailDto.ProductId,
                Quantity = orderDetailDto.Quantity
            };

            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            // DTO'ya ID'yi ekleyin
            orderDetailDto.OrderDetailId = orderDetail.OrderDetailId;

            return orderDetailDto;
        }

        public async Task UpdateAsync(int id, OrderDetailRequestDTO orderDetailDto)
        {
            if (id != orderDetailDto.OrderDetailId)
            {
                throw new ArgumentException("OrderDetail ID mismatch");
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);

            if (orderDetail == null)
            {
                throw new KeyNotFoundException("OrderDetail not found");
            }

            orderDetail.OrderId = orderDetailDto.OrderId;
            orderDetail.ProductId = orderDetailDto.ProductId;
            orderDetail.Quantity = orderDetailDto.Quantity;

            _context.Entry(orderDetail).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                throw new KeyNotFoundException("OrderDetail not found");
            }

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
        }
    }
}
