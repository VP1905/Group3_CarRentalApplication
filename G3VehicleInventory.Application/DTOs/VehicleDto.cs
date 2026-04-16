using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3VehicleInventory.Application.DTOs
{
    //DTO used to send vehicle information from the application layer to the API response
    public class VehicleDto
    {
        public int Id { get; set; }

        [Required]
        public string VehicleCode { get; set; } = string.Empty;

        [Required]
        public int LocationId { get; set; }

        [Required]
        public string VehicleType { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
