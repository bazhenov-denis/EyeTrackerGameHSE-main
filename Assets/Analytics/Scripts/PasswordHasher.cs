using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    public static string GenerateSalt(int size = 16)
    {
        byte[] saltBytes = new byte[size];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

    public static string ComputeHash(string password, string salt)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            string saltedPassword = password + salt;
            byte[] bytes = Encoding.UTF8.GetBytes(saltedPassword);
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
