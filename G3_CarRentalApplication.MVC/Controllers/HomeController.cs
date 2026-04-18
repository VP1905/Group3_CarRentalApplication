using G3_CarRentalApplication.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace G3_CarRentalApplication.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HomeController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private string CustomerApiBaseUrl => _configuration["ApiSettings:CustomerApiBaseUrl"]!;
        private string VehicleApiBaseUrl => _configuration["ApiSettings:VehicleApiBaseUrl"]!;
        private string MaintenanceApiBaseUrl => _configuration["ApiSettings:MaintenanceApiBaseUrl"]!;
        private string ReservationApiBaseUrl => _configuration["ApiSettings:ReservationApiBaseUrl"]!;

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();

            var client = _httpClientFactory.CreateClient();

            try
            {
                // Customers
                var customers = await client.GetFromJsonAsync<List<CustomerViewModel>>(
                    $"{CustomerApiBaseUrl}api/G3Customer");
                model.TotalCustomers = customers?.Count ?? 0;

                // Vehicles
                var vehicles = await client.GetFromJsonAsync<List<VehicleViewModel>>(
                    $"{VehicleApiBaseUrl}api/Vehicles");
                model.AvailableVehicles = vehicles?.Count(v =>
                    v.Status != null && v.Status.Equals("Available", StringComparison.OrdinalIgnoreCase)) ?? 0;

                // Maintenance
                var maintenanceRecords = await client.GetFromJsonAsync<List<MaintenanceViewModel>>(
                    $"{MaintenanceApiBaseUrl}api/maintenance");
                model.TotalMaintenanceJobs = maintenanceRecords?.Count ?? 0;

                // Reservations
                var reservations = await client.GetFromJsonAsync<List<ReservationPageViewModel>>(
                    $"{ReservationApiBaseUrl}api/G3Reservation");
                model.TotalReservations = reservations?.Count ?? 0;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Unable to load dashboard data: {ex.Message}";
            }

            return View(model);
        }
    }
}