using System;
using System.Data.SqlClient;

/// <summary>
/// Manages all database interactions for login and registration.
/// </summary>
public class DBManager
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
    public Stakeholder? LoginUser(string email, string password)
    {
        Stakeholder? user = null;
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