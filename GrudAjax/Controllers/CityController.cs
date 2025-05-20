
using Microsoft.AspNetCore.Mvc;using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrudAjax.Data;
using GrudAjax.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GrudAjax.Controllers
{
    public class CityController : Controller
    {
        private readonly AppDbContext _context;

        public CityController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cities = _context.Cities
                .Include(c => c.Country)
                .ToList();

            return View(cities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            City city = new City();
            ViewBag.Countries = GetCountries();
            return View(city);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(City city)
        {
            _context.Add(city);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            City city = _context.Cities
                .Include(c => c.Country)
                .FirstOrDefault(c => c.Id == Id);

            return View(city);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
          var city = _context.Cities
            .AsNoTracking() // ← ensure fresh data
            .FirstOrDefault(c => c.Id == Id);

          if (city == null)
          {
            return NotFound();
          }

          ViewBag.Countries = GetCountries(); // this uses Country.Id
          return View(city); // model.CityId must match an item in ViewBag.Countries
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(City city)
        {
            _context.Attach(city);
            _context.Entry(city).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            City city = _context.Cities
                .Include(c => c.Country)
                .FirstOrDefault(c => c.Id == Id);

            return View(city);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(City city)
        {
            _context.Attach(city);
            _context.Entry(city).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetCountries()
        {
            var lstCountries = _context.Countries
                .Select(ct => new SelectListItem
                {
                    Value = ct.Id.ToString(),
                    Text = ct.Name
                }).ToList();

            lstCountries.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "----Select Country----"
            });

            return lstCountries;
        }

        private string GetCountryName(int countryId)
        {
          var country = _context.Countries.FirstOrDefault(c => c.Id == countryId);
          return country?.Name ?? "";
        }

        [HttpGet]

        public IActionResult CreateModalForm(int countryId)
        {
          City city = new City();
          city.CountryId = countryId;
          city.CountryName =GetCountryName(countryId);
          return PartialView("_CreateModalForm", city);
        }

        [HttpPost]

        public IActionResult CreateModalForm(City city)
        {
          _context.Add(city);
          _context.SaveChanges();
          return NoContent();
        }

    }
}
