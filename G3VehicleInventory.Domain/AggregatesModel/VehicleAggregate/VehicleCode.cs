using G3VehicleInventory.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate
{
    public class VehicleCode
    {
        public string Value { get; private set; } = string.Empty;

        private VehicleCode()
        {
        }

        public VehicleCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new VehicleInventoryDomainException("Vehicle code is required.");

            value = value.Trim();

            if (value.Length > 20)
                throw new VehicleInventoryDomainException("Vehicle code cannot exceed 20 characters.");

            Value = value;
        }

        public override string ToString() => Value;
    }
}
