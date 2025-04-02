using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area(Role.Role_Admin)]
    [Authorize(Roles = Role.Role_Admin)]

    public class HomeController : Controller
    {
        // GET: HomeController
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IReceiptRepository _receiptRepository;

        public HomeController(ApplicationDbContext context, UserManager<User> userManager,IReceiptRepository receiptRepository)
        {
            _context = context;
            _userManager = userManager;
            _receiptRepository = receiptRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync(Role.Role_Customer);
            var receipts = await _receiptRepository.GetAllAsync();
            var total = receipts.Sum(p=> p.TotalPrice);
            ViewBag.TotalPrice = total;
            ViewBag.TotalReceiptCount = receipts.Count();
            ViewBag.TotalUserCount = users.Count;
            return View();
        }
    }
}
