using AutoMapper;
using Companies.API.Repositories;
using NServiceBus;

namespace Companies.API.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> companyService;
        public ICompanyService CompanyService => companyService.Value;

        public ServiceManager(IUnitOfWork iUoW, IMapper mapper, IMessageSession messageSession)
        {
            companyService = new Lazy<ICompanyService>(() => new CompanyService(iUoW, mapper, messageSession));
        }
    }
}
