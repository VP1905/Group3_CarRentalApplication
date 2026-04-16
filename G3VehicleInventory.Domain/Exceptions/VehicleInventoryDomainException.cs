using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Domain.Exceptions
{
    // DomainException is used to represent violations of business rules
    // or invalid state transitions within the Domain layer. 
    public class VehicleInventoryDomainException : Exception
    {
        public VehicleInventoryDomainException()
        {
        }

        public VehicleInventoryDomainException(string message) : base(message)
        {
        }

        public VehicleInventoryDomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
