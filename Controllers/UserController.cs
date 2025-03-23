using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using school_major_project.DataAccess;
using school_major_project.Models;
using school_major_project.ViewModel;
using Tesseract;
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
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        [Route("chinh-sua-tai-khoan")]
        [Authorize]
        public async Task<ActionResult> Edit()
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
                birthday = user.birthday,
                pointSaving = user.pointSaving,
                isStudent = user.isStudent,
                Email = user.Email
            };

            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }
                

                // Kiểm tra nếu email đã thay đổi
                if (model.Email != user.Email)
                {
                    // Kiểm tra xem email mới đã tồn tại chưa
                    var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
                    if (userWithSameEmail != null && userWithSameEmail.Id != user.Id)
                    {
                        ModelState.AddModelError("Email", "Email đã được sử dụng");
                        return View(model);
                    }
                    user.Email = model.Email;
                    user.EmailConfirmed = false; // Yêu cầu xác thực lại email

                    // Có thể gửi email xác thực tại đây
                    // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // await _emailService.SendEmailConfirmationAsync(user.Email, code);
                }

                // Cập nhật số điện thoại
                user.PhoneNumber = model.PhoneNumber;

                // Cập nhật các trường tùy chỉnh khác nếu có

                // Lưu thay đổi vào database
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Cập nhật cookie đăng nhập nếu thông tin đăng nhập thay đổi
                    await _signInManager.RefreshSignInAsync(user);

                    TempData["SuccessMessage"] = "Cập nhật thông tin tài khoản thành công";
                    return RedirectToAction("Index", "Home");
                }

                // Nếu cập nhật không thành công, thêm lỗi vào ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log lỗi
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin người dùng");
                ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin tài khoản");
            }

            return View(model);
        }
        [Route("xoa-tai-khoan")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
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


        [Route("lich-su-dat-ve")]
        public IActionResult History()
        {
            return View();
        }

        

    }
}
