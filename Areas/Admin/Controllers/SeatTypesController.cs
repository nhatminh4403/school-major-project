using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/loai-ghe")]
    [Authorize(Roles = "Admin")]

    public class SeatTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISeatTypeRepository _seatTypeRepository;

        public SeatTypesController(ApplicationDbContext context, ISeatTypeRepository seatTypeRepository)
        {
            _context = context;
            _seatTypeRepository = seatTypeRepository;
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/admin/images/seattypes", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn     
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/admin/images/seattypes/" + image.FileName; // Trả về đường dẫn tương đối
        }
        [Route("")]
        public async Task<IActionResult> Index()
        {

            var types = await _seatTypeRepository.GetAllSeatTypeAsync();
            return View(types);
        }



        [Route("tao-moi")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Route("tao-moi")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SeatType seatType, IFormFile ImageDescription)
        {
            if (ImageDescription != null)
            {
                seatType.ImageDescription = await SaveImage(ImageDescription);
            }
            else
            {
                ModelState.Remove("ImageDescription"); // Bỏ qua validation cho thuộc tính này
            }

            if (ModelState.IsValid)
            {
                await _seatTypeRepository.AddAsync(seatType);
                return RedirectToAction(nameof(Index));
            }
            return View(seatType);
        }

        [Route("xoa/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seatType = await _seatTypeRepository.GetByIdAsync(id);
            if (seatType != null)
            {
                await _seatTypeRepository.DeleteAsync(seatType.Id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SeatTypeExists(int id)
        {
            return _context.SeatTypes.Any(e => e.Id == id);
        }
    }
}
