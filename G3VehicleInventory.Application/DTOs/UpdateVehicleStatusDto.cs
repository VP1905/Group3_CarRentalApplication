using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Application.DTOs
{
    // Data Transfer Object used to update the status of an existing vehicle.
    public class UpdateVehicleStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
