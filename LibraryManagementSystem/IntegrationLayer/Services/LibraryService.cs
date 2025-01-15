using System;
using System.Linq;
using System.Text;
using Common.DTOs;
using PersistenceLayer;
using PersistenceLayer.Models;
using Microsoft.AspNetCore.Identity;
using Common.Utils;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Soap
{
    public class LibraryService : ILibraryService
    {
        private readonly LibraryContext _context;

        public LibraryService(LibraryContext context)
        {
            _context = context;
        }
        public string RegistrarUsuario(string name, string email, string status, string userType, string password)
        {
            var existingUser = _context.users.FirstOrDefault(u => u.email == email);
            if (existingUser != null)
                return "El correo electrónico ya está registrado.";

            var hashedPassword = PasswordHasher.HashPassword(password); 

            var user = new User
            {
                name = name,
                email = email,
                usertype = userType,
                status = status,
                password = hashedPassword 
            };

            _context.users.Add(user);
            _context.SaveChanges();

            return $"Usuario {name} registrado con éxito. ID de usuario: {user.id}";
        }

        public List<TransactionDTO> GetTransactions()
        {
            var transactions = _context.transactions
            .Include(t => t.User)  // Incluir la relación con User
            .Include(t => t.Book)  // Incluir la relación con Book
            .Select(t => new TransactionDTO
            {
                Id = t.id, // Convertir a string si es necesario
                UserId = t.userid,
                BookId = t.bookid,
                BorrowDate = t.borrowdate ?? DateTime.MinValue, // Si borrowdate es null, asignar DateTime.MinValue
                ReturnDate = t.returndate ?? DateTime.MinValue, // Lo mismo para returnDate
                Status = t.status,
                Fine = t.fine,
                UserName = t.User.name,  // Asignar el nombre del usuario
                BookTitle = t.Book.title // Asignar el título del libro
            })
            .ToList();


            return transactions;
        }
        public UserDTO ValidateUser(string email, string password)
        {
            var user = _context.users.FirstOrDefault(u => u.email == email);

            if (user == null)
                return null;

            // Comparar contraseñas
            var hashedPassword = PasswordHasher.HashPassword(password);
            if (user.password != hashedPassword)
                return null;

            return new UserDTO
            {
                Id = user.id,
                Name = user.name,
                Email = user.email,
                UserType = user.usertype,
                Status = user.status
            };
        }
        public List<UserDTO> GetAllUsers()
        {
            var users = _context.users
                .Select(u => new UserDTO
                {
                    Id = u.id,
                    Name = u.name,
                    Email = u.email,
                    UserType = u.usertype,
                    Status = u.status
                })
                .ToList();

            return users;
        }

        // Método para activar un usuario
        public string ActivateUser(Guid userId)
        {
            var user = _context.users.Find(userId);
            if (user == null)
                return "Usuario no encontrado.";

            user.status = "Activo";
            _context.SaveChanges();

            return "Usuario activado correctamente.";
        }
        // Método para suspender un usuario
        public string SuspendUser(Guid userId)
        {
            var user = _context.users.Find(userId);
            if (user == null)
                return "Usuario no encontrado.";

            user.status = "Inactivo";
            _context.SaveChanges();

            return "Usuario suspendido correctamente.";
        }
        public string EditUser(Guid userId, string name, string email, string status, string userType)
        {
            var user = _context.users.Find(userId);
            if (user == null)
                return "Usuario no encontrado.";

            user.name = name;
            user.email = email;
            user.status = status;
            user.usertype = userType;

            _context.SaveChanges();

            return $"Usuario {user.name} editado correctamente.";
        }
        public string DeleteUser(Guid userId)
        {
            var user = _context.users.Find(userId);
            if (user == null)
                return "Usuario no encontrado.";

            _context.users.Remove(user);
            _context.SaveChanges();

            return $"Usuario {user.name} eliminado correctamente.";
        }

    }
}


