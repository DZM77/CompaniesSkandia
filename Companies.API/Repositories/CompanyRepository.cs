using AutoMapper;
using Companies.API.Data;
using Companies.API.DataTransferObjects;
using Companies.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Companies.API.Repositories
{
    public class CompanyRepository
    {
        private readonly ApplicationDbContext context;

        public CompanyRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(bool includeEmployees)
        {
            return includeEmployees ? await context.Companies.Include(c => c.Employees).ToListAsync()
                                     : await context.Companies.ToListAsync();
        }
    }
}
