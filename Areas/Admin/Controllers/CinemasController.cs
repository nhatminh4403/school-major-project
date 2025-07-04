using Google.Type;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_major_project.Areas.Admin.Data;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/rap-phim")]
    [Authorize(Roles = "Admin")]

    public class CinemasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IScheduleRepository _scheduleRepository;
        public CinemasController(ApplicationDbContext context, IScheduleRepository scheduleRepository, ICinemaRepository cinemaRepository,
            ISeatRepository seatRepository)
        {
            _context = context;
            _seatRepository = seatRepository;
            _scheduleRepository = scheduleRepository;
            _cinemaRepository = cinemaRepository;
        }

        [Route("")]
        public async Task<IActionResult> Index(int? id = null)
        {
            var cinemas = await _cinemaRepository.GetAllAsync();
            var selectedCinema = id.HasValue ? await _cinemaRepository.GetByIdAsync(id.Value)
                : (cinemas.Any() ? cinemas.ElementAt(0) : null);
            if (selectedCinema == null)
            {
                return NotFound();
            }

            var viewModel = new CinemasViewModel
            {
                SelectedCinema = selectedCinema,
                Cinemas = cinemas
            };

            return View(viewModel);
        }


        // GET: Admin/Cinemas/Details/5
        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _cinemaRepository.GetByIdAsync(id.Value);
            if (cinema == null)
            {
                return NotFound();
            }

            return View(cinema);
        }
        [HttpGet]
        [Route("lay-thong-tin/{id}")]
        public async Task<IActionResult> GetCinemaInfo(int id)
        {
            try
            {
                Cinema cinema = await _cinemaRepository.GetByIdAsync(id);
                if (cinema == null)
                {
                    return NotFound(new { message = "Không tìm thấy rạp chiếu" });
                }

                return Json(new
                {
                    id = cinema.Id,
                    name = cinema.CinemaName,
                    address = cinema.CinemaAddress,
                    PhoneNumber = cinema.CinemaPhoneNumber,
                    map = cinema.Map
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ", error = ex.Message });
            }
        }
        // GET: Admin/Cinemas/Create
        [Route("tao-moi")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("tao-moi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                await _cinemaRepository.AddAsync(cinema);
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }

        // GET: Admin/Cinemas/Edit/5
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _cinemaRepository.GetByIdAsync(id.Value);
            if (cinema == null)
            {
                return NotFound();
            }
            return View(cinema);
        }

        [Route("chinh-sua/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cinema cinema)
        {
            if (id != cinema.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentCinema = await _cinemaRepository.GetByIdAsync(id);
                    currentCinema.CinemaAddress = cinema.CinemaAddress;
                    currentCinema.Map = cinema.Map;
                    currentCinema.CinemaName = cinema.CinemaName;
                    currentCinema.CinemaPhoneNumber = cinema.CinemaPhoneNumber;
                    await _cinemaRepository.UpdateAsync(currentCinema);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CinemaExists(cinema.Id))
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
            return View(cinema);
        }

        // GET: Admin/Cinemas/Delete/5
        [Route("xoa/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _cinemaRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CinemaExists(int id)
        {
            return _context.Cinemas.Any(e => e.Id == id);
        }
    }
}
