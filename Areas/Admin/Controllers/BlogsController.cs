using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/bai-viet")]
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlogRepository _blogRepository;
        public BlogsController(ApplicationDbContext context,IBlogRepository blogRepository)
        {
            _context = context;
            _blogRepository = blogRepository;
        }

        // GET: Admin/Blogs
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View(await _blogRepository.GetAllAsync());
        }

        // GET: Admin/Blogs/Details/5
        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Create
        [Route("tao-moi")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("tao-moi")]
        public async Task<IActionResult> Create(Blog blog, IFormFile BlogPoster)
        {

            if (ModelState.IsValid)
            {
                if (BlogPoster != null)
                {
                    blog.BlogPoster = await SaveImage(BlogPoster);
                }
                else
                {
                    ModelState.Remove("BlogPoster"); // Bỏ qua validation cho thuộc tính này
                }
                await _blogRepository.AddAsync(blog);
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Admin/Blogs/Edit/5
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogTitle,BlogContent,BlogCreatedDate,BlogPoster")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        [Route("xoa/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _blogRepository.DeleteAsync(id);
            return Json(new { success = true });
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/admin/images/blogs", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn     
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/admin/images/blogs/" + image.FileName; // Trả về đường dẫn tương đối
        }
    }
}
