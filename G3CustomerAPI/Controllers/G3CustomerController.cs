using G3CustomerAPI.Data;
using G3CustomerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G3CustomerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class G3CustomerController : ControllerBase
    {
        private readonly G3CustomerDbContext _context;

        public G3CustomerController(G3CustomerDbContext context)
        {
            _context = context;
        }

        // GET: api/G3Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<G3Customer>>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/G3Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<G3Customer>> GetCustomerById(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            return customer;
        }

        // POST: api/G3Customer
        [HttpPost]
        public async Task<ActionResult<G3Customer>> AddCustomer(G3Customer customer)
        {
            customer.CreatedAt = DateTime.UtcNow;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }

        // PUT: api/G3Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, G3Customer customer)
        {
            if (id != customer.Id)
                return BadRequest();

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Customers.Any(e => e.Id == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE: api/G3Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return NotFound(new { message = $"Customer with ID {id} was not found." });

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}