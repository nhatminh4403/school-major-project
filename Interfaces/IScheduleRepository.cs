using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IScheduleRepository : IModelRepository<Schedule>
    {

        Task<IEnumerable<Schedule>> GetSchedulesByFilmId(int id);

        Task<IEnumerable<Schedule>> GetSchedulesByRoomId(int id);
    }
}
