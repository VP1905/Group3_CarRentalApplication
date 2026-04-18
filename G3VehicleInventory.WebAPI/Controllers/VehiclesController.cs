using G3VehicleInventory.Application.DTOs;
using G3VehicleInventory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace G3VehicleInventory.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);

            if (vehicle == null)
                return NotFound(new { message = $"Vehicle with ID {id} was not found." });

            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdVehicle = await _vehicleService.CreateVehicleAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = createdVehicle.Id }, createdVehicle);
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateVehicleStatusDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingVehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (existingVehicle == null)
                return NotFound(new { message = $"Vehicle with ID {id} was not found." });

            await _vehicleService.UpdateVehicleStatusAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingVehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (existingVehicle == null)
                return NotFound(new { message = $"Vehicle with ID {id} was not found." });

            await _vehicleService.DeleteVehicleAsync(id);

            return NoContent();
        }
    }
}
