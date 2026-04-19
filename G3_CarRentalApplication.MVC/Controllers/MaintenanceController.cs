using G3_CarRentalApplication.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace G3_CarRentalApplication.MVC.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MaintenanceController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private string GatewayBaseUrl => _configuration["ApiSettings:GatewayBaseUrl"]!;

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            var records = await client.GetFromJsonAsync<List<MaintenanceViewModel>>(
                $"{GatewayBaseUrl}gateway/maintenance/api/maintenance");

            return View(records ?? new List<MaintenanceViewModel>());
        }

        public IActionResult Create()
        {
            return View(new MaintenanceViewModel
            {
                RepairDate = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsJsonAsync($"{GatewayBaseUrl}gateway/maintenance/api/maintenance", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Unable to create maintenance record.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Maintenance record added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var record = await client.GetFromJsonAsync<MaintenanceViewModel>(
                $"{GatewayBaseUrl}gateway/maintenance/api/maintenance/{id}");

            if (record == null)
                return NotFound();

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MaintenanceViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            var response = await client.PutAsJsonAsync($"{GatewayBaseUrl}gateway/maintenance/api/maintenance/{model.Id}", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Unable to update maintenance record.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Maintenance record updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var record = await client.GetFromJsonAsync<MaintenanceViewModel>(
                $"{GatewayBaseUrl}gateway/maintenance/api/maintenance/{id}");

            if (record == null)
                return NotFound();

            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.DeleteAsync($"{GatewayBaseUrl}gateway/maintenance/api/maintenance/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to delete maintenance record.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Maintenance record deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var record = await client.GetFromJsonAsync<MaintenanceViewModel>(
                $"{GatewayBaseUrl}gateway/maintenance/api/maintenance/{id}");

            if (record == null)
                return NotFound();

            return View(record);
        }
    }
}