using Companies.API.Controllers;
using Companies.API.Data;
using Companies.API.Paging;
using Employees.Tests.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
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

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpResponse = new Mock<HttpResponse>();
            var mockHeaderDictionary = new Mock<IHeaderDictionary>();

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(r => r.Headers).Returns(mockHeaderDictionary.Object);


            var res = await sut.GetEmployeesForCompany(expectedCompanyId, new ResourceParameters());

            Assert.NotNull(res);
            Assert.IsType<OkObjectResult>(res);
            mockHeaderDictionary.Verify(h => h.Add("X-Pagination", It.IsAny<StringValues>()), Times.Once);
        }
    }
}
