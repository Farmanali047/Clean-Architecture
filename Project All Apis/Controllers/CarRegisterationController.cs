using Application.Services;
using Core.Entites;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project_All_Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarRegisterationController : ControllerBase
    {
        private readonly CarRegisteration_Service _carRegisterationService;
        private readonly IRepository<CarRegisteration> _carRegisterationRepository;
        public CarRegisterationController(CarRegisteration_Service carRegisterationService,IRepository<CarRegisteration> repository)
        {
            _carRegisterationService = carRegisterationService;
            _carRegisterationRepository = repository;
        }

        // GET: api/CarRegisteration
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var carRegisterations = await _carRegisterationService.GetAllAsync();
            if (carRegisterations == null || carRegisterations.Count == 0)
            {
                return NotFound("No car registrations found.");
            }

            return Ok(carRegisterations);
        }

        // GET: api/CarRegisteration/available
        [HttpGet("available")]
        public async Task<IActionResult> AvailableAsync()
        {
            var availableCars = await _carRegisterationService.AvailableAsync();
            if (availableCars == null || availableCars.Count == 0)
            {
                return NotFound("No available cars found.");
            }

            return Ok(availableCars);
        }

        // GET: api/CarRegisteration/booked
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CarRegisteration insurance)
        {
            if (insurance == null)
            {
                return BadRequest("Boooking data cannot be null.");
            }

            var existingInsurance = await _carRegisterationRepository.GetAsync(id);
            if (existingInsurance == null)
            {
                return NotFound($"No booking record found with ID {id}.");
            }

            await _carRegisterationRepository.UpdateAsync(insurance);
            return Ok("Booking record updated successfully.");
        }
      

        // POST: api/CarRegisteration
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CarRegisteration carRegisteration)
        {
            if (carRegisteration == null)
            {
                return BadRequest("Car registration cannot be null.");
            }

            // Assuming there is an AddAsync method in the service to handle the addition
            await _carRegisterationRepository.AddAsync(carRegisteration);
            return Ok("Car registration added successfully.");
        }

   
        // DELETE: api/CarRegisteration/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var carRegisteration = await _carRegisterationRepository.GetAsync(id);

            if (carRegisteration == null)
            {
                return NotFound("Car registration not found.");
            }

            await _carRegisterationRepository.DeleteAsync(carRegisteration);
            return Ok("Car registration deleted successfully.");
        }

    }
}
