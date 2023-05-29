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
    //[Authorize(Roles ="Admin")]
   // [Authorize(Policy ="Test")]
    public class CompaniesController : ControllerBase
    {
      //  private readonly ApplicationDbContext _context;
        //private readonly IMapper mapper;
        //private readonly UserManager<User> userManager;
        //private readonly IUnitOfWork unitOfWork;
        private readonly IServiceManager serviceManager;
       // private readonly ICompanyService companyService;

        public CompaniesController(/*IMapper mapper, UserManager<User> userManager, IUnitOfWork unitOfWork, */IServiceManager serviceManager)
        {
           // _context = context;
            //this.mapper = mapper;
            //this.userManager = userManager;
            //this.unitOfWork = unitOfWork;
            this.serviceManager = serviceManager;
           // this.companyService = companyService;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany(bool includeEmployees = false)
        {
            var dtos = await serviceManager.CompanyService.GetCompaniesAsync(includeEmployees);
           // var dtos = mapper.Map<IEnumerable<CompanyDto>>(companies);

            //return Ok();
            return Ok(dtos);
        }



        //// GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid id)
        {

            var companyDto = await serviceManager.CompanyService.GetAsync(id);

            if (companyDto == null)
            {
                return NotFound();
            }

            return Ok(companyDto);
        }

        //// PUT: api/Companies/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCompany(Guid id, CompanyForUpdateDto dto)
        //{
        //    if (id != dto.Id)
        //    {
        //        //return new ProblemDetails()
        //        //{

        //        //};
        //        return BadRequest("Guid don't match");
        //    }

        //    // _context.Entry(company).State = EntityState.Modified;

        //    var companyFromDB = await _context.Companies.FirstOrDefaultAsync(c => c.Id.Equals(id));

        //    if (companyFromDB is null)
        //        return NotFound();

        //    //Add Employees if any new
        //    mapper.Map(dto, companyFromDB);
        //    await _context.SaveChangesAsync();

        //    return Ok(mapper.Map<CompanyDto>(companyFromDB)); //For demo purpose

        //    return NoContent();
        //}

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

        //// DELETE: api/Companies/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCompany(Guid id)
        //{

        //    var company = await _context.Companies.FindAsync(id);
        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Companies.Remove(company);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CompanyExists(Guid id)
        //{
        //    return (_context.Companies?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
