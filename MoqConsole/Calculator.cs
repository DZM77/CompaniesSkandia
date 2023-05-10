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
        return validator.ValidateName(employee.Name);

    }

    public bool HandleMessage(string text)
    {
        var res = false;

        if (validator.Handler.CheckMessage.Message != text)
            res = false;
        else
        {
            res = true;
            validator.MethodToVerifyItsCalled();
        }


        return res;
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

        public void MethodToVerifyItsCalled()
        {
            throw new NotImplementedException();
        }

        public void Validate(string name)
        {
            throw new NotImplementedException();
        }

        public bool ValidateName(string name)
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
        bool ValidateName(string name);
        void Validate(string name);
        void MethodToVerifyItsCalled();

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
