using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Application.Services
{
    //Data Transfer Object used when creating a new vehicle.
    public class CreateVehicleDto
    {
        [Required]
        [MaxLength(20)]
        public string VehicleCode { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int LocationId { get; set; }

        [Required]
        public string VehicleType { get; set; } = string.Empty;
    }
}
