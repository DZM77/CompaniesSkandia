using AutoMapper;
using Companies.API.DataTransferObjects;
using Companies.API.Exceptions;
using Companies.API.Repositories;
using Notify.Messages;
using NServiceBus;

namespace Companies.API.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IMessageSession messageSession;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper, IMessageSession messageSession)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.messageSession = messageSession;
        }

        public async Task DeleteAsync(Guid id)
        {
            var company = await unitOfWork.CompanyRepository.GetAsync(id);

            if (company == null) throw new CompanyNotFoundException(id);

            unitOfWork.CompanyRepository.DeleteCompany(company);
            await unitOfWork.CompleteAsync();
        }

        public async Task<CompanyDto> GetAsync(Guid id)
        {
            var company = await unitOfWork.CompanyRepository.GetAsync(id);

            if (company == null) throw new CompanyNotFoundException(id);

            var companyDto = mapper.Map<CompanyDto>(company);

            return companyDto;
        }

        public async Task<IEnumerable<CompanyDto>> GetCompaniesAsync(bool includeEmployees)
        {
            
            var companies = await unitOfWork.CompanyRepository.GetCompaniesAsync(includeEmployees);
            var dtos = mapper.Map<IEnumerable<CompanyDto>>(companies);

            var message = new CompanyControllerMessage
            {
                Message = "Hej"
            };
            await messageSession.Send(message).ConfigureAwait(false);

            return dtos;
        }

        public async Task UpdateAsync(Guid id, CompanyForUpdateDto dto)
        {
            var company = await unitOfWork.CompanyRepository.GetAsync(id);

            if (company is null) throw new CompanyNotFoundException(id);

            mapper.Map(dto, company);
            await unitOfWork.CompleteAsync();
        }
    }
}
