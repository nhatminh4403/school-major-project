using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.DTO;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.ModelServices
{
    public class SeatService : ISeatRepository
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
        public async Task<IEnumerable<Seat>> GetByRoomIdAsync(int id)
        {
            return await _context.Seats.Include(p => p.SeatType).Include(p=>p.Room).ThenInclude(p=> p.Schedules).Where(p => p.RoomId == id).ToListAsync();
        }
        public async Task<IEnumerable<SeatDTO>> GetSeatsByRoomId(int roomId)
        {
            var seatsInRoom = await _context.Seats
                                         .Include(s => s.SeatType)
                                         .Include(s => s.Room)    
                                         .Where(s => s.RoomId == roomId)
                                         .ToListAsync();

            if (seatsInRoom == null)
            {
                return Enumerable.Empty<SeatDTO>();
            }

            // Map Seat entities to SeatDTO
            return seatsInRoom.Select(seat => new SeatDTO
            {
                Id = seat.SeatId,
                SeatNumber = seat.SeatNumber ?? "N/A",
                SeatType = seat.SeatType?.TypeDescription ?? "Unknown",
                RoomName = seat.Room?.Name ?? "Unknown Room",

                Price = seat.SeatType?.Price ?? 0,

                Image = seat.SeatImage ?? "/assets/img/seat/default.png",

                Status = false
            });
        }

        public async Task<IEnumerable<SeatDTO>> GetSeatsByScheduleIdAndRoomId(int roomId, int scheduleId)
        {
            // 1. Get all relevant seats with their type and room info
            var seatsInRoom = await _context.Seats
                                         .Include(s => s.SeatType)
                                         .Include(s => s.Room)
                                         .Where(s => s.RoomId == roomId)
                                         .ToListAsync();

            if (seatsInRoom == null || !seatsInRoom.Any())
            {
                return Enumerable.Empty<SeatDTO>(); // No seats in the room
            }

            // 2. Get IDs of seats booked for THIS specific schedule
            var bookedSeatIds = await _context.ReceiptDetails
                .Where(rd => rd.ScheduleId == scheduleId && rd.SeatId != null)
                .Select(rd => rd.SeatId)
                .Distinct()
                .ToListAsync();

            // 3. Map to SeatDTO, setting Status based on bookedSeatIds
            return seatsInRoom.Select(seat => new SeatDTO
            {
                Id = seat.SeatId,
                SeatNumber = seat.SeatNumber ?? "N/A",
                SeatType = seat.SeatType?.TypeDescription ?? "Unknown",
                RoomName = seat.Room?.Name ?? "Unknown Room",
                Price = seat.SeatType?.Price ?? 0,

                Image = seat.SeatImage ?? "/assets/img/seat/default.png",
                Status = bookedSeatIds.Contains(seat.SeatId)
            });

        }

    }
}
