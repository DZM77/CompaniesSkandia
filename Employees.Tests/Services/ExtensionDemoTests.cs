using Companies.API.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Tests.Services
{
    public class ExtensionDemoTests
    {

        [Fact]
        public void DemoMethod_ShouldReturnValue()
        {
            const string expected = "from config";
            var iconfig = new Mock<IConfiguration>();
            var sut = new ExtensionServiceDemo(iconfig.Object);

            ConfigExtensions.Implementation = (c, k) => expected;

            var actual = sut.DemoMethod();

            Assert.Equal(expected, actual);
        }

        //public string DemoMethod(IConfiguration c, string k) 
        //{
        //    return "from config";
        //}
    }
}
