using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.HelperClass;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.Services;
using school_major_project.ViewModel;

namespace school_major_project.Controllers
{
    
    public class FilmsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IFilmRepository _filmRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly UserManager<User> _userManager;
        public FilmsController(ApplicationDbContext context, IFilmRepository filmRepository, ICategoryRepository categoryRepository,
            ICountryRepository countryRepository, IRatingRepository ratingRepository, UserManager<User> userManager) : base(context)
        {
            _context = context;
            _filmRepository = filmRepository;
            _categoryRepository = categoryRepository;
            _countryRepository = countryRepository;
            _ratingRepository = ratingRepository;
            _userManager = userManager;
        }

        // GET: Films
        [Route("/tat-ca-phim/")]
        [Route("/tat-ca-phim/trang-{page}")]
        public async Task<IActionResult> AllFilms(int? page = 1, int pageSize = 6)
        {
            var films = await _filmRepository.GetAllAsync();
            var totalFilms = films.Count();
            var countries = await _countryRepository.GetAllAsync();
            var filmspaging = _context.Films.Include(p => p.Categories).Skip(((page ?? 1) - 1) * pageSize).Take(pageSize).ToList();
            FilmPagingViewModel viewModel = new FilmPagingViewModel
            {
                Films = filmspaging,
                CurrentPage = page ?? 1,
                TotalPages = (int)Math.Ceiling((double)totalFilms / pageSize),
                Countries = countries
            };
            return View(viewModel);
        }

        [Route("/phim-theo-quoc-gia-{id}/trang-{page}")]
        public async Task<IActionResult> FilmsByCountry(int id, int? page = 1, int pageSize = 6)
        {
            var films = await _filmRepository.GetFilmsByCountryAsync(id);
            var totalFilms = films.Count();
            var countries = await _countryRepository.GetAllAsync();
            var filmsPaging = films.Skip(((page ?? 1) - 1) * pageSize).Take(pageSize).ToList();
            FilmPagingViewModel viewModel = new FilmPagingViewModel
            {
                Films = filmsPaging,
                CurrentPage = page ?? 1,
                TotalPages = (int)Math.Ceiling((double)totalFilms / pageSize),
                Countries = countries
            };
            Country country = await _countryRepository.GetByIdAsync(id);
            ViewBag.CountryName = country.Name;
            ViewBag.CountryId = country.Id;
            ViewBag.Quantity = totalFilms;

            return View(viewModel);
        }

        [Route("/chi-tiet-phim/{name}")]
        public async Task<IActionResult> Details(string name)
        {
            Console.WriteLine(name);
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            var allFilms = await _filmRepository.GetAllAsync();

            var film = allFilms.FirstOrDefault(f =>
                            f.Name.RemoveDiacritics().Equals(name, StringComparison.OrdinalIgnoreCase));

            if (film == null)
                return NotFound();

            List<string> actors = await _filmRepository.GetActorsListByFilmId(film.Id);

            double avg = await _context.Ratings
                   .Where(r => r.FilmId == film.Id)
                   .Select(r => r.Star)
                   .DefaultIfEmpty()
                   .AverageAsync();

            var currentUser = await _userManager.GetUserAsync(User);
            var hasRated = currentUser != null &&
                           await _ratingRepository.HasUserRated(currentUser.Id, film.Id);

            var ratings = await _context.Ratings
                .Where(r => r.FilmId == film.Id)
                .Include(r => r.User)
                .OrderByDescending(r => r.RatingDate)
                .ToListAsync();

            ViewBag.HasRated = hasRated;
            FilmDetailVM viewmodel = new FilmDetailVM
            {
                Film = film,
                AllCategories = film.Categories.ToList(),
                AllRatings = ratings,
                ListOfActors = actors,
                averageRating = avg,
                numberOfRating = film.Rating.Count(),
            };

            return View(viewmodel);
        }

        [Route("/tim-kiem-phim/tu-khoa={searchname}")]
        [Route("/tim-kiem-phim/tu-khoa={searchname}/trang-{page}")]
        public async Task<IActionResult> SearchByName(string searchname, int? page = 1, int pageSize = 6)
        {
            var films = await _filmRepository.GetAllAsync();
            var countries = await _countryRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchname))
            {
                string normalizedName =  UrlHelper.RemoveDiacritics(searchname).ToLower();
                films = films.Where(f => UrlHelper.RemoveDiacritics(f.Name).ToLower().Contains(normalizedName)).ToList();
            }
            var filmspaging = films.Skip(((page ?? 1) - 1) * pageSize).Take(pageSize).ToList();
            var totalFilms = films.Count();
            var filmVM = new FilmVM
            {
                Films = filmspaging,
                Categories = categories,
                Countries = countries,
                CurrentPage = page ?? 1,

                TotalPages = (int)Math.Ceiling((double)totalFilms / pageSize),
            };
            ViewBag.KeyWord = searchname;
            ViewBag.Quantity = films.Count();
            return View(filmVM);
        }
        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }

        [Route("/phim-theo-the-loai/{name}/trang-{page}")]
        public async Task<IActionResult> GetFilmsByCategory(string name, int? page = 1, int pageSize = 6)
        {
            var countries = await _countryRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            var category = categories.FirstOrDefault(c => c.CategoryDescription.RemoveDiacritics().Equals(name, StringComparison.OrdinalIgnoreCase));
            var films = await _filmRepository.GetFilmsByCategoryAsync(category.Id);

            var totalFilms = films.Count();
            var filmsPaging = films.Skip(((page ?? 1) - 1) * pageSize).Take(pageSize).ToList();
            FilmPagingViewModel viewModel = new FilmPagingViewModel
            {
                Films = filmsPaging,
                CurrentPage = page ?? 1,
                TotalPages = (int)Math.Ceiling((double)totalFilms / pageSize),

                Countries = countries
            };
            //Category category =category;
            ViewBag.CategoryName = category.CategoryDescription;
            //ViewBag.CategoryId = category.Id;
            ViewBag.Quantity = totalFilms;
            return View(viewModel);
        }
    }
}
