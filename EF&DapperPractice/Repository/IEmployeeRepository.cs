using EF_DapperPractice.Models;
using System.Runtime.InteropServices;

namespace EF_DapperPractice.Repository {
    public interface IEmployeeRepository {
        Employee Get(int id);
        List<Employee> GetAll();
        Employee Create(Employee company);
        Employee Update(Employee company);
        void Remove(Employee company);
    }
}
