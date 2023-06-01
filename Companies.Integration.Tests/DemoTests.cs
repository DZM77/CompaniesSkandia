using Companies.API;
using Companies.API.DataTransferObjects;
using Companies.API.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Companies.Integration.Tests
{
    public class DemoTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient httpClient;
        private IServiceManager serviceManager;

        public DemoTests(WebApplicationFactory<Program> applicationFactory)
        {
            applicationFactory.ClientOptions.BaseAddress = new Uri("https://localhost:7019/api/");
            httpClient = applicationFactory.CreateClient();

            var scope = applicationFactory.Services.CreateScope();
            serviceManager = scope.ServiceProvider.GetRequiredService<IServiceManager>();

        }

        [Fact]
        public async Task Index_ShouldReturnOk()
        {
            var response = await httpClient.GetAsync("demo");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task Index_ShouldReturnExpectedMessage()
        {
            var response = await httpClient.GetAsync("demo");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("Working", content);
            Assert.Equal("text/plain", response.Content.Headers.ContentType.MediaType);

        }

        [Fact]
        public async Task Index2_ShouldReturnExpectedMediaType()
        {
            var response = await httpClient.GetAsync("demo/dto");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var dto = JsonSerializer.Deserialize<CompanyDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal("Working", dto.Name);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

        }

        [Fact]
        public async Task Index3_ShouldReturnExpectedMessage_WithStream()
        {
            var response = await httpClient.GetStreamAsync("demo/dto");

            var dto = await JsonSerializer.DeserializeAsync<CompanyDto>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal("Working", dto.Name);

        }

        [Fact]
        public async Task Index4_ShouldReturnExpectedMessageSimplifyed()
        {
            var dto = await httpClient.GetFromJsonAsync<CompanyDto>("demo/dto");
            Assert.Equal("Working", dto.Name);

        }

        [Fact]
        public async Task GetTokenAndTryToGetResponse()
        {
            //Ska inte göras för varje test såklart. 
            var res = await httpClient.PostAsJsonAsync("authentication/login", new UserForAuthenticationDto { UserName = "kalle@anka.com", Password = "ABC" });
            var tokenString = await res.Content.ReadAsStringAsync();
            var tokenDto =   JsonSerializer.Deserialize<TokenDto>(tokenString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var request = new HttpRequestMessage(HttpMethod.Get, "companies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.AccessToken);
            
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var dtos = JsonSerializer.Deserialize<IEnumerable<CompanyDto>>(content);

            Assert.IsAssignableFrom<IEnumerable<CompanyDto>>(dtos);
        } 
        
        [Fact]
        public async Task GetTokenAndTryToGetResponse2()
        {
            await serviceManager.AuthenticationService.ValidateUserAsync(new UserForAuthenticationDto { UserName = "kalle@anka.com", Password = "ABC" });
            var token = await serviceManager.AuthenticationService.CreateTokenAsync(false);

            var request = new HttpRequestMessage(HttpMethod.Get, "companies");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var response = await httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var dtos = JsonSerializer.Deserialize<IEnumerable<CompanyDto>>(content);

            Assert.IsAssignableFrom<IEnumerable<CompanyDto>>(dtos);
        }


       
    }
}