-- =========================================================================
-- DLPM Database Setup and Seeding Script (Optimized for LocalDB/AttachDB)
-- Target Environment: Microsoft SQL Server LocalDB
-- ASSUMES the DLPM_DB context is already established by Visual Studio/C# connection.
-- =========================================================================
SET NOCOUNT ON; -- Prevents row count messages from cluttering the output

-- 1. DATABASE CONTEXT AND CLEANUP
-- Ensure we are using the correct database context.
USE DLPM;
GO
-- The remaining script will run in one batch after the USE statement is executed.

-- If running this script multiple times, these drops ensure a clean restart
-- Dropping dependent tables first, then core tables, to respect foreign key constraints.
IF OBJECT_ID('ModuleAssessment', 'U') IS NOT NULL DROP TABLE ModuleAssessment;
IF OBJECT_ID('ModuleProgress', 'U') IS NOT NULL DROP TABLE ModuleProgress;
IF OBJECT_ID('Enrollment', 'U') IS NOT NULL DROP TABLE Enrollment;
IF OBJECT_ID('ModulePartner', 'U') IS NOT NULL DROP TABLE ModulePartner;
-- Drop the index before dropping the table to prevent potential errors
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_IDNumber_NonNULL' AND object_id = OBJECT_ID('Stakeholder'))
    DROP INDEX UQ_IDNumber_NonNULL ON Stakeholder;
IF OBJECT_ID('Stakeholder', 'U') IS NOT NULL DROP TABLE Stakeholder; -- The new consolidated table
IF OBJECT_ID('Module', 'U') IS NOT NULL DROP TABLE Module;


-- 2. CREATE CORE TABLES

-- Create the Consolidated Stakeholder Table (replaces Participant & Partner)
CREATE TABLE Stakeholder (
    StakeholderID INT PRIMARY KEY IDENTITY(1,1),

    -- Discriminator: Identifies the role ('Admin', 'Participant', 'Instructor', 'Partner')
    RoleType VARCHAR(20) NOT NULL,

    -- Common Fields for all roles
    EmailAddress VARCHAR(50) NOT NULL UNIQUE, -- Used as unique login/contact identifier
    ContactNumber VARCHAR(20) NULL,
    PasswordHash VARCHAR(100) NULL, -- Stores secure hash of user password
    
    -- General User/Instructor/Admin Fields
    FirstName VARCHAR(50) NULL,
    LastName VARCHAR(50) NULL,

    -- Partner-Specific Fields
    PartnerName VARCHAR(100) NULL, -- Only used if RoleType = 'Partner'

    -- Participant-Specific Fields
    IDNumber VARCHAR(15) NULL, 
    DateOfBirth DATE NULL,
    Gender VARCHAR(10) NULL,
    Address VARCHAR(200) NULL,
    EducationLevel VARCHAR(50) NULL,
    EmploymentStatus VARCHAR(50) NULL,
    HouseholdIncome DECIMAL(10,2) NULL,
    DisabilityStatus VARCHAR(50) NULL
);

-- Add a UNIQUE constraint on IDNumber ONLY where it is not NULL (for Participants)
CREATE UNIQUE NONCLUSTERED INDEX UQ_IDNumber_NonNULL
ON Stakeholder (IDNumber)
WHERE IDNumber IS NOT NULL;


-- Create the Module Table
CREATE TABLE Module (
    ModuleID INT PRIMARY KEY IDENTITY(1,1),
    ModuleName VARCHAR(100) NOT NULL,
    Description TEXT,
    ContentOutline TEXT
);


-- 3. CREATE DEPENDENT TABLES (using StakeholderID for FKs)

-- Links Modules to the new 'Partner' Stakeholders
CREATE TABLE ModulePartner (
    ModulePartnerID INT PRIMARY KEY IDENTITY(1,1),
    ModuleID INT NOT NULL,
    PartnerStakeholderID INT NOT NULL, -- References StakeholderID where RoleType is 'Partner'
    ContributionDescription TEXT,
    FOREIGN KEY (ModuleID) REFERENCES Module(ModuleID),
    FOREIGN KEY (PartnerStakeholderID) REFERENCES Stakeholder(StakeholderID)
);

