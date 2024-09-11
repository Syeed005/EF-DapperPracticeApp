using Dapper;
using EF_DapperPractice.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EF_DapperPractice.Repository {
    public class BonusRepository : IBonusRepository {
        private readonly IDbConnection _dbConnection;

        public BonusRepository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public List<Employee> GetAllEmployeedWithCompany(int id)
        {
            string sql = "select E.*, C.* from [DapperTraining].[dbo].[Employees] as E \r\n inner join Companies as C on E.CompanyId = C.CompanyId";
            if (id != 0)
            {
                sql += " and E.companyId = @Id";
            }
            var employees = _dbConnection.Query<Employee, Company, Employee>(sql, (e, c) =>
            {
                e.Company = c;
                return e;
            }, new { id},splitOn: "CompanyId");
            return employees.ToList();
        }
    }
}
