
using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ICinema
    {
        Task<IEnumerable<Cinema>> GetAllAsync();
        Task<Cinema> GetByIdAsync(int id);
        Task AddAsync(Cinema theatre);
        Task UpdateAsync(Cinema theatre);
        Task DeleteAsync(int id);
    }
}
