using MoqConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqDemoTests
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData(1,2)]
        [InlineData(3,4)]
        [InlineData(5,6)]
        public void Add_ShouldReturnSum(int nr1 , int nr2)
        {
            var sut = new Calculator();
            var res = sut.Add(nr1, nr2);
            Assert.Equal(nr1 + nr2, res);
        }
    }
}
