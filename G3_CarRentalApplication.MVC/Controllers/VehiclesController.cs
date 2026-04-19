using G3_CarRentalApplication.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace G3_CarRentalApplication.MVC.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public VehiclesController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private string VehicleApiBaseUrl => _configuration["ApiSettings:VehicleApiBaseUrl"]!;

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            var vehicles = await client.GetFromJsonAsync<List<VehicleViewModel>>(
                $"{VehicleApiBaseUrl}api/Vehicles");

            return View(vehicles ?? new List<VehicleViewModel>());
        }

        public IActionResult Create()
        {
            return View(new VehicleViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            var createBody = new
            {
                vehicleCode = model.VehicleCode,
                locationId = model.LocationId,
                vehicleType = model.VehicleType
            };

            var response = await client.PostAsJsonAsync($"{VehicleApiBaseUrl}api/Vehicles", createBody);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Unable to create vehicle. {error}");
                return View(model);
            }

            TempData["SuccessMessage"] = "Vehicle added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var vehicle = await client.GetFromJsonAsync<VehicleViewModel>(
                $"{VehicleApiBaseUrl}api/Vehicles/{id}");

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            var updateBody = new
            {
                status = model.Status
            };

            var response = await client.PutAsJsonAsync(
                $"{VehicleApiBaseUrl}api/Vehicles/{model.Id}/status",
                updateBody);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Unable to update vehicle status. {error}");
                return View(model);
            }

            TempData["SuccessMessage"] = "Vehicle status updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var vehicle = await client.GetFromJsonAsync<VehicleViewModel>(
                $"{VehicleApiBaseUrl}api/Vehicles/{id}");

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.DeleteAsync($"{VehicleApiBaseUrl}api/Vehicles/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to delete vehicle.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Vehicle deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var vehicle = await client.GetFromJsonAsync<VehicleViewModel>(
                $"{VehicleApiBaseUrl}api/Vehicles/{id}");

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }
    }
}