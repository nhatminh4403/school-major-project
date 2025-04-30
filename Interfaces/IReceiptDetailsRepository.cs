using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IReceiptDetailsRepository : IModelRepository<ReceiptDetail>
    {

        Task<IEnumerable<ReceiptDetail>> FindByScheduleId(int scheduleId);

    }
}
