using Employee_Management_Web_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_Web_API.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options){}

        // enable us to access this model from database
        public DbSet<Employee> Employees { get; set; }
    }
   
}
