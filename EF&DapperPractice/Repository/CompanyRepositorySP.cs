using Dapper;
using EF_DapperPractice.Data;
using EF_DapperPractice.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EF_DapperPractice.Repository {
    public class CompanyRepositorySP : ICompanyRepository {
        //private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public CompanyRepositorySP(IConfiguration configuration) {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Create(Company company) {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", 0, DbType.Int32, ParameterDirection.Output);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            _dbConnection.Execute("usp_AddCompany", parameters, commandType: CommandType.StoredProcedure);
            company.CompanyId = parameters.Get<int>("@CompanyId");
            return company;
        }

        public Company Get(int id) {
            return _dbConnection.Query<Company>("usp_GetCompany", new { @CompanyId = id }, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public List<Company> GetAll() {
            return _dbConnection.Query<Company>("usp_GetALLCompany", commandType:CommandType.StoredProcedure).ToList();
        }

        public void Remove(Company company) {
            _dbConnection.Execute("usp_RemoveCompany", new { @CompanyId = company.CompanyId });
        }

        public Company Update(Company company) {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", company.CompanyId, DbType.Int32);
            parameters.Add("@Name", company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@City", company.City);
            parameters.Add("@State", company.State);
            parameters.Add("@PostalCode", company.PostalCode);
            _dbConnection.Execute("usp_AddCompany", parameters, commandType: CommandType.StoredProcedure);
            return company;
        }
    }
}
