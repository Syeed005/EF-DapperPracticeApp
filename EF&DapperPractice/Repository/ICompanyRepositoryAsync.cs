using EF_DapperPractice.Models;
using System.Runtime.InteropServices;

namespace EF_DapperPractice.Repository {
    public interface ICompanyRepositoryAsync {
        Task<Company> GetAsync(int id);
        Task<List<Company>> GetAllAsync();
        Task<Company> CreateAsync(Company company);
        Task<Company> UpdateAsync(Company company);
        Task RemoveAsync(Company company);
        Task SaveAsync();
    }
}
