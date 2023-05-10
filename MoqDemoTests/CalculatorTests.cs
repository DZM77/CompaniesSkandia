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
        [InlineData(1, 2)]
        [InlineData(3, 4)]
        [InlineData(5, 6)]
        public void Add_ShouldReturnSum(int nr1, int nr2)
        {
            var sut = new Calculator();
            var res = sut.Add(nr1, nr2);
            Assert.Equal(nr1 + nr2, res);
        }


        [Theory]
       // [MemberData(nameof(GetNumbers2))]
        [ClassData(typeof(ClassDataExample))]
        public void Add_ShouldReturnSum2(int nr1, int nr2)
        {
            var sut = new Calculator();
            var res = sut.Add(nr1, nr2);
            Assert.Equal(nr1 + nr2, res);
        }

        public static IEnumerable<object[]> GetNumbers(int numberOfDataSets)
        {
            var dataset = new List<object[]>
            {
                new object[] { 1,2},
                new object[] { 3,4},
                new object[] { 5,6},
                new object[] { 4,6}
            };

            return dataset.Count <= numberOfDataSets ? dataset : dataset.Take(numberOfDataSets);
        }

        public static TheoryData<int, int> GetNumbers2
        {
            get
            {
                return new TheoryData<int, int>
                {
                   { 1,2},
                   { 3,4},
                   { 5,6}
                };

            }
        }



    }

    public class ClassDataExample : TheoryData<int, int>
    {
        public ClassDataExample()
        {
            Add(1, 2);
            Add(3, 4);
            Add(4, 5);
        }
    }


    public class UseMemberDataFromAnotherClass
    {
        [Theory]
        [MemberData(nameof(CalculatorTests.GetNumbers), 2, MemberType = typeof(CalculatorTests))]
        public void Demo(int nr1, int nr2)
        {
            var sut = new Calculator();

            var res = sut.Add(nr1, nr2);

            Assert.Equal(nr1 + nr2, res);
        }
    }
}
