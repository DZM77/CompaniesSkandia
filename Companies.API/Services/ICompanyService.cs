using Companies.API.DataTransferObjects;

namespace Companies.API.Services
{
    public interface ICompanyService
    {
        Task DeleteAsync(Guid id);
        Task<CompanyDto> GetAsync(Guid id);
        Task<IEnumerable<CompanyDto>> GetCompaniesAsync(bool includeEmployees);
        Task UpdateAsync(Guid id, CompanyForUpdateDto dto);
    }
}