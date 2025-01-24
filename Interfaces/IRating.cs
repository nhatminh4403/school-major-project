using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IRating
    {
        Task<IEnumerable<Rating>> GetAllAsync();
        Task<Rating> GetByIdAsync(int id);
        Task AddAsync(Rating rating);
        Task UpdateAsync(Rating rating);
        Task DeleteAsync(int id);
    }
}
