using Dapper;
using Dapper.Contrib.Extensions;
using EF_DapperPractice.Data;
using EF_DapperPractice.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EF_DapperPractice.Repository {
    public class CompanyRepositoryContrib : ICompanyRepository {
        //private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public CompanyRepositoryContrib(IConfiguration configuration) {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Create(Company company) {
            var id = _dbConnection.Insert(company);
            company.CompanyId = (int)id;
            return company;
        }

        public Company Get(int id) {
            return _dbConnection.Get<Company>(id);
        }

        public List<Company> GetAll() {
            return _dbConnection.GetAll<Company>().ToList();
        }

        public void Remove(Company company) {
            _dbConnection.Delete(company);
        }

        public Company Update(Company company) {
            var status = _dbConnection.Update(company);
            return company;
        }
    }
}
