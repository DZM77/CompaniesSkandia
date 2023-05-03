using Moq;
using MoqConsole;
using static MoqConsole.EmployeeService;

namespace Employees.Tests
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void RegisterUser_ReturnFalse_WhenIncorrectName()
        {
            const string incorrectName = "incorrect";
            const SalaryLevel validatedSalaryLevel = SalaryLevel.Junior;

            var mockValidator = new Mock<IValidator>(MockBehavior.Strict);

            var employee = new Employee()
            {
                Name = incorrectName,
            };

            mockValidator.Setup(x => x.ValidateName(employee)).Returns(false);
            mockValidator.Setup(x => x.ValidateSalaryLevel(employee)).Returns(validatedSalaryLevel);

            var sut = new EmployeeService(mockValidator.Object);
            var actual = sut.RegisterUser(employee);

            Assert.False(actual);

        }

        [Fact]
        public void HandleMessageR_ShouldReturnTrueIFMatch()
        {
            //var iMessageMock = new Mock<IMessage>();
            //iMessageMock.Setup(x => x.Message).Returns("Text");

            //var iHandlerMock = new Mock<IHandler>();
            //iHandlerMock.Setup(x => x.CheckMessage).Returns(iMessageMock.Object);

            //var mockValidator = new Mock<IValidator>();
            //mockValidator.Setup(x => x.Handler).Returns(iHandlerMock.Object);

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(x => x.Handler.CheckMessage.Message).Returns("Text");

            var sut = new EmployeeService(mockValidator.Object);
            var actual = sut.HandleMessage("Text");

            Assert.True(actual);
        }
    }
}