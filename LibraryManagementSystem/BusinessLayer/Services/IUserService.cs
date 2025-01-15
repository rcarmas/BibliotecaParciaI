using Common.DTOs;

namespace BusinessLayer.Services
{
    public interface IUserService
    {
        List<UserDTO> GetAllUsers();
        UserDTO GetUserById(Guid id);
        void AddUser(UserDTO userDto);
        void SuspendUser(Guid userId);  // Cambié a string
        string UpdateUser(Guid id, UserDTO user);  // Cambié a string
        List<TransactionDTO> GetLoanHistory(Guid userId);  // Cambié a string
    }
}

