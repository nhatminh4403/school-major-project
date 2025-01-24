using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ISchedule
    {
        Task<IEnumerable<Schedule>> GetAllAsync();
        Task<Schedule> GetByIdAsync(int id);
        Task AddAsync(Schedule filmSchedule);
        Task UpdateAsync(Schedule filmSchedule);
        Task DeleteAsync(int id);
    }
}
