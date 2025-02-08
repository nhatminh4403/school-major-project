using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Services
{
    public class FoodService : IFoodRepository
    {
        private readonly ApplicationDbContext _context;
        public FoodService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Food>> GetAllAsync()
        {
            return await _context.Foods.ToListAsync();
        }
        public async Task<Food> GetByIdAsync(int id)
        {
            return await _context.Foods.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Food food)
        {
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Food food)
        {
            _context.Foods.Update(food);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var food = await GetByIdAsync(id);
            if (food != null)
            {
                _context.Foods.Remove(food);
                await _context.SaveChangesAsync();
            }
        }
    }
}
