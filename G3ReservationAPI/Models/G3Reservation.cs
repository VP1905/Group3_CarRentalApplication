using System.ComponentModel.DataAnnotations;

namespace G3ReservationAPI.Models
{
    public class G3Reservation
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than 0.")]
        public int CustomerId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "VehicleId must be greater than 0.")]
        public int VehicleId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Active";
    }
}