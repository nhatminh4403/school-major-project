using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.ViewModel;
using System.Diagnostics;
using System.Net.Mail;
namespace school_major_project.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IFilmRepository _filmRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        public HomeController(IFilmRepository ifilm,ApplicationDbContext applicationDbContext,ICountryRepository country, IEmailService emailService,
            UserManager<User> userManager,
            ICategoryRepository categoryRepository) : base(applicationDbContext)
        {
            _countryRepository = country;
            _categoryRepository = categoryRepository;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _filmRepository = ifilm;
            _emailService = emailService;
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
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }

            }
            ViewBag.HeaderCategories = _categoryRepository.GetAllAsync();
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Route("/ve-chung-toi")]
        public IActionResult About()
        {
            return View();
        }

        [Route("lien-he")]
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _emailService.SendEmailAsync(
                    "nhatminh4403@gmail.com",
                    $"Contact form: {model.Subject}",
                    $"Name: {model.Name}<br>Email: {model.Email}<br>Message: {model.Message}",
                    true);

                return RedirectToAction("ThankYou");
            }

            return View(model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
