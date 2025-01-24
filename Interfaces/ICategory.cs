using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ICategory
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category filmCategory);
        Task UpdateAsync(Category filmCategory);
        Task DeleteAsync(int id);
    }
}
