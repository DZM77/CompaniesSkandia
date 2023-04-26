using AutoMapper;
using Companies.API.Data;
using Companies.API.DataTransferObjects;
using Companies.API.Paging;
using Companies.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,[FromQuery] ResourceParameters parameters)
        {
            var company = await context.Companies
                                      .Where(c => c.Id.Equals(companyId))
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync();

            if (company is null) return NotFound();

            var employees = string.IsNullOrWhiteSpace(parameters.SearchByName) ?
              context.Users.Where(e => e.CompanyId.Equals(companyId)) :
              context.Users.Where(e => e.CompanyId.Equals(companyId)).Where(e => e.Name!.StartsWith(parameters.SearchByName));

            var pagedResult = await PagedList<User>.CreateAsync(employees, parameters.PageNumber, parameters.PageSize);

            var employeeDtos = mapper.Map<IEnumerable<EmployeeDto>>(pagedResult);

            Response.Headers.Add("X-Pagination",
                   JsonSerializer.Serialize(pagedResult.MetaData));


            return Ok(employeeDtos);

        }

        

        [HttpGet("{employeeId:guid}")]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, Guid employeeId)
        {
            var company = await context.Companies
                                      .Where(c => c.Id.Equals(companyId))
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync();

            if (company is null) return NotFound();

            var employee = await context.Users.FirstOrDefaultAsync(e => e.CompanyId.Equals(companyId));

            if (employee is null) return NotFound();

            var employeeDto = mapper.Map<EmployeeDto>(employee);

            return Ok(employeeDto);

        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(
            Guid companyId,
            Guid id,
            JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("No patchdokument");

            var company = await context.Companies
                                        .FirstOrDefaultAsync(c => c.Id.Equals(companyId));

            if (company is null) return NotFound();

            var employeeToPatch = await context.Users.FirstOrDefaultAsync(e => e.Id.Equals(id));

            var dto = mapper.Map<EmployeeForUpdateDto>(employeeToPatch);

            patchDoc.ApplyTo(dto, ModelState);

            TryValidateModel(dto);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            mapper.Map(dto, employeeToPatch);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
