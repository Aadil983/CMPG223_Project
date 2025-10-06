using System.Security.Cryptography;
using System.Text;

public static class Constants
{
    public const string ConnectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Aadil\\Documents\\DLPM.mdf; Integrated Security = True; Connect Timeout = 30";
}

/// <summary>
/// Utility class for hashing and verifying passwords using SHA256.
/// </summary>
public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convert byte array to a hexadecimal string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        string hashedEnteredPassword = HashPassword(enteredPassword);
        return hashedEnteredPassword.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
    }
}
