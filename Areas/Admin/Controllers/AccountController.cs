using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]

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
            var customerAccount = new List<AccountVM>();
            foreach(var user in users)
            {
                var account = new AccountVM
                {
                    User = user,
                    LockoutEnd = user.LockoutEnd
                };
                customerAccount.Add(account);
            }

            return View(customerAccount);
        }

        // GET: UserController/Details/5
        [Route("chi-tiet-tai-khoan/{name}")]
        public async Task<IActionResult> Details(string name)
        {
            var users = await _userManager.GetUsersInRoleAsync("Customer");
            var user = users.ToList().FirstOrDefault(u => u.FullName.RemoveDiacritics().Equals(name, StringComparison.OrdinalIgnoreCase));
            if(user == null)
            {
                return NotFound();
            }
            AccountVM accountVM = new AccountVM
            {
                
                User = user,LockoutEnd = user.LockoutEnd
            };
            return View(accountVM);
        }

        [Route("khoa-tai-khoan")]
        [HttpPost]
        public async Task<IActionResult> LockAccount(string userId, int? days)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            if (days.HasValue)
            {
                if (days.Value == -1)
                {
                    user.LockoutEnd = DateTimeOffset.MaxValue;
                }
                else if (days.Value == 0)  
                {
                    user.LockoutEnd = null; 
                }
                else
                {
                    user.LockoutEnd = DateTimeOffset.UtcNow.AddDays(days.Value);  
                }
            }
            else
            {
                user.LockoutEnd = DateTimeOffset.MaxValue;  
            }

            user.LockoutEnabled = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest("Không thể khóa tài khoản này");
        }
    }
}
