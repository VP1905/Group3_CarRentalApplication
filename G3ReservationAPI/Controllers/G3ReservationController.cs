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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public G3ReservationController(
            G3ReservationDbContext context,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<G3Reservation>>> GetAllReservations()
        {
            return await _context.Reservations.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<G3Reservation>> GetReservationById(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            return reservation;
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<G3Reservation>>> GetReservationsByCustomer(int customerId)
        {
            return await _context.Reservations
                .Where(r => r.CustomerId == customerId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<G3Reservation>> CreateReservation(G3Reservation reservation)
        {
            if (reservation.CustomerId <= 0)
                return BadRequest("Invalid customer ID.");

            if (reservation.VehicleId <= 0)
                return BadRequest("Invalid vehicle ID.");

            if (reservation.StartDate >= reservation.EndDate)
                return BadRequest("Start date must be earlier than end date.");

            var customerApiBase = _configuration["ServiceUrls:CustomerApi"];
            var vehicleApiBase = _configuration["ServiceUrls:VehicleApi"];

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Remove("X-Internal-Gateway");
            client.DefaultRequestHeaders.Add("X-Internal-Gateway", _configuration["GatewayAccess:InternalSecret"]!);

            // Check customer exists
            var customerResponse = await client.GetAsync($"{customerApiBase}/api/G3Customer/{reservation.CustomerId}");
            if (!customerResponse.IsSuccessStatusCode)
                return BadRequest("Customer does not exist.");

            // Check vehicle exists
            var vehicleResponse = await client.GetAsync($"{vehicleApiBase}/api/Vehicles/{reservation.VehicleId}");
            if (!vehicleResponse.IsSuccessStatusCode)
                return BadRequest("Vehicle does not exist.");

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