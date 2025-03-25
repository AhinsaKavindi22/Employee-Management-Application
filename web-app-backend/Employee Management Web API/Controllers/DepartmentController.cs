using Employee_Management_Web_API.Data;
using Employee_Management_Web_API.DTO;
using Employee_Management_Web_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly EmployeeContext _employeeContext;

        public DepartmentController(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        //{
        //    // IEnumrable is used for return list of employees
        //    if (_employeeContext.Departments == null)
        //    {
        //        return NotFound();
        //    }
        //    return await _employeeContext.Departments.ToListAsync();
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            if (_employeeContext.Departments == null)
            {
                return NotFound();
            }

            var departments = await _employeeContext.Departments
                .Include(d => d.Employees) // Join Employees table
                .Select(d => new DepartmentDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Employees = d.Employees.Select(emp => new EmployeeDTO
                    {
                        Id = emp.Id,
                        Name = emp.Name,
                        Age = emp.Age,
                        IsActive = emp.IsActive,
                        DepartmentName = emp.Department != null ? emp.Department.Name : "No Department" // Handle null department
                    }).ToList()
                })
                .ToListAsync();

            return Ok(departments);
        }
    }
}
