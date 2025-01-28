using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Services
{
    public class RatingService : IRating
    {
        private readonly ApplicationDbContext _context;
        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Rating>> GetAllAsync()
        {
            return await _context.Ratings.ToListAsync();
        }
        public async Task<Rating> GetByIdAsync(int id)
        {
            return await _context.Ratings.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Rating rating)
        {
            _context.Ratings.Update(rating);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var rating = await GetByIdAsync(id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
            }
        }
    }
}
