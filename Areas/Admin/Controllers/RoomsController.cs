using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_major_project.Areas.Admin.Data;
using school_major_project.DataAccess;
using school_major_project.DTO;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/phong-chieu")]
    [Authorize(Roles = "Admin")]

    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoomRepository _roomRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ISeatRepository _seatRepository;
        public RoomsController(ApplicationDbContext context, IRoomRepository roomRepository, ICinemaRepository cinemaRepository,
            IScheduleRepository scheduleRepository, ISeatRepository seatRepository)
        {
            _context = context;
            _roomRepository = roomRepository;
            _cinemaRepository = cinemaRepository;
            _scheduleRepository = scheduleRepository;
            _seatRepository = seatRepository;
        }

        // GET: Admin/Rooms
        [Route("")]
        public async Task<IActionResult> Index(string sortOrder)
        {
            var rooms = await _roomRepository.GetAllAsync();

            rooms = sortOrder switch
            {
                "id_asc" => rooms.OrderBy(r => r.Cinema.Id).ToList(),
                "id_desc" => rooms.OrderByDescending(r => r.Cinema.Id).ToList(),
                _ => rooms
            };
            ViewBag.SortOrder = sortOrder;
            return View(rooms);
        }


        // GET: Admin/Rooms/Details/5
        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int id, [FromQuery] int? scheduleId)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            var viewModel = new RoomDetailVM
            {
                Room = room,
                RoomName = room.Name,
                Seats = Enumerable.Empty<SeatDTO>()
            };

            // 3. Lấy danh sách lịch chiếu cho phòng này
            var schedules = await _scheduleRepository.GetSchedulesByRoomId(room.Id);
            viewModel.Schedules = schedules?.OrderBy(s => s.ScheduleTime).ToList() ?? new List<Schedule>();

            if (!viewModel.Schedules.Any())
            {
                viewModel.HasNoSchedules = true;

                viewModel.Seats = await _seatRepository.GetSeatsByRoomId(room.Id);
                viewModel.SelectedScheduleId = null;
            }

            else
            {
                viewModel.HasNoSchedules = false;


                int? targetScheduleId = scheduleId;

                if (targetScheduleId == null)
                {
                    targetScheduleId = viewModel.Schedules.First().Id;
                }

                viewModel.SelectedScheduleId = targetScheduleId;

                if (targetScheduleId.HasValue && targetScheduleId.Value == 0)
                {
                    viewModel.Seats = await _seatRepository.GetSeatsByRoomId(room.Id);
                }
                else if (targetScheduleId.HasValue)
                {

                    viewModel.Seats = await _seatRepository.GetSeatsByScheduleIdAndRoomId(room.Id, targetScheduleId.Value);
                }
            }


            viewModel.Seats ??= Enumerable.Empty<SeatDTO>();

            return View("Details", viewModel);
        }


        private void CreateSeatsForRoom(Room room)
        {
            List<Seat> seats = new List<Seat>();

            // Lấy các loại ghế từ database
            var seatTypeMap = _context.SeatTypes.ToDictionary(st => st.TypeDescription.ToLower(), st => st);

            // Tạo ghế Regular (Hàng A-D)
            if (seatTypeMap.TryGetValue("regular".ToLower(), out SeatType? regularType))
            {
                string[] regularRows = { "A", "B", "C", "D" };
                foreach (var row in regularRows)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        seats.Add(CreateSeat(room, regularType, $"{row}{i}", false));
                    }
                }
            }

            // Tạo ghế VIP (Hàng E-H)
            if (seatTypeMap.TryGetValue("VIP".ToLower(), out SeatType? vipType))
            {
                string[] vipRows = { "E", "F", "G", "H" };
                foreach (var row in vipRows)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        seats.Add(CreateSeat(room, vipType, $"{row}{i}", false));
                    }
                }
            }

            // Tạo ghế Couple (Hàng J)
            if (seatTypeMap.TryGetValue("couple".ToLower(), out SeatType? coupleType))
            {
                for (int i = 1; i <= 12; i += 2)
                {
                    string seatSymbol = $"J{i}J{i + 1}"; // Ví dụ: J1J2, J3J4, ...
                    seats.Add(CreateSeat(room, coupleType, seatSymbol, false));
                }
            }

            // Lưu tất cả ghế vào database
            _context.Seats.AddRange(seats);
            _context.SaveChanges();
        }

        private Seat CreateSeat(Room room, SeatType seatType, string symbol, bool status)
        {
            string imagePath = "";
            switch (seatType.TypeDescription.ToLower())
            {
                case "regular":
                    imagePath = seatType.ImageDescription;
                    break;
                case "vip":
                    imagePath = seatType.ImageDescription;
                    break;
                case "couple":
                    imagePath = seatType.ImageDescription;
                    break;
            }

            return new Seat
            {
                RoomId = room.Id,
                SeatTypeId = seatType.Id,
                SeatNumber = symbol,
                Status = status,
                SeatImage = imagePath,
                SeatType = seatType,
                Room = room

            };
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("tao-moi/{id}")]
        public async Task<IActionResult> Create(int id, string roomName = null)
        {

            var cinema = await _cinemaRepository.GetByIdAsync(id);

            if (cinema == null)
            {
                return Json(new { success = false, message = "Rạp phim không tồn tại." });
            }

            string finalRoomName = roomName;
            if (string.IsNullOrWhiteSpace(finalRoomName))
            {
                int roomCount = await _context.Rooms.CountAsync(r => r.CinemaId == id);
                finalRoomName = $"Phòng {roomCount + 1}";
            }


            var room = new Room
            {
                CinemaId = id,
                Name = finalRoomName,
                Description = "Phòng mới tạo",
            };

            try
            {
                _context.Add(room);
                await _context.SaveChangesAsync();

                CreateSeatsForRoom(room);

                return Json(new
                {
                    success = true,
                    message = $"Đã tạo phòng '{room.Name}' thành công!",
                    room = new
                    {
                        id = room.Id,
                        name = room.Name,

                        cinemaLocation = cinema.CinemaAddress,
                        cinemaMap = cinema.Map,
                        cinemaPhone = cinema.CinemaPhoneNumber,
                        detailsUrl = Url.Action("Details", "Rooms", new { area = "Admin", id = room.Id }),
                        editUrl = Url.Action("Edit", "Rooms", new { area = "Admin", id = room.Id })
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating room: {ex.Message}");

                return Json(new { success = false, message = "Có lỗi xảy ra khi tạo phòng hoặc tạo ghế: " + ex.Message });
            }
        }

        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {

            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Admin/Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id, Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Room currentRoom = await _roomRepository.GetByIdAsync(id);
                    currentRoom.Name = room.Name;
                    currentRoom.Description = room.Description;
                    await _roomRepository.UpdateAsync(currentRoom);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Location", room.CinemaId);
            return View(room);
        }

        [Route("xoa/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _roomRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
