//using AspNetCore;
//using AspNetCore;
using Dapper;
using EF_DapperPractice.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace EF_DapperPractice.Repository {
    public class BonusRepository : IBonusRepository {
        private readonly IDbConnection _dbConnection;

        public BonusRepository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public void CreateTestCompany(Company company) {
            string sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode) \r\nSELECT CAST(SCOPE_IDENTITY() as int);";
            company.CompanyId = _dbConnection.Query<int>(sql, company).Single();

            string sqlEmp = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId) \r\nSELECT CAST(SCOPE_IDENTITY() as int);";
            //foreach (Employee employee in company.Employees) {
            //    employee.CompanyId = company.CompanyId;
            //    _dbConnection.Query<int>(sqlEmp, employee).Single();
            //}

            company.Employees.Select(x => {
                x.CompanyId = company.CompanyId;
                return x;
                }).ToList();

            _dbConnection.Execute(sqlEmp, company.Employees);

            //company.CompanyId = _dbConnection.Execute(sql, new {company.Name, company.Address, company.City, company.State, company.PostalCode});

        }

        public void CreateTestCompanyWithTransaction(Company company) {

            using (var transaction = new TransactionScope()) {
                try {
                    string sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode) \r\nSELECT CAST(SCOPE_IDENTITY() as int);";
                    company.CompanyId = _dbConnection.Query<int>(sql, company).Single();

                    string sqlEmp = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId) \r\nSELECT CAST(SCOPE_IDENTITY() as int);";

                    company.Employees.Select(x => {
                        x.CompanyId = company.CompanyId;
                        return x;
                    }).ToList();

                    _dbConnection.Execute(sqlEmp, company.Employees);

                    transaction.Complete();
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }            
        }

        public List<Company> FilterCompanyByName(string name) {
            string sql = "select * from [DapperTraining].[dbo].[Companies] where Name like '%'+@Name+'%'";
            return  _dbConnection.Query<Company>(sql, new {Name = name}).ToList();          
        }

        public List<Company> GetAllCompanyWithEmployees()
        {
            string sql = "select C.*, E.* from [DapperTraining].[dbo].[Companies] as C \r\n inner join Employees as E on E.CompanyId = C.CompanyId";
            Dictionary<int, Company> uniqueCompany = new Dictionary<int, Company>();
            var company = _dbConnection.Query<Company, Employee, Company>(sql, (c, e) =>
            {
                if (!uniqueCompany.TryGetValue(c.CompanyId, out var currentCompany))
                {
                    currentCompany = c;
                    uniqueCompany.Add(currentCompany.CompanyId, currentCompany);
                }
                currentCompany.Employees.Add(e);
                return currentCompany;
            }, splitOn: "EmployeeId");
            return company.Distinct().ToList();
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

        public Company GetCompanyWithEmployees(int id)
        {
            //var p = new
            //{
            //    CompanyId = id
            //};
            string sql = "select * from [DapperTraining].[dbo].[Companies] where CompanyId = @CompanyId;" +
                "select * from [DapperTraining].[dbo].[Employees] where CompanyId = @CompanyId;";

            Company company;
            using (var lists = _dbConnection.QueryMultiple(sql, new {CompanyId = id}))
            {
                company = lists.Read<Company>().ToList().FirstOrDefault();
                company.Employees = lists.Read<Employee>().ToList();
            }

            return company;
        }

        public async Task<Company> GetCompanyWithEmployeesAsync(int id) {
            //var p = new
            //{
            //    CompanyId = id
            //};
            string sql = "select * from [DapperTraining].[dbo].[Companies] where CompanyId = @CompanyId;" +
                "select * from [DapperTraining].[dbo].[Employees] where CompanyId = @CompanyId;";

            Company company;
            using (var lists = await _dbConnection.QueryMultipleAsync(sql, new { CompanyId = id })) {
                company = lists.Read<Company>().ToList().FirstOrDefault();
                company.Employees = lists.Read<Employee>().ToList();
            }

            return company;
        }

        public void RemoveRange(int[] companies) {
            string sql = "Delete from [DapperTraining].[dbo].[Companies] where CompanyId in @Companies";
            _dbConnection.Query(sql, new { Companies = companies });
        }
    }
}
