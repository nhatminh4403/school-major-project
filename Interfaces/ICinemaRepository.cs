
using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ICinemaRepository : IModelRepository<Cinema>
    {

        Task<Cinema> GetSelectedCinema(int? id);
    }
}