-- Links Modules to the new 'Participant' Stakeholders
CREATE TABLE Enrollment (
    EnrollmentID INT PRIMARY KEY IDENTITY(1,1),
    ParticipantStakeholderID INT NOT NULL, -- References StakeholderID where RoleType is 'Participant'
    ModuleID INT NOT NULL,
    EnrollmentDate DATE NOT NULL,
    StartDate DATE,
    EndDate DATE,
    FOREIGN KEY (ParticipantStakeholderID) REFERENCES Stakeholder(StakeholderID),
    FOREIGN KEY (ModuleID) REFERENCES Module(ModuleID)
);

-- Tracks progress on an enrollment
CREATE TABLE ModuleProgress (
    ProgressID INT PRIMARY KEY IDENTITY(1,1),
    EnrollmentID INT NOT NULL,
    ProgressPercentage INT,
    CompletionStatus VARCHAR(20),
    FOREIGN KEY (EnrollmentID) REFERENCES Enrollment(EnrollmentID)
);

-- Stores assessment results for an enrollment
CREATE TABLE ModuleAssessment (
    AssessmentID INT PRIMARY KEY IDENTITY(1,1),
    EnrollmentID INT NOT NULL,
    AssessmentDate DATE NOT NULL,
    Mark DECIMAL(5,2),
    PassStatus VARCHAR(10),
    FOREIGN KEY (EnrollmentID) REFERENCES Enrollment(EnrollmentID)
);


-- =========================================================================
-- 4. INITIAL DATA POPULATION (SEeding)
-- Uses a transaction block to ensure all inserts complete successfully or none do.
-- =========================================================================

