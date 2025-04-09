using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/thuc-an")]
    public class FoodsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFoodRepository _foodRepository;
        public FoodsController(ApplicationDbContext context, IFoodRepository foodRepository)
        {
            _context = context;
            _foodRepository = foodRepository;
        }

        // GET: Admin/Foods
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var foods = await _foodRepository.GetAllAsync();
            return View(foods);
        }

        // GET: Admin/Foods/Details/5
        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _foodRepository.GetByIdAsync(id.Value);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/admin/images/foods", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn     
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/admin/images/foods/" + image.FileName; // Trả về đường dẫn tương đối
        }
        // GET: Admin/Foods/Create
        [Route("tao-moi")]
        public IActionResult Create()
        {
            return View();
        }

        // Update the Create method to use the instance of SaveImage
        [HttpPost]
        [Route("tao-moi")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Food food, IFormFile Poster)
        {
            if (ModelState.IsValid)
            {
                if (Poster != null)
                {
                    food.Poster = await SaveImage(Poster);
                }
                else
                {
                    ModelState.Remove("Poster"); // Bỏ qua validation cho thuộc tính này
                }
                await _foodRepository.AddAsync(food);
                return RedirectToAction(nameof(Index));
            }
            return View(food);
        }

        // GET: Admin/Foods/Edit/5
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {

            var food = await _foodRepository.GetByIdAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        // POST: Admin/Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Food food, IFormFile Poster)
        {
            if (id != food.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentFood = await _foodRepository.GetByIdAsync(id);

                    if (Poster == null)
                    {
                        food.Poster = currentFood.Poster;
                    }
                    else
                    {
                        food.Poster = await SaveImage(Poster);
                    }
                    currentFood.Price = food.Price;
                    currentFood.ComboName = food.ComboName;
                    currentFood.Description = food.Description;
                    await _foodRepository.UpdateAsync(food);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.Id))
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
            return View(food);
        }

        // GET: Admin/Foods/Delete/
        [Route("xoa/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _foodRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }
    }
}
