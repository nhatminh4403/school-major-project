using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/khuyen-mai")]
    public class PromotionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPromotionRepository _promotionRepository;
        public PromotionsController(ApplicationDbContext context, IPromotionRepository promotionRepository)
        {
            _context = context;
            _promotionRepository = promotionRepository;
        }

        // GET: Admin/Promotions
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View(await _promotionRepository.GetAllAsync());
        }

        // GET: Admin/Promotions/Details/5
        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {

            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // GET: Admin/Promotions/Create
        [Route("tao-moi")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("tao-moi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                promotion.Code = promotion.Code.ToUpper();
                await _promotionRepository.AddAsync(promotion);
                return RedirectToAction(nameof(Index));
            }
            return View(promotion);
        }

        // GET: Admin/Promotions/Edit/5
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return View(promotion);
        }

        [Route("chinh-sua/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Promotion promotion)
        {
            if (id != promotion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentPromotion = await _promotionRepository.GetByIdAsync(id);
                    currentPromotion.Code = promotion.Code.ToUpper();
                    currentPromotion.Description = promotion.Description;
                    currentPromotion.StartDate = promotion.StartDate;
                    currentPromotion.EndDate = promotion.EndDate;
                    currentPromotion.DiscountRate = promotion.DiscountRate;
                    currentPromotion.RedemptionPoint = promotion.RedemptionPoint;

                    await _promotionRepository.UpdateAsync(currentPromotion);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionExists(promotion.Id))
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
            return View(promotion);
        }

        [Route("xoa/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _promotionRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionExists(int id)
        {
            return _context.Promotions.Any(e => e.Id == id);
        }
    }
}
