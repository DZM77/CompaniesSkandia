using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Employees.Tests")]
namespace MoqConsole;

public class Calculator
{
    public Calculator()
    {
    }

    internal int Add(int num1, int num2)
    {
        return num1 + num2;
    }
}

public class EmployeeService
{
    private readonly IValidator validator;

    public EmployeeService(IValidator validator)
    {
        this.validator = validator;
    }

    public bool RegisterUser(Employee employee)
    {
        var salaryLevel = validator.ValidateSalaryLevel(employee);
        return validator.ValidateName(employee);

    }

    public bool HandleMessage(string text)
    {
        if (validator.Handler.CheckMessage.Message != text)
        {
            return false;
        }

        return true;
    }

    public interface IMessage
    {
        string Message { get; }
    }

    public interface IHandler
    {
        IMessage CheckMessage { get; }
    }

    public class EmployeeValidator : IValidator
    {
        public IHandler Handler { get; }

        public bool ValidateName(Employee employee)
        {
            //Not used in tests
            throw new NotImplementedException();
        }

        public SalaryLevel ValidateSalaryLevel(Employee employee)
        {
            //Not used in tests
            throw new NotImplementedException();
        }
    }

    public interface IValidator
    {
        SalaryLevel ValidateSalaryLevel(Employee employee);
        bool ValidateName(Employee employee);

        IHandler Handler { get; }
    }

    public enum SalaryLevel
    {
        Default,
        NotSet,
        Junior,
        Senior
    }

    public class Employee
    {
        public string Name { get; set; } = string.Empty;
        public int Salary { get; set; }
        public SalaryLevel SalaryLevel { get; set; } = SalaryLevel.NotSet;
    }
}
