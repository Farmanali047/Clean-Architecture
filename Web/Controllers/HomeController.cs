using Microsoft.AspNetCore.Mvc;
using ProjectCarRental.Models;
using System.Diagnostics;
using System;
using Microsoft.AspNetCore.Authorization;
using static Dapper.SqlMapper;
using Application.Services;
using Core.Entites;
using Microsoft.Extensions.Caching.Memory;

namespace ProjectCarRental.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly Bookingform_service _bookingform1;
        private readonly CarRegisteration_Service _carRegisteration;
        private readonly Generic_Service<Insurance> _repositoryin;
        private readonly Generic_Service<Pakage> _repopak;
        private readonly Generic_Service<Bookingform> _repBook;
        private readonly IMemoryCache _memoryCache; 

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, Bookingform_service bookingform1, CarRegisteration_Service carRegisteration,
            Generic_Service<Insurance> repositoryinsur, Generic_Service<Pakage> repopak, Generic_Service<Bookingform> repBook,IMemoryCache memoryCache)
        {
            _logger = logger;
            _env = env;
            _bookingform1 = bookingform1;
            _carRegisteration = carRegisteration;
            _repositoryin = repositoryinsur;
            _repopak = repopak;
            _repBook = repBook;
            _memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index action started.");
            Debug.WriteLine("in the index");
            Trace.WriteLine("the indec miefj");
            try
            {
                if (User.Identity.Name == "Admin@drive.com")
                {
                    _logger.LogInformation("Admin user logged in.");
                    return RedirectToAction("Admin", "Admin");
                }
                else
                {
                    string data = String.Empty;
                    if (HttpContext.Request.Cookies.ContainsKey("first_request"))
                    {
                        string firstVisitedDateTime = HttpContext.Request.Cookies["first_request"];
                        data = "Welcome back " + firstVisitedDateTime;
                        _logger.LogInformation($"Returning user. First visit: {firstVisitedDateTime}");
                    }
                    else
                    {
                        CookieOptions option = new CookieOptions();
                        option.Expires = System.DateTime.Now.AddDays(3);
                        data = "You visited first time";
                        HttpContext.Response.Cookies.Append("first_request", System.DateTime.Now.ToString(), option);
                        _logger.LogInformation("First-time visitor. Cookie set.");
                    }

                    return View("index", data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Index action.");
                return View("Error");
            }
            finally
            {
                _logger.LogInformation("Index action ended.");
            }
        }

        public async Task<IActionResult> About()
        {
            _logger.LogInformation("About action started.");
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            _logger.LogInformation("Contact action started.");
            return View();
        }

        public IActionResult Rental()
        {
            _logger.LogInformation("Rental action started.");
            return View();
        }

        public async Task<IActionResult> Pakage()
        {
            _logger.LogInformation("Pakage action started.");
            try
            {
                const string cacheKey = "PakageData";
                if (!_memoryCache.TryGetValue(cacheKey, out List<Pakage> list))
                {
                    list = await _repopak.GetAllAsync();
                    _logger.LogInformation("Pakage data retrieved from repository.");

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                        SlidingExpiration = TimeSpan.FromMinutes(10)
                    };

                    _memoryCache.Set(cacheKey, list, cacheOptions);
                }
                else
                {
                    _logger.LogInformation("Pakage data retrieved from cache.");
                }

                return View(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Pakage action.");
                return View("Error");
            }
        }


     

        public async Task<IActionResult> Insurance()
        {
            _logger.LogInformation("Insurance action started.");
            try
            {
                const string cacheKey = "InsuranceData";
                if (!_memoryCache.TryGetValue(cacheKey, out List<Insurance> list))
                {
                    list = await _repositoryin.GetAllAsync();
                    _logger.LogInformation("Insurance data retrieved from repository.");

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                        SlidingExpiration = TimeSpan.FromMinutes(10)
                    };

                    _memoryCache.Set(cacheKey, list, cacheOptions);
                }
                else
                {
                    _logger.LogInformation("Insurance data retrieved from cache.");
                }

                return View(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Insurance action.");
                return View("Error");
            }
        }


        public async Task<IActionResult> Available()
        {
            _logger.LogInformation("Available action started.");
            try
            {
                object data = await _carRegisteration.GetAllAsync();
                _logger.LogInformation("Available cars data retrieved.");
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Available action.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Booked()
        {
            _logger.LogInformation("Booked action started.");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Bookingform()
        {
            _logger.LogInformation("Bookingform action started.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Bookingform(Bookingform cr)
        {
            _logger.LogInformation("Bookingform POST action started.");
            try
            {
                await _repBook.AddAsync(cr);
                _logger.LogInformation("Booking form submitted.");
                return View(cr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Bookingform POST action.");
                return View("Error");
            }
        }

        [Authorize]
        public async Task<IActionResult> Removed(int id)
        {
            _logger.LogInformation($"Removed action started for booking ID: {id}.");
            try
            {
                var booking = await _repBook.GetAsync(id);
                _logger.LogInformation($"Booking data retrieved for ID: {id}.");
                return View(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Removed action for ID: {id}.");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Removed(Bookingform car)
        {
            _logger.LogInformation($"Removed POST action started for booking ID: {car.Id}.");
            try
            {
                await _repBook.DeleteAsync(car);
                _logger.LogInformation($"Booking removed for ID: {car.Id}.");
                return RedirectToAction("ViewBookings", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Removed POST action for ID: {car.Id}.");
                return View("Error");
            }
        }

        public async Task<IActionResult> ViewBookings()
        {
            _logger.LogInformation("ViewBookings action started.");
            try
            {
                var data = await _bookingform1.GetAll1Async(User.Identity?.Name);
                _logger.LogInformation("Bookings data retrieved.");
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ViewBookings action.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Totalcars()
        {
            _logger.LogInformation("Totalcars action started.");
            try
            {
                object data = await _carRegisteration.GetAllAsync();
                _logger.LogInformation("Total cars data retrieved.");
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Totalcars action.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Admin()
        {
            _logger.LogInformation("Admin action started.");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            _logger.LogInformation("Error action started.");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
