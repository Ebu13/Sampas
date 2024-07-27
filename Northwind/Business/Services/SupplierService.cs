using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Business.Services
{
    public class SupplierService : IGenericService<SupplierRequestDTO>
    {
        private readonly NorthwindContext _context;

        public SupplierService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SupplierRequestDTO>> GetAllAsync()
        {
            return await _context.Suppliers
                .Select(s => new SupplierRequestDTO
                {
                    SupplierId = s.SupplierId,
                    SupplierName = s.SupplierName,
                    ContactName = s.ContactName,
                    Address = s.Address,
                    City = s.City,
                    PostalCode = s.PostalCode,
                    Country = s.Country,
                    Phone = s.Phone
                })
                .ToListAsync();
        }

        public async Task<SupplierRequestDTO> GetByIdAsync(int id)
        {
            var supplier = await _context.Suppliers
                .Where(s => s.SupplierId == id)
                .Select(s => new SupplierRequestDTO
                {
                    SupplierId = s.SupplierId,
                    SupplierName = s.SupplierName,
                    ContactName = s.ContactName,
                    Address = s.Address,
                    City = s.City,
                    PostalCode = s.PostalCode,
                    Country = s.Country,
                    Phone = s.Phone
                })
                .FirstOrDefaultAsync();

            if (supplier == null)
            {
                throw new KeyNotFoundException("Supplier not found");
            }

            return supplier;
        }

        public async Task<SupplierRequestDTO> CreateAsync(SupplierRequestDTO supplierDto)
        {
            var supplier = new Supplier
            {
                SupplierName = supplierDto.SupplierName,
                ContactName = supplierDto.ContactName,
                Address = supplierDto.Address,
                City = supplierDto.City,
                PostalCode = supplierDto.PostalCode,
                Country = supplierDto.Country,
                Phone = supplierDto.Phone
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            // DTO'ya ID'yi ekleyin
            supplierDto.SupplierId = supplier.SupplierId;

            return supplierDto;
        }

        public async Task UpdateAsync(int id, SupplierRequestDTO supplierDto)
        {
            if (id != supplierDto.SupplierId)
            {
                throw new ArgumentException("Supplier ID mismatch");
            }

            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                throw new KeyNotFoundException("Supplier not found");
            }

            supplier.SupplierName = supplierDto.SupplierName;
            supplier.ContactName = supplierDto.ContactName;
            supplier.Address = supplierDto.Address;
            supplier.City = supplierDto.City;
            supplier.PostalCode = supplierDto.PostalCode;
            supplier.Country = supplierDto.Country;
            supplier.Phone = supplierDto.Phone;

            _context.Entry(supplier).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                throw new KeyNotFoundException("Supplier not found");
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }
    }
}
