using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Business.Request;
using Northwind.Models;

namespace Northwind.Business.Services
{
    public class CategoryService : IGenericService<CategoryRequestDTO>
    {
        private readonly NorthwindContext _context;

        public CategoryService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryRequestDTO>> GetAllAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryRequestDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                })
                .ToListAsync();
        }

        public async Task<CategoryRequestDTO> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryRequestDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                })
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            return category;
        }

        public async Task<CategoryRequestDTO> CreateAsync(CategoryRequestDTO categoryDto)
        {
            var category = new Category
            {
                CategoryName = categoryDto.CategoryName,
                Description = categoryDto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // DTO'ya ID'yi ekleyin
            categoryDto.CategoryId = category.CategoryId;

            return categoryDto;
        }

        public async Task UpdateAsync(int id, CategoryRequestDTO categoryDto)
        {
            if (id != categoryDto.CategoryId)
            {
                throw new ArgumentException("Category ID mismatch");
            }

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            category.CategoryName = categoryDto.CategoryName;
            category.Description = categoryDto.Description;

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
