using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IFood
    {
        Task<IEnumerable<Food>> GetAllAsync();
        Task<Food> GetByIdAsync(int id);
        Task AddAsync(Food food);
        Task UpdateAsync(Food  food);
        Task DeleteAsync(int id);
    }
}
