using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IFilmRepository
    {
        Task<IEnumerable<Film>> GetAllAsync();
        Task<Film> GetByIdAsync(int id);
        Task AddAsync(Film film);
        Task UpdateAsync(Film film);
        Task DeleteAsync(int id);

        Task<Film> GetByName(string name);
        Task<List<string>> GetActorsListByFilmId(int filmId);

        Task<IEnumerable<Film>> GetFilmsByCountryAsync(int countryId);
        Task<IEnumerable<Film>> GetFilmsByCategoryAsync(int categoryId);

    }
}
