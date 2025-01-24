using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ISeatType
    {
        Task<IEnumerable<SeatType>> GetAllSeatTypeAsync();
        Task<SeatType> GetByIdAsync(int id);
        Task AddAsync(SeatType seat);
        Task UpdateAsync(SeatType seat);
        Task DeleteAsync(int id);
    }
}
