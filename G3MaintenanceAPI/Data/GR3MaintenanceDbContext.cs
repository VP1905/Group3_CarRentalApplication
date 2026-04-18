using Microsoft.EntityFrameworkCore;
using G3MaintenanceAPI.Models;

namespace G3MaintenanceAPI.Data
{
    public class GR3MaintenanceDbContext : DbContext
    {
        public GR3MaintenanceDbContext(DbContextOptions<GR3MaintenanceDbContext> options)
            : base(options)
        {
        }

        public DbSet<GR3RepairHistory> RepairHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GR3RepairHistory>()
                .Property(r => r.Cost)
                .HasPrecision(10, 2);
        }
    }
}