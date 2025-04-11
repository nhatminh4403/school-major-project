using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Services
{
    public class CategoryService : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.Include(p => p.Films).ToListAsync();
        }
        public async Task<Category> GetByIdAsync(int id) => await _context.Categories.Include(p => p.Films).FirstOrDefaultAsync(p => p.Id == id);
        public async Task AddAsync(Category filmCategory)
        {
            _context.Categories.Add(filmCategory);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Category filmCategory)
        {
            _context.Categories.Update(filmCategory);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Category>> GetByIdsAsync(int[] ids)
        {
            return await _context.Categories.Where(c => ids.Contains(c.Id)).ToListAsync();
        }
    }
}
