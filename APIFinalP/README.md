
# APIFinalP
## Project Name - Hospital Management System

### Features Added/Updated Since Last Update

### Date: May 20, 2024

```csharp
 1. Database Schema Creation

 Created a Hospital schema: 
   -Based on the project, I tryed to establish the foundational structure 
   for the hospital management system.
 I created multiple tables under Hospital schema like:-
        - Patient, 
        - Doctor, 
        - Nurse, 
        - Department, 
        - Appointment, 
        - Medicalrecord, 
        - Prescription, 
        - Tast, 
        - Ward, 
        - Admission, 
        - Payment, 
        - Registration, and 
        - DoctorFeedback.
 
````
## Table and Data Management:

```csharp
    By adding a relevant records to ensure that the tables have realistic 
    and comprehensive data sets for testing purposes and established 
    primary keys, foreign keys, and other constraints to ensure data integrity 
    and enforce relationships between tables. 
    And also I added constraints to make sure necessary fields contain data (e.g., names, IDs).
````
  
## Issues Encountered

````Csharp
## A. Foreign Key Constraints Conflicts
   * Encountered issues with foreign key dependencies: *
   - While creating tables, there were conflicts due to foreign key dependencies. 
     For example, the Doctor table references the Department and Patient tables, 
     so these referenced tables must be created first.
   
  
## B. Resolution: 
    - By carefully creating a sequence tables the Department and Patient tables 
      were created before the Doctor table to ensure that all foreign key references were valid.

## C.Constraint Validation Errors:
    * Date Validation Errors:
      At the time I create an Apointment table and try to wire with foreign key I got Constraint error
      like CHECK (AppointmentDate <= GETDATE())  before make sure dates are not in the future.
      Errors occurred when sample data did not meet these criteria.
    * Resolution:
      I adjusted the sample data to comply with these constraints.
      For instance, changed future dates to valid past or present dates.

## D. Dropping and Recreating Tables
      
    * I got a lot of challenges when I want to change the data by dropping and recreating tables
      posed, especially with tables having foreign key relationships. Dropping a table without handling 
      dependent tables first could lead to orphaned records and constraint violations.
    * Resolution: 
        By managing the drop and recreate operations carefully by first removing foreign key 
        constraints, then dropping the dependent tables before the main table, and 
        recreating them in the correct order.
``````

### Additional Context on Issues

 I got errors with data consistency across related tables that were challenging me. 
 Before I checked the Doctor table,  I made data that every data of the Doctor table entry 
 has valid references in the Department and Patient tables. However, 
 I Implemented a comprehensive validation and testing to ensure that 
 all foreign key relationships are correctly maintained. 
 This included writing SQL scripts to check for orphaned records and integrity violations.

 
 Go
## CREATE SCHEMA Hospital;
Go

-- DROP SCHEMA Hospital;

/*DROP TABLE Hospital.Patient;*/
````csharp

CREATE TABLE [Hospital].[Patient] (
    [Patient_Id] INT           IDENTITY (1, 1) NOT NULL,
    [First Name] VARCHAR (50)  NOT NULL,
    [Last Name]  VARCHAR (50)  NULL,
    [Age]        INT           NOT NULL,
    [Gender]     VARCHAR (10)  NOT NULL,
    [Address]    VARCHAR (225) NOT NULL,
    CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED ([Patient_Id] ASC)
);

