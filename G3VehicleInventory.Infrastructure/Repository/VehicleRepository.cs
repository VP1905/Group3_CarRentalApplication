using G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate;
using G3VehicleInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Infrastructure.Repository
{
    // This class belongs to the Infrastructure layer and is responsible
    // only for data access logic. It does NOT contain any business rules
    // or domain logic.
    public class VehicleRepository : IVehicleRepository
    {
        private readonly InventoryDbContext _context;

        public VehicleRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public Vehicle Add(Vehicle vehicle)
        {
            return _context.Vehicles.Add(vehicle).Entity;
        }

        public Vehicle Update(Vehicle vehicle)
        {
            return _context.Vehicles.Update(vehicle).Entity;
        }

        public async Task<Vehicle?> FindByIdAsync(int id)
        {
            return await _context.Vehicles
                .Include(v => v.Inventory)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles
                .Include(v => v.Inventory)
                .OrderBy(v => v.Id)
                .ToListAsync();
        }

        public void Delete(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
