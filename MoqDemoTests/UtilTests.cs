using Companies.TestDemo;
using Moq;
using MoqConsole;

namespace MoqDemoTests
{
    public class UtilTests
    {

        [Fact]
        public void AskForString_ShouldReturnString_WithCustomMoq()
        {
            //Arrange
            const string expected = "Input";
            var mockUI = new MockUI { SetInput = expected };

            //Act
            var actual = Util.AskForString("", mockUI);

            //Assert
            Assert.Equal(expected, actual);

        }

        [Fact]
        public void AskForString_ShouldReturnString_WithMoq()
        {
            const string expected = "Input";
            var mockUI = new Mock<IUI>();
            mockUI.Setup(x => x.GetInput()).Returns(expected);

            var actual = Util.AskForString("", mockUI.Object);

            Assert.Equal(expected, actual);

        }
    }
}