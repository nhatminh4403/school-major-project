using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.Areas.Admin.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/lich-chieu")]
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IFilmRepository _filmRepository;
        public SchedulesController(ApplicationDbContext context, IFilmRepository filmRepository,IScheduleRepository scheduleRepository)
        {
            _context = context;
            _filmRepository = filmRepository;
            _scheduleRepository = scheduleRepository;
        }

        // GET: Admin/Schedules
        [Route("")]
        public async Task<IActionResult> Index()
        {
            DateTime today = DateTime.Today;

            var filmSchedules = await _scheduleRepository.GetAllAsync();
            var films = await _filmRepository.GetAllAsync();
            var GetReleasedFilmsWithoutSchedules =films.Where(film => film.StartTime <= today).Where(film => film.Schedules == null || !film.Schedules.Any()).ToList();
            var upcoming = films.Where(film => film.StartTime > today).ToList();


            var viewModel = new ScheduleVM
            {
                Schedules =filmSchedules,
                UpcomingFilms = upcoming,
                ReleasedFilmsWithoutSchedules = GetReleasedFilmsWithoutSchedules,
            };
            return View(viewModel);
        }

        // GET: Admin/Schedules/Details/5
        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Film)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }


        // GET: Admin/Schedules/Create
        [Route("tao-moi")]
        public IActionResult Create()
        {
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Actors");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description");
            return View();
        }

        // POST: Admin/Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ScheduleTime,FilmId,RoomId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Actors", schedule.FilmId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description", schedule.RoomId);
            return View(schedule);
        }

        // GET: Admin/Schedules/Edit/5
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Actors", schedule.FilmId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description", schedule.RoomId);
            return View(schedule);
        }

        // POST: Admin/Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ScheduleTime,FilmId,RoomId")] Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Actors", schedule.FilmId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description", schedule.RoomId);
            return View(schedule);
        }

        // GET: Admin/Schedules/Delete/5
        [Route("xoa/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Film)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Admin/Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
