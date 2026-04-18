using G3MaintenanceAPI.Data;
using G3MaintenanceAPI.DTOs;
using G3MaintenanceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G3MaintenanceAPI.Controllers
{
    [ApiController]
    [Route("api/maintenance")]
    public class G3MaintenanceController : ControllerBase
    {
        private readonly GR3MaintenanceDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public G3MaintenanceController(
            GR3MaintenanceDbContext context,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private string VehicleApiBaseUrl => _configuration["ApiSettings:VehicleApiBaseUrl"]!;

        private async Task<bool> VehicleExistsAsync(int vehicleId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{VehicleApiBaseUrl}api/vehicles/{vehicleId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // GET: api/maintenance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GR3RepairHistoryDto>>> GetAll()
        {
            var records = await _context.RepairHistories
                .Select(r => new GR3RepairHistoryDto
                {
                    Id = r.Id,
                    VehicleId = r.VehicleId,
                    RepairDate = r.RepairDate,
                    Description = r.Description,
                    Cost = r.Cost,
                    PerformedBy = r.PerformedBy
                })
                .ToListAsync();

            return Ok(records);
        }

        // GET: api/maintenance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GR3RepairHistoryDto>> GetById(int id)
        {
            var record = await _context.RepairHistories.FindAsync(id);

            if (record == null)
                return NotFound();

            return Ok(new GR3RepairHistoryDto
            {
                Id = record.Id,
                VehicleId = record.VehicleId,
                RepairDate = record.RepairDate,
                Description = record.Description,
                Cost = record.Cost,
                PerformedBy = record.PerformedBy
            });
        }

        // GET: api/maintenance/vehicles/5/repairs
        [HttpGet("vehicles/{vehicleId}/repairs")]
        public async Task<IActionResult> GetRepairHistory(int vehicleId)
        {
            var history = await _context.RepairHistories
                .Where(r => r.VehicleId == vehicleId)
                .Select(r => new GR3RepairHistoryDto
                {
                    Id = r.Id,
                    VehicleId = r.VehicleId,
                    RepairDate = r.RepairDate,
                    Description = r.Description,
                    Cost = r.Cost,
                    PerformedBy = r.PerformedBy
                })
                .ToListAsync();

            return Ok(history);
        }

        // POST: api/maintenance
        [HttpPost]
        public async Task<ActionResult<GR3RepairHistoryDto>> Create([FromBody] GR3RepairHistoryDto repair)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (repair.VehicleId <= 0)
            {
                return BadRequest(new
                {
                    error = "InvalidParameter",
                    message = "VehicleId must be greater than zero."
                });
            }

            var vehicleExists = await VehicleExistsAsync(repair.VehicleId);
            if (!vehicleExists)
            {
                return BadRequest(new
                {
                    error = "InvalidVehicle",
                    message = $"Vehicle with ID {repair.VehicleId} does not exist."
                });
            }

            var entity = new GR3RepairHistory
            {
                VehicleId = repair.VehicleId,
                RepairDate = repair.RepairDate == default ? DateTime.Now : repair.RepairDate,
                Description = repair.Description,
                Cost = repair.Cost,
                PerformedBy = repair.PerformedBy
            };

            _context.RepairHistories.Add(entity);
            await _context.SaveChangesAsync();

            var result = new GR3RepairHistoryDto
            {
                Id = entity.Id,
                VehicleId = entity.VehicleId,
                RepairDate = entity.RepairDate,
                Description = entity.Description,
                Cost = entity.Cost,
                PerformedBy = entity.PerformedBy
            };

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }

        // PUT: api/maintenance/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GR3RepairHistoryDto repair)
        {
            if (id != repair.Id)
                return BadRequest("ID mismatch.");

            var entity = await _context.RepairHistories.FindAsync(id);

            if (entity == null)
                return NotFound();

            if (repair.VehicleId <= 0)
            {
                return BadRequest(new
                {
                    error = "InvalidParameter",
                    message = "VehicleId must be greater than zero."
                });
            }

            var vehicleExists = await VehicleExistsAsync(repair.VehicleId);
            if (!vehicleExists)
            {
                return BadRequest(new
                {
                    error = "InvalidVehicle",
                    message = $"Vehicle with ID {repair.VehicleId} does not exist."
                });
            }

            entity.VehicleId = repair.VehicleId;
            entity.RepairDate = repair.RepairDate;
            entity.Description = repair.Description;
            entity.Cost = repair.Cost;
            entity.PerformedBy = repair.PerformedBy;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/maintenance/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.RepairHistories.FindAsync(id);

            if (entity == null)
                return NotFound();

            _context.RepairHistories.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/maintenance/repairs
        [HttpPost("repairs")]
        public async Task<IActionResult> AddRepair([FromBody] GR3RepairHistoryDto repair)
        {
            if (repair.VehicleId <= 0)
            {
                return BadRequest(new
                {
                    error = "InvalidParameter",
                    message = "VehicleId must be greater than zero."
                });
            }

            if (string.IsNullOrWhiteSpace(repair.Description))
            {
                return BadRequest(new
                {
                    error = "InvalidParameter",
                    message = "Description must not be empty."
                });
            }

            if (repair.Cost < 0)
            {
                return BadRequest(new
                {
                    error = "InvalidParameter",
                    message = "Cost cannot be negative."
                });
            }

            var vehicleExists = await VehicleExistsAsync(repair.VehicleId);
            if (!vehicleExists)
            {
                return BadRequest(new
                {
                    error = "InvalidVehicle",
                    message = $"Vehicle with ID {repair.VehicleId} does not exist."
                });
            }

            var entity = new GR3RepairHistory
            {
                VehicleId = repair.VehicleId,
                RepairDate = repair.RepairDate == default ? DateTime.Now : repair.RepairDate,
                Description = repair.Description,
                Cost = repair.Cost,
                PerformedBy = repair.PerformedBy
            };

            _context.RepairHistories.Add(entity);
            await _context.SaveChangesAsync();

            repair.Id = entity.Id;
            repair.RepairDate = entity.RepairDate;

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, repair);
        }
    }
}