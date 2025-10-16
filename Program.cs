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
    /// 
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
        public string Email { get; internal set; }
        public int? PartnerID { get; internal set; }
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

    /// <summary>
    /// Model representing the result saved in the ModuleAssessment table.
    /// </summary>
    public class ModuleAssessmentResult
    {
        public int EnrollmentID { get; set; }
        public int AssessmentID { get; set; }
        public DateTime AssessmentDate { get; set; }
        public decimal Mark { get; set; }
        public string PassStatus { get; set; } // "Pass" or "Fail"
        public string AssessmentName { get; internal set; }
        public string ModuleName { get; internal set; }
    }

    /// <summary>
    /// Model for a single assessment attempt record.
    /// (Used for simplified historical reporting).
    /// </summary>
    public class AssessmentAttempt
    {
        public int AssessmentAttemptID { get; set; }
        public int EnrollmentID { get; set; }
        public DateTime AttemptDate { get; set; }
    }

    /// <summary>
    /// Model used to display comprehensive Assessment data in the Admin maintenance grid.
    /// The ModuleName serves as the descriptive name since AssessmentName does not exist.
    /// </summary>
    public class AssessmentMaintenanceModel
    {
        // Assessment Fields
        public int AssessmentID { get; set; }
        // AssessmentName REMOVED
        public decimal PassPercentage { get; set; }

        // Foreign Keys
        public int ModuleID { get; set; }
        public int PartnerID { get; set; }

        // Joined Display Names (ModuleName is the primary descriptor)
        public string ModuleName { get; set; }
        public string PartnerName { get; set; }
        public string AssessmentName { get; internal set; }
    }

    /// <summary>
    /// Simplified model for binding data to the Module ComboBox.
    /// </summary>
    public class ModuleModel
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
    }

    /// <summary>
    /// Simplified model for binding data to the Partner ComboBox.
    /// </summary>
    public class PartnerModel
    {
        public int PartnerID { get; set; }
        public string PartnerName { get; set; }
    }

    /// <summary>
    /// Model to display enrollment and assessment status on the Participant Dashboard.
    /// </summary>
    public class ParticipantAssessmentStatusModel
    {
        public int ModuleID { get; set; }
        public int AssessmentID { get; set; }
        public string ModuleName { get; set; }
        public string AssessmentName { get; set; }
        public decimal PassPercentage { get; set; }
        public bool IsEnrolled { get; set; }
        public bool HasTakenAssessment { get; set; }
        public decimal? LastScore { get; set; } // Nullable decimal for last score
        public DateTime? LastAttemptDate { get; set; } // Nullable DateTime for last attempt
        public string PartnerName { get; set; } // The name of the Partner stakeholder associated with the Module
        public string EnrollmentStatus { get; internal set; }
        public int EnrollmentID { get; internal set; }
        public DateTime EnrollmentDate { get; internal set; }
        public string PassStatus { get; internal set; }
    }

    public class ModuleEnrollmentModel
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public int? EnrollmentID { get; set; }          // null => not enrolled
        public DateTime? EnrollmentDate { get; set; }   // null if not enrolled
        public bool IsEnrolled => EnrollmentID.HasValue;
    }


    /// <summary>
    /// Model representing an Enrollment record, used for display and approval.
    /// </summary>
    public class EnrollmentModel
    {
        public int EnrollmentID { get; set; }
        public int StakeholderID { get; set; } // Participant ID
        public string ParticipantName { get; set; }
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } // Pending, Approved, Denied
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

        /// <summary>
        /// Retrieves all assessments, joining Module and Partner names for the Admin maintenance grid.
        /// </summary>
        public List<AssessmentMaintenanceModel> GetAllAssessmentsForAdmin()
        {
            List<AssessmentMaintenanceModel> assessments = new List<AssessmentMaintenanceModel>();

            // <<< FIXED SQL QUERY: Joins Module, ModulePartner, and Stakeholder (P) >>>
            string sql = @"
                SELECT 
                    M.ModuleID AS AssessmentID, 
                    M.ModuleID,
                    P.StakeholderID AS PartnerID, 
                    MP.PartnerStakeholderID, -- Use this to map to PartnerID in the C# model
                    M.ModuleName,
                    P.PartnerName -- Use the PartnerName column from Stakeholder
                FROM Module M
                JOIN ModulePartner MP ON M.ModuleID = MP.ModuleID
                JOIN Stakeholder P ON MP.PartnerStakeholderID = P.StakeholderID
                ORDER BY M.ModuleID;";

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
                                assessments.Add(new AssessmentMaintenanceModel
                                {
                                    // Use ModuleID as the unique AssessmentID for maintenance
                                    AssessmentID = reader.GetInt32(reader.GetOrdinal("AssessmentID")),

                                    // <<< HARDCODED: This value is not in your database schema >>>
                                    PassPercentage = 0.50m,

                                    ModuleID = reader.GetInt32(reader.GetOrdinal("ModuleID")),
                                    // PartnerID comes from the PartnerStakeholderID field
                                    PartnerID = reader.GetInt32(reader.GetOrdinal("PartnerID")),
                                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                                    // The partner name is available in the Stakeholder table as PartnerName
                                    PartnerName = reader.GetString(reader.GetOrdinal("PartnerName"))
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlex)
            {
                string errorMessage = $"SQL Error ({sqlex.Number}): {sqlex.Message}";
                Console.WriteLine($"\n[DB ERROR] {errorMessage}");
                throw new InvalidOperationException($"Database Query Failed. Detail: {errorMessage}", sqlex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve assessment list from database. Error: {ex.Message}", ex);
            }
            return assessments;
        }

        /// <summary>
        /// Retrieves all active Modules for ComboBox binding.
        /// </summary>
        public List<ModuleModel> Modules
        {
            get
            {
                List<ModuleModel> modules = new List<ModuleModel>();
                string sql = "SELECT ModuleID, ModuleName FROM Module ORDER BY ModuleName;";

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
                                    modules.Add(new ModuleModel
                                    {
                                        ModuleID = reader.GetInt32(reader.GetOrdinal("ModuleID")),
                                        ModuleName = reader.GetString(reader.GetOrdinal("ModuleName"))
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[DB ERROR] Error fetching modules: {ex.Message}");
                    throw new InvalidOperationException("Failed to retrieve Modules for ComboBox.", ex);
                }
                return modules;
            }
        }

        /// <summary>
        /// Retrieves the assessment status for a specific participant across all available modules/assessments.
        /// Includes derived enrollment status if no explicit enrollment status column exists.
        /// </summary>
        public List<ParticipantAssessmentStatusModel> GetParticipantAssessmentStatus(int participantStakeholderID)
        {
            List<ParticipantAssessmentStatusModel> statuses = new List<ParticipantAssessmentStatusModel>();

            string sql = @"
        SELECT 
            M.ModuleID,
            M.ModuleName,
            MP.PartnerStakeholderID,
            S.PartnerName,
            E.EnrollmentID,
            E.EnrollmentDate,
            E.StartDate,
            E.EndDate,
            MA.AssessmentID,
            MA.AssessmentDate,
            MA.Mark,
            MA.PassStatus
        FROM Enrollment E
        INNER JOIN Module M ON E.ModuleID = M.ModuleID
        LEFT JOIN ModulePartner MP ON M.ModuleID = MP.ModuleID
        LEFT JOIN Stakeholder S ON MP.PartnerStakeholderID = S.StakeholderID
        LEFT JOIN ModuleAssessment MA ON E.EnrollmentID = MA.EnrollmentID
        WHERE E.ParticipantStakeholderID = @ParticipantID
        ORDER BY M.ModuleName, MA.AssessmentDate DESC;";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@ParticipantID", SqlDbType.Int).Value = participantStakeholderID;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int moduleID = reader.GetInt32(reader.GetOrdinal("ModuleID"));
                            string moduleName = reader.GetString(reader.GetOrdinal("ModuleName"));
                            int enrollmentID = reader.GetInt32(reader.GetOrdinal("EnrollmentID"));
                            DateTime enrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate"));

                            decimal? mark = reader.IsDBNull(reader.GetOrdinal("Mark"))
                                ? (decimal?)null
                                : reader.GetDecimal(reader.GetOrdinal("Mark"));

                            string partnerName = reader.IsDBNull(reader.GetOrdinal("PartnerName"))
                                ? "N/A"
                                : reader.GetString(reader.GetOrdinal("PartnerName"));

                            DateTime? assessmentDate = reader.IsDBNull(reader.GetOrdinal("AssessmentDate"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("AssessmentDate"));

                            string passStatus = reader.IsDBNull(reader.GetOrdinal("PassStatus"))
                                ? "Not Attempted"
                                : reader.GetString(reader.GetOrdinal("PassStatus"));

                            // Derive enrollment status
                            string enrollmentStatus;
                            if (assessmentDate.HasValue && passStatus.Equals("Pass", StringComparison.OrdinalIgnoreCase))
                                enrollmentStatus = "Completed";
                            else if (assessmentDate.HasValue)
                                enrollmentStatus = "In Progress";
                            else
                                enrollmentStatus = "Enrolled";

                            statuses.Add(new ParticipantAssessmentStatusModel
                            {
                                ModuleID = moduleID,
                                ModuleName = moduleName,
                                PartnerName = partnerName,
                                EnrollmentID = enrollmentID,
                                EnrollmentDate = enrollmentDate,
                                LastScore = mark,
                                LastAttemptDate = assessmentDate,
                                EnrollmentStatus = enrollmentStatus,
                                PassStatus = passStatus
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred while retrieving participant assessments.\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            return statuses;
        }

        public int EnrollParticipantInModule(int participantStakeholderID, int moduleID)
        {
            string sql = @"
        INSERT INTO Enrollment (ParticipantStakeholderID, ModuleID, EnrollmentDate)
        OUTPUT INSERTED.EnrollmentID
        VALUES (@ParticipantID, @ModuleID, @EnrollmentDate);
    ";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@ParticipantID", SqlDbType.Int).Value = participantStakeholderID;
                    cmd.Parameters.Add("@ModuleID", SqlDbType.Int).Value = moduleID;
                    cmd.Parameters.Add("@EnrollmentDate", SqlDbType.Date).Value = DateTime.Now.Date;

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (SqlException sqlex) when (sqlex.Number == 2627 /*PK violation*/ || sqlex.Number == 2601 /*unique index*/)
            {
                // Already enrolled or duplicate — return -2 to indicate duplicate
                return -2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred while enrolling in the module.\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return -1;
            }
        }
        public List<ModuleEnrollmentModel> GetModulesWithEnrollmentStatus(int participantStakeholderID)
        {
            var list = new List<ModuleEnrollmentModel>();

            string sql = @"
        SELECT
            M.ModuleID,
            M.ModuleName,
            E.EnrollmentID,
            E.EnrollmentDate
        FROM Module M
        LEFT JOIN Enrollment E 
            ON M.ModuleID = E.ModuleID
            AND E.ParticipantStakeholderID = @ParticipantID
        ORDER BY M.ModuleName;";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@ParticipantID", SqlDbType.Int).Value = participantStakeholderID;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int moduleId = reader.GetInt32(reader.GetOrdinal("ModuleID"));
                            string moduleName = reader.IsDBNull(reader.GetOrdinal("ModuleName"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("ModuleName"));

                            int? enrollmentId = reader.IsDBNull(reader.GetOrdinal("EnrollmentID"))
                                ? (int?)null
                                : reader.GetInt32(reader.GetOrdinal("EnrollmentID"));

                            DateTime? enrollmentDate = reader.IsDBNull(reader.GetOrdinal("EnrollmentDate"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("EnrollmentDate"));

                            list.Add(new ModuleEnrollmentModel
                            {
                                ModuleID = moduleId,
                                ModuleName = moduleName,
                                EnrollmentID = enrollmentId,
                                EnrollmentDate = enrollmentDate
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred while loading assessments for the dashboard.\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            return list;
        }

        /// <summary>
        /// Retrieves all stakeholders flagged as Partners for ComboBox binding.
        /// </summary>
        public List<PartnerModel> GetPartners()
        {
            List<PartnerModel> partners = new List<PartnerModel>();
            // Query Stakeholder table, filtering by RoleType='Partner' and retrieving PartnerName.
            string sql = @"
                SELECT 
                    StakeholderID AS PartnerID, 
                    PartnerName 
                FROM Stakeholder 
                WHERE RoleType = 'Partner' AND PartnerName IS NOT NULL
                ORDER BY PartnerName;";

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
                                partners.Add(new PartnerModel
                                {
                                    PartnerID = reader.GetInt32(reader.GetOrdinal("PartnerID")),
                                    PartnerName = reader.GetString(reader.GetOrdinal("PartnerName"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error fetching partners: {ex.Message}");
                throw new InvalidOperationException("Failed to retrieve Partners for ComboBox.", ex);
            }
            return partners;
        }

        /// <summary>
        /// Retrieves all Modules for the Module Maintenance grid.
        /// </summary>
        public List<ModuleMaintenanceModel> GetAllModules()
        {
            List<ModuleMaintenanceModel> modules = new List<ModuleMaintenanceModel>();
            string sql = "SELECT ModuleID, ModuleName, Description, ContentOutline FROM Module ORDER BY ModuleName;";

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
                                modules.Add(new ModuleMaintenanceModel
                                {
                                    ModuleID = reader.GetInt32(reader.GetOrdinal("ModuleID")),
                                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader.GetString(reader.GetOrdinal("Description")),
                                    ContentOutline = reader.IsDBNull(reader.GetOrdinal("ContentOutline")) ? string.Empty : reader.GetString(reader.GetOrdinal("ContentOutline"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationException applicationException = new ApplicationException($"Failed to retrieve module list: {ex.Message}", ex);
                throw applicationException;
            }
            return modules;
        }

        /// <summary>
        /// Adds a new Module to the database.
        /// </summary>
        public int AddModule(string moduleName, string description, string contentOutline)
        {
            string sql = @"
                INSERT INTO Module (ModuleName, Description, ContentOutline) 
                VALUES (@Name, @Description, @ContentOutline); 
                SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", moduleName);
                        command.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                        command.Parameters.AddWithValue("@ContentOutline", (object)contentOutline ?? DBNull.Value);

                        // ExecuteScalar returns the ID of the new row
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error adding module: {ex.Message}");
                return -1; // Indicate failure
            }
        }

        /// <summary>
        /// Updates an existing Module in the database.
        /// </summary>
        public bool UpdateModule(int moduleID, string moduleName, string description, string contentOutline)
        {
            string sql = @"
                UPDATE Module 
                SET ModuleName = @Name, Description = @Description, ContentOutline = @ContentOutline 
                WHERE ModuleID = @ID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID", moduleID);
                        command.Parameters.AddWithValue("@Name", moduleName);
                        command.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                        command.Parameters.AddWithValue("@ContentOutline", (object)contentOutline ?? DBNull.Value);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error updating module: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes a Module from the database.
        /// </summary>
        public bool DeleteModule(int moduleId)
        {
            bool success = false;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Module WHERE ModuleID = @ModuleID;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ModuleID", moduleId);

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        success = rows > 0;
                    }
                    catch (SqlException ex)
                    {
                        // Check if deletion failed due to FK constraints
                        if (ex.Number == 547)
                        {
                            MessageBox.Show("This module cannot be deleted because it is linked to assessments, enrollments, or partners.",
                                "Foreign Key Constraint", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting module: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            return success;
        }


        /// <summary>
        /// Retrieves all stakeholders with RoleType 'Partner' for the Partner Maintenance grid.
        /// </summary>
        public List<PartnerMaintenanceModel> GetAllPartnersForMaintenance()
        {
            List<PartnerMaintenanceModel> partners = new List<PartnerMaintenanceModel>();
            string sql = @"
                SELECT 
                    StakeholderID, 
                    PartnerName, 
                    EmailAddress, 
                    ContactNumber 
                FROM Stakeholder 
                WHERE RoleType = 'Partner'
                ORDER BY PartnerName;";

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
                                partners.Add(new PartnerMaintenanceModel
                                {
                                    PartnerID = reader.GetInt32(reader.GetOrdinal("StakeholderID")),
                                    PartnerName = reader.IsDBNull(reader.GetOrdinal("PartnerName")) ? string.Empty : reader.GetString(reader.GetOrdinal("PartnerName")),
                                    EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? string.Empty : reader.GetString(reader.GetOrdinal("EmailAddress")),
                                    ContactNumber = reader.IsDBNull(reader.GetOrdinal("ContactNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("ContactNumber"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationException applicationException = new ApplicationException($"Failed to retrieve partner list: {ex.Message}", ex);
                throw applicationException;
            }
            return partners;
        }

        /// <summary>
        /// Adds a new Partner (Stakeholder with RoleType='Partner') to the database.
        /// </summary>
        public int AddPartner(string partnerName, string email, string contactNumber)
        {
            string sql = @"
                INSERT INTO Stakeholder (RoleType, PartnerName, EmailAddress, ContactNumber) 
                VALUES ('Partner', @PartnerName, @Email, @ContactNumber); 
                SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PartnerName", partnerName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@ContactNumber", (object)contactNumber ?? DBNull.Value);

                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (SqlException sqlex) when (sqlex.Number == 2627) // Unique constraint violation (EmailAddress)
            {
                Console.WriteLine($"\n[DB ERROR] Error adding partner: Email address already exists.");
                return -2; // Custom error code for duplicate email
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error adding partner: {ex.Message}");
                return -1; // Indicate generic failure
            }
        }

        /// <summary>
        /// Updates an existing Partner (Stakeholder) in the database.
        /// </summary>
        public bool UpdatePartner(int partnerID, string partnerName, string email, string contactNumber)
        {
            string sql = @"
                UPDATE Stakeholder 
                SET PartnerName = @PartnerName, EmailAddress = @Email, ContactNumber = @ContactNumber 
                WHERE StakeholderID = @PartnerID AND RoleType = 'Partner';";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PartnerID", partnerID);
                        command.Parameters.AddWithValue("@PartnerName", partnerName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@ContactNumber", (object)contactNumber ?? DBNull.Value);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException sqlex) when (sqlex.Number == 2627) // Unique constraint violation (EmailAddress)
            {
                Console.WriteLine($"\n[DB ERROR] Error updating partner: Email address already exists.");
                return false; // Return false for update failure
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error updating partner: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes a Partner (Stakeholder) from the database.
        /// </summary>
        public bool DeletePartner(int partnerID)
        {
            // Partner records can be linked via the ModulePartner table (PartnerStakeholderID FK)
            string sql = "DELETE FROM Stakeholder WHERE StakeholderID = @PartnerID AND RoleType = 'Partner';";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PartnerID", partnerID);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException sqlex) when (sqlex.Number == 547)
            {
                // Error 547 is Foreign Key constraint violation
                Console.WriteLine($"\n[DB ERROR] Cannot delete partner due to foreign key constraint: {sqlex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error deleting partner: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves all stakeholders (Admin, Participant, Instructor, Partner) from the database.
        /// Uses a direct select from the consolidated Stakeholder table.
        /// </summary>
        public List<Stakeholder> GetAllStakeholders()
        {
            List<Stakeholder> stakeholders = new List<Stakeholder>();

            // UPDATED SQL: Direct select from Stakeholder, no joins necessary.
            // NOTE: Using EmailAddress from the table to map to the Email property in the C# model.
            string sql = @"
                SELECT 
                    StakeholderID, 
                    FirstName, 
                    LastName, 
                    EmailAddress, 
                    RoleType, 
                    ContactNumber, 
                    PartnerName
                FROM Stakeholder
                ORDER BY LastName, FirstName";

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
                                // Get column indices once for efficiency and use in null checks
                                int contactNumberOrdinal = reader.GetOrdinal("ContactNumber");
                                int partnerNameOrdinal = reader.GetOrdinal("PartnerName");
                                int firstNameOrdinal = reader.GetOrdinal("FirstName");
                                int lastNameOrdinal = reader.GetOrdinal("LastName");

                                stakeholders.Add(new Stakeholder
                                {
                                    // Non-nullable fields
                                    StakeholderID = reader.GetInt32(reader.GetOrdinal("StakeholderID")),
                                    Email = reader.GetString(reader.GetOrdinal("EmailAddress")), // Mapping EmailAddress (SQL) to Email (C#)
                                    RoleType = reader.GetString(reader.GetOrdinal("RoleType")),

                                    // CRITICAL: Use IsDBNull() for potentially NULL columns (FirstName, LastName, ContactNumber, PartnerName)
                                    FirstName = reader.IsDBNull(firstNameOrdinal) ? null : reader.GetString(firstNameOrdinal),
                                    LastName = reader.IsDBNull(lastNameOrdinal) ? null : reader.GetString(lastNameOrdinal),
                                    ContactNumber = reader.IsDBNull(contactNumberOrdinal) ? null : reader.GetString(contactNumberOrdinal),
                                    PartnerName = reader.IsDBNull(partnerNameOrdinal) ? null : reader.GetString(partnerNameOrdinal)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // CRITICAL LOGGING: This is where you will see the exact database error if the fetch fails.
                Console.WriteLine($"\n[DB ERROR - GetAllStakeholders] Failed to fetch data. Error: {ex.Message}");
            }
            return stakeholders;
        }

        /// <summary>
        /// Adds a new Stakeholder to the database. Uses a plain text password (insecure) for this project scope.
        /// </summary>
        public int AddStakeholder(
            string firstName,
            string lastName,
            string email,
            string roleType,
            string partnerName = null,
            string contactNumber = null,
            string idNumber = null)
        {
            string sql = @"
        INSERT INTO Stakeholder (FirstName, LastName, EmailAddress, RoleType, PartnerName, ContactNumber, IDNumber)
        VALUES (@FirstName, @LastName, @Email, @RoleType, @PartnerName, @ContactNumber, @IDNumber);
        SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@RoleType", roleType);
                        command.Parameters.AddWithValue("@PartnerName", (object)partnerName ?? DBNull.Value);
                        command.Parameters.AddWithValue("@ContactNumber", (object)contactNumber ?? DBNull.Value);
                        command.Parameters.AddWithValue("@IDNumber", (object)idNumber ?? DBNull.Value);

                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (SqlException sqlex) when (sqlex.Number == 2627) // Duplicate email
            {
                Console.WriteLine("[DB ERROR] Error adding stakeholder: Email address already exists.");
                return -2;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] Error adding stakeholder: {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Updates an existing Stakeholder's details (excluding password).
        /// </summary>
        public bool UpdateStakeholder(int stakeholderID, string firstName, string lastName, string email, string roleType, string partnerName = null, string contactNumber = null)
        {
            string sql = @"
                UPDATE Stakeholder 
                SET FirstName = @FirstName, 
                    LastName = @LastName, 
                    EmailAddress = @Email, 
                    RoleType = @RoleType,
                    PartnerName = @PartnerName,
                    ContactNumber = @ContactNumber
                WHERE StakeholderID = @ID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID", stakeholderID);
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@RoleType", roleType);
                        command.Parameters.AddWithValue("@PartnerName", (object)partnerName ?? DBNull.Value);
                        command.Parameters.AddWithValue("@ContactNumber", (object)contactNumber ?? DBNull.Value);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException sqlex) when (sqlex.Number == 2627) // Unique constraint violation on EmailAddress
            {
                Console.WriteLine($"\n[DB ERROR] Error updating stakeholder: Email address already exists.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error updating stakeholder: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Updates only the PasswordHash for a stakeholder.
        /// </summary>
        public bool UpdateStakeholderPassword(int stakeholderID, string newPassword)
        {
            // In a real application, 'newPassword' would be the hashed value.
            string sql = "UPDATE Stakeholder SET PasswordHash = @Password WHERE StakeholderID = @ID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID", stakeholderID);
                        command.Parameters.AddWithValue("@Password", newPassword);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error updating stakeholder password: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes a Stakeholder from the database.
        /// </summary>
        public bool DeleteStakeholder(int stakeholderID)
        {
            string sql = "DELETE FROM Stakeholder WHERE StakeholderID = @ID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID", stakeholderID);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException sqlex) when (sqlex.Number == 547)
            {
                // Error 547 is Foreign Key constraint violation
                Console.WriteLine($"\n[DB ERROR] Cannot delete stakeholder due to foreign key constraint: {sqlex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error deleting stakeholder: {ex.Message}");
                return false;
            }
        }

        // --- SUPPORTING METHODS (FOR COMBOBOXES) ---

        /// <summary>
        /// Retrieves all active Modules for ComboBox binding.
        /// </summary>
        public List<ModuleModel> GetModules()
        {
            List<ModuleModel> modules = new List<ModuleModel>();
            string sql = "SELECT ModuleID, ModuleName FROM Module ORDER BY ModuleName;";

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
                                modules.Add(new ModuleModel
                                {
                                    ModuleID = reader.GetInt32(reader.GetOrdinal("ModuleID")),
                                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error fetching modules: {ex.Message}");
                throw new InvalidOperationException("Failed to retrieve Modules for ComboBox.", ex);
            }
            return modules;
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
        /// Retrieves all Modules for use in a lookup dropdown. (GetAllModulesForLookup)
        /// </summary>
        public List<ModuleLookup> GetAllModulesForLookup()
        {
            List<ModuleLookup> modules = new List<ModuleLookup>();
            string sql = "SELECT ModuleID, ModuleName FROM Module ORDER BY ModuleName";
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            modules.Add(new ModuleLookup
                            {
                                ModuleID = reader.GetInt32(reader.GetOrdinal("ModuleID")),
                                ModuleName = reader.GetString(reader.GetOrdinal("ModuleName"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error fetching module lookups: {ex.Message}");
            }
            return modules;
        }

        /// <summary>
        /// Retrieves all Community Partners for use in a lookup dropdown. (GetAllPartnersForLookup)
        /// </summary>
        public List<PartnerLookup> GetAllPartnersForLookup()
        {
            List<PartnerLookup> partners = new List<PartnerLookup>();
            string sql = "SELECT PartnerID, PartnerName FROM CommunityPartner ORDER BY PartnerName";
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            partners.Add(new PartnerLookup
                            {
                                PartnerID = reader.GetInt32(reader.GetOrdinal("PartnerID")),
                                PartnerName = reader.GetString(reader.GetOrdinal("PartnerName"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error fetching partner lookups: {ex.Message}");
            }
            return partners;
        }

        /// <summary>
        /// Retrieves all Assessments from the database, including Module and Partner names. (GetAllAssessments)
        /// </summary>
        public List<Assessment> GetAllAssessments()
        {
            List<Assessment> assessments = new List<Assessment>();
            string sql = @"
            SELECT 
                A.AssessmentID, A.AssessmentName, A.ModuleID, M.ModuleName, 
                A.PartnerID, P.PartnerName, A.DateCreated, A.PassPercentage
            FROM Assessment A
            JOIN Module M ON A.ModuleID = M.ModuleID
            JOIN CommunityPartner P ON A.PartnerID = P.PartnerID
            ORDER BY A.AssessmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            assessments.Add(new Assessment
                            {
                                AssessmentID = reader.GetInt32(reader.GetOrdinal("AssessmentID")),
                                AssessmentName = reader.GetString(reader.GetOrdinal("AssessmentName")),
                                ModuleID = reader.GetInt32(reader.GetOrdinal("ModuleID")),
                                ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                                PartnerID = reader.GetInt32(reader.GetOrdinal("PartnerID")),
                                PartnerName = reader.GetString(reader.GetOrdinal("PartnerName")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                PassPercentage = reader.GetDecimal(reader.GetOrdinal("PassPercentage"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error fetching assessments: {ex.Message}");
            }
            return assessments;
        }

        /// <summary>
        /// Adds a new Assessment. (AddAssessment)
        /// </summary>
        public int AddAssessment(string name, int moduleId, int partnerId, decimal passPercentage)
        {
            int newId = -1;
            string sql = @"
            INSERT INTO Assessment (AssessmentName, ModuleID, PartnerID, DateCreated, PassPercentage)
            VALUES (@Name, @ModuleID, @PartnerID, @DateCreated, @PassPercentage);
            SELECT CAST(scope_identity() AS INT);";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@ModuleID", moduleId);
                        command.Parameters.AddWithValue("@PartnerID", partnerId);
                        command.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                        command.Parameters.AddWithValue("@PassPercentage", passPercentage);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            newId = (int)result;
                        }
                    }
                }
                Console.WriteLine($"[SUCCESS] Assessment '{name}' added with ID: {newId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] Failed to add assessment: {ex.Message}");
            }
            return newId;
        }

        /// <summary>
        /// Updates an existing Assessment. (UpdateAssessment)
        /// </summary>
        public bool UpdateAssessment(int assessmentId, string name, int moduleId, int partnerId, decimal passPercentage)
        {
            string sql = @"
            UPDATE Assessment SET 
                AssessmentName = @Name, 
                ModuleID = @ModuleID, 
                PartnerID = @PartnerID, 
                PassPercentage = @PassPercentage 
            WHERE AssessmentID = @AssessmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@AssessmentID", assessmentId);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@ModuleID", moduleId);
                        command.Parameters.AddWithValue("@PartnerID", partnerId);
                        command.Parameters.AddWithValue("@PassPercentage", passPercentage);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"[SUCCESS] Assessment ID {assessmentId} updated.");
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] Failed to update assessment: {ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// Deletes an Assessment by its ID. (DeleteAssessment)
        /// </summary>
        public bool DeleteAssessment(int assessmentId)
        {
            string sql = "DELETE FROM Assessment WHERE AssessmentID = @AssessmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@AssessmentID", assessmentId);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"[SUCCESS] Assessment ID {assessmentId} deleted.");
                            return true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    Console.WriteLine($"[DB ERROR] Cannot delete Assessment ID {assessmentId}. It may be linked to results or questions (Error 547).");
                    return false;
                }
                Console.WriteLine($"[DB ERROR] Failed to delete assessment: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GENERAL ERROR] Failed to delete assessment: {ex.Message}");
            }
            return false;
        }

        public List<ModuleAssessmentResult> GetAllResultsForParticipant(int participantId)
        {
            List<ModuleAssessmentResult> results = new List<ModuleAssessmentResult>();
            string sql = @"
        SELECT MAR.*, MA.AssessmentName, M.ModuleName
        FROM ModuleAssessmentResult MAR
        INNER JOIN ModuleAssessment MA ON MAR.AssessmentID = MA.AssessmentID
        INNER JOIN Module M ON MA.ModuleID = M.ModuleID
        WHERE MAR.ParticipantID = @PId
        ORDER BY MAR.AssessmentDate DESC";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@PId", participantId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new ModuleAssessmentResult
                                {
                                    AssessmentID = reader.GetInt32(reader.GetOrdinal("AssessmentID")),
                                    EnrollmentID = reader.GetInt32(reader.GetOrdinal("EnrollmentID")),
                                    AssessmentDate = reader.GetDateTime(reader.GetOrdinal("AssessmentDate")),
                                    Mark = reader.GetDecimal(reader.GetOrdinal("Mark")),
                                    PassStatus = reader["PassStatus"].ToString(),
                                    AssessmentName = reader["AssessmentName"].ToString(),
                                    ModuleName = reader["ModuleName"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR - GetAllResultsForParticipant] {ex.Message}");
            }

            return results;
        }


        public bool RequestEnrollment(int participantId, int moduleId)
        {
            // Check if already enrolled or pending
            string checkSql = "SELECT COUNT(*) FROM Enrollment WHERE StakeholderID = @PId AND ModuleID = @MId AND Status != 'Denied'";

            // Insert the new enrollment request
            string insertSql = "INSERT INTO Enrollment (StakeholderID, ModuleID, RequestDate, Status) VALUES (@PId, @MId, GETDATE(), 'Pending')";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // 1. Check existing enrollment
                    using (SqlCommand checkCommand = new SqlCommand(checkSql, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@PId", participantId);
                        checkCommand.Parameters.AddWithValue("@MId", moduleId);
                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("You are already enrolled or have a pending request for this module.", "Enrollment Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }

                    // 2. Insert new request
                    using (SqlCommand insertCommand = new SqlCommand(insertSql, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@PId", participantId);
                        insertCommand.Parameters.AddWithValue("@MId", moduleId);
                        return insertCommand.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR - RequestEnrollment] Failed to request enrollment: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Admin approves or denies a specific enrollment request.
        /// If approved, Status becomes 'Approved'. If denied, Status becomes 'Denied'.
        /// </summary>
        public bool UpdateEnrollmentStatus(int enrollmentId, string newStatus)
        {
            if (newStatus != "Approved" && newStatus != "Denied")
            {
                throw new ArgumentException("Status must be 'Approved' or 'Denied'.");
            }

            string updateSql = "UPDATE Enrollment SET Status = @Status, ApprovalDate = GETDATE() WHERE EnrollmentID = @EId";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(updateSql, connection))
                    {
                        command.Parameters.AddWithValue("@Status", newStatus);
                        command.Parameters.AddWithValue("@EId", enrollmentId);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR - UpdateEnrollmentStatus] Failed to update enrollment status: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves all pending enrollment requests (Admin View).
        /// </summary>
        public List<EnrollmentModel> GetPendingEnrollments()
        {
            List<EnrollmentModel> enrollments = new List<EnrollmentModel>();
            string sql = @"
        SELECT 
            E.EnrollmentID, 
            E.StakeholderID, 
            M.ModuleID,
            M.ModuleName,
            E.RequestDate,
            E.Status,
            S.FirstName + ' ' + S.LastName AS ParticipantName
        FROM Enrollment E
        INNER JOIN Module M ON E.ModuleID = M.ModuleID
        INNER JOIN Stakeholder S ON E.StakeholderID = S.StakeholderID
        -- Show all, but order Pending ones first
        ORDER BY 
            CASE WHEN E.Status = 'Pending' THEN 0 ELSE 1 END,
            E.RequestDate DESC";

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
                                enrollments.Add(new EnrollmentModel
                                {
                                    EnrollmentID = reader.GetInt32(reader.GetOrdinal("EnrollmentID")),
                                    StakeholderID = reader.GetInt32(reader.GetOrdinal("StakeholderID")),
                                    ModuleID = reader.GetInt32(reader.GetOrdinal("ModuleID")),
                                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                                    RequestDate = reader.GetDateTime(reader.GetOrdinal("RequestDate")),
                                    Status = reader.GetString(reader.GetOrdinal("Status")),
                                    ParticipantName = reader.GetString(reader.GetOrdinal("ParticipantName"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR - GetPendingEnrollments] {ex.Message}");
            }
            return enrollments;
        }

        /// <summary>
        /// Retrieves all available modules for a participant to request enrollment (Participant View).
        /// Excludes modules the participant is already Approved or Pending for.
        /// </summary>
        public List<EnrollmentModel> GetAvailableModules(int participantId)
        {
            List<EnrollmentModel> modules = new List<EnrollmentModel>();
            string sql = @"
        SELECT 
            M.ModuleID, 
            M.ModuleName,
            ISNULL(E.Status, 'Not Enrolled') AS EnrollmentStatus
        FROM Module M
        LEFT JOIN Enrollment E 
            ON M.ModuleID = E.ModuleID 
            AND E.StakeholderID = @PId
        ORDER BY M.ModuleName";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PId", participantId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                modules.Add(new EnrollmentModel
                                {
                                    ModuleID = reader.GetInt32(reader.GetOrdinal("ModuleID")),
                                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                                    Status = reader.GetString(reader.GetOrdinal("EnrollmentStatus"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR - GetAvailableModules] {ex.Message}");
            }
            return modules;
        }

        public class ModuleLookup
        {
            public int ModuleID { get; set; }
            public string ModuleName { get; set; }
        }

        // Simple class to fetch Community Partner data for the dropdown
        public class PartnerLookup
        {
            public int PartnerID { get; set; }
            public string PartnerName { get; set; }
        }

        // Full class representing an Assessment record, including display names for FKs
        public class Assessment
        {
            public int AssessmentID { get; set; }
            public string AssessmentName { get; set; }
            public int ModuleID { get; set; }
            public string ModuleName { get; set; } // Used for display in DGV and ComboBox
            public int PartnerID { get; set; }
            public string PartnerName { get; set; } // Used for display in DGV and ComboBox
            public DateTime DateCreated { get; set; }
            public decimal PassPercentage { get; set; }
        }

        public class ParticipantPerformanceSummary
        {
            public string ParticipantName { get; set; }
            public string IDNumber { get; set; }
            public int AssessmentsTaken { get; set; }
            public decimal AverageScorePercentage { get; set; }
            public int AssessmentsPassed { get; set; }
            public string OverallStatus { get; set; } // e.g., "High Performer", "Needs Review"
        }

        // Report 2: Summary of assessment/module performance
        public class AssessmentModuleSummary
        {
            public string ModuleName { get; set; }
            public string AssessmentName { get; set; }
            public int TotalParticipants { get; set; }
            public decimal AverageScorePercentage { get; set; }
            public decimal PassRatePercentage { get; set; }
            public DateTime DateCreated { get; set; }
        }

        // =========================================================================================
        // REPORTING METHODS (Required for ReportsForm.cs)
        // =========================================================================================

        /// <summary>
        /// Retrieves a summary of participant performance, filtered by optional Module or Partner.
        /// </summary>
        public List<ParticipantPerformanceSummary> GetParticipantPerformanceSummary(int? moduleId, int? partnerId)
        {
            List<ParticipantPerformanceSummary> summary = new List<ParticipantPerformanceSummary>();

            string sql = $@"
    WITH ParticipantScores AS (
        SELECT
            S.StakeholderID,
            S.FirstName + ' ' + S.LastName AS ParticipantName,
            S.IDNumber,
            MA.Mark AS Score,
            CASE WHEN MA.PassStatus = 'Pass' THEN 1 ELSE 0 END AS Passed,
            M.ModuleID,
            MP.PartnerStakeholderID AS PartnerID
        FROM Stakeholder S
        JOIN Enrollment E ON S.StakeholderID = E.ParticipantStakeholderID
        JOIN Module M ON E.ModuleID = M.ModuleID
        LEFT JOIN ModuleAssessment MA ON E.EnrollmentID = MA.EnrollmentID
        LEFT JOIN ModulePartner MP ON M.ModuleID = MP.ModuleID
        WHERE S.RoleType = 'Participant'
        {(moduleId.HasValue ? " AND M.ModuleID = @ModuleID" : "")}
        {(partnerId.HasValue ? " AND MP.PartnerStakeholderID = @PartnerID" : "")}
    )
    SELECT
        ParticipantName,
        IDNumber,
        COUNT(StakeholderID) AS AssessmentsTaken,
        ISNULL(CAST(AVG(Score) AS DECIMAL(5,2)), 0) AS AverageScorePercentage,
        SUM(Passed) AS AssessmentsPassed
    FROM ParticipantScores
    GROUP BY ParticipantName, IDNumber
    ORDER BY AverageScorePercentage DESC;";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (moduleId.HasValue) command.Parameters.AddWithValue("@ModuleID", moduleId.Value);
                        if (partnerId.HasValue) command.Parameters.AddWithValue("@PartnerID", partnerId.Value);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int taken = reader.GetInt32(reader.GetOrdinal("AssessmentsTaken"));
                                int passed = reader.GetInt32(reader.GetOrdinal("AssessmentsPassed"));
                                decimal avgScore = reader.GetDecimal(reader.GetOrdinal("AverageScorePercentage"));
                                string status = "Needs Review";

                                if (taken > 0)
                                {
                                    decimal passRate = (decimal)passed / taken;
                                    if (passRate >= 0.8M) status = "High Performer";
                                    else if (passRate >= 0.5M) status = "Consistent";
                                    else status = "Needs Review";
                                }

                                summary.Add(new ParticipantPerformanceSummary
                                {
                                    ParticipantName = reader.GetString(reader.GetOrdinal("ParticipantName")),
                                    IDNumber = reader.IsDBNull(reader.GetOrdinal("IDNumber")) ? "N/A" : reader.GetString(reader.GetOrdinal("IDNumber")),
                                    AssessmentsTaken = taken,
                                    AverageScorePercentage = avgScore,
                                    AssessmentsPassed = passed,
                                    OverallStatus = status
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error fetching Participant Performance Summary: {ex.Message}");
            }

            return summary;
        }

        /// <summary>
        /// Retrieves summary data for all Modules and their Assessments, filtered by an optional date range.
        /// </summary>
        public List<AssessmentModuleSummary> GetAssessmentSummaryByModule(DateTime? startDate, DateTime? endDate)
        {
            List<AssessmentModuleSummary> summary = new List<AssessmentModuleSummary>();

            string sql = $@"
    WITH AssessmentStats AS (
        SELECT
            M.ModuleName,
            M.ModuleID,
            ISNULL(MIN(MA.AssessmentDate), GETDATE()) AS DateCreated,
            MA.Mark AS Score,
            MA.PassStatus,
            E.EnrollmentID
        FROM Module M
        LEFT JOIN Enrollment E ON M.ModuleID = E.ModuleID
        LEFT JOIN ModuleAssessment MA ON E.EnrollmentID = MA.EnrollmentID
        WHERE 1=1
        {(startDate.HasValue ? " AND MA.AssessmentDate >= @StartDate" : "")}
        {(endDate.HasValue ? " AND MA.AssessmentDate <= @EndDate" : "")}
        GROUP BY M.ModuleName, M.ModuleID, MA.Mark, MA.PassStatus, E.EnrollmentID
    )
    SELECT
        ModuleName,
        'Module Assessment' AS AssessmentName,
        MIN(DateCreated) AS DateCreated,
        COUNT(Score) AS TotalParticipants,
        ISNULL(CAST(AVG(Score) AS DECIMAL(5,2)), 0) AS AverageScorePercentage,
        ISNULL(CAST(SUM(CASE WHEN PassStatus = 'Pass' THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(Score), 0) AS DECIMAL(5,2)), 0) AS PassRatePercentage
    FROM AssessmentStats
    GROUP BY ModuleName
    ORDER BY ModuleName;";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (startDate.HasValue) command.Parameters.AddWithValue("@StartDate", startDate.Value.Date);
                        if (endDate.HasValue) command.Parameters.AddWithValue("@EndDate", endDate.Value.Date.AddDays(1).AddSeconds(-1));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                summary.Add(new AssessmentModuleSummary
                                {
                                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                                    AssessmentName = reader.GetString(reader.GetOrdinal("AssessmentName")),
                                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                    TotalParticipants = reader.GetInt32(reader.GetOrdinal("TotalParticipants")),
                                    AverageScorePercentage = reader.GetDecimal(reader.GetOrdinal("AverageScorePercentage")),
                                    PassRatePercentage = reader.GetDecimal(reader.GetOrdinal("PassRatePercentage"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error fetching Assessment Module Summary: {ex.Message}");
            }

            return summary;
        }


        /// <summary>
        /// Retrieves ALL historical results for a specific participant on a specific assessment.
        /// Requires joining through Enrollment to link StakeholderID to ModuleAssessment records.
        /// </summary>
        public List<ModuleAssessmentResult> GetAssessmentResultsHistory(int participantId, int assessmentId)
        {
            List<ModuleAssessmentResult> history = new List<ModuleAssessmentResult>();

            // Joins Stakeholder -> Enrollment -> ModuleAssessment
            string sql = @"
        SELECT 
            MA.AssessmentID, 
            MA.EnrollmentID, 
            MA.AssessmentDate, 
            MA.Mark, 
            MA.PassStatus
        FROM ModuleAssessment MA
        JOIN Enrollment E ON MA.EnrollmentID = E.EnrollmentID
        WHERE E.StakeholderID = @ParticipantID 
          AND MA.AssessmentID = @AssessmentID
        ORDER BY MA.AssessmentDate DESC;"; // Order by date descending (most recent first)

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ParticipantID", participantId);
                        command.Parameters.AddWithValue("@AssessmentID", assessmentId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                history.Add(new ModuleAssessmentResult
                                {
                                    AssessmentID = reader.GetInt32(reader.GetOrdinal("AssessmentID")),
                                    EnrollmentID = reader.GetInt32(reader.GetOrdinal("EnrollmentID")),
                                    AssessmentDate = reader.GetDateTime(reader.GetOrdinal("AssessmentDate")),
                                    Mark = reader.GetDecimal(reader.GetOrdinal("Mark")),
                                    PassStatus = reader.GetString(reader.GetOrdinal("PassStatus"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[DB ERROR] Error fetching assessment results history: {ex.Message}");
            }
            return history;
        }

        public int AddStandaloneAssessment(string assessmentName, int moduleId, int partnerId, decimal passPercentage)
        {
            int newId = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO AssessmentLibrary (AssessmentName, ModuleID, PartnerStakeholderID, PassPercentage)
            VALUES (@AssessmentName, @ModuleID, @PartnerID, @PassPercentage);
            SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AssessmentName", assessmentName);
                    cmd.Parameters.AddWithValue("@ModuleID", moduleId);
                    cmd.Parameters.AddWithValue("@PartnerID", partnerId);
                    cmd.Parameters.AddWithValue("@PassPercentage", passPercentage);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            newId = Convert.ToInt32(result);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error adding standalone assessment: " + ex.Message);
                    }
                }
            }

            return newId;
        }

        public bool UpdateStandaloneAssessment(int assessmentId, string assessmentName, int moduleId, int partnerId, decimal passPercentage)
        {
            bool success = false;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE AssessmentLibrary
            SET AssessmentName = @AssessmentName,
                ModuleID = @ModuleID,
                PartnerStakeholderID = @PartnerID,
                PassPercentage = @PassPercentage
            WHERE AssessmentID = @AssessmentID;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AssessmentName", assessmentName);
                    cmd.Parameters.AddWithValue("@ModuleID", moduleId);
                    cmd.Parameters.AddWithValue("@PartnerID", partnerId);
                    cmd.Parameters.AddWithValue("@PassPercentage", passPercentage);
                    cmd.Parameters.AddWithValue("@AssessmentID", assessmentId);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        success = rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating standalone assessment: " + ex.Message);
                    }
                }
            }

            return success;
        }

        public bool DeleteStandaloneAssessment(int assessmentId)
        {
            bool success = false;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM AssessmentLibrary WHERE AssessmentID = @AssessmentID;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AssessmentID", assessmentId);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        success = rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error deleting standalone assessment: " + ex.Message);
                    }
                }
            }

            return success;
        }


        /// <summary>
        /// Retrieves available assessments for a specific participant, including their last result.
        /// </summary>
        public List<AvailableAssessment> GetAvailableAssessmentsForParticipant(int participantId)
        {
            List<AvailableAssessment> availableAssessments = new List<AvailableAssessment>();

            string sql = @"
                SELECT 
                    A.AssessmentID, 
                    M.ModuleName, 
                    CP.PartnerName, 
                    A.PassPercentage,
                    
                    (SELECT TOP 1 MA.Mark FROM ModuleAssessment MA JOIN Enrollment E2 ON MA.EnrollmentID = E2.EnrollmentID
                     WHERE E2.StakeholderID = @ParticipantID AND MA.AssessmentID = A.AssessmentID ORDER BY MA.AssessmentDate DESC) AS LastScore,

                    (SELECT TOP 1 MA.AssessmentDate FROM ModuleAssessment MA JOIN Enrollment E3 ON MA.EnrollmentID = E3.EnrollmentID
                     WHERE E3.StakeholderID = @ParticipantID AND MA.AssessmentID = A.AssessmentID ORDER BY MA.AssessmentDate DESC) AS LastAttemptDate

                FROM Assessment A
                JOIN Enrollment E ON A.ModuleID = E.ModuleID
                WHERE E.StakeholderID = @ParticipantID
                JOIN Module M ON A.ModuleID = M.ModuleID
                JOIN Stakeholder CP ON A.PartnerID = CP.StakeholderID
                GROUP BY A.AssessmentID, M.ModuleName, CP.PartnerName, A.PassPercentage
                ORDER BY M.ModuleName;";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ParticipantID", participantId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                availableAssessments.Add(new AvailableAssessment
                                {
                                    AssessmentID = reader.GetInt32(reader.GetOrdinal("AssessmentID")),
                                    ModuleName = reader.GetString(reader.GetOrdinal("ModuleName")),
                                    PartnerName = reader.GetString(reader.GetOrdinal("PartnerName")),
                                    PassPercentage = reader.GetDecimal(reader.GetOrdinal("PassPercentage")),
                                    //HasTaken = lastDate.HasValue,
                                    //LastScore = lastScore,
                                    //LastAttemptDate = lastDate
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve available assessments.", ex);
            }
            return availableAssessments;
        }

        internal void ExecuteNonQuery(string insertQuery, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        internal DataTable ExecuteQuery(string query)
        {
            throw new NotImplementedException();
        }
    }

    public class PartnerMaintenanceModel
    {
        public int PartnerID { get; internal set; }
        public string PartnerName { get; internal set; }
        public string EmailAddress { get; internal set; }
        public string ContactNumber { get; internal set; }
    }

    public class ModuleMaintenanceModel
    {
        public int ModuleID { get; internal set; }
        public string ModuleName { get; internal set; }
        public string Description { get; internal set; }
        public string ContentOutline { get; internal set; }
    }
}