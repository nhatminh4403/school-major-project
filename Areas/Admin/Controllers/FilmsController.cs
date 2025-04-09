using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/phim")]
    public class FilmsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFilmRepository _filmRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICategoryRepository _categoryRepository;
        public FilmsController(ApplicationDbContext context, IFilmRepository filmRepository, ICountryRepository countryRepository, ICategoryRepository categoryRepository)
        {
            _context = context;
            _filmRepository = filmRepository;
            _countryRepository = countryRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: Admin/Films
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var films = await _filmRepository.GetAllAsync();
            var countries = await _countryRepository.GetAllAsync();
            ViewBag.Countries = countries;
            return View(films);
        }

        // GET: Admin/Films/Details/5
        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {


            var film = await _filmRepository.GetByIdAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Admin/Films/Create
        [Route("tao-moi")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var countries = await _countryRepository.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.Countries = countries;
            return View();
        }

        [Route("tao-moi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Film film, IFormFile PosterUrl, int[] SelectedCategoryIds)
        {
            

            if (ModelState.IsValid)
            {
                if (PosterUrl != null)
                {
                    if (ValidateImageExtension(PosterUrl.FileName))
                    {
                        if (!ValidatImageSize(PosterUrl, 5242880))
                        {
                            ModelState.AddModelError("PosterUrl", "Image size is too big. The limit is only 5MB");
                            return View(film);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PosterUrl", "Invalid image format for main image. Please upload a jpg, jpeg, jfif, or png file.");
                        return View(film);
                    }
                    film.PosterUrl = await SaveImage(PosterUrl);
                }
                else
                {
                    ModelState.Remove("PosterUrl");
                }
                if (SelectedCategoryIds != null && SelectedCategoryIds.Length > 0)
                {
                    film.Categories = new List<Category>();

                    foreach (var categoryId in SelectedCategoryIds)
                    {
                        var category = await _categoryRepository.GetByIdAsync(categoryId);
                        if (category != null)
                        {
                            film.Categories.Add(category);
                        }
                    }
                }
                await _filmRepository.AddAsync(film);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var countries = await _countryRepository.GetAllAsync();
                var categories = await _categoryRepository.GetAllAsync();
                ViewBag.Categories = categories;
                ViewBag.Countries = countries;
                return View(film);

            }
        }

        // GET: Admin/Films/Edit/5
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var countries = await _countryRepository.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.Countries = countries;

            var film = await _filmRepository.GetByIdAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            ViewBag.SelectedCategoryIds = film.Categories?.Select(c => c.Id).ToArray() ?? Array.Empty<int>();

            return View(film);
        }

        // POST: Admin/Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id, int[] SelectedCategoryIds, Film film,IFormFile PosterUrl)
        {
            ModelState.Remove("PosterUrl");
            if (id != film.Id)
            {
                return NotFound();
            }
            if (SelectedCategoryIds == null || !SelectedCategoryIds.Any())
            {
                ModelState.AddModelError("SelectedCategoryIds", "Bạn phải chọn ít nhất một thể loại.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    Film currentFilm = await _filmRepository.GetByIdAsync(id);
                    if (PosterUrl == null)
                    {
                        film.PosterUrl = currentFilm.PosterUrl;
                    }
                    else 
                    {
                        if (ValidateImageExtension(PosterUrl.FileName))
                        {
                            if (!ValidatImageSize(PosterUrl, 5242880))
                            {
                                ModelState.AddModelError("PosterUrl", "Image size is too big. The limit is only 5MB");
                                return View(film);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("PosterUrl", "Invalid image format for main image. Please upload a jpg, jpeg, jfif, or png file.");
                            return View(film);
                        }
                        film.PosterUrl = await SaveImage(PosterUrl);
                        currentFilm.PosterUrl = film.PosterUrl;
                    }

                    currentFilm.Actors = film.Actors;
                    currentFilm.Name = film.Name;
                    currentFilm.Description = film.Description;
                    currentFilm.TrailerUrl = film.TrailerUrl;
                    currentFilm.DirectorName = film.DirectorName;
                    currentFilm.Language = film.Language;
                    currentFilm.FilmRated = film.FilmRated;
                    currentFilm.FilmDuration = film.FilmDuration;
                    currentFilm.Quality = film.Quality;
                    currentFilm.StartTime = film.StartTime;
                    currentFilm.CountryId = film.CountryId;

                    var selectedCategories = await _categoryRepository.GetByIdsAsync(SelectedCategoryIds); // Assuming you have/create this method

                    currentFilm.Categories.Clear(); 
                    foreach (var category in selectedCategories)
                    {
                        currentFilm.Categories.Add(category); 
                    }
                    await _filmRepository.UpdateAsync(currentFilm);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
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
            var categories = await _categoryRepository.GetAllAsync();
            var countries = await _countryRepository.GetAllAsync();
            ViewBag.Categories = categories;
            ViewBag.Countries = countries;
            return View(film);
        }

        // GET: Admin/Films/Delete/5
        [Route("xoa/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _filmRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }
       
        private bool ValidateImageExtension(string fileName)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".jfif" };
            return allowedExtensions.Contains(Path.GetExtension(fileName).ToLower());
        }
        private bool ValidatImageSize(IFormFile file, long maximumSize)
        {
            return file.Length <= maximumSize;
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var directoryPath = Path.Combine("wwwroot/admin/images/films");

            // Create directory if it doesn't exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var savePath = Path.Combine(directoryPath, image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/admin/images/films/" + image.FileName;
        }
    }
}
