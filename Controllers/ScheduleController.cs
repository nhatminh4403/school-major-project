using Microsoft.AspNetCore.Mvc;
using school_major_project.DataAccess;
using school_major_project.HelperClass;
using school_major_project.Interfaces;
namespace school_major_project.Controllers
{
    [Route("lich-chieu")]
    public class ScheduleController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly ISeatRepository _seatRepository;

        public ScheduleController(ApplicationDbContext context, IScheduleRepository scheduleRepository,
            IRoomRepository roomRepository, IFilmRepository filmRepository,
            ICinemaRepository cinemaRepository, ISeatRepository seatRepository) : base(context)
        {
            _context = context;
            _scheduleRepository = scheduleRepository;
            _roomRepository = roomRepository;
            _filmRepository = filmRepository;
            _cinemaRepository = cinemaRepository;
            _seatRepository = seatRepository;

        }
        // GET: ScheduleController
        [Route("{name}")]
        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            var allFilms = await _filmRepository.GetAllAsync();

            var film = allFilms.FirstOrDefault(f =>
                            f.Name.RemoveDiacritics().Equals(name, StringComparison.OrdinalIgnoreCase));

            var schedules = await _scheduleRepository.GetSchedulesByFilmId(film.Id);

            TempData["Film"] = film;
            return View(schedules);
        }
    }
}
