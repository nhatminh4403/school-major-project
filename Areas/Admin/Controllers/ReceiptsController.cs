using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin/hoa-don")]
    public class ReceiptsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IReceiptRepository _receiptRepository;
        public ReceiptsController(ApplicationDbContext context, IReceiptRepository receipt)
        {
            _context = context;
            _receiptRepository = receipt;
        }

        // GET: Admin/Receipts
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var receipts = await _receiptRepository.GetAllAsync();
            return View(receipts);
        }

        // GET: Admin/Receipts/Details/5
        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {


            var receipt = await _receiptRepository.GetByIdAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }

            return Json(new { Receipt = receipt }, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 64 // Increase if needed
            });
        }


    }
}
