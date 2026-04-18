using G3_CarRentalApplication.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace G3_CarRentalApplication.MVC.Controllers
{
    public class ReservationsController : Controller
    {
        public IActionResult Index()
        {
            var model = new ReservationPageViewModel
            {
                Reservation = new CreateReservationViewModel
                {
                    ReservationStartDate = DateTime.Today,
                    ReservationEndDate = DateTime.Today.AddDays(1)
                }
            };

            LoadDummyData(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ReservationPageViewModel model)
        {
            if (model.Reservation.ReservationEndDate <= model.Reservation.ReservationStartDate)
            {
                ModelState.AddModelError(string.Empty, "End date must be greater than start date.");
            }

            LoadDummyData(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Message = "Frontend form submitted successfully. API integration will be added later.";
            return View(model);
        }

        public IActionResult History()
        {
            var reservations = new List<ReservationHistoryViewModel>
            {
                new ReservationHistoryViewModel
                {
                    ReservationId = 1,
                    CustomerFirstName = "John",
                    CustomerLastName = "Smith",
                    VehicleCode = "VH001",
                    VehicleType = "SUV",
                    LocationId = 101,
                    ReservationStartDate = DateTime.Today,
                    ReservationEndDate = DateTime.Today.AddDays(3),
                    ReservationStatus = "Reserved",
                    VehicleStatus = "Reserved"
                },
                new ReservationHistoryViewModel
                {
                    ReservationId = 2,
                    CustomerFirstName = "Emma",
                    CustomerLastName = "Johnson",
                    VehicleCode = "VH002",
                    VehicleType = "Sedan",
                    LocationId = 102,
                    ReservationStartDate = DateTime.Today.AddDays(-5),
                    ReservationEndDate = DateTime.Today.AddDays(-2),
                    ReservationStatus = "Completed",
                    VehicleStatus = "Available"
                }
            };

            return View(reservations);
        }

        private void LoadDummyData(ReservationPageViewModel model)
        {
            model.Customers = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "John Smith" },
                new SelectListItem { Value = "2", Text = "Emma Johnson" },
                new SelectListItem { Value = "3", Text = "David Miller" }
            };

            model.Vehicles = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "VH001 - SUV - Location 101" },
                new SelectListItem { Value = "2", Text = "VH002 - Sedan - Location 102" },
                new SelectListItem { Value = "3", Text = "VH003 - Truck - Location 103" }
            };
        }
    }
}