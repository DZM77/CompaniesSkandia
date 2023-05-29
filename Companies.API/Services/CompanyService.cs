using AutoMapper;
using Companies.API.DataTransferObjects;
using Companies.API.Repositories;

namespace Companies.API.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetCompaniesAsync(bool includeEmployees)
        {
            //Maybe some logic here... 
            var companies = await unitOfWork.CompanyRepository.GetCompaniesAsync(includeEmployees);
            var dtos = mapper.Map<IEnumerable<CompanyDto>>(companies);
            return dtos;
        }
    }
}
