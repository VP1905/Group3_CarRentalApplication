using G3_CarRentalApplication.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace G3_CarRentalApplication.MVC.Controllers
{
    public class VehiclesController : Controller
    {
        private static List<VehicleViewModel> _vehicles = new List<VehicleViewModel>
        {
            new VehicleViewModel
            {
                Id = 1,
                VehicleCode = "VH001",
                LocationId = 101,
                VehicleType = "SUV",
                Status = "Available"
            },
            new VehicleViewModel
            {
                Id = 2,
                VehicleCode = "VH002",
                LocationId = 102,
                VehicleType = "Sedan",
                Status = "Reserved"
            }
        };

        public IActionResult Index()
        {
            return View(_vehicles);
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

            model.Id = _vehicles.Any() ? _vehicles.Max(x => x.Id) + 1 : 1;
            _vehicles.Add(model);

            TempData["SuccessMessage"] = "Vehicle added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var vehicle = _vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VehicleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var vehicle = _vehicles.FirstOrDefault(x => x.Id == model.Id);
            if (vehicle == null)
                return NotFound();

            vehicle.VehicleCode = model.VehicleCode;
            vehicle.LocationId = model.LocationId;
            vehicle.VehicleType = model.VehicleType;
            vehicle.Status = model.Status;

            TempData["SuccessMessage"] = "Vehicle updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var vehicle = _vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var vehicle = _vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                return NotFound();

            _vehicles.Remove(vehicle);

            TempData["SuccessMessage"] = "Vehicle deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var vehicle = _vehicles.FirstOrDefault(x => x.Id == id);
            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }
    }
}