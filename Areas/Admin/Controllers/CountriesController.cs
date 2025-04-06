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
    [Route("admin/quoc-gia")]
    public class CountriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICountryRepository _countryRepository;
        public CountriesController(ApplicationDbContext context, ICountryRepository countryRepository)
        {
            _context = context;
            _countryRepository = countryRepository;
        }

        // GET: Admin/Countries
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View(await _countryRepository.GetAllAsync());
        }

        // GET: Admin/Countries/Details/5
        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Admin/Countries/Create
        [Route("tao-moi")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("tao-moi")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                await _countryRepository.AddAsync(country);
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Admin/Countries/Edit/5
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {

            var country = await _countryRepository.GetByIdAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Admin/Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id, Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentCountry = await _countryRepository.GetByIdAsync(id);
                    currentCountry.Name = country.Name;
                    await _countryRepository.UpdateAsync(currentCountry);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
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
            return View(country);
        }

        // GET: Admin/Countries/Delete/5
        [Route("xoa/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _countryRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}
