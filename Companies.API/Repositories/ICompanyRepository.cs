using Companies.API.Entities;

namespace Companies.API.Repositories
{
    public interface ICompanyRepository
    {
        void DeleteCompany(Company company);
        Task<Company?> GetAsync(Guid id);
        Task<IEnumerable<Company>> GetCompaniesAsync(bool includeEmployees);
    }
}