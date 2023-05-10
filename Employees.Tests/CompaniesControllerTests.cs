using Companies.API.Controllers;
using Employees.Tests.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Tests
{
    public class CompaniesControllerTests
    {
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

    }
}
