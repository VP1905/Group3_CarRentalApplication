using System;
using System.ComponentModel.DataAnnotations;

namespace G3MaintenanceAPI.Models
{
    public class GR3RepairHistory
    {
        public int Id { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public DateTime RepairDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 100000)]
        public decimal Cost { get; set; }

        [Required]
        [StringLength(100)]
        public string PerformedBy { get; set; } = string.Empty;
    }
}