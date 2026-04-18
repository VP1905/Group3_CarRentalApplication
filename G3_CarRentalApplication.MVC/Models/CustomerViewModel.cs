namespace G3_CarRentalApplication.MVC.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

}
