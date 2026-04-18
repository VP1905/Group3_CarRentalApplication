using G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate;
using G3VehicleInventory.Infrastructure.Data;
using G3VehicleInventory.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace G3VehicleInventory.Infrastructure
{
    public static class DependencyInjection
    {
        //Registers database context and repository implementations
        //The IServiceCollection used to resigster application services.
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IVehicleRepository, VehicleRepository>();

            return services;
        }
    }
}