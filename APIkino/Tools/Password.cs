using System.Security.Cryptography;
using System.Text;

namespace APIkino.Tools
{
    public class Password
    {
        public static string PasswordHashing(string password)
        {
            //we use the same password salt
            //to make the hash value of the user password
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashPassword);
        }
    }
}
