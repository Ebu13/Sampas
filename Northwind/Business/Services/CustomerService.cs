using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Business.Services
{
    public class CustomerService : IGenericService<CustomerRequestDTO>
    {
        private readonly NorthwindContext _context;

        public CustomerService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerRequestDTO>> GetAllAsync()
        {
            return await _context.Customers
                .Select(c => new CustomerRequestDTO
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    ContactName = c.ContactName,
                    Address = c.Address,
                    City = c.City,
                    PostalCode = c.PostalCode,
                    Country = c.Country,
                    Password = c.Password
                })
                .ToListAsync();
        }

        public async Task<CustomerRequestDTO> GetByIdAsync(int id)
        {
            var customer = await _context.Customers
                .Where(c => c.CustomerId == id)
                .Select(c => new CustomerRequestDTO
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    ContactName = c.ContactName,
                    Address = c.Address,
                    City = c.City,
                    PostalCode = c.PostalCode,
                    Country = c.Country,
                    Password = c.Password
                })
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }

            return customer;
        }

        public async Task<CustomerRequestDTO> CreateAsync(CustomerRequestDTO customerDto)
        {
            var customer = new Customer
            {
                CustomerName = customerDto.CustomerName,
                ContactName = customerDto.ContactName,
                Address = customerDto.Address,
                City = customerDto.City,
                PostalCode = customerDto.PostalCode,
                Country = customerDto.Country,
                Password = customerDto.Password
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // DTO'ya ID'yi ekleyin
            customerDto.CustomerId = customer.CustomerId;

            return customerDto;
        }

        public async Task UpdateAsync(int id, CustomerRequestDTO customerDto)
        {
            if (id != customerDto.CustomerId)
            {
                throw new ArgumentException("Customer ID mismatch");
            }

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }

            customer.CustomerName = customerDto.CustomerName;
            customer.ContactName = customerDto.ContactName;
            customer.Address = customerDto.Address;
            customer.City = customerDto.City;
            customer.PostalCode = customerDto.PostalCode;
            customer.Country = customerDto.Country;
            customer.Password = customerDto.Password;

            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}
