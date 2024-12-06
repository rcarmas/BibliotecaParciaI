using System;
using System.ServiceModel;
using SoapCore;

namespace LibraryManagementSystem.Services.Soap
{
    [ServiceContract]
    public interface ILibraryService
    {
        [OperationContract]
        string RegistrarPrestamo(int userId, string bookId, DateTime borrowDate, DateTime returnDate);

        [OperationContract]
        string RegistrarDevolucion(int transactionId);

        [OperationContract]
        string GenerarReportePrestamosActivos(DateTime? startDate, DateTime? endDate);

        [OperationContract]
        string GenerarReporteHistorialUsuarios(int userId);

        [OperationContract]
        string RegistrarUsuario(string name, string email, string userType);

        [OperationContract]
        string SuspenderUsuario(int userId);
    }
}
