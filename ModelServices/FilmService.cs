using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Services
{
    public class FilmService : IFilmRepository
    {
        private readonly ApplicationDbContext _context;
        public FilmService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Film>> GetAllAsync()
        {
            return await _context.Films.Include(p => p.Categories).Include(p => p.Rating).Include(p => p.Schedules).Include(p => p.Country).ToListAsync();
        }
        public async Task<Film> GetByIdAsync(int id)
        {
            return await _context.Films.Include(p => p.Categories).Include(p => p.Rating).Include(p => p.Schedules).Include(p => p.Country).FirstOrDefaultAsync(p => p.Id == id);
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
        public async Task<IEnumerable<Film>> GetFilmsByCountryAsync(int countryId)
        {
            var filmsByCountry = await _context.Films.Include(p => p.Categories).Include(p => p.Rating).Include(p => p.Schedules).Where(p => p.CountryId == countryId).ToListAsync();
            return filmsByCountry;
        }

        public async Task<List<string>> GetActorsListByFilmId(int filmId)
        {
            List<string> ActorsList;
            Film film = await GetByIdAsync(filmId);
            if (film.Actors == null)
            {
                return new List<string>();
            }

            ActorsList = new List<string>(film.Actors.Split(',').Select(x => x.Trim()).ToArray());

            return ActorsList;
        }

        public async Task<Film> GetByName(string name)
        {
            return await _context.Films.Include(p => p.Categories).Include(p => p.Rating).Include(p => p.Schedules).FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Film>> GetFilmsByCategoryAsync(int categoryId)
        {
            var filmsByCategories = await _context.Films.Include(p => p.Categories).Include(p => p.Rating).Include(p => p.Schedules).Where(p => p.Categories.Any(c => c.Id == categoryId)).ToListAsync();
            return filmsByCategories;
        }

        public async Task<Film> GetByScheduleIdAsync(int scheduleId)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(p => p.Id == scheduleId);
            var film = await _context.Films.Include(p => p.Categories).Include(p => p.Rating).Include(p => p.Schedules).FirstOrDefaultAsync(p => p.Id == schedule.FilmId);
            return film;
        }
    }
}
