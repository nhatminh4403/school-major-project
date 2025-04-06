using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetAllSeatAsync();
        Task<Seat> GetByIdAsync(int id);
        Task AddAsync(Seat seat);
        Task UpdateAsync(Seat seat);
        Task DeleteAsync(int id);
        Task<IEnumerable<Seat>> GetSeatsByRoomId(int id);
        Task<IEnumerable<Seat>> GetSeatsByScheduleIdAndRoomId(int roomId, int scheduleId);
    }
}
