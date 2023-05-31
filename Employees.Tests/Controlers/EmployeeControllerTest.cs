using Companies.API.Controllers;
using Companies.API.Data;
using Employees.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Tests.Controlers
{
    public class EmployeeControllerTest : IClassFixture<DataBaseFixture>
    {
        private readonly DataBaseFixture fixture;
        private ApplicationDbContext context;

        public EmployeeControllerTest(DataBaseFixture fixture)
        {
            context = fixture.Context;
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetEmployeesForCompany_CompanyDontExist_ShouldReturnNotFound()
        {
            var notExistingId = new Guid();
            var sut = new EmplyeesController(context, fixture.Mapper);

            var res = await sut.GetEmployeesForCompany(notExistingId, null);

            Assert.NotNull(res);
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async Task GetEmployeesForCompany_CompanyExist_ShouldReturnOk()
        {
            var expectedCompanyId = context.Companies.First().Id;
            var sut = new EmplyeesController(context, fixture.Mapper);

            var res = await sut.GetEmployeesForCompany(expectedCompanyId, null);

            Assert.NotNull(res);
            Assert.IsType<OkObjectResult>(res);
        }
    }
}
