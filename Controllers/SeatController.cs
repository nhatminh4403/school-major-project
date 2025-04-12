using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.ViewModel;

namespace school_major_project.Controllers
{
    [Route("chon-ghe")]
    [Authorize]
    public class SeatController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IReceiptDetailsRepository _receiptDetailsRepository;

        public SeatController(ApplicationDbContext context,IScheduleRepository scheduleRepository, 
            IRoomRepository roomRepository, IFilmRepository filmRepository,ISeatRepository seatRepository,IReceiptDetailsRepository receiptDetailsRepository) : base(context)
        {
            _context = context;
            _scheduleRepository = scheduleRepository;
            _roomRepository = roomRepository;
            _filmRepository = filmRepository;
            _seatRepository = seatRepository;
            _receiptDetailsRepository = receiptDetailsRepository;
        }
        [Route("lich-chieu/{scheduleId}")]
        public async Task<IActionResult> ChooseSeat([FromRoute] int scheduleId)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);

            if(schedule == null)
            {
                return NotFound();
            }


            var room = await _roomRepository.GetByScheduleIdAsync(scheduleId);
            var seats = await _seatRepository.GetByRoomIdAsync(room.Id);
            var film = await _filmRepository.GetByIdAsync(schedule.FilmId);

            var receiptDetails = await _receiptDetailsRepository.FindByScheduleId(scheduleId);
            var bookedSeatIdsForSchedule = new HashSet<int>(receiptDetails.Select(bd => bd.Seat.SeatId));

            foreach (var seat in seats)
            {
                seat.Status = bookedSeatIdsForSchedule.Contains(seat.SeatId);
            }


            var seatsByType = seats
               .Where(seat => seat.SeatType != null && !string.IsNullOrEmpty(seat.SeatType.TypeDescription))
               .GroupBy(seat => seat.SeatType.TypeDescription.ToLower())
               .ToDictionary(group => group.Key, group => group.ToList());

            Console.WriteLine("Seats by type:");
            foreach (var seatType in seatsByType)
            {
                Console.WriteLine($"Type: {seatType.Key}, Count: {seatType.Value.Count}");
            }
            var viewModel = new SeatListVM
            {
                
                Schedule = schedule,
                SeatsWithStatus = seats.ToList(),
                SeatsByType = seatsByType,
                //Categories = film.Categories.ToList(),
                CurrentTime = DateTime.Now.ToString("HH:mm:ss"),
                Film = film,
                Room = room,
                Cinema = room.Cinema,
                CinemaAddress = room.Cinema.Location,
                CinemaName = room.Cinema.Name,
                RoomName = room.Name,
                SelectedScheduleId = scheduleId,
                SelectedRoomId = room.Id
                
            };

            return View(viewModel);
        }
    }
}
