using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.ViewModel;
namespace school_major_project.Controllers
{
    public class BaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            // Khởi tạo layoutModel ở đây
            var films = _context.Films
                    .Include(p => p.Categories) // Include thông tin về category 
                    .Include(p => p.Schedules)
                    .ToList();
            var categories = _context.Categories.Include(p => p.Films).ToList();
            var seats = _context.Seats.ToList();
            var blog = _context.Blogs.ToList();
            var schedules = _context.Schedules.Include(p => p.Film).Include(p => p.Room).Include(p => p.Room.Cinema).ToList();
            var rooms = _context.Rooms.Include(p => p.Cinema).Include(p => p.Schedules).ToList();
            var cinemas = _context.Cinemas.Include(p => p.Rooms).ToList();
            var countries = _context.Countries.ToList();
            var filmVM = new BaseFilmVM
            {
                Films = films,
                Categories = categories,
                Schedules = schedules,
                Seats = seats,
                Rooms = rooms,
                Blogs = blog,
                Cinemas = cinemas,
                Countries = countries
            };
            // Giả sử bạn có một service để lấy dữ liệu thể loại phim

            ViewData["LayoutViewModel"] = filmVM;
        }
    }
}
