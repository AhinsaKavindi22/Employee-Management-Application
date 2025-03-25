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

        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment(Department department)
        {
            _employeeContext.Departments.Add(department);
            await _employeeContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDepartments), new { id = department.Id }, department);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetEmployeeById(int id)
        {
            if (_employeeContext.Employees == null)
            {
                return NotFound();
            }

            var department = await _employeeContext.Departments
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
                .FirstOrDefaultAsync();

            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditDepartment(int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }


            _employeeContext.Entry(department).State = EntityState.Modified;
            try
            {
                await _employeeContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            if (_employeeContext.Departments == null)
            {
                return NotFound();
            }

            var department = await _employeeContext.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            // Check if there are any employees assigned to this department
            var employeesInDepartment = await _employeeContext.Employees
                .AnyAsync(e => e.DepartmentId == department.Id);

            // If there are employees in the department, return a message
            if (employeesInDepartment)
            {
                return BadRequest("Cannot delete department because there are employees assigned to it.");
            }

            _employeeContext.Departments.Remove(department);
            await _employeeContext.SaveChangesAsync();

            return Ok();

        }
    }
}
