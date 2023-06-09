﻿using Companies.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Tests.Extensions
{
    public static class ControllerExtensions
    {
        public static void SetUserIsAuthenticated(this ControllerBase controller, bool isAuthenticated)
        {
            var mockHttpContext = new Mock<HttpContext>();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            mockHttpContext.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(isAuthenticated);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var controllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
           
            controller.ControllerContext = controllerContext;
        }
    }
}
