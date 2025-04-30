using school_major_project.DTO;
using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ISeatRepository : IModelRepository<Seat>
    {

        Task<IEnumerable<Seat>> GetByRoomIdAsync(int id);
        Task<IEnumerable<SeatDTO>> GetSeatsByRoomId(int id);
        Task<IEnumerable<SeatDTO>> GetSeatsByScheduleIdAndRoomId(int roomId, int scheduleId);
    }
}
