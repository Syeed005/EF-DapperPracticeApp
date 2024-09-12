using EF_DapperPractice.Models;

namespace EF_DapperPractice.Repository {
    public interface IBonusRepository {
        List<Employee> GetAllEmployeedWithCompany(int id);
        Company GetCompanyWithEmployees(int id);
        Task<Company> GetCompanyWithEmployeesAsync(int id);
        List<Company> GetAllCompanyWithEmployees();
        void CreateTestCompany(Company company);
        void CreateTestCompanyWithTransaction(Company company);
        void RemoveRange(int[] data);
        List<Company> FilterCompanyByName(string name);
    }
}