BEGIN TRANSACTION
BEGIN TRY

    -- Declare variables to hold IDs and Hash
    DECLARE @DefaultPasswordHash VARCHAR(100) = 'DummyHashedPassword123';
    DECLARE @PartnerKCC INT, @PartnerDSA INT;
    DECLARE @P_Thabo INT, @P_Zanele INT, @P_Sipho INT;
    DECLARE @M_Basic INT, @M_Internet INT, @M_Social INT;
    DECLARE @E_Thabo_M1 INT, @E_Zanele_M1 INT, @E_Sipho_M2 INT;


    -- 4.1 Populate Stakeholder Table and capture generated IDs

    INSERT INTO Stakeholder (RoleType, FirstName, LastName, PartnerName, EmailAddress, PasswordHash, IDNumber, DateOfBirth, Gender, ContactNumber)
    VALUES
    -- ADMINS
    ('Admin', 'Aadil', 'Ismail', NULL, 'aadil.admin@dlpm.org.za', @DefaultPasswordHash, NULL, NULL, 'Male', '0711001001'),
    ('Admin', 'Katleho', 'Mofokeng', NULL, 'katleho.admin@dlpm.org.za', @DefaultPasswordHash, NULL, NULL, 'Male', '0711001002'),
    -- INSTRUCTOR
    ('Instructor', 'Jabulani', 'Ndebele', NULL, 'jabu.train@dlpm.org.za', @DefaultPasswordHash, NULL, NULL, 'Male', '0845551212');

    -- PARTNERS (Inserting separately so we can capture their IDs immediately after insertion)
    INSERT INTO Stakeholder (RoleType, FirstName, LastName, PartnerName, EmailAddress, PasswordHash, IDNumber, DateOfBirth, Gender, ContactNumber)
    VALUES ('Partner', NULL, NULL, 'Khayelitsha Community Centre', 'lindiwe@kcc.org.za', NULL, NULL, NULL, NULL, '0215551234');
    SET @PartnerKCC = SCOPE_IDENTITY(); -- Capture the ID of KCC

    INSERT INTO Stakeholder (RoleType, FirstName, LastName, PartnerName, EmailAddress, PasswordHash, IDNumber, DateOfBirth, Gender, ContactNumber)
    VALUES ('Partner', NULL, NULL, 'Digital Skills SA', 'peter.vw@digitalskills.co.za', NULL, NULL, NULL, NULL, '0119998877');
    SET @PartnerDSA = SCOPE_IDENTITY(); -- Capture the ID of Digital Skills SA

    -- PARTICIPANTS (Inserting separately to capture IDs)
    INSERT INTO Stakeholder (RoleType, FirstName, LastName, PartnerName, EmailAddress, PasswordHash, IDNumber, DateOfBirth, Gender, ContactNumber)
    VALUES ('Participant', 'Thabo', 'Mokoena', NULL, 'thabo.mokoena@email.com', @DefaultPasswordHash, '8501015000085', '1985-01-01', 'Male', '0812345678');
    SET @P_Thabo = SCOPE_IDENTITY();

    INSERT INTO Stakeholder (RoleType, FirstName, LastName, PartnerName, EmailAddress, PasswordHash, IDNumber, DateOfBirth, Gender, ContactNumber)
    VALUES ('Participant', 'Zanele', 'Dlamini', NULL, 'zaneled@email.com', @DefaultPasswordHash, '9005126000089', '1990-05-12', 'Female', '0729876543');
    SET @P_Zanele = SCOPE_IDENTITY();

    INSERT INTO Stakeholder (RoleType, FirstName, LastName, PartnerName, EmailAddress, PasswordHash, IDNumber, DateOfBirth, Gender, ContactNumber)
    VALUES ('Participant', 'Sipho', 'Khumalo', NULL, 'sipho.k@email.com', @DefaultPasswordHash, '9503257000087', '1995-03-25', 'Male', '0601112233');
    SET @P_Sipho = SCOPE_IDENTITY();

    -- 4.2 Populate Module Table and capture generated IDs

    INSERT INTO Module (ModuleName, Description, ContentOutline)
    VALUES
    ('Basic Computer Skills', 'A foundational course for new computer users covering hardware, software, and basic productivity.', 'Module 1: Intro to PCs, Module 2: OS Navigation, Module 3: Basic Software');
    SET @M_Basic = SCOPE_IDENTITY();

    INSERT INTO Module (ModuleName, Description, ContentOutline)
    VALUES
    ('Internet and Email Essentials', 'Covers safe and effective use of the internet, cloud services, and professional email communication.', 'Module 1: Web Browsing, Module 2: Email Management, Module 3: Online Safety');
    SET @M_Internet = SCOPE_IDENTITY();

    INSERT INTO Module (ModuleName, Description, ContentOutline)
    VALUES
    ('Social Media for Small Business', 'Learn how to leverage key social media platforms for business growth and marketing.', 'Module 1: Platforms, Module 2: Content Strategy, Module 3: Ads and Analytics');
    SET @M_Social = SCOPE_IDENTITY();


    -- 4.3 Populate ModulePartner (Linking Modules to Partners)

    INSERT INTO ModulePartner (ModuleID, PartnerStakeholderID, ContributionDescription)
    VALUES
    (@M_Basic, @PartnerKCC, 'Provided training venue and computer lab access.'),
    (@M_Internet, @PartnerKCC, 'Facilitated a guest lecture on online safety and digital citizenship.'),
    (@M_Social, @PartnerDSA, 'Co-developed the Social Media course content and provided specialist trainers.');


    -- 4.4 Populate Enrollment (Linking Participants to Modules)

    INSERT INTO Enrollment (ParticipantStakeholderID, ModuleID, EnrollmentDate, StartDate, EndDate)
    VALUES
    (@P_Thabo, @M_Basic, '2024-01-10', '2024-01-15', '2024-03-15');
    SET @E_Thabo_M1 = SCOPE_IDENTITY();

    INSERT INTO Enrollment (ParticipantStakeholderID, ModuleID, EnrollmentDate, StartDate, EndDate)
    VALUES
    (@P_Zanele, @M_Basic, '2024-01-10', '2024-01-15', '2024-03-15');
    SET @E_Zanele_M1 = SCOPE_IDENTITY();

    INSERT INTO Enrollment (ParticipantStakeholderID, ModuleID, EnrollmentDate, StartDate, EndDate)
    VALUES
    (@P_Sipho, @M_Internet, '2024-02-05', '2024-02-10', '2024-04-10');
    SET @E_Sipho_M2 = SCOPE_IDENTITY();


    -- 4.5 Populate ModuleProgress (Simulated Data)

    INSERT INTO ModuleProgress (EnrollmentID, ProgressPercentage, CompletionStatus)
    VALUES
    (@E_Thabo_M1, 100, 'Completed'),
    (@E_Zanele_M1, 75, 'In Progress'),
    (@E_Sipho_M2, 100, 'Completed');

    -- 4.6 Populate ModuleAssessment (Simulated Data)

    INSERT INTO ModuleAssessment (EnrollmentID, AssessmentDate, Mark, PassStatus)
    VALUES
    (@E_Thabo_M1, '2024-03-10', 85.50, 'Pass'),
    (@E_Sipho_M2, '2024-04-05', 92.00, 'Pass');

    -- If everything succeeded, commit the transaction
    COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    -- If any error occurred during data insert, rollback the transaction
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    -- Re-throw the error for the user to see
    THROW;
END CATCH
GO
