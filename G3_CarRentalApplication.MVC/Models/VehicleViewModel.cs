namespace G3_CarRentalApplication.MVC.Models
{
    public class VehicleViewModel
    {
        public int Id { get; set; }
        public string VehicleCode { get; set; } = "";
        public int LocationId { get; set; }
        public string VehicleType { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
