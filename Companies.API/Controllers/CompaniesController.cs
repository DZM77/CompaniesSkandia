using AutoMapper;
using Companies.API.Data;
using Companies.API.DataTransferObjects;
using Companies.API.Entities;
using Companies.API.Repositories;
using Companies.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Companies.API.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public CompaniesController(IServiceManager serviceManager) => this.serviceManager = serviceManager;
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany(bool includeEmployees = false) => 
            Ok(await serviceManager.CompanyService.GetCompaniesAsync(includeEmployees));
        

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid id) => 
            Ok(await serviceManager.CompanyService.GetAsync(id));


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(Guid id, CompanyForUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Guid don't match");
            }

            //var companyFromDB = await _context.Companies.FirstOrDefaultAsync(c => c.Id.Equals(id));

            //if (companyFromDB is null)
            //    return NotFound();

            ////Add Employees if any new
            //mapper.Map(dto, companyFromDB);
            //await _context.SaveChangesAsync();

            //return Ok(mapper.Map<CompanyDto>(companyFromDB)); //For demo purpose
            await serviceManager.CompanyService.UpdateAsync(id, dto);

            return NoContent();
        }

        //// POST: api/Companies
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Company>> PostCompany(CompanyForCreationDto company)
        //{

        //    if (company is null)
        //    {
        //        return BadRequest();
        //    }

        //    var createdCompany = mapper.Map<Company>(company);
        //    //ToDo Register Employyes as Users with Role Employees

        //    _context.Companies.Add(createdCompany);
        //    await _context.SaveChangesAsync();

        //    var companyToReturn = mapper.Map<CompanyDto>(createdCompany);

        //    return CreatedAtAction(nameof(GetCompany), new { id = companyToReturn.Id }, companyToReturn);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await serviceManager.CompanyService.DeleteAsync(id);
            return NoContent();
        }
    }
}
