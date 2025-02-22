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
        [Route("/tat-ca-phim/")]
        [Route("/tat-ca-phim/trang-{page}")]
        public async Task<IActionResult> AllFilms(int? page = 1, int pageSize = 6)
        {
            var films = await _filmRepository.GetAllAsync();
            var totalFilms = films.Count();
            var countries = await _countryRepository.GetAllAsync();
            var filmspaging = _context.Films.Include(p=>p.Categories).Skip(((page ?? 1) - 1) * pageSize).Take(pageSize).ToList();
            FilmPagingViewModel viewModel = new FilmPagingViewModel
            {
                Films = filmspaging,
                CurrentPage = page ?? 1,
                TotalPages = (int)Math.Ceiling((double)totalFilms / pageSize),
                Countries = countries
            };
            return View(viewModel);
        }

        [Route("/chi-tiet-phim/{id}")]
        public async Task<IActionResult> Details(int id)
        {

            var film = await _filmRepository.GetByIdAsync(id);
            if (film == null)
            {
                return NotFound();
            }

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
                ListOfActors =actors,
                averageRating = avg,
                numberOfRating= film.Rating.Count(),
            };

            return View(viewmodel);
        }

        
        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }
    }
}
