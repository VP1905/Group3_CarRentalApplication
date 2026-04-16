using G3VehicleInventory.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate
{
    public class Vehicle : Entity, IAggregateRoot
    {
        public VehicleCode VehicleCode { get; private set; } = default!;
        public VehicleType VehicleType { get; private set; }

        public Inventory Inventory { get; private set; } = default!;

        protected Vehicle()
        {
        }

        public Vehicle(VehicleCode vehicleCode, VehicleType vehicleType, LocationId locationId)
        {
            VehicleCode = vehicleCode ?? throw new ArgumentNullException(nameof(vehicleCode));
            VehicleType = vehicleType;
            Inventory = new Inventory(locationId, VehicleStatus.Available);
        }

        public void MarkAvailable()
        {
            Inventory.MarkAvailable();
        }

        public void MarkReserved()
        {
            Inventory.MarkReserved();
        }

        public void MarkRented()
        {
            Inventory.MarkRented();
        }

        public void MarkServiced()
        {
            Inventory.MarkServiced();
        }
    }
}
