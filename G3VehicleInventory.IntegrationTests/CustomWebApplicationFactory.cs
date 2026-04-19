using G3VehicleInventory.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace G3VehicleInventory.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                services.RemoveAll(typeof(DbContextOptions<InventoryDbContext>));

                // Add InMemory database for tests
                services.AddDbContext<InventoryDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InventoryTestDb");
                });

                // Build service provider and seed test data
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                SeedData(db);
            });
        }

        private static void SeedData(InventoryDbContext db)
        {
            var vehicle1 = new G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate.Vehicle(
                new G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate.VehicleCode("TEST001"),
                G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate.VehicleType.SUV,
                new G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate.LocationId(1));

            var vehicle2 = new G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate.Vehicle(
                new G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate.VehicleCode("TEST002"),
                G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate.VehicleType.Sedan,
                new G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate.LocationId(1));

            db.Vehicles.Add(vehicle1);
            db.Vehicles.Add(vehicle2);
            db.SaveChanges();
        }
    }
}