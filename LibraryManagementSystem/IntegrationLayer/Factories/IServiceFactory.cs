using BusinessLayer.Services;

namespace IntegrationLayer.Factories
{
    public interface IServiceFactory
    {
        IUserService CreateUserService();
        IBookService CreateBookService();
        ITransactionService CreateTransactionService();
    }
}
