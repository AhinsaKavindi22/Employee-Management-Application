using Employee_Management_Web_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_Web_API.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department
                {
                    Id = 1,
                    Name = "IT"
                },
                new Department
                {
                    Id = 2,
                    Name = "HR"
                },
                new Department
                {
                    Id = 3,
                    Name = "Payroll"
                }
            );
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "John",
                    Age = "30",
                    IsActive = 1,
                    DepartmentId = 1
                },
                new Employee
                {
                    Id = 2,
                    Name = "Smith",
                    Age = "25",
                    IsActive = 1,
                    DepartmentId = 2
                },
                new Employee
                {
                    Id = 3,
                    Name = "Peter",
                    Age = "35",
                    IsActive = 1,
                    DepartmentId = 3
                }
            );
            
        }

        // enable us to access this model from database
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<User> Users { get; set; }
    }

}
