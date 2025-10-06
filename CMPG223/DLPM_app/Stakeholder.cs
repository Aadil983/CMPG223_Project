using System;

/// Model class representing a row in the consolidated Stakeholder table.
public class Stakeholder
{
    // Common Fields
    public int StakeholderID { get; set; }
    public string RoleType { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public string? PasswordHash { get; set; } // Stores secure hash
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // Partner-Specific Field
    public string? PartnerName { get; set; }

    // Participant-Specific Fields (Nullable)
    public string? IDNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? EducationLevel { get; set; }
    public string? EmploymentStatus { get; set; }
    public decimal? HouseholdIncome { get; set; }
    public string? DisabilityStatus { get; set; }
}