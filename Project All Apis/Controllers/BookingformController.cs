using Core.Entites;
using Core.Interfaces;
using InfraStructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project_All_Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingformController : ControllerBase
    {
        private readonly IBookingform _bookingformService;
        private readonly IRepository<Bookingform> _bookingformRepository;
        public BookingformController(IBookingform bookingformService, IRepository<Bookingform> bookingformRepository)
        {
            _bookingformService = bookingformService;
            _bookingformRepository = bookingformRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Bookingform bookingform)
        {
            if (bookingform == null)
            {
                return BadRequest("Bookingform cannot be null.");
            }

            await _bookingformService.AddAsync(bookingform);
            return Ok("Bookingform added successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateStatusAsync(int id)
        {
           var v=  await _bookingformRepository.GetAsync(id);
            return Ok(v);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Bookingform bookingform)
        {
            if (bookingform == null)
            {
                return BadRequest("Bookingform is null.");
            }

            // Check if the provided id matches the id in the bookingform object
            if (id != bookingform.Id)
            {
                return BadRequest("Bookingform id does not match the URL id.");
            }

            // Fetch the existing booking form
            var existingBookingForm = await _bookingformRepository.GetAsync(id);
            if (existingBookingForm == null)
            {
                return NotFound("Bookingform not found.");
            }

            // Update the booking form
            await _bookingformRepository.UpdateAsync(bookingform);

            return NoContent();
        }


        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetAll1Async(string email)
        {
            var bookingforms = await _bookingformService.GetAll1Async(email);
            if (bookingforms == null || bookingforms.Count == 0)
            {
                return NotFound("No booking forms found for the given email.");
            }

            return Ok(bookingforms);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var bookingforms = await _bookingformService.GetAllAsync();
            if (bookingforms == null || bookingforms.Count == 0)
            {
                return NotFound("No booking forms found.");
            }

            return Ok(bookingforms);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUserAsync()
        {
            var users = await _bookingformService.GetUserAsync();
            if (users == null || users.Count == 0)
            {
                return NotFound("No users found.");
            }

            return Ok(users);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var bookingforms = await _bookingformService.GetByNameAsync(name);
            if (bookingforms == null || bookingforms.Count == 0)
            {
                return NotFound("No booking forms found with the given name.");
            }

            return Ok(bookingforms);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var carRegisteration = await _bookingformRepository.GetAsync(id);

            if (carRegisteration == null)
            {
                return NotFound("Booking not found.");
            }

            await _bookingformRepository.DeleteAsync(carRegisteration);
            return Ok("Car Booking  deleted successfully.");
        }

    }

}
