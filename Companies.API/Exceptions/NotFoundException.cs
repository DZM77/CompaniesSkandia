namespace Companies.API.Exceptions
{
    public class NotFoundException : Exception
    {
        public string Title { get; }
        public NotFoundException(string message, string title = "Not Found") : base(message)
        {
            Title = title;
        }
    }

    public class CompanyNotFoundException : NotFoundException
    {
        public CompanyNotFoundException(Guid id) : base($"The company with id: {id} dosen't exists")
        {

        }
    }

    public class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(Guid id) : base($"The employee with id: {id} dosen't exists")
        {

        }
    }
}
