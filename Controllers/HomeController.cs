using Microsoft.AspNetCore.Mvc;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.ViewModel;
using System.Diagnostics;

namespace school_major_project.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IFilmRepository _filmRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        public HomeController(IFilmRepository ifilm,ApplicationDbContext applicationDbContext,ICountryRepository country, 
            ICategoryRepository categoryRepository) : base(applicationDbContext)
        {
            _countryRepository = country;
            _categoryRepository = categoryRepository;
            _applicationDbContext = applicationDbContext;           
            _filmRepository = ifilm;
        }

        public async Task<IActionResult> Index(int page =1,int pageSize = 6)
        {
            var films = await _filmRepository.GetAllAsync();
            var totalFilms = films.Count();
            var countries = await _countryRepository.GetAllAsync();
            var paging = _applicationDbContext.Films.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            FilmPagingViewModel viewModel = new FilmPagingViewModel
            {
                Films = paging,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalFilms / pageSize),
                Countries = countries
            };

            ViewBag.HeaderCategories = _categoryRepository.GetAllAsync();
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
