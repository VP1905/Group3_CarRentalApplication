using G3CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace G3CustomerAPI.Data
{
    public class G3CustomerDbContext : DbContext
    {
        public G3CustomerDbContext(DbContextOptions<G3CustomerDbContext> options)
            : base(options)
        {
        }

        public DbSet<G3Customer> Customers { get; set; }
    }
}
