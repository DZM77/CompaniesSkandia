using Moq;
using MoqConsole;

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
    }
}