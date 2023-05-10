using Companies.API.Entities;

namespace Companies.API.Repositories
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesAsync(bool includeEmployees);
    }
}