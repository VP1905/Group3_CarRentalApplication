using System.ComponentModel.DataAnnotations;

namespace G3_CarRentalApplication.MVC.Models
{
    public class MaintenanceViewModel
    {
        public int Id { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RepairDate { get; set; }

        [Required]
        public string Description { get; set; } = "";

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public string PerformedBy { get; set; } = "";
    }

}
