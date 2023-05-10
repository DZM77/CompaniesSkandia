namespace Companies.API.Repositories
{
    public interface IUnitOfWork
    {
        ICompanyRepository CompanyRepository { get; }

        Task CompleteAsync();
    }
}