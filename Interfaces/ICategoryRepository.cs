using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category filmCategory);
        Task UpdateAsync(Category filmCategory);
        Task DeleteAsync(int id);
        Task <IEnumerable<Category>> GetByIdsAsync(int[] ids);
    }
}
