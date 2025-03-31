using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using school_major_project.DataAccess;
using school_major_project.HelperClass;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Controllers
{

    public class BlogsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlogRepository _blogRepository;
        private readonly UserManager<User> _userManager;
        private readonly ICommentRepository _commentRepository;
        public BlogsController(ApplicationDbContext context, IBlogRepository blogRepository, ICommentRepository commentRepository, UserManager<User> userManager) : base(context)
        {
            _blogRepository = blogRepository;
            _context = context;
            _commentRepository = commentRepository;
            _userManager = userManager;
        }

        // GET: Blogs
        [Route("/blogs")]
        public async Task<IActionResult> Index()
        {
            var blogs = await _blogRepository.GetAllAsync();

            return View(blogs);
        }

        // GET: Blogs/Details/5
        [Route("/blogs/blog/{title}")]
        public async Task<IActionResult> Details(string title)
        {
            if (title == null)
            {
                return NotFound();
            }


            var blogs = await _blogRepository.GetAllAsync();
            var blog = blogs.FirstOrDefault(b => b.BlogTitle.RemoveDiacritics().Equals(title, StringComparison.OrdinalIgnoreCase));
            if (blog == null)
            {
                return NotFound();
            }


            return View(blog);
        }

        // POST: Foods/Delete/5
        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        [Route("/blogs/blog/{title}/comment/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id, string title)
        {
            var blogs = await _blogRepository.GetAllAsync();
            var blog = blogs.FirstOrDefault(b => b.BlogTitle.RemoveDiacritics().Equals(title, StringComparison.OrdinalIgnoreCase));

            var comments = await _commentRepository.GetCommentsByBlogIdAsync(blog.Id);
            var comment = comments.FirstOrDefault(c => c.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != comment.UserId && !User.IsInRole("Admin"))
            {
                return Forbid(); // Trả về 403 nếu không phải người tạo comment và không phải admin
            }
            await _commentRepository.DeleteAsync(comment.Id);
            return Redirect($"/blogs/blog/{title}");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/blogs/blog/{title}/comment")]
        public async Task<IActionResult> CreateComment(string title, string content)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest(new { message = "Bài blog không hợp lệ." });
            }

            var blogs = await _blogRepository.GetAllAsync();
            var blog = blogs.FirstOrDefault(b => b.BlogTitle.RemoveDiacritics().Equals(title, StringComparison.OrdinalIgnoreCase));
            if (blog == null)
            {
                return NotFound(new { message = "Không tìm thấy bài blog." });
            }

            if (string.IsNullOrEmpty(content))
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để bình luận." });
            }

            var comment = new Comment
            {
                Content = content,
                UserId = user.Id,
                BlogId = blog.Id,
                DateComment = DateTime.Now
            };

            await _commentRepository.AddAsync(comment);

            return Ok(new
            {
                message = "Bình luận đã được thêm!",
                comment = new
                {
                    UserId = user.Id,
                    content = comment.Content,
                    dateComment = comment.DateComment.ToString("dd/MM/yyyy HH:mm")
                }
            });
        }

    }
}
