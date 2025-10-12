using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1;
using System.Data;

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
        public string Email { get; internal set; }
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

        public string ContentLink { get; internal set; }
        public int DurationHours { get; internal set; }
        public string Status { get; internal set; }
        public DateTime DateCreated { get; internal set; }
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

        // =================================================================
        // TRAINING MODULE METHODS (NEW CRUD IMPLEMENTATION)
        // =================================================================

        /// <summary>
        /// Retrieves all Training Modules from the database.
        /// </summary>
        public List<TrainingModule> GetAllModules()
        {
            List<TrainingModule> modules = new List<TrainingModule>();
            string sql = "SELECT ModuleID, ModuleName, Description, ContentLink, DurationHours, Status, DateCreated FROM TrainingModule";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                modules.Add(new TrainingModule
                                {
                                    ModuleID = reader.GetInt32(reader.GetOrdinal("ModuleID")),
                                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    ContentLink = reader.IsDBNull(reader.GetOrdinal("ContentLink")) ? string.Empty : reader.GetString(reader.GetOrdinal("ContentLink")),
                                    DurationHours = reader.GetInt32(reader.GetOrdinal("DurationHours")),
                                    Status = reader.GetString(reader.GetOrdinal("Status")),
                                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")) // Assuming DateCreated is not nullable
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error fetching modules: {ex.Message}");
            }
            return modules;
        }

        /// <summary>
        /// Adds a new Training Module to the database.
        /// </summary>
        /// <returns>The ID of the newly created module, or -1 on failure.</returns>
        public int AddModule(string moduleName, string description, string contentLink, int durationHours, string status)
        {
            int newId = -1;
            string sql = @"
            INSERT INTO TrainingModule (ModuleName, Description, ContentLink, DurationHours, Status, DateCreated)
            VALUES (@ModuleName, @Description, @ContentLink, @DurationHours, @Status, @DateCreated);
            SELECT CAST(scope_identity() AS INT);";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ModuleName", moduleName);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@ContentLink", contentLink);
                        command.Parameters.AddWithValue("@DurationHours", durationHours);
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@DateCreated", DateTime.Now); // Use current time for creation

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            newId = (int)result;
                        }
                    }
                }
                Console.WriteLine($"[SUCCESS] Module '{moduleName}' added with ID: {newId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] Failed to add module: {ex.Message}");
            }
            return newId;
        }

        /// <summary>
        /// Updates an existing Training Module in the database.
        /// </summary>
        /// <returns>True if the module was updated, false otherwise.</returns>
        public bool UpdateModule(int moduleId, string moduleName, string description, string contentLink, int durationHours, string status)
        {
            string sql = @"
            UPDATE TrainingModule SET 
                ModuleName = @ModuleName, 
                Description = @Description, 
                ContentLink = @ContentLink, 
                DurationHours = @DurationHours, 
                Status = @Status 
            WHERE ModuleID = @ModuleID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ModuleID", moduleId);
                        command.Parameters.AddWithValue("@ModuleName", moduleName);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@ContentLink", contentLink);
                        command.Parameters.AddWithValue("@DurationHours", durationHours);
                        command.Parameters.AddWithValue("@Status", status);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"[SUCCESS] Module ID {moduleId} updated.");
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] Failed to update module: {ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// Deletes a Training Module by its ID.
        /// </summary>
        /// <returns>True if the module was deleted, false otherwise.</returns>
        public bool DeleteModule(int moduleId)
        {
            string sql = "DELETE FROM TrainingModule WHERE ModuleID = @ModuleID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ModuleID", moduleId);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"[SUCCESS] Module ID {moduleId} deleted.");
                            return true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Check for foreign key constraint violation (module is enrolled by participants or has related records)
                if (ex.Number == 547)
                {
                    Console.WriteLine($"[DB ERROR] Cannot delete Module ID {moduleId}. It is currently referenced by participant enrollments or assessments.");
                    return false;
                }
                Console.WriteLine($"[DB ERROR] Failed to delete module: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GENERAL ERROR] Failed to delete module: {ex.Message}");
            }
            return false;
        }

        public class CommunityPartner
        {
            public int PartnerID { get; set; }
            public string PartnerName { get; set; }
            public string ContactPerson { get; set; }
            public string ContactEmail { get; set; }
            public string ContactPhone { get; set; }
            public DateTime DateJoined { get; set; }
        }

        /// <summary>
        /// Retrieves all Community Partners from the database.
        /// </summary>
        public List<CommunityPartner> GetAllPartners()
        {
            List<CommunityPartner> partners = new List<CommunityPartner>();
            string sql = "SELECT PartnerID, PartnerName, ContactPerson, ContactEmail, ContactPhone, DateJoined FROM CommunityPartner";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                partners.Add(new CommunityPartner
                                {
                                    PartnerID = reader.GetInt32(reader.GetOrdinal("PartnerID")),
                                    PartnerName = reader.GetString(reader.GetOrdinal("PartnerName")),
                                    ContactPerson = reader.GetString(reader.GetOrdinal("ContactPerson")),
                                    ContactEmail = reader.GetString(reader.GetOrdinal("ContactEmail")),
                                    ContactPhone = reader.GetString(reader.GetOrdinal("ContactPhone")),
                                    DateJoined = reader.GetDateTime(reader.GetOrdinal("DateJoined"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error fetching partners: {ex.Message}");
            }
            return partners;
        }

        /// <summary>
        /// Adds a new Community Partner to the database.
        /// </summary>
        public int AddPartner(string name, string person, string email, string phone)
        {
            int newId = -1;
            string sql = @"
        INSERT INTO CommunityPartner (PartnerName, ContactPerson, ContactEmail, ContactPhone, DateJoined)
        VALUES (@Name, @Person, @Email, @Phone, @DateJoined);
        SELECT CAST(scope_identity() AS INT);";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Person", person);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@DateJoined", DateTime.Now);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            newId = (int)result;
                        }
                    }
                }
                Console.WriteLine($"[SUCCESS] Partner '{name}' added with ID: {newId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] Failed to add partner: {ex.Message}");
            }
            return newId;
        }

        /// <summary>
        /// Updates an existing Community Partner in the database.
        /// </summary>
        public bool UpdatePartner(int partnerId, string name, string person, string email, string phone)
        {
            string sql = @"
        UPDATE CommunityPartner SET 
            PartnerName = @Name, 
            ContactPerson = @Person, 
            ContactEmail = @Email, 
            ContactPhone = @Phone 
        WHERE PartnerID = @PartnerID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PartnerID", partnerId);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Person", person);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Phone", phone);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"[SUCCESS] Partner ID {partnerId} updated.");
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] Failed to update partner: {ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// Deletes a Community Partner by its ID.
        /// </summary>
        public bool DeletePartner(int partnerId)
        {
            string sql = "DELETE FROM CommunityPartner WHERE PartnerID = @PartnerID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PartnerID", partnerId);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"[SUCCESS] Partner ID {partnerId} deleted.");
                            return true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    Console.WriteLine($"[DB ERROR] Cannot delete Partner ID {partnerId}. It may be linked to a module or assessment.");
                    return false;
                }
                Console.WriteLine($"[DB ERROR] Failed to delete partner: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GENERAL ERROR] Failed to delete partner: {ex.Message}");
            }
            return false;
        }

        public List<Stakeholder> GetAllParticipants()
        {
            List<Stakeholder> participants = new List<Stakeholder>();
            string sql = "SELECT * FROM Stakeholder WHERE RoleType = 'Participant' ORDER BY LastName, FirstName";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                participants.Add(new Stakeholder
                                {
                                    StakeholderID = reader.GetInt32(reader.GetOrdinal("StakeholderID")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    RoleType = reader.GetString(reader.GetOrdinal("RoleType")), // Should be 'Participant'
                                    IDNumber = reader.GetString(reader.GetOrdinal("IDNumber")),
                                    ContactNumber = reader.GetString(reader.GetOrdinal("ContactNumber")),
                                    // Password is not fetched for security reasons
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error fetching participants: {ex.Message}");
            }
            return participants;
        }

        /// <summary>
        /// Updates non-sensitive details for an existing Stakeholder.
        /// </summary>
        public bool UpdateStakeholderDetails(int stakeholderId, string firstName, string lastName, string email, string idNumber, string contactNumber)
        {
            string sql = @"
        UPDATE Stakeholder SET 
            FirstName = @FirstName, 
            LastName = @LastName, 
            Email = @Email, 
            IDNumber = @IDNumber, 
            ContactNumber = @ContactNumber 
        WHERE StakeholderID = @StakeholderID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@StakeholderID", stakeholderId);
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@IDNumber", idNumber);
                        command.Parameters.AddWithValue("@ContactNumber", contactNumber);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"[SUCCESS] Stakeholder ID {stakeholderId} updated.");
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] Failed to update stakeholder: {ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// Deletes a Stakeholder by their ID.
        /// </summary>
        public bool DeleteStakeholder(int stakeholderId)
        {
            // NOTE: This assumes cascade deletion is handled in the DB or there are no dependent records.
            string sql = "DELETE FROM Stakeholder WHERE StakeholderID = @StakeholderID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@StakeholderID", stakeholderId);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"[SUCCESS] Stakeholder ID {stakeholderId} deleted.");
                            return true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    Console.WriteLine($"[DB ERROR] Cannot delete Stakeholder ID {stakeholderId}. It may be linked to assessments or other records.");
                    return false;
                }
                Console.WriteLine($"[DB ERROR] Failed to delete stakeholder: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GENERAL ERROR] Failed to delete stakeholder: {ex.Message}");
            }
            return false;
        }
    }
}