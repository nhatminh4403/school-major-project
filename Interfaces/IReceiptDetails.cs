using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IReceiptDetails
    {
        Task<IEnumerable<ReceiptDetail>> GetAllAsync();
        Task<ReceiptDetail> GetByIdAsync(int id);
    }
}
