using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country> GetByIdAsync(int id);
        Task AddAsync(Country filmCategory);
        Task UpdateAsync(Country filmCategory);
        Task DeleteAsync(int id);
    }
}
