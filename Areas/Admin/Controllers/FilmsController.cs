using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Admin/Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,PosterUrl,TrailerUrl,DirectorName,Language,FilmRated,FilmDuration,Actors,Quality,StartTime,CountryId")] Film film)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
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
