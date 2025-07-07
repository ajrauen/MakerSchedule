using Microsoft.AspNetCore.Identity;

namespace GeneratePasswordHash
{
    class Program
    {
        static void Main(string[] args)
        {
            var passwordHasher = new PasswordHasher<object>();
            var password = "@dmin!23";
            var hashedPassword = passwordHasher.HashPassword(null, password);
            
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Hashed Password: {hashedPassword}");
        }
    }
} 