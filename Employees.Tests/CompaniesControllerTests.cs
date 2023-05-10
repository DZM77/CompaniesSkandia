using Companies.API.Controllers;
using Microsoft.AspNetCore.Mvc;
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
    }
}