CREATE TABLE [Hospital].[Doctor] (
    [Doctor_Id]      INT            IDENTITY (1, 1) NOT NULL,
    [First Name]     NVARCHAR (50)  NOT NULL,
    [Last Name]      NVARCHAR (50)  NOT NULL,
    [Specialization] NVARCHAR (100) NOT NULL,
    [Department_Id]  INT            NOT NULL,
    [Patient_Id]     INT            NOT NULL,
    CONSTRAINT [PK_Doctor] PRIMARY KEY CLUSTERED ([Doctor_Id] ASC),
    CONSTRAINT [FK_Doctor_Department] FOREIGN KEY ([Department_Id]) REFERENCES [Hospital].[Department] ([Department_Id]),
    CONSTRAINT [FK_Doctor_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);

DROP TABLE Hospital.Doctor;
DROP TABLE Hospital.Nurse;

CREATE TABLE [Hospital].[Nurse] (
    [Nurse_Id]      INT           IDENTITY (1, 1) NOT NULL,
    [First Name]    NVARCHAR (50) NOT NULL,
    [Last Name]     NVARCHAR (50) NOT NULL,
    [Department_Id] INT           NOT NULL,
    [Patient_Id]    INT           NOT NULL,
    CONSTRAINT [PK_Nurse] PRIMARY KEY CLUSTERED ([Nurse_Id] ASC),
    CONSTRAINT [FK_Nurse_Department] FOREIGN KEY ([Department_Id]) REFERENCES [Hospital].[Department] ([Department_Id]),
    CONSTRAINT [FK_Nurse_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);

SELECT * FROM [Hospital].[Patient];


SELECT [Doctor_Id] FROM [Hospital].[Doctor];
SELECT[Nurse_Id] FROM[Hospital].[Nurse];
Select Patient_Id FROM Hospital.Patient;
SELECT * FROM [Hospital].[Patient];

CREATE TABLE [Hospital].[Appointment] (
    [Appointment_id]   INT      IDENTITY (1, 1) NOT NULL,
    [Patient_Id]       INT      NOT NULL,
    [Doctor_Id]        INT      NOT NULL,
    [Nurse_Id]         INT      NOT NULL,
    [AppointmentDate]  DATE     NOT NULL,
    [RegistrationDate] DATETIME NOT NULL,
    CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED ([Appointment_id] ASC),
    CONSTRAINT [CK_Appointment_DateNotinFuture] CHECK (AppointmentDate<=GetDate()),
    CONSTRAINT [FK_Appointment_Doctor] FOREIGN KEY ([Doctor_Id]) REFERENCES [Hospital].[Doctor] ([Doctor_Id]),
    CONSTRAINT [FK_Appointment_Nurse] FOREIGN KEY ([Nurse_Id]) REFERENCES [Hospital].[Nurse] ([Nurse_Id]),
    CONSTRAINT [FK_Appointment_Pateint] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);
DROP TABLE Hospital.Appointment;


CREATE TABLE [Hospital].[Department] (
    [Department_Id] INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([Department_Id] ASC)
);

DROP TABLE [Hospital].[Department];


DROP TABLE [Hospital].[Medicalrecord];

CREATE TABLE [Hospital].[Medicalrecord] (
    [Record_Id]  INT            IDENTITY (1, 1) NOT NULL,
    [Patient_Id] INT            NOT NULL,
    [Doctor-Id]  INT            NOT NULL,
    [Diagnosis]  NVARCHAR (350) NOT NULL,
    [Treatment]  NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Medicalrecord] PRIMARY KEY CLUSTERED ([Record_Id] ASC),
    CONSTRAINT [FK_Medicalrecord_Doctor] FOREIGN KEY ([Doctor-Id]) REFERENCES [Hospital].[Doctor] ([Doctor_Id]),
    CONSTRAINT [FK_Medicalrecord_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);

CREATE TABLE [Hospital].[Prescription] (
    [Prescription_Id] INT            IDENTITY (1, 1) NOT NULL,
    [Patient_Id]      INT            NOT NULL,
    [Doctor_Id]       INT            NOT NULL,
    [Medication]      NVARCHAR (100) NOT NULL,
    [Dosage]          NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_Prescription] PRIMARY KEY CLUSTERED ([Prescription_Id] ASC),
    CONSTRAINT [FK_Prescription_Doctor] FOREIGN KEY ([Doctor_Id]) REFERENCES [Hospital].[Doctor] ([Doctor_Id]),
    CONSTRAINT [FK_Prescription_Peteint] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);
DROP TABLE Hospital.Prescription;


CREATE TABLE [Hospital].[Tast] (
    [Test_Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Patient_Id] INT            NOT NULL,
    [Test Name]  NVARCHAR (255) NOT NULL,
    [Test Date]  DATETIME       NOT NULL,
    [ResultDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_Tast] PRIMARY KEY CLUSTERED ([Test_Id] ASC),
    CONSTRAINT [CK_Tast_DateNotinFuture] CHECK (ResultDate <=GetDate()),
    CONSTRAINT [FK_Tast_Patein] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);

CREATE TABLE [Hospital].[Ward] (
    [Ward_Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Ward Number] INT           NOT NULL,
    [Ward Type]   NVARCHAR (50) NOT NULL,
    [Capacity]    INT           NOT NULL,
    CONSTRAINT [PK_Ward] PRIMARY KEY CLUSTERED ([Ward_Id] ASC)
);

/*DROP TABLE Hospital.Ward;*/

CREATE TABLE [Hospital].[Admission] (
    [Admission_Id]  INT      IDENTITY (1, 1) NOT NULL,
    [Patient_Id]    INT      NOT NULL,
    [Ward_Id]       INT      NOT NULL,
    [AdmissionDate] DATETIME NOT NULL,
    [DischargeDate] DATETIME NOT NULL,
    CONSTRAINT [PK_Admission] PRIMARY KEY CLUSTERED ([Admission_Id] ASC),
    CONSTRAINT [CK_Admission_ValidDate] CHECK (DischargeDate<=GETDATE()),
    CONSTRAINT [CK_Admission_ValidDates] CHECK (AdmissionDate<=GetDate()),
    CONSTRAINT [FK_Admission_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id]),
    CONSTRAINT [FK_Admission_Ward] FOREIGN KEY ([Ward_Id]) REFERENCES [Hospital].[Ward] ([Ward_Id])
);

