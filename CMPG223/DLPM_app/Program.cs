using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1;
using System.Security.Cryptography; // Needed for PasswordHasher

namespace DLPM
{
    // =================================================================
    // 1. APPLICATION ENTRY POINT
    // =================================================================
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Start the application at the login form
            Application.Run(new LoginForm());
        }
    }


    // =================================================================
    // 2. CONSTANTS (Configuration)
    // =================================================================
    internal static class Constants
    {
        // WARNING: Update this connection string to match your local database path
        // The user's provided path is: C:\\Users\\Aadil\\Documents\\DLPM.mdf
        public const string ConnectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\Aadil\\Documents\\DLPM.mdf; Integrated Security = True; Connect Timeout = 30";
    }


    // =================================================================
    // 3. SECURITY UTILITY
    // =================================================================
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


    // =================================================================
    // 4. DATA MODELS
    // =================================================================

    /// <summary>
    /// Model class representing a row in the consolidated Stakeholder table.
    /// </summary>
    public class Stakeholder
    {
        // Common Fields
        public int StakeholderID { get; set; }
        public string RoleType { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string ContactNumber { get; set; }
        public string PasswordHash { get; set; } // Stores secure hash
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Partner-Specific Field
        public string PartnerName { get; set; }

        // Participant-Specific Fields (Nullable value types remain)
        public string IDNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string EducationLevel { get; set; }
        public string EmploymentStatus { get; set; }
        public decimal? HouseholdIncome { get; set; }
        public string DisabilityStatus { get; set; }
    }

    /// <summary>
    /// Model class representing a row in the TrainingModule table.
    /// </summary>
    public class TrainingModule
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int DurationWeeks { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Optional: Used for displaying in the UI
        public string DisplayInfo => $"{ModuleName} ({DurationWeeks} weeks)";
    }


    // =================================================================
    // 5. DATABASE MANAGER (DAL)
    // =================================================================
    internal class DBManager
    {
        private readonly string _connectionString;

        public DBManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        // --- AUTHENTICATION METHODS ---

        /// <summary>
        /// Attempts to register a new participant user.
        /// </summary>
        /// <returns>The ID of the newly created stakeholder, or -1 on failure.</returns>
        public int RegisterParticipant(string firstName, string lastName, string email, string password, string idNumber, string contactNumber)
        {
            string hashedPassword = PasswordHasher.HashPassword(password);
            int newId = -1;

            // SQL command to insert a new participant
            string sql = @"
            INSERT INTO Stakeholder (RoleType, FirstName, LastName, EmailAddress, PasswordHash, IDNumber, ContactNumber)
            VALUES (@RoleType, @FirstName, @LastName, @Email, @PasswordHash, @IDNumber, @ContactNumber);
            SELECT CAST(scope_identity() AS INT);";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@RoleType", "Participant");
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        command.Parameters.AddWithValue("@IDNumber", idNumber);
                        command.Parameters.AddWithValue("@ContactNumber", contactNumber);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            newId = (int)result;
                        }
                    }
                }
                Console.WriteLine($"\n[SUCCESS] Participant {email} registered successfully with ID: {newId}.");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    Console.WriteLine($"\n[ERROR] Registration failed. A user with that Email or ID Number already exists.");
                }
                else
                {
                    Console.WriteLine($"\n[DB ERROR] An SQL error occurred during registration: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[GENERAL ERROR] An unexpected error occurred: {ex.Message}");
            }
            return newId;
        }

        /// <summary>
        /// Attempts to log in a user by verifying their email and password hash.
        /// </summary>
        /// <returns>The authenticated Stakeholder object, or null if login fails.</returns>
        public Stakeholder LoginUser(string email, string password)
        {
            Stakeholder user = null;
            string sql = "SELECT StakeholderID, RoleType, EmailAddress, FirstName, LastName, PasswordHash FROM Stakeholder WHERE EmailAddress = @Email";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new Stakeholder
                                {
                                    StakeholderID = reader.GetInt32(reader.GetOrdinal("StakeholderID")),
                                    RoleType = reader.GetString(reader.GetOrdinal("RoleType")),
                                    EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                                    FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                                    PasswordHash = reader.IsDBNull(reader.GetOrdinal("PasswordHash")) ? null : reader.GetString(reader.GetOrdinal("PasswordHash")),
                                };
                            }
                        }
                    }

                    if (user != null && user.PasswordHash != null && PasswordHasher.VerifyPassword(password, user.PasswordHash))
                    {
                        Console.WriteLine($"\n[SUCCESS] Login successful! Welcome, {user.FirstName ?? user.EmailAddress}. Role: {user.RoleType}");
                        return user;
                    }
                    else
                    {
                        Console.WriteLine("\n[FAILURE] Login failed: Invalid email or password.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR] An error occurred during login: {ex.Message}");
                return null;
            }
        }
    }
}