using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IReceiptDetailsRepository
    {
        Task<IEnumerable<ReceiptDetail>> GetAllAsync();
        Task<ReceiptDetail> GetByIdAsync(int id);
        Task<IEnumerable<ReceiptDetail>> FindByScheduleId(int scheduleId);
    }
}
