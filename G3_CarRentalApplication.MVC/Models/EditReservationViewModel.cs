using System.ComponentModel.DataAnnotations;

namespace G3_CarRentalApplication.MVC.Models
{
    public class EditReservationViewModel
    {
        public int ReservationId { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public string CustomerFirstName { get; set; } = "";
        public string CustomerLastName { get; set; } = "";
        public string VehicleCode { get; set; } = "";
        public string VehicleType { get; set; } = "";
        public int LocationId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReservationStartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReservationEndDate { get; set; }

        [Required]
        public string ReservationStatus { get; set; } = "";

        [Required]
        public string VehicleStatus { get; set; } = "";

        public string CustomerFullName => $"{CustomerFirstName} {CustomerLastName}";
    }
}
