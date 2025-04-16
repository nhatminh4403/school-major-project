using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IPromotionRepository
    {
        Task<IEnumerable<Promotion>> GetAllAsync();
        Task<Promotion> GetByIdAsync(int id);

        Task<Promotion> GetByCodeAsync(string code);

        Task AddAsync(Promotion promotion);
        Task UpdateAsync(Promotion promotion);
        Task DeleteAsync(int id);
    }
}
