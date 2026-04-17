using G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate;
using Microsoft.EntityFrameworkCore;

namespace G3VehicleInventory.Infrastructure.Data
{
    // This class is responsible for database access
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Inventory> Inventories => Set<Inventory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>(static entity =>
            {
                entity.ToTable("Vehicles");

                entity.HasKey(v => v.Id);

                entity.OwnsOne(v => v.VehicleCode, code =>
                {
                    code.Property(c => c.Value)
                        .HasColumnName("VehicleCode")
                        .HasMaxLength(20)
                        .IsRequired();
                });

                entity.Property(v => v.VehicleType)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();

                entity.HasOne(v => v.Inventory)
                    .WithOne()
                    .HasForeignKey<Inventory>(i => i.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("Inventories");

                entity.HasKey(i => i.Id);

                entity.OwnsOne(i => i.LocationId, location =>
                {
                    location.Property(l => l.Value)
                        .HasColumnName("LocationId")
                        .IsRequired();
                });

                entity.Property(i => i.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();
            });
        }

        internal async Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
