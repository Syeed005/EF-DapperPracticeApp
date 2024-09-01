using EF_DapperPractice.Models;
using System.Runtime.InteropServices;

namespace EF_DapperPractice.Repository {
    public interface ICompanyRepository {
        Company Get(int id);
        List<Company> GetAll();
        Company Create(Company company);
        Company Update(Company company);
        void Remove(Company company);
    }
}
