using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IFilmRepository : IModelRepository<Film>
    {

        Task<Film> GetByName(string name);
        Task<List<string>> GetActorsListByFilmId(int filmId);

        Task<IEnumerable<Film>> GetFilmsByCountryAsync(int countryId);
        Task<IEnumerable<Film>> GetFilmsByCategoryAsync(int categoryId);
        Task<Film> GetByScheduleIdAsync(int scheduleId);
    }
}
