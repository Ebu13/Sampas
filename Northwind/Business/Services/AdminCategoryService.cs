using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Business.Request;
using Northwind.Models;

namespace Northwind.Business.Services
{
    public class AdminCategoryService : IGenericService<AdminCategoryRequestDTO>
    {
        private readonly NorthwindContext _context;

        public AdminCategoryService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AdminCategoryRequestDTO>> GetAllAsync()
        {
            return await _context.Categories
                .Join(_context.Products,
                      c => c.CategoryId,
                      p => p.CategoryId,
                      (c, p) => new AdminCategoryRequestDTO
                      {
                          CategoryId = c.CategoryId,
                          CategoryName = c.CategoryName,
                          Description = c.Description,
                          ProductName = p.ProductName,
                          Unit = p.Unit,
                          Price = p.Price
                      })
                .ToListAsync();
        }

        public async Task<AdminCategoryRequestDTO> GetByIdAsync(int id)
        {
            var adminCategory = await _context.Categories
                .Where(c => c.CategoryId == id)
                .Join(_context.Products,
                      c => c.CategoryId,
                      p => p.CategoryId,
                      (c, p) => new AdminCategoryRequestDTO
                      {
                          CategoryId = c.CategoryId,
                          CategoryName = c.CategoryName,
                          Description = c.Description,
                          ProductName = p.ProductName,
                          Unit = p.Unit,
                          Price = p.Price
                      })
                .FirstOrDefaultAsync();

            if (adminCategory == null)
            {
                throw new KeyNotFoundException("Admin category not found");
            }

            return adminCategory;
        }

        public async Task<AdminCategoryRequestDTO> CreateAsync(AdminCategoryRequestDTO adminCategoryDto)
        {
            var category = new Category
            {
                CategoryName = adminCategoryDto.CategoryName,
                Description = adminCategoryDto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var product = new Product
            {
                ProductName = adminCategoryDto.ProductName,
                CategoryId = category.CategoryId,
                Unit = adminCategoryDto.Unit,
                Price = adminCategoryDto.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            adminCategoryDto.CategoryId = category.CategoryId;

            return adminCategoryDto;
        }

        public async Task UpdateAsync(int id, AdminCategoryRequestDTO adminCategoryDto)
        {
            if (id != adminCategoryDto.CategoryId)
            {
                throw new ArgumentException("Category ID mismatch");
            }

            var category = await _context.Categories.FindAsync(id);
            var product = await _context.Products
                .Where(p => p.CategoryId == id && p.ProductName == adminCategoryDto.ProductName)
                .FirstOrDefaultAsync();

            if (category == null || product == null)
            {
                throw new KeyNotFoundException("Category or product not found");
            }

            category.CategoryName = adminCategoryDto.CategoryName;
            category.Description = adminCategoryDto.Description;
            product.Unit = adminCategoryDto.Unit;
            product.Price = adminCategoryDto.Price;

            _context.Entry(category).State = EntityState.Modified;
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            var products = await _context.Products
                .Where(p => p.CategoryId == id)
                .ToListAsync();

            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            _context.Products.RemoveRange(products);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
