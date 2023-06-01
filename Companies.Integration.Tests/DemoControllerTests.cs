using Companies.API;
using Companies.API.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
//using Newtonsoft.Json;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Companies.API.Data;
using Companies.API.Entities;
using System.IO;
using System.Net.Http.Headers;
using Companies.API.Services;
using NuGet.Packaging.Signing;

namespace Companies.Integration.Tests
{
    public class DemoControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient httpClient;
        private readonly ApplicationDbContext context;

        public DemoControllerTests(CustomWebApplicationFactory<Program> applicationFactory)
        {
            applicationFactory.ClientOptions.BaseAddress = new Uri("https://localhost:7019/api/");
            httpClient = applicationFactory.CreateClient();
            context = applicationFactory.Context;
        }

        [Fact]
        public async Task Get_ShouldReturn_FromInMemoryDatabase()
        {
            var dto = await httpClient.GetFromJsonAsync<CompanyDto>("demo/getonefromservice");
            Assert.Equal("TestCompanyName", dto.Name);
          
        } 
                
        [Fact]
        public async Task GetAll_ShouldReturnAll()
        {
            var dtos = await httpClient.GetFromJsonAsync<IEnumerable<CompanyDto>>("demo/getall");
            Assert.Equal(context.Companies.Count(), dtos?.Count());
        } 
      
    }
}