using G3_CarRentalApplication.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace G3_CarRentalApplication.MVC.Controllers
{
    public class MaintenanceController : Controller
    {
        public IActionResult Index()
        {
            var records = new List<MaintenanceViewModel>
            {
                new MaintenanceViewModel
                {
                    Id = 1,
                    VehicleId = 1,
                    RepairDate = DateTime.Today.AddDays(-10),
                    Description = "Oil change",
                    Cost = 120,
                    PerformedBy = "Mike Auto Shop"
                },
                new MaintenanceViewModel
                {
                    Id = 2,
                    VehicleId = 2,
                    RepairDate = DateTime.Today.AddDays(-5),
                    Description = "Brake service",
                    Cost = 250,
                    PerformedBy = "City Garage"
                }
            };

            return View(records);
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
        public IActionResult Create(MaintenanceViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            TempData["SuccessMessage"] = "Maintenance record added successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}