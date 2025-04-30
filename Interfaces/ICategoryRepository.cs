using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ICategoryRepository : IModelRepository<Category>
    {

        Task<IEnumerable<Category>> GetByIdsAsync(int[] ids);
    }
}
