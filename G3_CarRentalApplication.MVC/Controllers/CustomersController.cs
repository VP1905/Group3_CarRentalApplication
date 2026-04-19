using G3_CarRentalApplication.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace G3_CarRentalApplication.MVC.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CustomersController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private string GatewayBaseUrl => _configuration["ApiSettings:GatewayBaseUrl"]!;

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            var customers = await client.GetFromJsonAsync<List<CustomerViewModel>>(
                $"{GatewayBaseUrl}gateway/customers/api/G3Customer");

            return View(customers ?? new List<CustomerViewModel>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Remove("X-Api-Key");
            client.DefaultRequestHeaders.Add("X-Api-Key", _configuration["ApiSettings:ApiKey"]);

            var response = await client.PostAsJsonAsync($"{GatewayBaseUrl}gateway/customers/api/G3Customer", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Unable to create customer.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Customer added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var customer = await client.GetFromJsonAsync<CustomerViewModel>(
                $"{GatewayBaseUrl}gateway/customers/api/G3Customer/{id}");

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            var response = await client.PutAsJsonAsync($"{GatewayBaseUrl}gateway/customers/api/G3Customer/{model.Id}", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Unable to update customer.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Customer updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var customer = await client.GetFromJsonAsync<CustomerViewModel>(
                $"{GatewayBaseUrl}gateway/customers/api/G3Customer/{id}");

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.DeleteAsync($"{GatewayBaseUrl}gateway/customers/api/G3Customer/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to delete customer.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Customer deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var customer = await client.GetFromJsonAsync<CustomerViewModel>(
                $"{GatewayBaseUrl}gateway/customers/api/G3Customer/{id}");

            if (customer == null)
                return NotFound();

            return View(customer);
        }
    }
}