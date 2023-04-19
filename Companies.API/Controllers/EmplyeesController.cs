using AutoMapper;
using Companies.API.Data;
using Companies.API.DataTransferObjects;
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
        private readonly IMapper mapper;

        public EmplyeesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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

            //var employeeDtos = employees.Select(e => new EmployeeDto()
            //{
            //    Id = e.Id,
            //    Name = e.Name,
            //    Age = e.Age,
            //    Position = e.Position
            //});

            var employeeDtos = mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(employeeDtos);

        }
    }
}
