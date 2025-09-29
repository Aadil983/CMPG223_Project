# CMPG223_Project
Digital Literacy Program

Use following SQL's to create the database in Microsoft SQL Server Management:

CREATE DATABASE DLPM_DB;
GO

USE DLPM_DB;
GO

CREATE TABLE Participant (
    ParticipantID INT PRIMARY KEY IDENTITY(1,1),
    IDNumber VARCHAR(15) NOT NULL UNIQUE,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DateOfBirth DATE,
    Gender VARCHAR(10),
    ContactNumber VARCHAR(20),
    EmailAddress VARCHAR(50) UNIQUE
);

CREATE TABLE Partner (
    PartnerID INT PRIMARY KEY IDENTITY(1,1),
    PartnerName VARCHAR(100) NOT NULL,
    ContactPerson VARCHAR(100),
    ContactNumber VARCHAR(20),
    EmailAddress VARCHAR(50),
    PartnershipStartDate DATE
);

CREATE TABLE Module (
    ModuleID INT PRIMARY KEY IDENTITY(1,1),
    ModuleName VARCHAR(100) NOT NULL,
    Description TEXT,
    ContentOutline TEXT
);

CREATE TABLE Enrollment (
    EnrollmentID INT PRIMARY KEY IDENTITY(1,1),
    ParticipantID INT NOT NULL,
    ModuleID INT NOT NULL,
    EnrollmentDate DATE NOT NULL,
    StartDate DATE,
    EndDate DATE,
    FOREIGN KEY (ParticipantID) REFERENCES Participant(ParticipantID),
    FOREIGN KEY (ModuleID) REFERENCES Module(ModuleID)
);

CREATE TABLE ModuleProgress (
    ProgressID INT PRIMARY KEY IDENTITY(1,1),
    EnrollmentID INT NOT NULL,
    ProgressPercentage INT,
    CompletionStatus VARCHAR(20),
    FOREIGN KEY (EnrollmentID) REFERENCES Enrollment(EnrollmentID)
);

CREATE TABLE ModuleAssessment (
    AssessmentID INT PRIMARY KEY IDENTITY(1,1),
    EnrollmentID INT NOT NULL,
    AssessmentDate DATE NOT NULL,
    Mark DECIMAL(5,2),
    PassStatus VARCHAR(10),
    FOREIGN KEY (EnrollmentID) REFERENCES Enrollment(EnrollmentID)
);

CREATE TABLE ModulePartner (
    ModulePartnerID INT PRIMARY KEY IDENTITY(1,1),
    ModuleID INT NOT NULL,
    PartnerID INT NOT NULL,
    ContributionDescription TEXT,
    FOREIGN KEY (ModuleID) REFERENCES Module(ModuleID),
    FOREIGN KEY (PartnerID) REFERENCES Partner(PartnerID)
);
GO
