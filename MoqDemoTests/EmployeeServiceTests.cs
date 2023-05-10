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

            var mockValidator = new Mock<IValidator>();

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

            mockValidator.Verify(x => x.MethodToVerifyItsCalled(), Times.Exactly(2));
        }

        [Fact]
        public void HandleMessage_WhenMessageNotMatch_MethodToVerifyItsCalled_ShouldRun_Once()
        {

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(x => x.Handler.CheckMessage.Message).Returns("");

            var sut = new EmployeeService(mockValidator.Object);
            sut.HandleMessage("Text");

            mockValidator.Verify(x => x.MethodToVerifyItsCalled(), Times.Once);

        }
        
        [Fact]
        public void HandleMessage_WhenMessageMatch_TestProp_ShouldBeCalledOncee()
        {

            var mockValidator = new Mock<IValidator>();
            mockValidator.Setup(x => x.Handler.CheckMessage.Message).Returns("Text");

            var sut = new EmployeeService(mockValidator.Object);
            sut.HandleMessage("Text");

            mockValidator.Verify(x => x.TestProp);
            mockValidator.VerifyGet(x => x.TestProp);

            mockValidator.VerifySet(x => x.TestProp = It.IsAny<string>());
            mockValidator.VerifySet(x => x.TestProp = It.Is<string>(s => s.Count() < 10));
            mockValidator.VerifySet(x => x.TestProp = "Hej");

            mockValidator.VerifyGet(x => x.Handler.CheckMessage.Message);
            mockValidator.Verify(x => x.MethodToVerifyItsCalled(), Times.AtLeastOnce);
            mockValidator.VerifyNoOtherCalls();


        } 
        
        [Fact]
        public void RegisterUser_WhenValidNames_ShouldReturnTrue()
        {

            var mockValidator = new Mock<IValidator>();
            mockValidator.SetupSequence(x => x.ValidateName(It.IsAny<string>())).Returns(true).Returns(false);

            var sut = new EmployeeService(mockValidator.Object);
            var res = sut.RegisterUser(new Employee { Name = "Test"});

            Assert.True(res);   

        }

        [Fact]
        public void ExceptionDemo_WhenNameIsNull_ShouldThrowArgumentNullException()
        {

           // var mockValidator = new Mock<IValidator>();

            var sut = new EmployeeService(Mock.Of<IValidator>());


            Assert.Throws<ArgumentNullException>(() => sut.ExceptionDemo(null));

        }

        [Fact]
        public void ExceptionDemo_WhenNameIsKalle_ShouldThrowArgumentException()
        {

            var mockValidator = new Mock<IValidator>();
           // mockValidator.Setup(x => x.ValidateName(It.Is<string>(x => x.Equals("Kalle")))).Throws<ArgumentException>();


            var sut = new EmployeeService(mockValidator.Object);


            Assert.Throws<ArgumentException>(() => sut.ExceptionDemo("Kalle"));

        }
    }
}