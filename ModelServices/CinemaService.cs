using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Services
{
    public class CinemaService : ICinema
    {
        private readonly ApplicationDbContext _context;
        public CinemaService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cinema>> GetAllAsync()
        {
            return await _context.Cinemas.Include(p =>p.Rooms).ToListAsync();
        }
        public async Task<Cinema> GetByIdAsync(int id)
        {
            return await _context.Cinemas.Include(p => p.Rooms).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Cinema cinema)
        {
            _context.Cinemas.Add(cinema);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Cinema cinema)
        {
            _context.Cinemas.Update(cinema);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var cinema = await GetByIdAsync(id);
            if (cinema != null)
            {
                _context.Cinemas.Remove(cinema);
                await _context.SaveChangesAsync();
            }
        }
    }
}
