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
using System.Runtime.CompilerServices;

namespace EF_DapperPractice.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly ICompanyRepositoryAsync _companyRepoAsync;
        private readonly IBonusRepository _bonusRepo;

        public CompaniesController(ICompanyRepository companyRepo, IBonusRepository bonusRepo, ICompanyRepositoryAsync companyRepoAsync) {
            _companyRepo = companyRepo;
            _bonusRepo = bonusRepo;
            _companyRepoAsync = companyRepoAsync;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            //return View(await _companyRepo.GetAllAsync());
            return View(await _companyRepoAsync.GetAllAsync());
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var company = _companyRepo.Get(id.GetValueOrDefault());
            //var company = _bonusRepo.GetCompanyWithEmployees(id.GetValueOrDefault());
            var company = await _bonusRepo.GetCompanyWithEmployeesAsync(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (ModelState.IsValid)
            {
                //_companyRepo.Create(company);
                await _companyRepoAsync.CreateAsync(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _companyRepoAsync.GetAsync(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _companyRepoAsync.UpdateAsync(company);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _companyRepoAsync.GetAsync(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            } else {
                await _companyRepoAsync.RemoveAsync(company);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
