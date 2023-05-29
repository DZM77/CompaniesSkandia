using AutoMapper;
using Companies.API;
using Companies.API.Controllers;
using Companies.API.Entities;
using Companies.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Tests.Fixtures
{
    public class CompaniesControllerFixture : IDisposable
    {
        public Mock<ICompanyRepository> MockRepo { get; }
        public CompaniesController Controller { get; }

        public CompaniesControllerFixture()
        {
            MockRepo = new Mock<ICompanyRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(u => u.CompanyRepository).Returns(MockRepo.Object);

            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            }));

            var mockUserStore = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(mockUserStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            Controller = new CompaniesController(mapper, userManager, mockUoW.Object);
        }

        public void Dispose()
        {
            //Nothing to do here
        }
    }
}
