using EF_DapperPractice.Models;

namespace EF_DapperPractice.Repository {
    public interface IBonusRepository {
        List<Employee> GetAllEmployeedWithCompany(int id);
        Company GetCompanyWithEmployees(int id);
        List<Company> GetAllCompanyWithEmployees();
        void CreateTestCompany(Company company);
        void RemoveRange(int[] data);
        List<Company> FilterCompanyByName(string name);
    }
}
