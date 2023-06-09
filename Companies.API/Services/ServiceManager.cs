using AutoMapper;
using Companies.API.Configuration;
using Companies.API.Entities;
using Companies.API.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NServiceBus;

namespace Companies.API.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthenticationService> authenticationService;
        private readonly Lazy<ICompanyService> companyService;
        public ICompanyService CompanyService => companyService.Value;
        public IAuthenticationService AuthenticationService => authenticationService.Value;

        public ServiceManager(IUnitOfWork iUoW, IMapper mapper, IMessageSession messageSession, UserManager<User> userManager, IOptions<JwtConfigurations> options, IConfiguration configuration)
        {
            authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(mapper, userManager, options, configuration));
            companyService = new Lazy<ICompanyService>(() => new CompanyService(iUoW, mapper, messageSession, authenticationService));
        }
    }
}
