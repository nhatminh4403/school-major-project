using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_major_project.Areas.Admin.Data;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.Services;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/rap-phim")]
    public class CinemasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IScheduleRepository _scheduleRepository;
        public CinemasController(ApplicationDbContext context,IScheduleRepository scheduleRepository,ICinemaRepository cinemaRepository,
            ISeatRepository seatRepository)
        {
            _context = context;
            _seatRepository = seatRepository;
            _scheduleRepository = scheduleRepository;
            _cinemaRepository = cinemaRepository;
        }

        // GET: Admin/Cinemas
        [Route("")] 
        public async Task<IActionResult> Index(int? id = null)
        {
            var cinemas = await _cinemaRepository.GetAllAsync();
            //var selectedCinema = await _cinemaRepository.GetSelectedCinema(id);
            var selectedCinema = id.HasValue ? await _cinemaRepository.GetByIdAsync(id.Value): (cinemas.Any() ? cinemas.ElementAt(0) : null);
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

            var cinema = await _context.Cinemas
                .FirstOrDefaultAsync(m => m.Id == id);
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
                Cinema cinema =await _cinemaRepository.GetByIdAsync(id);
                if (cinema == null)
                {
                    return NotFound(new { message = "Không tìm thấy rạp chiếu" });
                }

                return Json(new
                {
                    id = cinema.Id,
                    name = cinema.Name,
                    address = cinema.Location,
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

        // POST: Admin/Cinemas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location")] Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cinema);
                await _context.SaveChangesAsync();
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

            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return NotFound();
            }
            return View(cinema);
        }

        // POST: Admin/Cinemas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location")] Cinema cinema)
        {
            if (id != cinema.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cinema);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }

            return View(cinema);
        }

        // POST: Admin/Cinemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema != null)
            {
                _context.Cinemas.Remove(cinema);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CinemaExists(int id)
        {
            return _context.Cinemas.Any(e => e.Id == id);
        }
    }
}
