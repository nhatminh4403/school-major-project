using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IRatingRepository : IModelRepository<Rating>
    {

        Task<bool> HasUserRated(string userId, int filmId);
    }
}
