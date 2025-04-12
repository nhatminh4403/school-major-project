using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/ghe-ngoi")]
    [Authorize(Roles = "Admin")]

    public class SeatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISeatRepository _seatRepository;
        public SeatsController(ApplicationDbContext context, ISeatRepository seatRepository)
        {
            _context = context;
            _seatRepository = seatRepository;
        }

        // GET: Admin/Seats
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Seats.Include(s => s.Room).Include(s => s.SeatType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Seats/Details/5
        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seats
                .Include(s => s.Room)
                .Include(s => s.SeatType)
                .FirstOrDefaultAsync(m => m.SeatId == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // GET: Admin/Seats/Create
        [Route("tao-moi")]
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description");
            ViewData["SeatTypeId"] = new SelectList(_context.SeatTypes, "Id", "TypeDescription");
            return View();
        }

        // POST: Admin/Seats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeatId,SeatNumber,SeatPrice,Status,SeatImage,RoomId,SeatTypeId")] Seat seat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description", seat.RoomId);
            ViewData["SeatTypeId"] = new SelectList(_context.SeatTypes, "Id", "TypeDescription", seat.SeatTypeId);
            return View(seat);
        }

        // GET: Admin/Seats/Edit/5
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description", seat.RoomId);
            ViewData["SeatTypeId"] = new SelectList(_context.SeatTypes, "Id", "TypeDescription", seat.SeatTypeId);
            return View(seat);
        }

        // POST: Admin/Seats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeatId,SeatNumber,SeatPrice,Status,SeatImage,RoomId,SeatTypeId")] Seat seat)
        {
            if (id != seat.SeatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeatExists(seat.SeatId))
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description", seat.RoomId);
            ViewData["SeatTypeId"] = new SelectList(_context.SeatTypes, "Id", "TypeDescription", seat.SeatTypeId);
            return View(seat);
        }

        // GET: Admin/Seats/Delete/5
        [Route("xoa/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _seatRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool SeatExists(int id)
        {
            return _context.Seats.Any(e => e.SeatId == id);
        }
    }
}
