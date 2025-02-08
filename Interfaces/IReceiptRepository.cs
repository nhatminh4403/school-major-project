using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IReceiptRepository
    {
        Task<IEnumerable<Receipt>> GetAllAsync();
        Task<Receipt> GetByIdAsync(int id);
        Task AddAsync(Receipt receipt);
        Task UpdateAsync(Receipt receipt);
        Task DeleteAsync(int id);
    }
}
