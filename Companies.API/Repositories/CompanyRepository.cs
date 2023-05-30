using AutoMapper;
using Companies.API.Data;
using Companies.API.DataTransferObjects;
using Companies.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Companies.API.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext context;

        public CompanyRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void DeleteCompany(Company company)
        {
            context.Companies.Remove(company);
        }

        public async Task<Company?> GetAsync(Guid id)
        {
            return await context.Companies.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(bool includeEmployees)
        {
            return includeEmployees ? await context.Companies.Include(c => c.Employees).ToListAsync()
                                     : await context.Companies.ToListAsync();
        }
    }
}
