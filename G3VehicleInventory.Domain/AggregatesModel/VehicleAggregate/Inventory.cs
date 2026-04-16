using G3VehicleInventory.Domain.Exceptions;
using G3VehicleInventory.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate
{
    public class Inventory : Entity
    {
        // The vehicle this inventory record belongs to
        public int VehicleId { get; private set; }
        // Where the vehicle is located.
        public LocationId LocationId { get; private set; } = default!;
        // Current status of the vehicle (Available, Reserved, Rented, UnderService)
        public VehicleStatus Status { get; private set; }

        // Required by EF Core to reconstruct the object from the database — not for direct use
        protected Inventory() { }

        // The only valid way to create an inventory record.
        public Inventory(LocationId locationId, VehicleStatus status)
        {
            LocationId = locationId ?? throw new ArgumentNullException(nameof(locationId));
            Status = status;
        }

        // Marks the vehicle as available.
        public void MarkAvailable()
        {
            if (Status == VehicleStatus.Reserved)
                throw new VehicleInventoryDomainException(
                    "A reserved vehicle cannot be marked as available without explicit release.");

            Status = VehicleStatus.Available;
        }

        // Marks the vehicle as reserved.
        public void MarkReserved()
        {
            if (Status == VehicleStatus.Rented)
                throw new VehicleInventoryDomainException("A rented vehicle cannot be reserved.");

            if (Status == VehicleStatus.UnderService)
                throw new VehicleInventoryDomainException("A vehicle under service cannot be reserved.");

            Status = VehicleStatus.Reserved;
        }

        // Marks the vehicle as rented.
        public void MarkRented()
        {
            if (Status == VehicleStatus.Rented)
                throw new VehicleInventoryDomainException("A vehicle cannot be rented if it is already rented.");

            if (Status == VehicleStatus.Reserved)
                throw new VehicleInventoryDomainException("A vehicle cannot be rented if it is reserved.");

            if (Status == VehicleStatus.UnderService)
                throw new VehicleInventoryDomainException("A vehicle cannot be rented if it is under service.");

            Status = VehicleStatus.Rented;
        }

        // Sends the vehicle to service.
        public void MarkServiced()
        {
            if (Status == VehicleStatus.Rented)
                throw new VehicleInventoryDomainException("A rented vehicle cannot be moved to service.");

            Status = VehicleStatus.UnderService;
        }
    }
}
