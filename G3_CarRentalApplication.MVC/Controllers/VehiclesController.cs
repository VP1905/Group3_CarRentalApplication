using G3_CarRentalApplication.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace G3_CarRentalApplication.MVC.Controllers
{
    public class VehiclesController : Controller
    {
        public IActionResult Index()
        {
            var vehicles = new List<VehicleViewModel>
            {
                new VehicleViewModel { Id = 1, VehicleCode = "VH001", LocationId = 101, VehicleType = "SUV", Status = "Available" },
                new VehicleViewModel { Id = 2, VehicleCode = "VH002", LocationId = 102, VehicleType = "Sedan", Status = "Reserved" }
            };

            return View(vehicles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VehicleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            TempData["SuccessMessage"] = "Vehicle created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var vehicle = new VehicleViewModel
            {
                Id = id,
                VehicleCode = "VH001",
                LocationId = 101,
                VehicleType = "SUV",
                Status = "Available"
            };

            return View(vehicle);
        }
    }
}