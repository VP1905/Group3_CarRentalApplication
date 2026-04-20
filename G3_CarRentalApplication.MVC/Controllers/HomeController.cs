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

        private string GatewayBaseUrl => _configuration["ApiSettings:GatewayBaseUrl"]!;
        private string ApiKey => _configuration["ApiSettings:ApiKey"]!;

        private HttpClient CreateGatewayClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Remove("X-API-Key");
            client.DefaultRequestHeaders.Add("X-API-Key", ApiKey);
            return client;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();
            var client = CreateGatewayClient();

            try
            {
                var customers = await client.GetFromJsonAsync<List<CustomerViewModel>>(
                    $"{GatewayBaseUrl}gateway/customers/api/G3Customer");
                model.TotalCustomers = customers?.Count ?? 0;

                var vehicles = await client.GetFromJsonAsync<List<VehicleViewModel>>(
                    $"{GatewayBaseUrl}gateway/vehicles/api/Vehicles");
                model.AvailableVehicles = vehicles?.Count(v =>
                    !string.IsNullOrWhiteSpace(v.Status) &&
                    v.Status.Equals("Available", StringComparison.OrdinalIgnoreCase)) ?? 0;

                var maintenanceRecords = await client.GetFromJsonAsync<List<MaintenanceViewModel>>(
                    $"{GatewayBaseUrl}gateway/maintenance/api/maintenance");
                model.TotalMaintenanceJobs = maintenanceRecords?.Count ?? 0;

                var reservations = await client.GetFromJsonAsync<List<ReservationApiModel>>(
                    $"{GatewayBaseUrl}gateway/reservations/api/G3Reservation");
                model.TotalReservations = reservations?.Count ?? 0;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Unable to load dashboard data: {ex.Message}";
            }

            return View(model);
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