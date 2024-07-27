using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Models;

namespace Northwind.Business.Services
{
    public class SupplierDetailService : IGenericService<SupplierDetailRequestDTO>
    {
        private readonly NorthwindContext _context;

        public SupplierDetailService(NorthwindContext context)
        {
            _context = context;
        }

        // Tüm tedarikçi detaylarını al
        public async Task<IEnumerable<SupplierDetailRequestDTO>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category) // Kategorileri dahil et
                .Include(p => p.Supplier) // Tedarikçileri dahil et
                .Select(p => new SupplierDetailRequestDTO
                {
                    ProductID = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryID = p.CategoryId,
                    CategoryName = p.Category.CategoryName,
                    SupplierID = p.SupplierId,
                    SupplierName = p.Supplier.SupplierName,
                    ContactName = p.Supplier.ContactName,
                    Address = p.Supplier.Address,
                    City = p.Supplier.City,
                    PostalCode = p.Supplier.PostalCode,
                    Country = p.Supplier.Country,
                    Phone = p.Supplier.Phone
                })
                .ToListAsync();
        }

        // Belirli bir tedarikçi detayını al
        public async Task<SupplierDetailRequestDTO> GetByIdAsync(int supplierId)
        {
            var supplierDetail = await _context.Products
                .Include(p => p.Category) // Kategorileri dahil et
                .Include(p => p.Supplier) // Tedarikçileri dahil et
                .Where(p => p.SupplierId == supplierId)
                .Select(p => new SupplierDetailRequestDTO
                {
                    ProductID = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryID = p.CategoryId,
                    CategoryName = p.Category.CategoryName,
                    SupplierID = p.SupplierId,
                    SupplierName = p.Supplier.SupplierName,
                    ContactName = p.Supplier.ContactName,
                    Address = p.Supplier.Address,
                    City = p.Supplier.City,
                    PostalCode = p.Supplier.PostalCode,
                    Country = p.Supplier.Country,
                    Phone = p.Supplier.Phone
                })
                .FirstOrDefaultAsync();

            if (supplierDetail == null)
            {
                throw new KeyNotFoundException("Supplier not found");
            }

            return supplierDetail;
        }

        // Yeni bir tedarikçi detayı oluştur
        public async Task<SupplierDetailRequestDTO> CreateAsync(SupplierDetailRequestDTO supplierDetailDto)
        {
            var supplier = new Supplier
            {
                SupplierName = supplierDetailDto.SupplierName,
                ContactName = supplierDetailDto.ContactName,
                Address = supplierDetailDto.Address,
                City = supplierDetailDto.City,
                PostalCode = supplierDetailDto.PostalCode,
                Country = supplierDetailDto.Country,
                Phone = supplierDetailDto.Phone
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            // Ürün oluştur
            var product = new Product
            {
                ProductName = supplierDetailDto.ProductName,
                CategoryId = supplierDetailDto.CategoryID,
                SupplierId = supplier.SupplierId // Tedarikçi ile ilişkilendir
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            supplierDetailDto.SupplierID = supplier.SupplierId; // ID'yi DTO'ya ekle
            supplierDetailDto.ProductID = product.ProductId; // Ürün ID'sini DTO'ya ekle

            return supplierDetailDto;
        }

        // Tedarikçi detayını güncelle
        public async Task UpdateAsync(int supplierId, SupplierDetailRequestDTO supplierDetailDto)
        {
            if (supplierId != supplierDetailDto.SupplierID)
            {
                throw new ArgumentException("Supplier ID mismatch");
            }

            var supplier = await _context.Suppliers.FindAsync(supplierId);
            if (supplier == null)
            {
                throw new KeyNotFoundException("Supplier not found");
            }

            supplier.SupplierName = supplierDetailDto.SupplierName;
            supplier.ContactName = supplierDetailDto.ContactName;
            supplier.Address = supplierDetailDto.Address;
            supplier.City = supplierDetailDto.City;
            supplier.PostalCode = supplierDetailDto.PostalCode;
            supplier.Country = supplierDetailDto.Country;
            supplier.Phone = supplierDetailDto.Phone;

            _context.Entry(supplier).State = EntityState.Modified;

            // Ürün güncelle
            var product = await _context.Products.FirstOrDefaultAsync(p => p.SupplierId == supplierId);
            if (product != null)
            {
                product.ProductName = supplierDetailDto.ProductName;
                product.CategoryId = supplierDetailDto.CategoryID;
                _context.Entry(product).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        // Tedarikçiyi sil
        public async Task DeleteAsync(int supplierId)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId);
            if (supplier == null)
            {
                throw new KeyNotFoundException("Supplier not found");
            }

            // İlişkili ürünleri sil
            var products = await _context.Products.Where(p => p.SupplierId == supplierId).ToListAsync();
            _context.Products.RemoveRange(products);

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }
    }
}
