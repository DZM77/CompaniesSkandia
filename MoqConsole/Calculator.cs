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

internal class EmployeeService
{
    private readonly IValidator validator;

    public EmployeeService(IValidator validator)
    {
        this.validator = validator;
    }

    public bool RegisterUser(Employee employee)
    {
        return validator.ValidateName(employee);

    }
}

public class EmployeeValidator : IValidator
{
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
