using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Services
{
    public class BlogService : IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Blog>> GetAllAsync()
        {
            return await _context.Blogs.Include(p => p.comments).ToListAsync();
        }

        public async Task<Blog> GetByIdAsync(int id)
        {

            return await _context.Blogs.Include(p => p.comments).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Blog> GetByNameAsync(string title_name)
        {
            return await _context.Blogs.Include(p => p.comments).FirstOrDefaultAsync(p => p.BlogTitle == title_name);
        }
        public async Task AddAsync(Blog blog)
        {
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var blog = await GetByIdAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
            }
        }
    }
}
