using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Controllers
{
    [Route("chon-ghe")]
    [Authorize(Roles = Role.Role_Customer)]
    public class SeatController : BaseController
    {
        private readonly ISeatRepository _repository;
        private readonly ApplicationDbContext _context;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IFilmRepository _filmRepository;

        public SeatController(ISeatRepository repository, ApplicationDbContext context,
            IScheduleRepository scheduleRepository, IRoomRepository roomRepository, IFilmRepository filmRepository) : base(context)
        {
            _repository = repository;
            _context = context;
            _scheduleRepository = scheduleRepository;
            _roomRepository = roomRepository;
            _filmRepository = filmRepository;
        }
        [Route("lich-chieu/{scheduleId}")]
        public async Task<IActionResult> ChooseSeat(int scheduleId)
        {
            var schedule = _scheduleRepository.GetByIdAsync(scheduleId);


            var room = await _roomRepository.GetByScheduleIdAsync(scheduleId);
            return View(room);
        }

        // GET: SeatController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

    }
}
