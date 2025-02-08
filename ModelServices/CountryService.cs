using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.ModelServices
{
    public class CountryService : ICountryRepository
    {
        private readonly ApplicationDbContext _context;
        public CountryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await _context.Countries.ToListAsync();
        }
        public async Task<Country> GetByIdAsync(int id)
        {
            return await _context.Countries.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Country country)
        {
            _context.Countries.Update(country);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var country = await GetByIdAsync(id);
            if (country != null)
            {
                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();
            }
        }
    }
}
