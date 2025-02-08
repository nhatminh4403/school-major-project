using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Services
{
    public class PromotionService : IPromotionRepository
    {
        private readonly ApplicationDbContext _context;
        public PromotionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Promotion>> GetAllAsync()
        {
            return await _context.Promotions.ToListAsync();
        }
        public async Task<Promotion> GetByIdAsync(int id)
        {
            return await _context.Promotions.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var promotion = await GetByIdAsync(id);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
