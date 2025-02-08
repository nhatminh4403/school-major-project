using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.ModelServices
{
    public class SeatTypeSerivce : ISeatTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public SeatTypeSerivce(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SeatType>> GetAllSeatTypeAsync()
        {
            return await _context.SeatTypes.Include(p => p.Seats).ToListAsync();
        }
        public async Task<SeatType> GetByIdAsync(int id)
        {
            return await _context.SeatTypes.Include(p => p.Seats).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(SeatType seatType)
        {
            _context.SeatTypes.Add(seatType);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(SeatType seatType)
        {
            _context.SeatTypes.Update(seatType);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var seatType = await GetByIdAsync(id);
            if (seatType != null)
            {
                _context.SeatTypes.Remove(seatType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
