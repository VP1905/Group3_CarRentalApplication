namespace G3_CarRentalApplication.MVC.Models
{
    public class ReservationHistoryViewModel
    {
        public int ReservationId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; } = "";
        public string CustomerLastName { get; set; } = "";

        public int VehicleId { get; set; }
        public string VehicleCode { get; set; } = "";
        public int LocationId { get; set; }
        public string VehicleType { get; set; } = "";
        public string VehicleStatus { get; set; } = "";

        public DateTime ReservationStartDate { get; set; }
        public DateTime ReservationEndDate { get; set; }

        public string ReservationStatus { get; set; } = "";

        public string CustomerFullName => $"{CustomerFirstName} {CustomerLastName}";
    }

}
