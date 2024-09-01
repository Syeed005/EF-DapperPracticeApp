using EF_DapperPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_DapperPractice.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {
            
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //Write fluent api configuration here

            //property configuration
            modelBuilder.Entity<Company>().Ignore(t => t.Employees);

            modelBuilder.Entity<Employee>().HasOne(c => c.Company).WithMany(e => e.Employees).HasForeignKey(c => c.CompanyId);
        }
    }
}