SELECT Ward_Id FROM Hospital.Ward;

CREATE TABLE [Hospital].[Payment] (
    [Payment_Id]  INT          IDENTITY (1, 1) NOT NULL,
    [Patient_Id]  INT          NOT NULL,
    [Amount]      DECIMAL (18) NOT NULL,
    [PaymentDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED ([Payment_Id] ASC),
    CONSTRAINT [CK_Payment_Amount] CHECK (Amount > 0),
    CONSTRAINT [CK_Payment_Date] CHECK (PaymentDate>=GetDate()),
    CONSTRAINT [FK_Payment_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);

/*DROP TABLE Hospital.Payment;*/

CREATE TABLE [Hospital].[Registration] (
    [Registration_Id]  INT      IDENTITY (1, 1) NOT NULL,
    [Patient_Id]       INT      NOT NULL,
    [RegistrationDate] DATETIME NOT NULL,
    CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED ([Registration_Id] ASC),
    CONSTRAINT [CK_Registration_ValisDate] CHECK (RegistrationDate>=GetDate()),
    CONSTRAINT [FK_Registration_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);


CREATE TABLE [Hospital].[DoctorFeedback] (
    [Feedback_Id]  INT            IDENTITY (1, 1) NOT NULL,
    [Doctor_Id]    INT            NOT NULL,
    [Patient_Id]   INT            NOT NULL,
    [FeedbackText] NVARCHAR (255) NOT NULL,
    [FeedbackDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_DoctorFeedback] PRIMARY KEY CLUSTERED ([Feedback_Id] ASC),
    CONSTRAINT [CK_DoctorFeedback_DateValid] CHECK (FeedbackDate <= GETDATE()),
    CONSTRAINT [CK_DoctorFeedback_TextNotEmpty] CHECK (LEN([FeedbackText]) > 0),
    CONSTRAINT [FK_DoctorFeedback_Doctor] FOREIGN KEY ([Doctor_Id]) REFERENCES [Hospital].[Doctor] ([Doctor_Id]),
    CONSTRAINT [FK_DoctorFeedback_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);
DROP TABLE Hospital.DoctorFeedback;

SELECT [Treatment], [Diagnosis], [Patient_Id]
FROM [Hospital].[Medicalrecord];


