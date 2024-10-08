using EF_DapperPractice.Models;
using EF_DapperPractice.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EF_DapperPractice.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IBonusRepository _bonusRepo;

        public HomeController(ILogger<HomeController> logger, IBonusRepository bonusRepo) {
            _logger = logger;
            _bonusRepo = bonusRepo;
        }

        public IActionResult Index() {
            IEnumerable<Company> companies = _bonusRepo.GetAllCompanyWithEmployees();
            return View(companies);
        }

        public IActionResult AddTestRecords() {
            Company company = new Company() {
                Name = "Test" + Guid.NewGuid().ToString(),
                Address = "test address",
                City = "test city",
                PostalCode = "test postalCode",
                State = "test state",
                Employees = new List<Employee>()
            };

            company.Employees.Add(new Employee() {
                Email = "test Email",
                Name = "Test Name " + Guid.NewGuid().ToString(),
                Phone = " test phone",
                Title = "Test Manager"
            });

            company.Employees.Add(new Employee() {
                Email = "test Email 2",
                Name = "Test Name 2" + Guid.NewGuid().ToString(),
                Phone = " test phone 2",
                Title = "Test Manager 2"
            });

            _bonusRepo.CreateTestCompanyWithTransaction(company);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveTestRecords() {
            var companies = _bonusRepo.FilterCompanyByName("Test").Select(company => company.CompanyId).ToArray();
            _bonusRepo.RemoveRange(companies);

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
