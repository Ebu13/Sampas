using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Business.Services
{
    public class ComprehensiveOrderDetailService
    {
        private readonly NorthwindContext _context;

        public ComprehensiveOrderDetailService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComprehensiveOrderDetailRequestDTO>> GetAllAsync()
        {
            return await (from o in _context.Orders
                          join od in _context.OrderDetails on o.OrderId equals od.OrderId
                          join p in _context.Products on od.ProductId equals p.ProductId
                          join c in _context.Customers on o.CustomerId equals c.CustomerId
                          join e in _context.Employees on o.EmployeeId equals e.EmployeeId
                          orderby o.OrderDate descending
                          select new ComprehensiveOrderDetailRequestDTO
                          {
                              OrderID = o.OrderId,
                              OrderDate = o.OrderDate,
                              CustomerName = c.CustomerName,
                              EmployeeFirstName = e.FirstName,
                              EmployeeLastName = e.LastName,
                              Quantity = od.Quantity ?? 0,
                              ProductName = p.ProductName,
                              Price = p.Price ?? 0m
                          }).ToListAsync();
        }

        public async Task<ComprehensiveOrderDetailRequestDTO> GetByIdAsync(int orderId)
        {
            var orderDetail = await (from o in _context.Orders
                                     join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                     join p in _context.Products on od.ProductId equals p.ProductId
                                     join c in _context.Customers on o.CustomerId equals c.CustomerId
                                     join e in _context.Employees on o.EmployeeId equals e.EmployeeId
                                     where o.OrderId == orderId
                                     select new ComprehensiveOrderDetailRequestDTO
                                     {
                                         OrderID = o.OrderId,
                                         OrderDate = o.OrderDate,
                                         CustomerName = c.CustomerName,
                                         EmployeeFirstName = e.FirstName,
                                         EmployeeLastName = e.LastName,
                                         Quantity = od.Quantity ?? 0,
                                         ProductName = p.ProductName,
                                         Price = p.Price ?? 0m
                                     }).FirstOrDefaultAsync();

            if (orderDetail == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            return orderDetail;
        }

        public async Task<ComprehensiveOrderDetailRequestDTO> CreateAsync(ComprehensiveOrderDetailRequestDTO orderDetailDto)
        {
            // Sipariş oluşturmak için gerekli nesneleri oluşturun ve veritabanına ekleyin
            var order = new Order
            {
                OrderDate = orderDetailDto.OrderDate,
                CustomerId = await _context.Customers
                                           .Where(c => c.CustomerName == orderDetailDto.CustomerName)
                                           .Select(c => c.CustomerId)
                                           .FirstOrDefaultAsync(),
                EmployeeId = await _context.Employees
                                            .Where(e => e.FirstName == orderDetailDto.EmployeeFirstName &&
                                                        e.LastName == orderDetailDto.EmployeeLastName)
                                            .Select(e => e.EmployeeId)
                                            .FirstOrDefaultAsync()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderDetail = new OrderDetail
            {
                OrderId = order.OrderId,
                ProductId = await _context.Products
                                          .Where(p => p.ProductName == orderDetailDto.ProductName)
                                          .Select(p => p.ProductId)
                                          .FirstOrDefaultAsync(),
                Quantity = orderDetailDto.Quantity
            };

            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            // DTO'ya ID'yi ekleyin
            orderDetailDto.OrderID = order.OrderId;

            return orderDetailDto;
        }

        public async Task UpdateAsync(int orderId, ComprehensiveOrderDetailRequestDTO orderDetailDto)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            order.OrderDate = orderDetailDto.OrderDate;

            // Gerekirse customer ve employee bilgilerini güncelleyin
            order.CustomerId = await _context.Customers
                                              .Where(c => c.CustomerName == orderDetailDto.CustomerName)
                                              .Select(c => c.CustomerId)
                                              .FirstOrDefaultAsync();

            order.EmployeeId = await _context.Employees
                                              .Where(e => e.FirstName == orderDetailDto.EmployeeFirstName &&
                                                          e.LastName == orderDetailDto.EmployeeLastName)
                                              .Select(e => e.EmployeeId)
                                              .FirstOrDefaultAsync();

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Sipariş detayını güncelleyin
            var orderDetail = await _context.OrderDetails
                                             .Where(od => od.OrderId == orderId)
                                             .FirstOrDefaultAsync();

            if (orderDetail == null)
            {
                throw new KeyNotFoundException("Order detail not found");
            }

            orderDetail.Quantity = orderDetailDto.Quantity;
            orderDetail.ProductId = await _context.Products
                                                  .Where(p => p.ProductName == orderDetailDto.ProductName)
                                                  .Select(p => p.ProductId)
                                                  .FirstOrDefaultAsync();

            _context.Entry(orderDetail).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            // Sipariş detaylarını silin
            var orderDetails = _context.OrderDetails.Where(od => od.OrderId == orderId);
            _context.OrderDetails.RemoveRange(orderDetails);

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
