using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IPromotion
    {
        Task<IEnumerable<Promotion>> GetAllAsync();
        Task<Promotion> GetByIdAsync(int id);
        Task AddAsync(Promotion promotion);
        Task UpdateAsync(Promotion promotion);
        Task DeleteAsync(int id);
    }
}
