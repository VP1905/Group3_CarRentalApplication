using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate
{
    public enum VehicleStatus
    {
        Available = 1,
        Reserved = 2,
        Rented = 3,
        UnderService = 4
    }
}
