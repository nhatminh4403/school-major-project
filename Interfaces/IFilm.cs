using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IFilm
    {
        Task<IEnumerable<Film>> GetAllAsync();
        Task<Film> GetByIdAsync(int id);
        Task AddAsync(Film film);
        Task UpdateAsync(Film film);
        Task DeleteAsync(int id);
    }
}
