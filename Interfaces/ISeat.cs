using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ISeat
    {
        Task<IEnumerable<Seat>> GetAllSeatAsync();
        Task<Seat> GetByIdAsync(int id);
        Task AddAsync(Seat seat);
        Task UpdateAsync(Seat seat);
        Task DeleteAsync(int id);
    }
}
