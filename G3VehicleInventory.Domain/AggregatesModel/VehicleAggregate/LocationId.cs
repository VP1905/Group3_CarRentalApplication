using G3VehicleInventory.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate
{
    // A Value Object that wraps a location's integer ID.
    public class LocationId
    {
        public int Value { get; private set; }

        // Private parameterless constructor, prevents creating an empty/invalid LocationId
        private LocationId()
        {
        }

        // Throws immediately if the value is invalid, so a LocationId always holds a valid value.
        public LocationId(int value)
        {
            if (value <= 0)
                throw new VehicleInventoryDomainException("LocationId must be greater than 0.");

            Value = value;
        }

        // Makes it easy to print or log the LocationId as a plain number
        public override string ToString() => Value.ToString();
    }
}
