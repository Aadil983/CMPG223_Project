using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace DLPM
{
    // ====================================================================
    // Application Entry Point (Simulating the Login/Signup UI)
    // ====================================================================
    public static class Program
    {

        // The Main method must be decorated with [STAThread] for WinForms applications
        [STAThread]
        public static void Main(string[] args)
        {
            // Set up application environment for visual styles
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Console.WriteLine("--- DLPM System Starting ---");

            // Start the application by running the Login Form
            // All registration and login logic is now handled within LoginForm.cs
            Application.Run(new LoginForm());

            // Execution stops here until the LoginForm (and subsequent forms) are closed.
        }

        /* [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("--- Digital Literacy Program Management (DLPM) ---");


            // ----------------------------------------------------
            // DEMO 1: Registration of a new Participant
            // ----------------------------------------------------
            Console.WriteLine("\n\n--- Attempting New Participant Registration ---");

            // Example details for a new user
            string newEmail = "new.user@example.com";
            string newPass = "SecureP@ss123";


            // Instantiate the DB Manager using the connection string defined in Constants
            // NOTE: Ensure Constants.ConnectionString in Security.cs is correct!
            DBManager manager = new DBManager(Constants.ConnectionString);
            // This simulates a user submitting the registration form data
            manager.RegisterParticipant("Bongani", "Zulu", newEmail, newPass, "0010105000081", "0765432109");

            // Attempting to register the same user again (should fail due to UNIQUE constraint)
            Console.WriteLine("\n--- Attempting Duplicate Registration (Expected to fail) ---");
            manager.RegisterParticipant("Bongani", "Zulu", newEmail, newPass, "0010105000081", "0765432109");


            // ----------------------------------------------------
            // DEMO 2: Login Attempts using Seeded Data
            // ----------------------------------------------------
            Console.WriteLine("\n\n--- Login Attempts ---");

            // 1. Successful Login (using a seeded admin user with the default password)
            Stakeholder loggedInUser = manager.LoginUser("aadil.admin@dlpm.org.za", "DummyHashedPassword123");

            // After successful login, you would typically check the RoleType to redirect the user
            if (loggedInUser != null)
            {
                Console.WriteLine($"\n[INFO] User logged in as a {loggedInUser.RoleType}. Redirecting to dashboard...");
            }

            // 2. Failed Password
            manager.LoginUser("aadil.admin@dlpm.org.za", "WrongPassword");

            // 3. Failed Email (user not found)
            manager.LoginUser("nonexistent@dlpm.org.za", "DummyHashedPassword123");

            // For GUI: show message box instead of reading console
            System.Windows.Forms.MessageBox.Show("Program finished. Close to exit.");
        } */

    }

    public class Stakeholder
    /// Model class representing a row in the consolidated Stakeholder table.
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

    internal static class Constants
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

    internal class DBManager
    {
        private readonly string _connectionString;

        public DBManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Attempts to register a new participant user.
        /// </summary>
        /// <returns>The ID of the newly created stakeholder, or -1 on failure.</returns>
        public int RegisterParticipant(string firstName, string lastName, string email, string password, string idNumber, string contactNumber)
        {
            // PasswordHasher must be accessible (either in the same namespace or imported)
            string hashedPassword = PasswordHasher.HashPassword(password);
            int newId = -1;

            // SQL command to insert a new participant
            // NOTE: EnrollmentDate parameter has been removed as it is not in the Stakeholder table
            string sql = @"
            INSERT INTO Stakeholder (RoleType, FirstName, LastName, EmailAddress, PasswordHash, IDNumber, ContactNumber)
            VALUES (@RoleType, @FirstName, @LastName, @Email, @PasswordHash, @IDNumber, @ContactNumber);
            SELECT CAST(scope_identity() AS INT);"; // Returns the ID of the new row

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

                        // ExecuteScalar returns the first column of the first row (the new ID)
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
                // Error 2627 is a unique constraint violation (for Email or IDNumber)
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
                                // Map the data to the Stakeholder object (only fetching necessary login fields)
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
                }

                // Step 2: Verify the password
                if (user != null && user.PasswordHash != null && PasswordHasher.VerifyPassword(password, user.PasswordHash))
                {
                    Console.WriteLine($"\n[SUCCESS] Login successful! Welcome, {user.FirstName ?? user.EmailAddress}. Role: {user.RoleType}");
                    return user;
                }
                else
                {
                    // If user is null (email not found) or password verification fails
                    Console.WriteLine("\n[FAILURE] Login failed: Invalid email or password.");
                    return null;
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
