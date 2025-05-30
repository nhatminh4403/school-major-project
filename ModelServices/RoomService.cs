﻿using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.ModelServices
{
    public class RoomService : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms.Include(p => p.Cinema).Include(p => p.Schedules).ToListAsync();
        }
        public async Task<Room> GetByIdAsync(int id)
        {
            return await _context.Rooms.Include(p => p.Cinema).Include(p => p.Schedules).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var room = await GetByIdAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Room> GetByScheduleIdAsync(int id)
        {
            return await _context.Rooms
                                 .Include(r => r.Cinema)
                                 .FirstOrDefaultAsync(r => r.Schedules.Any(s => s.Id == id));
        }

    }
}
