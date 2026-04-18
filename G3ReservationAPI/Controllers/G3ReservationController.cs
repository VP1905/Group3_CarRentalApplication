using G3ReservationAPI.Data;
using G3ReservationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G3ReservationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class G3ReservationController : ControllerBase
    {
        private readonly G3ReservationDbContext _context;

        public G3ReservationController(G3ReservationDbContext context)
        {
            _context = context;
        }

        // GET: api/G3Reservation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<G3Reservation>>> GetAllReservations()
        {
            return await _context.Reservations.ToListAsync();
        }

        // GET: api/G3Reservation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<G3Reservation>> GetReservationById(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();
            return reservation;
        }

        // GET: api/G3Reservation/customer/5
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<G3Reservation>>> GetReservationsByCustomer(int customerId)
        {
            return await _context.Reservations
                .Where(r => r.CustomerId == customerId)
                .ToListAsync();
        }

        // POST: api/G3Reservation
        [HttpPost]
        public async Task<ActionResult<G3Reservation>> CreateReservation(G3Reservation reservation)
        {
            // Check if vehicle is already reserved
            bool isReserved = await _context.Reservations
                .AnyAsync(r => r.VehicleId == reservation.VehicleId && r.Status == "Active");

            if (isReserved)
                return BadRequest("Vehicle is already reserved.");

            reservation.CreatedAt = DateTime.UtcNow;
            reservation.Status = "Active";

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }

        // PUT: api/G3Reservation/5/cancel
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            reservation.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return Ok(reservation);
        }
    }
}