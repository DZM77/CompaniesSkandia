using Companies.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Tests
{
    public class CompaniesControllerTests
    {
        [Fact]
        public async Task GetCompany_ShouldReturn200OK()
        {
            var sut = new CompaniesController();

            var result = await sut.GetCompany();
            var resultType = result.Result as OkResult;

            Assert.IsType<OkResult>(resultType);
            Assert.Equal(200, resultType.StatusCode);
        }

        [Fact]
        public async Task GetCompany_IfNotAuthenticated_ShouldReturn400BadRequest()
        {
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(false);

            var controllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };


            var sut = new CompaniesController();
            sut.ControllerContext= controllerContext; 



            var result = await sut.GetCompany();
            var resultType = result.Result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(resultType);
           // Assert.Equal(400, resultType.StatusCode);
        }
    }
}
