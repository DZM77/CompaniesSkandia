using Companies.API.Data;
using Companies.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Companies.API.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmplyeesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public EmplyeesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
        {
            var company = await context.Companies
                                      .Where(c => c.Id.Equals(companyId))
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync();

            if (company is null) return NotFound();

            var employees = await context.Employees.Where(e => e.CompanyId.Equals(companyId)).ToListAsync();

            var employeeDtos = employees.Select(e => new EmployeeDto()
            {
                Id = e.Id,
                Name = e.Name,
                Age = e.Age,
                Position = e.Position
            });

            return Ok(employeeDtos);

        }
    }
}
