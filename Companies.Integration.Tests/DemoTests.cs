using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace Companies.Integration.Tests
{
    public class DemoTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient httpClient;

        public DemoTests(WebApplicationFactory<Program> applicationFactory)
        {
                httpClient = applicationFactory.CreateDefaultClient();
        }

        [Fact]
        public async Task Index_ShouldReturnOk()
        {
            var response = await httpClient.GetAsync("api/demo");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}