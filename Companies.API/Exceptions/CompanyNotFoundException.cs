namespace Companies.API.Exceptions
{
    public class CompanyNotFoundException : NotFoundException
    {
        public CompanyNotFoundException(Guid id) : base($"The company with id: {id} dosen't exists")
        {

        }
    }
}
