using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface IPromotionRepository : IModelRepository<Promotion>
    {
        Task<Promotion> GetByCodeAsync(string code);
    }
}
