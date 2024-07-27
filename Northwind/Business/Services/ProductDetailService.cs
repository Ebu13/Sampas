using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Business.Services
{
    public class ProductDetailService : IGenericService<ProductDetailRequestDTO>
    {
        private readonly NorthwindContext _context;

        public ProductDetailService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDetailRequestDTO>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Supplier) // Supplier bilgilerini almak için
                .Include(p => p.Category) // Category bilgilerini almak için
                .Select(p => new ProductDetailRequestDTO
                {
                    ProductID = p.ProductId,
                    ProductName = p.ProductName,
                    Unit = p.Unit,
                    Price = p.Price,
                    CategoryName = p.Category.CategoryName, // Kategorinin adını al
                    SupplierContactName = p.Supplier.ContactName // Tedarikçi iletişim adını al
                })
                .ToListAsync();
        }

        public async Task<ProductDetailRequestDTO> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .Where(p => p.ProductId == id)
                .Select(p => new ProductDetailRequestDTO
                {
                    ProductID = p.ProductId,
                    ProductName = p.ProductName,
                    Unit = p.Unit,
                    Price = p.Price,
                    CategoryName = p.Category.CategoryName,
                    SupplierContactName = p.Supplier.ContactName
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            return product;
        }

        public async Task<ProductDetailRequestDTO> CreateAsync(ProductDetailRequestDTO productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                Unit = productDto.Unit,
                Price = productDto.Price,
                // Kategori ve Tedarikçi ayarlarını buraya ekleyin, örneğin:
                // CategoryId = productDto.CategoryId,
                // SupplierId = productDto.SupplierId,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // DTO'ya ID'yi ekleyin
            productDto.ProductID = product.ProductId;

            return productDto;
        }

        public async Task UpdateAsync(int id, ProductDetailRequestDTO productDto)
        {
            if (id != productDto.ProductID)
            {
                throw new ArgumentException("Product ID mismatch");
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            product.ProductName = productDto.ProductName;
            product.Unit = productDto.Unit;
            product.Price = productDto.Price;
            // Kategori ve Tedarikçi bilgilerini güncellemek için gerekli alanları buraya ekleyin.

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
