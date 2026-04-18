using System.ComponentModel.DataAnnotations;

namespace G3_CarRentalApplication.MVC.Models
{
    public class CreateReservationViewModel
    {
        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime ReservationStartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime ReservationEndDate { get; set; }
    }
}
