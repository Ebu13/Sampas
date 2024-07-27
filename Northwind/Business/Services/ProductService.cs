using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;
using Northwind.Business.Request;

namespace Northwind.Business.Services
{
    public class ProductService : IGenericService<ProductRequestDTO>
    {
        private readonly NorthwindContext _context;

        public ProductService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductRequestDTO>> GetAllAsync()
        {
            return await _context.Products
                .Select(p => new ProductRequestDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    SupplierId = p.SupplierId,
                    CategoryId = p.CategoryId,
                    Unit = p.Unit,
                    Price = p.Price
                })
                .ToListAsync();
        }

        public async Task<ProductRequestDTO> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.ProductId == id)
                .Select(p => new ProductRequestDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    SupplierId = p.SupplierId,
                    CategoryId = p.CategoryId,
                    Unit = p.Unit,
                    Price = p.Price
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            return product;
        }

        public async Task<ProductRequestDTO> CreateAsync(ProductRequestDTO productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                SupplierId = productDto.SupplierId,
                CategoryId = productDto.CategoryId,
                Unit = productDto.Unit,
                Price = productDto.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // DTO'ya ID'yi ekleyin
            productDto.ProductId = product.ProductId;

            return productDto;
        }

        public async Task UpdateAsync(int id, ProductRequestDTO productDto)
        {
            if (id != productDto.ProductId)
            {
                throw new ArgumentException("Product ID mismatch");
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            product.ProductName = productDto.ProductName;
            product.SupplierId = productDto.SupplierId;
            product.CategoryId = productDto.CategoryId;
            product.Unit = productDto.Unit;
            product.Price = productDto.Price;

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
