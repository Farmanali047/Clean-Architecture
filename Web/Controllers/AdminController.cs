using Application.Services;
using Core.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProjectCarRental.Models;
using Web.Models;

namespace ProjectCarRental.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly Bookingform_service _bookingform1;
        private readonly CarRegisteration_Service _carRegisteration1;
        private readonly Generic_Service<Bookingform> _repBook;
        private readonly Generic_Service<CarRegisteration> _repCar;
        private readonly Generic_Service<Insurance> _repInsur;
        private readonly Generic_Service<Pakage> _repPak;
        private readonly Generic_Service<AspNetUsers> _repUser;
        private readonly IHubContext<ChatHub> _hubContext;

        public AdminController(
            ILogger<AdminController> logger,
            IWebHostEnvironment env,
            Bookingform_service bookingform1,
            CarRegisteration_Service carRegisteration1,
            Generic_Service<Bookingform> repBook,
            Generic_Service<CarRegisteration> repCar,
            Generic_Service<Insurance> repInsur,
            Generic_Service<Pakage> repPak,
            Generic_Service<AspNetUsers> repUser, IHubContext<ChatHub> hubContext)

        {
            _logger = logger;
            _env = env;
            _bookingform1 = bookingform1;
            _carRegisteration1 = carRegisteration1;
            _repBook = repBook;
            _repCar = repCar;
            _repInsur = repInsur;
            _repPak = repPak;
            _repUser = repUser;
            _hubContext= hubContext;
        }

        [Route("/Admin/Panel")]
        public IActionResult Admin()
        {
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var car = await _repCar.GetAsync(id);
            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CarRegisteration car)
        {
            await _repCar.DeleteAsync(car);
            return RedirectToAction("AllCars", "Admin");
        }

        public async Task<IActionResult> UpdateRental(int id)
        {
            var car = await _repCar.GetAsync(id);
            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRental(CarRegisteration d)
        {
            await _repCar.UpdateAsync(d);
            return RedirectToAction("AllCars", "Admin");
        }

        public async Task<IActionResult> ViewBookings()
        {
            var users = await _bookingform1.GetUserAsync();
            return View(users);
        }

        public async Task<IActionResult> Available()
        {
            var data = await _carRegisteration1.AvailableAsync();
            return View(data);
        }

        public async Task<IActionResult> AllUserRequest()
        {
            var requests = await _bookingform1.GetAllAsync();
            return View(requests);
        }

        public async Task<IActionResult> Accept(int id)
        {
            var booking = await _repBook.GetAsync(id);
            if (booking != null)
            {
                booking.Status = "Accepted";
                await _repBook.UpdateAsync(booking);
            }
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", booking.CarName);
            return RedirectToAction("AllUserRequest");
        }

        public IActionResult Booked()
        {
            return View();
        }

        public IActionResult Insurance()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insurance(Insurance insurance)
        {
            await _repInsur.AddAsync(insurance);
            return RedirectToAction("Insurance", "Home");
        }

        public IActionResult Pakage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Pakage(Pakage pakage)
        {
            await _repPak.AddAsync(pakage);
            return RedirectToAction("Pakage", "Home");
        }

        public IActionResult Total()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllCars()
        {
            string message = string.Empty;

            if (HttpContext.Request.Cookies.ContainsKey("first-visit"))
            {
                string? data = HttpContext.Request.Cookies["first-visit"];
                message = $"Welcome Back {data}";
            }
            else
            {
                CookieOptions option = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1)
                };

                message = "Welcome! You visited the first time";
                HttpContext.Response.Cookies.Append("first-visit", DateTime.Now.ToString(), option);
            }

            var data1 = await _carRegisteration1.GetAllAsync();

            CarData carData = new CarData
            {
                data = message,
                registrations = (List<CarRegisteration>)data1
            };

            return View(carData);
        }

        public IActionResult CarRegisteration()
        {
            return View();
        }

        public async Task<IActionResult> Search(string personName)
        {
            var bookingforms = await _repBook.GetAllAsync();
            var filteredBooking = bookingforms.Where(p => p.PersonName.Contains(personName, StringComparison.OrdinalIgnoreCase)).ToList();
            return View("_search", filteredBooking);
        }

        public string GetImageUrl(IFormFile myFile)
        {
            if (myFile != null && myFile.Length > 0)
            {
                string folderPath = Path.Combine("Cars", "Booking"); // Relative path from wwwroot
                string fileName = Path.Combine(folderPath, Guid.NewGuid().ToString() + myFile.FileName);
                string wwwRootPath = _env.WebRootPath;
                if (!Directory.Exists(Path.Combine(wwwRootPath, folderPath))) 
                {
                    Directory.CreateDirectory(Path.Combine(wwwRootPath, folderPath)); // Create directory if needed
                }

                string filePath = Path.Combine(wwwRootPath, fileName); // Full path for saving
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    myFile.CopyTo(fileStream);
                }
                return $"\\{fileName}"; 
            }
            return string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> CarRegisteration(CarRegisteration cr, IFormFile carImage)
        {
            if (carImage != null && carImage.Length > 0)
            {
                cr.ImgUrl = GetImageUrl(carImage);
                if (string.IsNullOrEmpty(cr.ImgUrl))
                {
                    return Json(new { success = false, message = "Failed to upload image." });
                }
            }
            else
            {
                return Json(new { success = false, message = "No image file uploaded." });
            }

            await _repCar.AddAsync(cr);
            return Json(new { success = true, message = "Car Registered Successfully." });
        }
    }
}
