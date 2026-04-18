using G3_CarRentalApplication.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace G3_CarRentalApplication.MVC.Controllers
{
    public class MaintenanceController : Controller
    {
        private static List<MaintenanceViewModel> _records = new List<MaintenanceViewModel>
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

        public IActionResult Index()
        {
            return View(_records);
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

            model.Id = _records.Any() ? _records.Max(x => x.Id) + 1 : 1;
            _records.Add(model);

            TempData["SuccessMessage"] = "Maintenance record added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var record = _records.FirstOrDefault(x => x.Id == id);
            if (record == null)
                return NotFound();

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MaintenanceViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var record = _records.FirstOrDefault(x => x.Id == model.Id);
            if (record == null)
                return NotFound();

            record.VehicleId = model.VehicleId;
            record.RepairDate = model.RepairDate;
            record.Description = model.Description;
            record.Cost = model.Cost;
            record.PerformedBy = model.PerformedBy;

            TempData["SuccessMessage"] = "Maintenance record updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var record = _records.FirstOrDefault(x => x.Id == id);
            if (record == null)
                return NotFound();

            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var record = _records.FirstOrDefault(x => x.Id == id);
            if (record == null)
                return NotFound();

            _records.Remove(record);

            TempData["SuccessMessage"] = "Maintenance record deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var record = _records.FirstOrDefault(x => x.Id == id);
            if (record == null)
                return NotFound();

            return View(record);
        }
    }
}