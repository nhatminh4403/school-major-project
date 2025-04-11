using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using school_major_project.Areas.Admin.Data;
using school_major_project.DataAccess;
using school_major_project.HelperClass;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/nguoi-dung")]
    public class AccountController : Controller
    {
        // GET: UserController

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AccountController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Customer");
            AccountVM accountVM = new AccountVM
            {
                Users = users,
            };
            return View(accountVM);
        }

        // GET: UserController/Details/5
        [Route("chi-tiet-tai-khoan/{name}")]
        public async Task<IActionResult> Details(string name)
        {
            var users = await _userManager.GetUsersInRoleAsync("Customer");
            var user = users.ToList().FirstOrDefault(u => u.FullName.RemoveDiacritics().Equals(name, StringComparison.OrdinalIgnoreCase));
            AccountVM accountVM = new AccountVM
            {
                Users = users,
                User = user,
            };
            return View(accountVM);
        }

        // GET: UserController/Edit/5
        [Route("chinh-sua/{id}")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        
        [Route("chinh-sua/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [Route("xoa/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
