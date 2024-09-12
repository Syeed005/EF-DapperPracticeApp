using Dapper;
using EF_DapperPractice.Data;
using EF_DapperPractice.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EF_DapperPractice.Repository {
    public class CompanyRepositoryDapperAsync : ICompanyRepositoryAsync {
        //private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public CompanyRepositoryDapperAsync(IConfiguration configuration) {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<Company> CreateAsync(Company company) {
            string sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode) \r\nSELECT CAST(SCOPE_IDENTITY() as int);";
            //company.CompanyId = _dbConnection.Execute(sql, new {company.Name, company.Address, company.City, company.State, company.PostalCode});
            var result = await _dbConnection.QueryAsync<int>(sql, company);
            company.CompanyId = result.Single();
            return company;
        }

        public async Task<Company> GetAsync(int id) {
            string sql = "SELECT * FROM Companies WHERE CompanyId = @Id";
            var result = await _dbConnection.QueryAsync<Company>(sql, new { Id = id });
            return result.Single();
        }

        public async Task<List<Company>> GetAllAsync() {
            string sql = "SELECT * FROM Companies";
            var result = await _dbConnection.QueryAsync<Company>(sql);
            return result.ToList();
        }

        public async Task RemoveAsync(Company company) {
            string sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = company.CompanyId });
        }

        public async Task<Company> UpdateAsync(Company company) {
            string sql = "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
            await _dbConnection.ExecuteAsync(sql, company);
            return company;
        }

        Task ICompanyRepositoryAsync.SaveAsync() {
            throw new NotImplementedException();
        }
    }
}
