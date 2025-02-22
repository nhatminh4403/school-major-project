using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using school_major_project.HelperClass;
using school_major_project.Interfaces;
using school_major_project.Models;
namespace school_major_project.Controllers
{
    public class RatingController : Controller
    {

        private readonly IRatingRepository _ratingRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly UserManager<User> _userManager;
        public RatingController(IRatingRepository ratingRepository, IFilmRepository filmRepository,UserManager<User> userManager)
        {
            _ratingRepository = ratingRepository;
            _userManager = userManager;
            _filmRepository = filmRepository;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RatingByFilm(int filmId, int star, string content)
        {
            var film = await _filmRepository.GetByIdAsync(filmId);
            if (film == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            Rating rating = new Rating
            {
                FilmId = filmId,
                UserId = user.Id,
                Star = star,
                RatingContent = content,
                RatingDate = DateTime.Now
            };

            await _ratingRepository.AddAsync(rating);

            // Get the film name and convert it to slug for redirect
            return RedirectToAction("Details", "Films", new { id = filmId });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
