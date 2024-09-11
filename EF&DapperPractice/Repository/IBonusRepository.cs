using EF_DapperPractice.Models;

namespace EF_DapperPractice.Repository {
    public interface IBonusRepository {
        List<Employee> GetAllEmployeedWithCompany(int id);
    }
}
