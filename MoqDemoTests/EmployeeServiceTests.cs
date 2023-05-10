using Moq;
using MoqConsole;
using static MoqConsole.EmployeeService;

namespace MoqDemoTests
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

            mockValidator.Setup(x => x.ValidateName(employee.Name)).Returns(false);
            mockValidator.Setup(x => x.Validate(It.Is<string>(x => x.StartsWith("K"))));
            mockValidator.Setup(x => x.ValidateSalaryLevel(employee)).Returns(validatedSalaryLevel);

            var sut = new EmployeeService(mockValidator.Object);
            var actual = sut.RegisterUser(employee);

            Assert.False(actual);

        }

        [Fact]
        public void HandleMessage_ShouldReturnTrueIFMatch()
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
        
        
        [Fact]
        public void HandleMessage_MethodToVerifyItsCalled_ShouldRun_Once()
        {
            

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(x => x.Handler.CheckMessage.Message).Returns("Text");

            var sut = new EmployeeService(mockValidator.Object);
            var actual = sut.HandleMessage("Text");

            mockValidator.Verify(x => x.MethodToVerifyItsCalled(), Times.Once());
        }
    }
}