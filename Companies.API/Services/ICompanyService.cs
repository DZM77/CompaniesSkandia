using Companies.API.DataTransferObjects;

namespace Companies.API.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetCompaniesAsync(bool includeEmployees);
    }
}