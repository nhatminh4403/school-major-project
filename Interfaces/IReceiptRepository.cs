using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IReceiptRepository : IModelRepository<Receipt>
    {


        Task<IEnumerable<Receipt>> GetByUserIdAsync(string userId);
    }
}
