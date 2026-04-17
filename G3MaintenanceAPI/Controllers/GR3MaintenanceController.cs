using G3MaintenanceAPI.Data;
using G3MaintenanceAPI.DTOs;
using G3MaintenanceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G3MaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GR3MaintenanceController : ControllerBase
    {
        private readonly GR3MaintenanceDbContext _context;

        public GR3MaintenanceController(GR3MaintenanceDbContext context)
        {
            _context = context;
        }

        [HttpGet("vehicles/{vehicleId}/repairs")]
        public async Task<ActionResult<IEnumerable<GR3RepairHistoryDto>>> GetRepairsByVehicleId(int vehicleId)
        {
            var repairs = await _context.RepairHistories
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

            return Ok(repairs);
        }

        [HttpGet("repairs")]
        public async Task<ActionResult<IEnumerable<GR3RepairHistoryDto>>> GetAllRepairs()
        {
            var repairs = await _context.RepairHistories
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

            return Ok(repairs);
        }

        [HttpGet("repairs/{id}")]
        public async Task<ActionResult<GR3RepairHistoryDto>> GetRepairById(int id)
        {
            var repair = await _context.RepairHistories.FindAsync(id);

            if (repair == null)
            {
                return NotFound($"Repair record with ID {id} was not found.");
            }

            var repairDto = new GR3RepairHistoryDto
            {
                Id = repair.Id,
                VehicleId = repair.VehicleId,
                RepairDate = repair.RepairDate,
                Description = repair.Description,
                Cost = repair.Cost,
                PerformedBy = repair.PerformedBy
            };

            return Ok(repairDto);
        }

        [HttpPost("repairs")]
        public async Task<ActionResult<GR3RepairHistoryDto>> CreateRepair([FromBody] GR3RepairHistoryDto repairDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var repair = new GR3RepairHistory
            {
                VehicleId = repairDto.VehicleId,
                RepairDate = repairDto.RepairDate,
                Description = repairDto.Description,
                Cost = repairDto.Cost,
                PerformedBy = repairDto.PerformedBy
            };

            _context.RepairHistories.Add(repair);
            await _context.SaveChangesAsync();

            repairDto.Id = repair.Id;

            return CreatedAtAction(nameof(GetRepairById), new { id = repair.Id }, repairDto);
        }
    }
}