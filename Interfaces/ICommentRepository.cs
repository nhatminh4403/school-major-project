using school_major_project.Models;

namespace school_major_project.Interfaces
{
    public interface ICommentRepository : IModelRepository<Comment>
    {
        new Task<IEnumerable<Comment>> GetAllAsync(); 
        new Task<Comment> GetByIdAsync(int id); 
        new Task AddAsync(Comment comment);
        new Task UpdateAsync(Comment comment);
        new Task DeleteAsync(int id);

        Task<IEnumerable<Comment>> GetCommentsByBlogIdAsync(int blogId);
    }
}
