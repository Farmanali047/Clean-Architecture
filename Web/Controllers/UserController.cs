using Application.Services;
using Core.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectCarRental.Controllers
{
    public class UserController : Controller
    {
        private readonly CarRegisteration_Service _carRegisteration;
        private readonly Generic_Service<Bookingform> _repBook;

        public UserController(CarRegisteration_Service carRegisteration, Generic_Service<Bookingform> repBook)
        {
            _carRegisteration = carRegisteration;
            _repBook = repBook;
        }

        public IActionResult Booked()
        {
            return View();
        }

        public async Task<IActionResult> Available()
        {
            var cars = await _carRegisteration.GetAllAsync();
            return View(cars);
        }

        [Authorize]
        public async Task<IActionResult> Bookingform()
        {
            string vcar = Request.Query["car"];
            ViewBag.car = vcar;

            string vcolor = Request.Query["color"];
            ViewBag.color = vcolor;

            string vplate = Request.Query["plate"];
            ViewBag.plate = vplate;

            await _carRegisteration.UpdatestatusAsync(vcar);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Bookingform(Bookingform cr)
        {
            ModelState.Remove(nameof(cr.Email));
            ModelState.Remove(nameof(cr.Status));

            if (ModelState.IsValid)
            {
                cr.Status = "Pending";
                cr.Email = User.Identity?.Name;

                try
                {
                    await _repBook.AddAsync(cr);
                    return Json(new { success = true, message = "Data successfully saved." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Json(new { success = false, message = "An error occurred while saving data." });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Invalid data", errors = errors });
            }
        }

        [Authorize]
        public async Task<IActionResult> Removed(int id)
        {
            var booking = await _repBook.GetAsync(id);
            await _repBook.DeleteAsync(booking);
            return RedirectToAction("ViewBookings", "Home");
        }

        [Authorize]
        public async Task<IActionResult> UpdateBooking(int id)
        {
            var booking = await _repBook.GetAsync(id);
            return View("UpdateBooking", booking);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateBooking(Bookingform obj)
        {
            ModelState.Remove(nameof(obj.Cnic));
            ModelState.Remove(nameof(obj.Status));

            if (ModelState.IsValid)
            {
                obj.Status = "Pending";

                await _repBook.UpdateAsync(obj);
                return Json(new { success = true, message = "Data successfully updated." });
            }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Invalid data", errors = errors });

            
        }

        [HttpPost]
        public async Task<IActionResult> Removed(Bookingform car)
        {
            await _repBook.DeleteAsync(car);
            return RedirectToAction("ViewBookings", "Home");
        }
    }
}
