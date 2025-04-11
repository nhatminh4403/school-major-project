using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Controllers
{
    [Route("khuyen-mai")]
    public class PromotionsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IPromotionRepository _promotionRepository;
        public PromotionsController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager,
            IPromotionRepository promotionRepository) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _promotionRepository = promotionRepository;
        }

        // GET: Promotions
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var promotions = await _promotionRepository.GetAllAsync();
            var currentUser = await _userManager.GetUserAsync(User);
            if (_signInManager.IsSignedIn(User))
            {
                var userId = _userManager.GetUserId(User);

          
                currentUser = await _context.Users 
                                         .Include(u => u.Promotions) 
                                         .FirstOrDefaultAsync(u => u.Id == userId);
            }
            ViewBag.CurrentUser = currentUser;
            return View(promotions);
        }

        [HttpPost]
        [Route("doi-khuyen-mai/{id}")]
        public async Task<IActionResult> Redeem(int id)
        {

            var user = await _context.Users
                                 .Include(u => u.Promotions)
                                 .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user == null)
            {
                return Unauthorized(new { message = "Người dùng không được xác thực hoặc không tồn tại." });
            }
            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null)
            {
                return NotFound(new { message = "Khuyến mãi không tồn tại." });
            }
            if (user.Promotions.Contains(promotion))
            {
                return BadRequest(new { message = "Bạn đã quy đổi khuyến mãi này rồi." });
            }
            if (user.PointSaving < promotion.RedemptionPoint)
            {
                return BadRequest(new { message = "Số điểm của bạn không đủ để quy đổi khuyến mãi này." });
            }
            if (user.Promotions == null)
            {
                user.Promotions = new List<Promotion>();
            }
            user.PointSaving -= promotion.RedemptionPoint;
            user.Promotions.Add(promotion);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Xử lý lỗi update nếu cần
                return BadRequest(new { message = "Số điểm của bạn không đủ để quy đổi khuyến mãi này." });

            }
            return Ok(new { message = "Quy đổi khuyến mãi thành công!", newPoints = user.PointSaving });
        }
    }
}
