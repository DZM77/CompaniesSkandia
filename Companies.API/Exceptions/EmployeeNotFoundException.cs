namespace Companies.API.Exceptions
{
    public class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(Guid id) : base($"The employee with id: {id} dosen't exists")
        {

        }
    }
}
