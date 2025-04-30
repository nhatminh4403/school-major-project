using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IRoomRepository : IModelRepository<Room>
    {

        Task<Room> GetByScheduleIdAsync(int id);
    }
}
