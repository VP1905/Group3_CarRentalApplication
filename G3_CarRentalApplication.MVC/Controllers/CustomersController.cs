using G3_CarRentalApplication.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace G3_CarRentalApplication.MVC.Controllers
{
    public class CustomersController : Controller
    {
        // Dummy frontend-only data
        private static List<CustomerViewModel> _customers = new List<CustomerViewModel>
        {
            new CustomerViewModel
            {
                CustomerId = 1,
                FirstName = "John",
                LastName = "Smith",
                Phone = "111-222-3333",
                Email = "john@email.com"
            },
            new CustomerViewModel
            {
                CustomerId = 2,
                FirstName = "Emma",
                LastName = "Johnson",
                Phone = "222-333-4444",
                Email = "emma@email.com"
            }
        };

        public IActionResult Index()
        {
            return View(_customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.CustomerId = _customers.Any() ? _customers.Max(x => x.CustomerId) + 1 : 1;
            _customers.Add(model);

            TempData["SuccessMessage"] = "Customer added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var customer = _customers.FirstOrDefault(x => x.CustomerId == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var customer = _customers.FirstOrDefault(x => x.CustomerId == model.CustomerId);

            if (customer == null)
                return NotFound();

            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Phone = model.Phone;
            customer.Email = model.Email;

            TempData["SuccessMessage"] = "Customer updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var customer = _customers.FirstOrDefault(x => x.CustomerId == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _customers.FirstOrDefault(x => x.CustomerId == id);

            if (customer == null)
                return NotFound();

            _customers.Remove(customer);

            TempData["SuccessMessage"] = "Customer deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var customer = _customers.FirstOrDefault(x => x.CustomerId == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }
    }
}