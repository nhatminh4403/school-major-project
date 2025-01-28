using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Services
{
    public class FilmService : IFilm
    {
        private readonly ApplicationDbContext _context;
        public FilmService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Film>> GetAllAsync()
        {
            return await _context.Films.Include(p => p.Categories).Include(p=> p.Rating).Include(p=>p.Schedules).ToListAsync();
        }
        public async Task<Film> GetByIdAsync(int id)
        {
            return await _context.Films.Include(p => p.Categories).Include(p => p.Rating).Include(p => p.Schedules).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Film film)
        {
            _context.Films.Add(film);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Film film)
        {
            _context.Films.Update(film);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var film = await GetByIdAsync(id);
            if (film != null)
            {
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();
            }
        }
    }
}
