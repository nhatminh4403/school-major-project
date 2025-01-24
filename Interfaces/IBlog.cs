using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IBlog
    {
        Task<IEnumerable<Blog>> GetAllAsync();
        Task<Blog> GetByIdAsync(int id);
        Task AddAsync(Blog blog);
        Task UpdateAsync(Blog blog);
        Task DeleteAsync(int id);
    }
}
