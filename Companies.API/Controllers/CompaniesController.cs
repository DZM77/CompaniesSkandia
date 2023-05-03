using AutoMapper;
using Companies.API.Data;
using Companies.API.DataTransferObjects;
using Companies.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Companies.API.Controllers
{
    [Route("api/companies")]
    [ApiController]
    //[Authorize(Roles ="Admin")]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public CompaniesController(ApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany(bool includeEmployees = false)
        {
            var user = await userManager.GetUserAsync(User);
            var user2 = await userManager.FindByIdAsync(user.Id);
            var user3 = await userManager.FindByNameAsync(user.UserName);
            var isInRole = await userManager.IsInRoleAsync(user, "Admin");

            if (User.IsInRole("Admin"))
            {
                //Do something
            }

            if (User.Identity.IsAuthenticated)
            {
                return Ok("User is authenticated");
            }
            else
                return Ok("User is not authenticated");



            var dtos = includeEmployees ? mapper.Map<IEnumerable<CompanyDto>>(await _context.Companies.Include(c => c.Employees).ToListAsync())
                                     : mapper.Map<IEnumerable<CompanyDto>>(await _context.Companies.ToListAsync());

            return Ok(dtos);
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(Guid id)
        {
            
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

           var companyDto = mapper.Map<CompanyDto>(company);

            return Ok(companyDto);
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(Guid id, CompanyForUpdateDto dto)
        {
            if (id != dto.Id)
            {
                //return new ProblemDetails()
                //{
                     
                //};
                return BadRequest("Guid don't match");
            }

            // _context.Entry(company).State = EntityState.Modified;

            var companyFromDB = await _context.Companies.FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (companyFromDB is null)
                return NotFound();

            //Add Employees if any new
            mapper.Map(dto, companyFromDB);
            await _context.SaveChangesAsync();

            return Ok(mapper.Map<CompanyDto>(companyFromDB)); //For demo purpose

            return NoContent();
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(CompanyForCreationDto company)
        {

            if (company is null)
            {
                return BadRequest();
            }
          
            var createdCompany = mapper.Map<Company>(company);
            //ToDo Register Employyes as Users with Role Employees

            _context.Companies.Add(createdCompany);
            await _context.SaveChangesAsync();

            var companyToReturn = mapper.Map<CompanyDto>(createdCompany);
  
            return CreatedAtAction(nameof(GetCompany), new { id = companyToReturn.Id }, companyToReturn);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
           
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(Guid id)
        {
            return (_context.Companies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
