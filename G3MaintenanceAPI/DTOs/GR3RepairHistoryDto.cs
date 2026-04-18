using System;
using System.ComponentModel.DataAnnotations;

namespace G3MaintenanceAPI.DTOs
{
    public class GR3RepairHistoryDto
    {
        public int Id { get; set; }

        [Required]
        public int VehicleId { get; set; }

        public DateTime RepairDate { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 100000)]
        public decimal Cost { get; set; }

        [Required]
        public string PerformedBy { get; set; } = string.Empty;
    }
}