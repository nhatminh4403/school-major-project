using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_major_project.Areas.Admin.Data;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;
namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/lich-chieu")]
    [Authorize(Roles = "Admin")]

    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ICinemaRepository _cinemaRepository;
        public SchedulesController(ApplicationDbContext context, IFilmRepository filmRepository,
            IScheduleRepository scheduleRepository, IRoomRepository roomRepository, ICinemaRepository cinemaRepository)
        {
            _context = context;
            _filmRepository = filmRepository;
            _scheduleRepository = scheduleRepository;
            _roomRepository = roomRepository;
            _cinemaRepository = cinemaRepository;
        }

        // GET: Admin/Schedules
        [Route("")]
        public async Task<IActionResult> Index()
        {
            DateTime today = DateTime.Today;

            var filmSchedules = await _scheduleRepository.GetAllAsync();
            var films = await _filmRepository.GetAllAsync();
            var GetReleasedFilmsWithoutSchedules = films.Where(film => film.StartTime <= today).Where(film => film.Schedules == null || !film.Schedules.Any()).ToList();
            var upcoming = films.Where(film => film.StartTime > today).ToList();


            var viewModel = new ScheduleVM
            {
                Schedules = filmSchedules,
                UpcomingFilms = upcoming,
                ReleasedFilmsWithoutSchedules = GetReleasedFilmsWithoutSchedules,
            };
            return View(viewModel);
        }


        [Route("tao-moi")]
        public async Task<IActionResult> Create()
        {
            var rooms = await _roomRepository.GetAllAsync();
            var films = await _filmRepository.GetAllAsync();
            var viewModel = new AddingScheduleVM();

            viewModel.RoomList = new SelectList(rooms.Select(r => new
            {
                r.Id,
                DisplayText = $"{(r.Cinema != null ? r.Cinema.CinemaName : "N/A")} - {r.Name}"
            }), "Id", "DisplayText");

            viewModel.FilmList = new SelectList(films, "Id", "Name");

            return View(viewModel);
        }

        [Route("tao-moi-tu-phim/{filmId}")]
        public async Task<IActionResult> CreateFromFilm(int filmId)
        {
            var rooms = await _roomRepository.GetAllAsync();
            var films = await _filmRepository.GetAllAsync();
            var viewModel = new AddingScheduleVM();

            viewModel.RoomList = new SelectList(rooms.Select(r => new
            {
                r.Id,
                DisplayText = $"{(r.Cinema != null ? r.Cinema.CinemaName : "N/A")} - {r.Name}"
            }), "Id", "DisplayText");

            viewModel.FilmList = new SelectList(films, "Id", "Name", filmId);
            viewModel.Schedule.FilmId = filmId;

            return View("Create", viewModel);
        }

        [HttpPost("tao-moi")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddingScheduleVM viewModel)
        {
            ModelState.Remove("Schedule.Film");
            ModelState.Remove("Schedule.Room");
            ModelState.Remove("Schedule.Id");
            if (ModelState.IsValid)
            {
                await _scheduleRepository.AddAsync(viewModel.Schedule);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["Title"] = "Tạo Lịch Chiếu Mới";

                var rooms = await _roomRepository.GetAllAsync();
                var films = await _filmRepository.GetAllAsync();

                viewModel.RoomList = new SelectList(rooms.Select(r => new
                {
                    r.Id,
                    DisplayText = $"{(r.Cinema != null ? r.Cinema.CinemaName : "N/A")} - {r.Name}"
                }), "Id", "DisplayText", viewModel.Schedule.RoomId);

                viewModel.FilmList = new SelectList(films, "Id", "Name", viewModel.Schedule.FilmId);

                TempData["Error"] = "Tạo lịch chiếu thất bại. Vui lòng kiểm tra lại thông tin.";
                return View(viewModel);
            }
        }

        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {


            var schedule = await _scheduleRepository.GetByIdAsync(id);
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
        public async Task<IActionResult> Edit(int id, Schedule schedule)
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
        [HttpPost]
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
