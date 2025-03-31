using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using school_major_project.DataAccess;
using school_major_project.Models;
using school_major_project.ViewModel;
namespace school_major_project.Controllers
{
    [Authorize(Roles = Role.Role_Customer)]
    [Route("tai-khoan")]
    public class UserController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public UserController(ApplicationDbContext context, UserManager<User> userManager, 
            SignInManager<User> signInManager, ILogger<UserController> logger, IWebHostEnvironment environment) : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _environment = environment;
        }


        [Route("chi-tiet-tai-khoan")]
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var userVM = new UserVM
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                age = user.age,
                birthday = user.Birthday,
                isStudent = user.IsStudent,
                Email = user.Email,
                Id = user.Id
            };
            ViewBag.PointSaving = user.PointSaving;
            ViewBag.UserName = user.UserName;
            ViewBag.Promotions = user.Promotions;
            return View(userVM);
        }

        [HttpGet]
        [Route("chinh-sua-tai-khoan")]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return NotFound();
            }

            var userVM = new UserVM
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                age = user.age,
                birthday = user.Birthday,
                isStudent = user.IsStudent,
                Email = user.Email,
                Id = user.Id
            };
            return View(userVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("chinh-sua-tai-khoan")]
        public async Task<IActionResult> Edit(UserVM model)
        {
            if (!ModelState.IsValid)
            {
                // Trả về view với model để giữ lại dữ liệu người dùng đã nhập
                return View(model);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                // Kiểm tra và cập nhật email
                if (model.Email != user.Email)
                {
                    var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
                    if (userWithSameEmail != null && userWithSameEmail.Id != user.Id)
                    {
                        ModelState.AddModelError("Email", "Email đã được sử dụng");
                        return View(model);
                    }
                    user.Email = model.Email;
                    user.EmailConfirmed = false;
                }
                user.Id = model.Id;
                // Cập nhật các trường thông tin
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                user.age = model.age;
                user.Birthday = model.birthday;
                user.IsStudent = model.isStudent;

                // Lưu thay đổi
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["SuccessMessage"] = "Cập nhật thông tin tài khoản thành công";
                    return RedirectToAction("Details", "User");
                }

                // Xử lý lỗi từ UpdateAsync
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin người dùng");
                ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin tài khoản");
                return View(model);
            }
        }
        [Route("xoa-tai-khoan")]
        public ActionResult Delete(int id)
        {
            return View();
        }

/*        [HttpPost]
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

*/
        [Route("lich-su-dat-ve")]
        public IActionResult History()
        {
            return View();
        }

        

    }
}
