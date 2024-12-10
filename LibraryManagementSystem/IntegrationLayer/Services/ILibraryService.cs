using System;
using System.ServiceModel;
using Common.DTOs;
using SoapCore;

namespace LibraryManagementSystem.Services.Soap
{
    [ServiceContract]
    public interface ILibraryService
    {
        [OperationContract]
        
        string RegistrarUsuario(string name, string email, string status,string userType, string password);
        [OperationContract]
        UserDTO ValidateUser(string email, string password);

        [OperationContract]
        List<UserDTO> GetAllUsers();  

        [OperationContract]
        string ActivateUser(Guid userId); 

        [OperationContract]
        string SuspendUser(Guid userId);
        [OperationContract]
        string EditUser(Guid userId, string name, string email, string status, string userType);
        [OperationContract]
        string DeleteUser(Guid userId);
        [OperationContract]
        List<TransactionDTO> GetTransactions();

    }
}
