using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllRoomAsync();
        Task<Room> GetByIdAsync(int id);
        Task AddAsync(Room theatreRoom);
        Task UpdateAsync(Room theatreRoom);
        Task DeleteAsync(int id);
    }
}
