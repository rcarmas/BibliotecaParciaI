using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystem.Services.Utilities
{
    public static class PasswordHelper
    {
        // Método para encriptar la contraseña
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();  // Devuelve la contraseña encriptada
            }
        }

        // Método para validar la contraseña
        public static bool ValidatePassword(string password, string hashedPassword)
        {
            string hashedInputPassword = HashPassword(password);
            return hashedInputPassword == hashedPassword;
        }
    }
}
