using AutoMapper;
using Companies.API;
using Companies.API.Controllers;
using Companies.API.DataTransferObjects;
using Companies.API.Entities;
using Companies.API.Repositories;
using Employees.Tests.Extensions;
using Employees.Tests.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Tests.Controlers
{
    public class CompaniesControllerTests : IClassFixture<CompaniesControllerFixture>
    {
        //private Mock<ICompanyRepository> mockRepo;
        //private CompaniesController controller;
        private readonly CompaniesControllerFixture fixture;
        #region old tests with mocked ClaimsPrincipal
        //[Fact]
        //public async Task GetCompany_ShouldReturn200OK()
        //{
        //    var sut = new CompaniesController();
        //    sut.SetUserIsAuthenticated(true);

        //    var result = await sut.GetCompany();
        //    var resultType = result.Result as OkResult;

        //    Assert.IsType<OkResult>(resultType);
        //    Assert.Equal(200, resultType.StatusCode);
        //}

        //[Fact]
        //public async Task GetCompany_IfNotAuthenticated_ShouldReturn400BadRequest()
        //{

        //    var sut = new CompaniesController();
        //    sut.SetUserIsAuthenticated(false);

        //    var result = await sut.GetCompany();
        //    var resultType = result.Result as BadRequestObjectResult;

        //    Assert.IsType<BadRequestObjectResult>(resultType);
        //   // Assert.Equal(400, resultType.StatusCode);
        //}

        //[Fact]
        //public async Task GetCompany_IfNotAuthenticated_ShouldReturn400BadRequest2()
        //{
        //    var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        //    mockClaimsPrincipal.SetupGet(c => c.Identity.IsAuthenticated).Returns(false);

        //    var sut = new CompaniesController();
        //    sut.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = new DefaultHttpContext()
        //        {
        //            User = mockClaimsPrincipal.Object
        //        }
        //    };

        //    var result = await sut.GetCompany();
        //    var resultType = result.Result as BadRequestObjectResult;

        //    Assert.IsType<BadRequestObjectResult>(resultType);
        //    Assert.Equal(400, resultType.StatusCode);
        //}
        #endregion

        public CompaniesControllerTests(CompaniesControllerFixture fixture) 
        {
            this.fixture = fixture;
            //mockRepo = new Mock<ICompanyRepository>();
            //var mockUoW = new Mock<IUnitOfWork>();
            //mockUoW.Setup(u => u.CompanyRepository).Returns(mockRepo.Object);

            //var mapper = new Mapper(new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<MapperProfile>();
            //}));

            //var mockUserStore = new Mock<IUserStore<User>>();
            //var userManager = new UserManager<User>(mockUserStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            //controller = new CompaniesController(mapper, userManager, mockUoW.Object);
        }

        [Fact]
        public async Task GetCompany_SholudReturnAllCompanies()
        {

            var companies = GetCompanys();
            fixture.MockRepo.Setup(m => m.GetCompaniesAsync(false)).ReturnsAsync(companies);

            var result = await fixture.Controller.GetCompany();

            //var resultType = result.Result as OkObjectResult;

            //Assert.IsType<OkObjectResult>(resultType);
            //Assert.Equal(StatusCodes.Status200OK, resultType.StatusCode);

            //var items = resultType.Value as List<CompanyDto>;
            //Assert.IsType<List<CompanyDto>>(items);

            //Assert.Equal(items.Count(), companies.Count);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsType<List<CompanyDto>>(okResult.Value);
            Assert.Equal(items.Count, companies.Count);

        }

        [Fact]
        public async Task GetCompany_WhenNotFound_ShouldReturnNotFound()
        {
            var nonExistingGuid = Guid.NewGuid();
           fixture.MockRepo.Setup(m => m.GetAsync(nonExistingGuid)).ReturnsAsync(() => null);

            var result = await fixture.Controller.GetCompany(nonExistingGuid);

            Assert.IsType<NotFoundResult>(result.Result);

        }

        private List<Company> GetCompanys()
        {
            return new List<Company>
            {
                new Company
                {
                     Id = Guid.NewGuid(),
                     Name = "Test",
                     Address = "Ankeborg, Sweden",
                     Employees = new List<User>()
                },
                 new Company
                {
                     Id = Guid.NewGuid(),
                     Name = "Test",
                     Address = "Ankeborg, Sweden",
                     Employees = new List<User>()
                }
            };

        }

        public void Dispose()
        {
            //Not used here...
        }


    }
}
