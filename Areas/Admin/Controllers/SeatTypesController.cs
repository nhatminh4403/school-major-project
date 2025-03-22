using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/loai-ghe")]
    public class SeatTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeatTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SeatTypes
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.SeatTypes.ToListAsync());
        }

        // GET: Admin/SeatTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seatType = await _context.SeatTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seatType == null)
            {
                return NotFound();
            }

            return View(seatType);
        }

        // GET: Admin/SeatTypes/Create
        [Route("tao-moi")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/SeatTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeDescription,price,pointGiving")] SeatType seatType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seatType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seatType);
        }

        // GET: Admin/SeatTypes/Edit/5
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seatType = await _context.SeatTypes.FindAsync(id);
            if (seatType == null)
            {
                return NotFound();
            }
            return View(seatType);
        }

        // POST: Admin/SeatTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeDescription,price,pointGiving")] SeatType seatType)
        {
            if (id != seatType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seatType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeatTypeExists(seatType.Id))
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
            return View(seatType);
        }

        // GET: Admin/SeatTypes/Delete/5
        [Route("xoa/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seatType = await _context.SeatTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seatType == null)
            {
                return NotFound();
            }

            return View(seatType);
        }

        // POST: Admin/SeatTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seatType = await _context.SeatTypes.FindAsync(id);
            if (seatType != null)
            {
                _context.SeatTypes.Remove(seatType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeatTypeExists(int id)
        {
            return _context.SeatTypes.Any(e => e.Id == id);
        }
    }
}
