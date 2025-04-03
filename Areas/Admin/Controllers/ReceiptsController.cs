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
    [Route("admin/hoa-don")]
    public class ReceiptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceiptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Receipts
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Receipts.Include(r => r.GetFood).Include(r => r.GetUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Receipts/Details/5
        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipts
                .Include(r => r.GetFood)
                .Include(r => r.GetUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }

        // GET: Admin/Receipts/Create
        [Route("tao-moi")]
        public IActionResult Create()
        {
            ViewData["ComboFoodId"] = new SelectList(_context.Foods, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Admin/Receipts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,TotalPrice,paymentType,seatName,ComboFoodId,UserId")] Receipt receipt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receipt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ComboFoodId"] = new SelectList(_context.Foods, "Id", "Id", receipt.ComboFoodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", receipt.UserId);
            return View(receipt);
        }

        // GET: Admin/Receipts/Edit/5
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipts.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            ViewData["ComboFoodId"] = new SelectList(_context.Foods, "Id", "Id", receipt.ComboFoodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", receipt.UserId);
            return View(receipt);
        }

        // POST: Admin/Receipts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,TotalPrice,paymentType,seatName,ComboFoodId,UserId")] Receipt receipt)
        {
            if (id != receipt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receipt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiptExists(receipt.Id))
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
            ViewData["ComboFoodId"] = new SelectList(_context.Foods, "Id", "Id", receipt.ComboFoodId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", receipt.UserId);
            return View(receipt);
        }

        // GET: Admin/Receipts/Delete/5
        [Route("xoa/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receipt = await _context.Receipts.FindAsync(id);
            if (receipt != null)
            {
                _context.Receipts.Remove(receipt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiptExists(int id)
        {
            return _context.Receipts.Any(e => e.Id == id);
        }
    }
}
