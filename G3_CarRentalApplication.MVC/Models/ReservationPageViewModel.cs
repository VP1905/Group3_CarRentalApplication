using Microsoft.AspNetCore.Mvc.Rendering;

namespace G3_CarRentalApplication.MVC.Models
{
    public class ReservationPageViewModel
    {
        public CreateReservationViewModel Reservation { get; set; } = new();
        public List<SelectListItem> Customers { get; set; } = new();
        public List<SelectListItem> Vehicles { get; set; } = new();
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
