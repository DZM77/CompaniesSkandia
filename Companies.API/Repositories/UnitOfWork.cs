using Companies.API.Data;

namespace Companies.API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private readonly Lazy<ICompanyRepository> companyRepository;

        public ICompanyRepository CompanyRepository => companyRepository.Value;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(context));
        }

        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
