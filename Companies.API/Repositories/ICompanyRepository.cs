using Companies.API.Entities;

namespace Companies.API.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company?> GetAsync(Guid id);
        Task<IEnumerable<Company>> GetCompaniesAsync(bool includeEmployees);
    }
}