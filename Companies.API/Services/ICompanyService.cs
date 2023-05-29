using Companies.API.DataTransferObjects;

namespace Companies.API.Services
{
    public interface ICompanyService
    {
        Task<CompanyDto> GetAsync(Guid id);
        Task<IEnumerable<CompanyDto>> GetCompaniesAsync(bool includeEmployees);
    }
}