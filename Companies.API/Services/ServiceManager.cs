using AutoMapper;
using Companies.API.Repositories;

namespace Companies.API.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> companyService;
        public ICompanyService CompanyService => companyService.Value;

        public ServiceManager(IUnitOfWork iUoW, IMapper mapper)
        {
            companyService = new Lazy<ICompanyService>(() => new CompanyService(iUoW, mapper));
        }
    }
}
