using Common.DTOs;
using PersistenceLayer;
using PersistenceLayer.Models;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly LibraryContext _context;

        public UserService(LibraryContext context)
        {
            _context = context;
        }

        public List<UserDTO> GetAllUsers()
        {
            return _context.users.Select(u => new UserDTO
            {
                Id = u.id,
                Name = u.name,
                Email = u.email,
                UserType = u.usertype,
                Status = u.status
            }).ToList();
        }

        public void AddUser(UserDTO userDto)
        {
            var user = new User
            {
                name = userDto.Name,
                email = userDto.Email,
                usertype = userDto.UserType,
                status = "Activo"
            };

            _context.users.Add(user);
            _context.SaveChanges();
        }

        public void SuspendUser(int userId)
        {
            var user = _context.users.Find(userId);
            if (user != null)
            {
                user.status = "Suspendido";
                _context.SaveChanges();
            }
        }

        public string UpdateUser(int id, UserDTO user)
        {
            var existingUser = _context.users.Find(id);
            if (existingUser == null)
                return "Usuario no encontrado.";  // Si no existe el usuario, devuelve un mensaje

            existingUser.name = user.Name;
            existingUser.email = user.Email;
            existingUser.usertype = user.UserType;
            existingUser.status = user.Status;

            _context.SaveChanges();  // Guarda los cambios

            return "Usuario actualizado exitosamente.";  // Si todo va bien, devuelve un mensaje
        }




        public List<TransactionDTO> GetLoanHistory(int userId)
        {
            var user = _context.users.Find(userId);
            if (user == null)
                return null;

            return _context.transactions
                .Where(t => t.userid == userId)
                .Select(t => new TransactionDTO
                {
                    Id = t.id,
                    BookId = t.Book.id, 
                    BorrowDate = t.borrowdate.Value, 
                    ReturnDate = t.returndate,
                    Status = t.status,
                    Fine = t.fine
                }).ToList();
        }



    }
}
