using Common.DTOs;

namespace BusinessLayer.Services
{
    public interface IUserService
    {
        List<UserDTO> GetAllUsers();
        void AddUser(UserDTO userDto);
        void SuspendUser(int userId);
        string UpdateUser(int id, UserDTO user); // Cambiado a 'void'
        List<TransactionDTO> GetLoanHistory(int userId);
    }
}
