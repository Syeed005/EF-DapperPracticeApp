using Dapper;
using EF_DapperPractice.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EF_DapperPractice.Repository
{
    public class EmployeeRepositoryDapperAsync
    {
        private readonly IDbConnection _dbConnection;

        public EmployeeRepositoryDapperAsync(IConfiguration configuration) {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<Employee> CreateAsync(Employee employee) {
            string sql = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId) \r\nSELECT CAST(SCOPE_IDENTITY() as int);";
            //company.CompanyId = _dbConnection.Execute(sql, new {company.Name, company.Address, company.City, company.State, company.PostalCode});
            
            var result = await _dbConnection.QueryAsync<int>(sql, employee);
            employee.CompanyId = result.Single();
            
            return employee;
        }

        public Employee Get(int id) {
            string sql = "SELECT * FROM Employees WHERE EmployeeId = @Id";
            return _dbConnection.Query<Employee>(sql, new { Id = id }).Single();
        }

        public List<Employee> GetAll() {
            string sql = "SELECT * FROM Employees";
            return _dbConnection.Query<Employee>(sql).ToList();
        }

        public void Remove(Employee employee) {
            string sql = "DELETE FROM Employees WHERE EmployeeId = @Id";
            _dbConnection.Execute(sql, new { Id = employee.EmployeeId });
        }

        public Employee Update(Employee employee) {
            string sql = "UPDATE Employees SET Name = @Name, Title = @Title, Email = @Email, Phone = @Phone, CompanyId = @CompanyId WHERE EmployeeId = @EmployeeId";
            _dbConnection.Execute(sql, employee);
            return employee;
        }
    }
}
