using EF_DapperPractice.Data;
using EF_DapperPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_DapperPractice.Repository {
    public class CompanyRepositoryEF : ICompanyRepositoryAsync {
        private readonly ApplicationDbContext _db;

        public CompanyRepositoryEF(ApplicationDbContext db) {
            _db = db;
        }

        public async Task<Company> CreateAsync(Company company) {
            await _db.Companies.AddAsync(company);
            await SaveAsync();
            return company;
        }

        public async Task<List<Company>> GetAllAsync() {
            return await _db.Companies.ToListAsync();
        }

        public async Task<Company> GetAsync(int id) {
            return await _db.Companies.FirstOrDefaultAsync(x => x.CompanyId == id);
        }

        public async Task RemoveAsync(Company company) {
            _db.Companies.Remove(company);
            await SaveAsync();           
        }

        public async Task SaveAsync() {
            await _db.SaveChangesAsync();
        }

        public async Task<Company> UpdateAsync(Company company) {
            _db.Companies.Update(company);
            await SaveAsync();
            return company;
        }
    }
}
