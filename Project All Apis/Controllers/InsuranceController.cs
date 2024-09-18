using Application.Services;
using Core.Entites;
using InfraStructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project_All_Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController : ControllerBase
    {
        private readonly Generic_Service<Insurance> _insuranceService;

        public InsuranceController(Generic_Service<Insurance> insuranceService)
        {
            _insuranceService = insuranceService;
        }

        // GET: api/Insurance
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var insurances = await _insuranceService.GetAllAsync();
            if (insurances == null || insurances.Count == 0)
            {
                return NotFound("No insurance records found.");
            }

            return Ok(insurances);
        }

        // GET: api/Insurance/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var insurance = await _insuranceService.GetAsync(id);
            if (insurance == null)
            {
                return NotFound($"No insurance record found with ID {id}.");
            }

            return Ok(insurance);
        }

        // POST: api/Insurance
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Insurance insurance)
        {
            if (insurance == null)
            {
                return BadRequest("Insurance data cannot be null.");
            }

            await _insuranceService.AddAsync(insurance);
            return Ok("Insurance record added successfully.");
        }

        // PUT: api/Insurance/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Insurance insurance)
        {
            if (insurance == null)
            {
                return BadRequest("Insurance data cannot be null.");
            }

            var existingInsurance = await _insuranceService.GetAsync(id);
            if (existingInsurance == null)
            {
                return NotFound($"No insurance record found with ID {id}.");
            }

            await _insuranceService.UpdateAsync(insurance);
            return Ok("Insurance record updated successfully.");
        }

        // DELETE: api/CarRegisteration/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var carRegisteration = await _insuranceService.GetAsync(id);

            if (carRegisteration == null)
            {
                return NotFound("Insurance  not found.");
            }

            await _insuranceService.DeleteAsync(carRegisteration);
            return Ok("Insurance deleted successfully.");
        }
    }
}
