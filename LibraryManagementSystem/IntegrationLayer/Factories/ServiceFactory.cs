using BusinessLayer.Services;
using PersistenceLayer;

namespace IntegrationLayer.Factories
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly LibraryContext _context;

        public ServiceFactory(LibraryContext context)
        {
            _context = context;
        }

        public IUserService CreateUserService() => new UserService(_context);
        public IBookService CreateBookService() => new BookService(_context);
        public ITransactionService CreateTransactionService() => new TransactionService(_context);
    }
}
