using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("phim")]
    public class FilmsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFilmRepository _filmRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly UserManager<User> _userManager;
        public FilmsController(ApplicationDbContext context, IFilmRepository filmRepository, ICategoryRepository categoryRepository, 
            ICountryRepository countryRepository,IRatingRepository ratingRepository,UserManager<User> userManager)
        {
            _context = context;
            _filmRepository = filmRepository;
            _categoryRepository = categoryRepository;
            _countryRepository = countryRepository;
            _ratingRepository = ratingRepository;
            _userManager = userManager;
        }

        // GET: Films
        [Route("phim/tat-ca-phim/trang-{page:int?}")]
        public async Task<IActionResult> AllFilms(int page = 1, int pageSize = 6)
        {
            var films = await _filmRepository.GetAllAsync();
            var totalFilms = films.Count();
            var countries = await _countryRepository.GetAllAsync();
            var filmspaging = _context.Films.Include(p=>p.Categories).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            FilmPagingViewModel viewModel = new FilmPagingViewModel
            {
                Films = filmspaging,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalFilms / pageSize),
                Countries = countries
            };
            return View(viewModel);
        }

        [Route("phim/chi-tiet-phim/{name}")]
        public async Task<IActionResult> Details(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            name = UrlHelper.ToUrlSlug(name);

            var film = await _filmRepository.GetByName(name);
            if (film == null)
            {
                return NotFound();
            }

            List<string> actors = await _filmRepository.GetActorsListByFilmId(film.Id);

            double avg = await _context.Ratings
                   .Where(r => r.FilmId == film.Id)
                   .Select(r => (double?)r.Star) // Chuyển về nullable tránh lỗi
                   .DefaultIfEmpty(0)
                   .AverageAsync() ?? 0;

            var currentUser = await _userManager.GetUserAsync(User);
            var hasRated = currentUser != null &&
                           await _ratingRepository.HasUserRated(currentUser.Id, film.Id);

            ViewBag.HasRated = hasRated;
            FilmDetailVM viewmodel = new FilmDetailVM
            {
                Film = film,
                AllCategories = film.Categories.ToList(),
                AllRatings = film.Rating.ToList(),
                ListOfActors =actors,
                averageRating = Math.Round(avg,1),
                numberOfRating= film.Rating.Count(),
                Rating = new Rating()
            };

            return View(viewmodel);
        }

        

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }
    }
}
