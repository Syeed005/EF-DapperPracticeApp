using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EF_DapperPractice.Data;
using EF_DapperPractice.Models;
using EF_DapperPractice.Repository;

namespace EF_DapperPractice.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly IEmployeeRepository _empRepo;
        [BindProperty]
        public Employee Employee { get; set; }

        public EmployeesController(ICompanyRepository companyRepo, IEmployeeRepository empRepo)
        {
            _companyRepo = companyRepo;
            _empRepo = empRepo;
        }

        
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = _empRepo.GetAll();
            foreach (Employee employee in employees) {
                employee.Company = _companyRepo.Get(employee.CompanyId);
            }
            return View(employees);
        }
        
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> selectListItems = _companyRepo.GetAll().Select(x => new SelectListItem {
                Text = x.Name,
                Value = x.CompanyId.ToString()
            });
            ViewBag.CompanyList = selectListItems;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePOST()
        {
            if (ModelState.IsValid)
            {
                _empRepo.Create(Employee);
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = _empRepo.Get(id.GetValueOrDefault());

            IEnumerable<SelectListItem> selectListItems = _companyRepo.GetAll().Select(x => new SelectListItem {
                Text = x.Name,
                Value = x.CompanyId.ToString()
            });
            ViewBag.CompanyList = selectListItems;
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPOST(int id)
        {
            if (id != Employee.EmployeeId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                _empRepo.Update(Employee);
            }
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = _empRepo.Get(id.GetValueOrDefault());
            if (Employee == null)
            {
                return NotFound();
            } else {
                _empRepo.Remove(Employee);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
