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

        public G3MaintenanceController(GR3MaintenanceDbContext context)
        {
            _context = context;
        }

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

            var entity = new GR3RepairHistory
            {
                VehicleId = repair.VehicleId,
                RepairDate = DateTime.Now,
                Description = repair.Description,
                Cost = repair.Cost,
                PerformedBy = repair.PerformedBy
            };

            _context.RepairHistories.Add(entity);
            await _context.SaveChangesAsync();

            repair.Id = entity.Id;
            repair.RepairDate = entity.RepairDate;

            return CreatedAtAction(
                nameof(GetRepairHistory),
                new { vehicleId = entity.VehicleId },
                repair
            );
        }
    }
}