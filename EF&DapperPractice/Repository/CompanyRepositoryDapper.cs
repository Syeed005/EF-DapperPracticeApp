using Dapper;
using EF_DapperPractice.Data;
using EF_DapperPractice.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EF_DapperPractice.Repository {
    public class CompanyRepositoryDapper : ICompanyRepository {
        //private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public CompanyRepositoryDapper(IConfiguration configuration) {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Create(Company company) {
            string sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode) \r\nSELECT CAST(SCOPE_IDENTITY() as int);";
            //company.CompanyId = _dbConnection.Execute(sql, new {company.Name, company.Address, company.City, company.State, company.PostalCode});
            company.CompanyId = _dbConnection.Query<int>(sql, company).Single();
            return company;
        }

        public Company Get(int id) {
            string sql = "SELECT * FROM Companies WHERE CompanyId = @Id";
            return _dbConnection.Query<Company>(sql, new { Id = id }).Single();
        }

        public List<Company> GetAll() {
            string sql = "SELECT * FROM Companies";
            return _dbConnection.Query<Company>(sql).ToList();
        }

        public void Remove(Company company) {
            string sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            _dbConnection.Execute(sql, new { Id = company.CompanyId });
        }

        public Company Update(Company company) {
            string sql = "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
            _dbConnection.Execute(sql, company);
            return company;
        }
    }
}
