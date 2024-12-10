using Common.DTOs;
using PersistenceLayer;
using PersistenceLayer.Models;
using LibraryManagementSystem.Services.Utilities;

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
                Id = u.id,  // Cambié a string
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
                id = userDto.Id.GetValueOrDefault(),  // Cambié a string
                name = userDto.Name,
                email = userDto.Email,
                usertype = userDto.UserType,
                status = "Activo",
                password = PasswordHelper.HashPassword(userDto.Password)  // Hashear la contraseña antes de almacenarla
            };

            _context.users.Add(user);
            _context.SaveChanges();
        }

        public void SuspendUser(Guid userId)  // Cambié a string
        {
            var user = _context.users.Find(userId);  // Cambié a string
            if (user != null)
            {
                user.status = "Suspendido";
                _context.SaveChanges();
            }
        }

        public string UpdateUser(string id, UserDTO user)  // Cambié a string
        {
            var existingUser = _context.users.Find(id);  // Cambié a string
            if (existingUser == null)
                return "Usuario no encontrado.";

            existingUser.name = user.Name;
            existingUser.email = user.Email;
            existingUser.usertype = user.UserType;
            existingUser.status = user.Status;

            _context.SaveChanges();
            return "Usuario actualizado exitosamente.";
        }

        public List<TransactionDTO> GetLoanHistory(Guid userId)  // Cambié a string
        {
            var user = _context.users.Find(userId);  // Cambié a string
            if (user == null)
                return null;

            return _context.transactions
                .Where(t => t.userid == userId)  // Cambié a string
                .Select(t => new TransactionDTO
                {
                    Id = t.id,
                    BookId = t.bookid,  // Cambié a string
                    BorrowDate = t.borrowdate.Value,
                    ReturnDate = t.returndate,
                    Status = t.status,
                    Fine = t.fine
                }).ToList();
        }

        public UserDTO GetUserById(Guid id)
        {
            var user = _context.users.FirstOrDefault(m=> m.id == id);
            var UserDto = new UserDTO
            {
                Email = user.email,
                Name = user.name,
                Password= user.password,
                UserType = user.usertype,
                Status = user.status,
                Id = user.id
            };
            return UserDto;
        }

        public string UpdateUser(Guid id, UserDTO user)
        {
            throw new NotImplementedException();
        }

       
    }
}
