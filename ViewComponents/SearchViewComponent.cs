using Microsoft.AspNetCore.Mvc;
using school_major_project.Interfaces;

namespace school_major_project.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly ICountryRepository _countryRepository;

        public SearchViewComponent(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // You can do data fetching or processing here
            var countries = await _countryRepository.GetAllAsync();

            return View(countries);
        }
    }
}
