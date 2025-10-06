using System;

// ====================================================================
// Application Entry Point (Simulating the Login/Signup UI)
// ====================================================================
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Digital Literacy Program Management (DLPM) ---");

        // Instantiate the DB Manager using the connection string defined in Constants
        // NOTE: Ensure Constants.ConnectionString in Security.cs is correct!
        DBManager manager = new DBManager(Constants.ConnectionString);

        // ----------------------------------------------------
        // DEMO 1: Registration of a new Participant
        // ----------------------------------------------------
        Console.WriteLine("\n\n--- Attempting New Participant Registration ---");

        // Example details for a new user
        string newEmail = "new.user@example.com";
        string newPass = "SecureP@ss123";

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
        Stakeholder? loggedInUser = manager.LoginUser("aadil.admin@dlpm.org.za", "DummyHashedPassword123");

        // After successful login, you would typically check the RoleType to redirect the user
        if (loggedInUser != null)
        {
            Console.WriteLine($"\n[INFO] User logged in as a {loggedInUser.RoleType}. Redirecting to dashboard...");
        }

        // 2. Failed Password
        manager.LoginUser("aadil.admin@dlpm.org.za", "WrongPassword");

        // 3. Failed Email (user not found)
        manager.LoginUser("nonexistent@dlpm.org.za", "DummyHashedPassword123");

        Console.WriteLine("\n--- Program finished. Press any key to exit. ---");
        Console.ReadKey();
    }
}
