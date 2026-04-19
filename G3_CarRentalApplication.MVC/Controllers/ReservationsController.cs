using G3_CarRentalApplication.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;

namespace G3_CarRentalApplication.MVC.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ReservationsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private string CustomerApiBaseUrl => _configuration["ApiSettings:CustomerApiBaseUrl"]!;
        private string VehicleApiBaseUrl => _configuration["ApiSettings:VehicleApiBaseUrl"]!;
        private string ReservationApiBaseUrl => _configuration["ApiSettings:ReservationApiBaseUrl"]!;

        public async Task<IActionResult> Index()
        {
            var model = new ReservationPageViewModel
            {
                Reservation = new CreateReservationViewModel
                {
                    ReservationStartDate = DateTime.Today,
                    ReservationEndDate = DateTime.Today.AddDays(1)
                }
            };

            await LoadApiData(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ReservationPageViewModel model)
        {
            if (model.Reservation.ReservationEndDate <= model.Reservation.ReservationStartDate)
            {
                ModelState.AddModelError("", "End date must be greater than start date.");
            }

            if (!ModelState.IsValid)
            {
                await LoadApiData(model);
                return View(model);
            }

            var client = _httpClientFactory.CreateClient();

            var requestBody = new
            {
                customerId = model.Reservation.CustomerId,
                vehicleId = model.Reservation.VehicleId,
                startDate = model.Reservation.ReservationStartDate,
                endDate = model.Reservation.ReservationEndDate,
                status = "Active"
            };

            var response = await client.PostAsJsonAsync(
                $"{ReservationApiBaseUrl}api/G3Reservation",
                requestBody);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                model.ErrorMessage = $"Unable to create reservation. {error}";
                await LoadApiData(model);
                return View(model);
            }

            TempData["SuccessMessage"] = "Reservation created successfully.";
            return RedirectToAction(nameof(History));
        }

        public async Task<IActionResult> History()
        {
            var model = await BuildReservationHistoryAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var reservations = await BuildReservationHistoryAsync();
            var reservation = reservations.FirstOrDefault(r => r.ReservationId == id);

            if (reservation == null)
                return NotFound();

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PutAsync(
                $"{ReservationApiBaseUrl}api/G3Reservation/{id}/cancel",
                null);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to cancel reservation.";
            }
            else
            {
                TempData["SuccessMessage"] = "Reservation cancelled successfully.";
            }

            return RedirectToAction(nameof(History));
        }

        private async Task<List<ReservationHistoryViewModel>> BuildReservationHistoryAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var reservations = await client.GetFromJsonAsync<List<ReservationApiModel>>(
                $"{ReservationApiBaseUrl}api/G3Reservation") ?? new List<ReservationApiModel>();

            var customers = await client.GetFromJsonAsync<List<CustomerViewModel>>(
                $"{CustomerApiBaseUrl}api/G3Customer") ?? new List<CustomerViewModel>();

            var vehicles = await client.GetFromJsonAsync<List<VehicleViewModel>>(
                $"{VehicleApiBaseUrl}api/Vehicles") ?? new List<VehicleViewModel>();

            return reservations.Select(r =>
            {
                var customer = customers.FirstOrDefault(c => c.Id == r.CustomerId);
                var vehicle = vehicles.FirstOrDefault(v => v.Id == r.VehicleId);

                return new ReservationHistoryViewModel
                {
                    ReservationId = r.Id,
                    CustomerId = r.CustomerId,
                    CustomerFirstName = customer?.FirstName ?? "",
                    CustomerLastName = customer?.LastName ?? "",
                    VehicleId = r.VehicleId,
                    VehicleCode = vehicle?.VehicleCode ?? "",
                    LocationId = vehicle?.LocationId ?? 0,
                    VehicleType = vehicle?.VehicleType ?? "",
                    VehicleStatus = vehicle?.Status ?? "",
                    ReservationStartDate = r.StartDate,
                    ReservationEndDate = r.EndDate,
                    ReservationStatus = r.Status
                };
            }).ToList();
        }

        private async Task LoadApiData(ReservationPageViewModel model)
        {
            var client = _httpClientFactory.CreateClient();

            var customers = await client.GetFromJsonAsync<List<CustomerViewModel>>(
                $"{CustomerApiBaseUrl}api/G3Customer") ?? new List<CustomerViewModel>();

            var vehicles = await client.GetFromJsonAsync<List<VehicleViewModel>>(
                $"{VehicleApiBaseUrl}api/Vehicles") ?? new List<VehicleViewModel>();

            model.Customers = customers.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.FirstName} {c.LastName}"
            }).ToList();

            model.Vehicles = vehicles
                .Where(v => string.Equals(v.Status, "Available", StringComparison.OrdinalIgnoreCase))
                .Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = $"{v.VehicleCode} - {v.VehicleType} - Location {v.LocationId}"
                }).ToList();
        }

        private class ReservationApiModel
        {
            public int Id { get; set; }
            public int CustomerId { get; set; }
            public int VehicleId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Status { get; set; } = "";
        }
    }
}