using Microsoft.EntityFrameworkCore;
using G3ReservationAPI.Models;

namespace G3ReservationAPI.Data
{
    public class G3ReservationDbContext : DbContext
    {
        public G3ReservationDbContext(DbContextOptions<G3ReservationDbContext> options)
            : base(options)
        {
        }

        public DbSet<G3Reservation> Reservations { get; set; }
    }
}