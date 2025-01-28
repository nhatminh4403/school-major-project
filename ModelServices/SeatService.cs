using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.ModelServices
{
    public class SeatService : ISeat
    {
        private readonly ApplicationDbContext _context;
        public SeatService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Seat>> GetAllSeatAsync()
        {
            return await _context.Seats.Include(p => p.SeatType).ToListAsync();
        }
        public async Task<Seat> GetByIdAsync(int id)
        {
            return await _context.Seats.Include(p => p.SeatType).FirstOrDefaultAsync(p => p.SeatId == id);
        }
        public async Task AddAsync(Seat seat)
        {
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var seat = await GetByIdAsync(id);
            if (seat != null)
            {
                _context.Seats.Remove(seat);
                await _context.SaveChangesAsync();
            }
        }
    }
}
