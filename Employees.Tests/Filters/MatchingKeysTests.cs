using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Companies.API.Controllers;
using Castle.Components.DictionaryAdapter;
using Companies.API.Filters;
using Companies.API.DataTransferObjects;

namespace Employees.Tests.Filters
{
    public class MatchingKeysTests
    {
        private MatchingKeysFilter filter;

        public MatchingKeysTests()
        {
            filter = new MatchingKeysFilter();
            
        }

        [Fact]
        public void OnActionExecuting_ValidDtoAndId_ReturnsNoResult()
        {
            // Arrange
            var context = GetContext<object>(new Dictionary<string, object> { { "id", new Guid() }, {"CompanyForUpdateDto", new CompanyForUpdateDto() } });

            // Act
            filter.OnActionExecuting(context);

            // Assert
            Assert.Null(context.Result);
        }


        [Fact]
        public void OnActionExecuting_DtoAndIdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var context = GetContext<object>(new Dictionary<string, object>());

            context.RouteData.Values["id"] = "123";
            context.ActionArguments["yourDto"] = new CompanyForUpdateDto() { Id = new Guid() };

            // Act
            filter.OnActionExecuting(context);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(context.Result);
            Assert.Equal("Guid don't match", result.Value);
        } 
        
        [Fact]
        public void OnActionExecuting_DtoAndIdMatch_ReturnsNoResult()
        {
            // Arrange
            var context = GetContext<object>(new Dictionary<string, object>());

            var guid = new Guid();
            context.RouteData.Values["id"] = guid;
            context.ActionArguments["yourDto"] = new CompanyForUpdateDto() { Id = guid };

            // Act
            filter.OnActionExecuting(context);

            // Assert
            Assert.Null(context.Result);
        }

        private static ActionExecutingContext GetContext<T>(Dictionary<string, T> keyValuePairs)
        {
            var routeValueDictionary = new RouteValueDictionary();

            foreach (var keyValue in keyValuePairs)
            {
                routeValueDictionary.Add(keyValue.Key, keyValue.Value);
            }

            var routeData = new RouteData(routeValueDictionary);


            var actionContext = new ActionContext(Mock.Of<HttpContext>(),
                                                  routeData,
                                                  Mock.Of<ActionDescriptor>());

            var mockController = new Mock<CompaniesController>();

            var actionExcecutingContext = new ActionExecutingContext(actionContext,
                                                                    new List<IFilterMetadata>(),
                                                                    routeValueDictionary,
                                                                    mockController);

            return actionExcecutingContext;

        }
    }
}



