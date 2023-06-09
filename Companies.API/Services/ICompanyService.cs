﻿using Companies.API.DataTransferObjects;

namespace Companies.API.Services
{
    public interface ICompanyService
    {
        Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);
        Task DeleteAsync(Guid id);
        Task<CompanyDto> GetAsync(Guid id);
        Task<IEnumerable<CompanyDto>> GetCompaniesAsync(bool includeEmployees);
        Task UpdateAsync(Guid id, CompanyForUpdateDto dto);
    }
}