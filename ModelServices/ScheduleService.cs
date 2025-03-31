using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.ModelServices
{
    public class ScheduleService : IScheduleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ScheduleService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            return await _dbContext.Schedules.Include(p => p.Film).Include(p => p.Room).Include(p => p.Room.Cinema).ToListAsync();
        }
        public async Task<Schedule> GetByIdAsync(int id)
        {
            return await _dbContext.Schedules.Include(p => p.Film).Include(p => p.Room).Include(p => p.Room.Cinema).FirstAsync(p => p.Id == id);
        }
        public async Task AddAsync(Schedule filmSchedule)
        {
            _dbContext.Schedules.Add(filmSchedule);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Schedule filmSchedule)
        {
            _dbContext.Schedules.Update(filmSchedule);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var schedule = await _dbContext.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _dbContext.Schedules.Remove(schedule);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Schedule>> GetSchedulesByFilmId(int id)
        {
            return await _dbContext.Schedules.
                Include(p => p.Room).ThenInclude(p => p.Cinema).
                Where(p => p.FilmId == id).ToListAsync();

        }
    }
}
