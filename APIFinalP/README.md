
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
CREATE SCHEMA Hospital;
Go

-- DROP SCHEMA Hospital;
SELECT * FROM [Hospital].[Patient];
/*DROP TABLE Hospital.Patient;*/

CREATE TABLE [Hospital].[Patient] (
    [Patient_Id] INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]  VARCHAR (50)  NOT NULL,
    [LastName]   VARCHAR (50)  NOT NULL,
    [Age]        INT           NOT NULL,
    [Gender]     VARCHAR (10)  NOT NULL,
    [Address]    VARCHAR (225) NOT NULL,
    CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED ([Patient_Id] ASC)
);

INSERT INTO [Hospital].[Patient] ([FirstName], [LastName], [Age], [Gender], [Address])
VALUES 
('John', 'Doe', 35, 'Male', '123 Main St'),
('Jane', 'Smith', 25, 'Female', '456 Elm St'),
('Michael', 'Johnson', 45, 'Male', '789 Oak St'),
('Emily', 'Brown', 30, 'Female', '101 Maple St'),
('David', 'Williams', 40, 'Male', '222 Pine St'),
('Sarah', 'Davis', 28, 'Female', '333 Cedar St'),
('Chris', 'Taylor', 50, 'Male', '444 Walnut St'),
('Doe', 'John', 46, 'Male', '123 Main St, Anytown, USA'),
('Alice', 'Shekur', 26, 'Female', '456 Elm St, Othertown, USA'),
('Alex', 'Denso', 36, 'Male', '789 Oak St, Anycity, USA '),
('Rubin', 'DeSantos', 16, 'Male', '101 Pine St, Othercity, USA'),
('Sara', 'Tomi', 22, 'Female', '202 Maple St, Somewhere, USA '),
('Naima', 'Torres', 22, 'Female', '606 Pineapple St, Anotherplace, USA '),
('Mohammed', 'Arabi', 68, 'Male', '909 Cherry St, Nowhereelse, USA '),
('Anas', 'Arabi', 25, 'Male', '1010 Apple St, Anytown, USA'),
('Lily', 'Ayew', 18, 'Female', '707 Orange St, Yetanotherplace, USA'),
('Ashely', 'David', 17, 'Female', '808 Banana St, Somewherelse, USA ');



CREATE TABLE [Hospital].[Doctor] (
    [Doctor_Id]      INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]      NVARCHAR (50)  NOT NULL,
    [LastName]       NVARCHAR (50)  NOT NULL,
    [Specialization] NVARCHAR (100) NOT NULL,
    [Department_Id]  INT            NOT NULL,
    [Patient_Id]     INT            NOT NULL,
    CONSTRAINT [PK_Doctor] PRIMARY KEY CLUSTERED ([Doctor_Id] ASC),
    CONSTRAINT [FK_Doctor_Department] FOREIGN KEY ([Department_Id]) REFERENCES [Hospital].[Department] ([Department_Id]),
    CONSTRAINT [FK_Doctor_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);

INSERT INTO [Hospital].[Doctor] ([FirstName], [LastName], [Specialization], [Department_Id], [Patient_Id])
VALUES 
    ('Robert', 'Jones', 'Cardiology', 23, 2),
    ('Lisa', 'Wilson', 'Neurology', 3, 15),
    ('Steven', 'Anderson', 'Pediatrics', 1, 11),
    ('Jennifer', 'Martinez', 'Oncology', 4, 13),
    ('Kevin', 'Clark', 'Orthopedics', 5, 7),
    ('Michelle', 'White', 'Dermatology', 6, 3),
    ('Daniel', 'Lopez', 'ENT', 7, 9),
    ('Tasnim', 'Clark', 'Endocrinologists', 8, 2),
    ('Hazwa', 'Jemal', 'Psychiatrists', 6, 3),
    ('Lina', 'Seid', 'Medical', 7, 1);
    
    

DROP TABLE Hospital.Doctor;
DROP TABLE Hospital.Nurse;

CREATE TABLE [Hospital].[Nurse] (
    [Nurse_Id]      INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]     NVARCHAR (50) NOT NULL,
    [LastName]      NVARCHAR (50) NOT NULL,
    [Department_Id] INT           NOT NULL,
    [Patient_Id]    INT           NOT NULL,
    CONSTRAINT [PK_Nurse] PRIMARY KEY CLUSTERED ([Nurse_Id] ASC),
    CONSTRAINT [FK_Nurse_Department] FOREIGN KEY ([Department_Id]) REFERENCES [Hospital].[Department] ([Department_Id]),
    CONSTRAINT [FK_Nurse_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);

SELECT * FROM [Hospital].[Nurse];

INSERT INTO [Hospital].[Nurse] ([FirstName], [LastName], [Department_Id], [Patient_Id])
VALUES 

('Obaama', 'Garcia', 8, 1),
('James', 'Bond', 2, 10),
('Walid', 'Kebir', 6, 13),
('William', 'Saliba', 7, 24),
('Oda', 'Gada', 8, 11),
('Abba', 'Ume', 2, 21),
('Bontu', 'Milki', 13, 23),
('James', 'Brown', 2, 2),
('Linda', 'Miller', 3, 3),
('William', 'Davis', 1, 4),
('Patricia', 'Moore', 2, 5),
('Richard', 'Hernandez', 3, 6),
('Karen', 'Young', 1, 7);

SELECT * FROM [Hospital].[Doctor];
SELECT * FROM[Hospital].[Nurse];
Select Patient_Id FROM Hospital.Patient;
SELECT * FROM [Hospital].[Patient];


CREATE TABLE [Hospital].[Appointment] (
    [Appointment_id]   INT      IDENTITY (1, 1) NOT NULL,
    [Patient_Id]       INT      NOT NULL,
    [Doctor_Id]        INT      NOT NULL,
    [AppointmentDate]  DATE     NOT NULL,
    [RegistrationDate] DATETIME NOT NULL,
    CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED ([Appointment_id] ASC),
    CONSTRAINT [CK_Registration_Appointment] CHECK ([RegistrationDate]<=[AppointmentDate]),
    CONSTRAINT [FK_Appointment_Doctor] FOREIGN KEY ([Doctor_Id]) REFERENCES [Hospital].[Doctor] ([Doctor_Id]),
    CONSTRAINT [FK_Appointment_Pateint] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);
DROP TABLE Hospital.Appointment;
SELECT * FROM [Hospital].[Appointment];

INSERT INTO [Hospital].[Appointment] ([Patient_Id], [Doctor_Id], [AppointmentDate], [RegistrationDate])
VALUES 
DECLARE @CurrentDate DATETIME = GETDATE();


DECLARE @CurrentDate DATETIME;
SET @CurrentDate = GETDATE();
INSERT INTO [Hospital].[Appointment] ([Patient_Id], [Doctor_Id], [AppointmentDate], [RegistrationDate])
VALUES 
(3, 12, DATEADD(DAY, 10, @CurrentDate), @CurrentDate),
(2, 9, DATEADD(DAY, 11, @CurrentDate), @CurrentDate),
(7, 10, DATEADD(DAY, 12, @CurrentDate), @CurrentDate),
(6, 8, DATEADD(DAY, 13, @CurrentDate), @CurrentDate),
(3, 11, DATEADD(DAY, 14, @CurrentDate), @CurrentDate),
(1, 6, DATEADD(DAY, 15, @CurrentDate), @CurrentDate),
(4, 7, DATEADD(DAY, 16, @CurrentDate), @CurrentDate);


ALTER TABLE [Hospital].[Appointment]
DROP CONSTRAINT [CK_Registration_Before_Appointment];

ALTER TABLE [Hospital].[Appointment]
ADD CONSTRAINT [CK_Registration_Before_Appointment]
CHECK (RegistrationDate <= AppointmentDate);

SELECT name, definition
FROM sys.check_constraints
WHERE name = 'CK_Registration_Appointment';


CREATE TABLE [Hospital].[Department] (
    [Department_Id] INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([Department_Id] ASC)
);

DROP TABLE [Hospital].[Department];

INSERT INTO [Hospital].[Department] ([Name])
VALUES 
( 'Cardiology'),
( 'Neurology'),
( 'Pediatrics'),
( 'Oncology'),
( 'Orthopedics'),
( 'Dermatology'),
( 'ENT');

SELECT *FROM [Hospital].[Department];
SELECT *FROM [Hospital].[Medicalrecord];

DROP TABLE [Hospital].[Medicalrecord];

CREATE TABLE [Hospital].[Medicalrecord] (
    [Medicalrecord_Id] INT            IDENTITY (1, 1) NOT NULL,
    [Patient_Id]       INT            NOT NULL,
    [Doctor_Id]        INT            NOT NULL,
    [Diagnosis]        NVARCHAR (350) NOT NULL,
    [Treatment]        NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Medicalrecord] PRIMARY KEY CLUSTERED ([Medicalrecord_Id] ASC),
    CONSTRAINT [FK_Medicalrecord_Doctor] FOREIGN KEY ([Doctor_Id]) REFERENCES [Hospital].[Doctor] ([Doctor_Id]),
    CONSTRAINT [FK_Medicalrecord_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);


INSERT INTO [Hospital].[Medicalrecord] ([Patient_Id], [Doctor_Id], [Diagnosis], [Treatment])
VALUES 
(7, 12, 'Hypertension', 'Prescribed medication and advised dietary changes'),
(2, 7, 'Migraine', 'Prescribed painkillers and advised rest'),
(4, 10, 'Fever', 'Prescribed antibiotics and advised plenty of fluids'),
(1, 9, 'Cancer', 'Referred to oncologist for further treatment'),
(5, 11, 'Fractured leg', 'Prescribed painkillers and advised bed rest'),
(6, 6, 'Eczema', 'Prescribed topical ointment and advised avoiding triggers'),
(3, 8, 'Ear infection', 'Prescribed antibiotics and advised ear drops');

SELECT *FROM [Hospital].[Prescription];
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

INSERT INTO [Hospital].[Prescription] ([Patient_Id], [Doctor_Id], [Medication], [Dosage])
VALUES 
(5, 6, 'Lisinopril', '10 mg once daily'),
(2, 10, 'Ibuprofen', '200 mg as needed for pain'),
(7, 9, 'Amoxicillin', '500 mg three times daily'),
(4, 7, 'Chemotherapy drugs', 'As per oncologist instructions'),
(1, 8, 'Acetaminophen', '500 mg every 4-6 hours as needed'),
(6, 6, 'Hydrocortisone cream', 'Apply thin layer twice daily'),
(3, 7, 'Amoxicillin', '250 mg three times daily');



ALTER TABLE Hospital.Admission
DROP CONSTRAINT CK_Admission_ValidDate;

ALTER TABLE Hospital.Admission
DROP CONSTRAINT CK_Admission_ValidDates;

ALTER TABLE Hospital.Admission
ADD CONSTRAINT CK_Admission_ValidDate CHECK ([DischargeDate] >= [AdmissionDate]);

ALTER TABLE Hospital.Admission
ADD CONSTRAINT CK_Admission_ValidDates CHECK ([AdmissionDate] >= '2020-01-01');


CREATE TABLE [Hospital].[Admission] (
    [Admission_Id]  INT      IDENTITY (1, 1) NOT NULL,
    [Patient_Id]    INT      NOT NULL,
    [AdmissionDate] DATETIME NOT NULL,
    [DischargeDate] DATETIME NOT NULL,
    CONSTRAINT [PK_Admission] PRIMARY KEY CLUSTERED ([Admission_Id] ASC),
    CONSTRAINT [CK_Admission_ValidDate] CHECK ([DischargeDate]>=[AdmissionDate]),
    CONSTRAINT [CK_Admission_ValidDates] CHECK ([AdmissionDate]>='2020-01-01'),
    CONSTRAINT [FK_Admission_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);

INSERT INTO [Hospital].[Admission] ([Patient_Id], [AdmissionDate], [DischargeDate])
VALUES 
(1, '2024-04-30', '2024-05-05'),
(2, '2024-05-01', '2024-05-06'),
(3, '2024-05-02', '2024-05-07'),
(4, '2024-05-03', '2024-05-08'),
(5, '2024-05-04', '2024-05-09'),
(6, '2024-05-05', '2024-05-10'),
(7, '2024-05-06', '2024-05-11');


SELECT * FROM Hospital.Admission;
SELECT * FROM Hospital.Payment;
/*SELECT * FROM Hospital.Patient WHERE Patient_Id IN (1, 2, 3, 4, 5, 6, 7);*/


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

INSERT INTO [Hospital].[Payment] ([Patient_Id], [Amount], [PaymentDate])
VALUES 
(1, 50000.00, GETDATE()),
(2, 17050.00, GETDATE()),
(3, 40000.00, GETDATE()),
(4, 12000.00, GETDATE()),
(5, 10200.00, GETDATE()),
(6, 25300.00, GETDATE()),
(7, 52600.00, GETDATE());


ALTER TABLE [Hospital].[Payment]
DROP CONSTRAINT CK_Payment_Date;

ALTER TABLE [Hospital].[Payment]
ADD CONSTRAINT CK_Payment_Date CHECK (PaymentDate >= '2000-01-01');

ALTER TABLE Hospital.Payment
DROP CONSTRAINT FK_Payment_Patient;

ALTER TABLE Hospital.Payment
ADD CONSTRAINT FK_Payment_Patient
FOREIGN KEY (Patient_Id)
REFERENCES Hospital.Patient(Patient_Id)
ON UPDATE CASCADE;


/*DROP TABLE Hospital.Payment;*/
DROP TABLE Hospital.Registration;

ALTER TABLE [Hospital].[Registration]
ADD [FirstName] NVARCHAR(50) NULL,
    [LastName] NVARCHAR(50) NULL;


ALTER TABLE [Hospital].[Registration]
ALTER COLUMN [FirstName] NVARCHAR(50) NOT NULL;

ALTER TABLE [Hospital].[Registration]
ALTER COLUMN [LastName] NVARCHAR(50) NOT NULL;

UPDATE Hospital.Registration
SET FirstName = NULL, LastName = NULL
WHERE FirstName = 'DefaultFirstName' AND LastName = 'DefaultLastName';

ALTER TABLE Hospital.Registration
ALTER COLUMN LastName VARCHAR(255) NULL
ALTER COLUMN FirstName VARCHAR(255) NULL

UPDATE Hospital.Registration
SET FirstName = 'NewFirstName', LastName = 'NewLastName'
WHERE FirstName IS NULL AND LastName IS NULL;


SELECT * FROM Hospital.Registration;
CREATE TABLE [Hospital].[Registration] (
    [Registration_Id]  INT      IDENTITY (1, 1) NOT NULL,
    [Patient_Id]       INT      NOT NULL,
    [RegistrationDate] DATETIME NOT NULL,
    CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED ([Registration_Id] ASC),
    CONSTRAINT [CK_Registration_ValisDate] CHECK (RegistrationDate>=GetDate()),
    CONSTRAINT [FK_Registration_Patient] FOREIGN KEY ([Patient_Id]) REFERENCES [Hospital].[Patient] ([Patient_Id])
);

INSERT INTO [Hospital].[Registration] ([Patient_Id], [FirstName], [LastName], [RegistrationDate])
VALUES 
(9, 'John', 'Dave', DATEADD(day, 1, GETDATE())),
(12, 'Jane', 'Smith', DATEADD(day, 2, GETDATE())),
(21, 'Michael', 'Johnson', DATEADD(day, 1, GETDATE())),
(10, 'Emily', 'Davis', DATEADD(day, 2, GETDATE())),
(4, 'James', 'Brown', DATEADD(day, 1, GETDATE())),
(8, 'Patricia', 'Taylor', DATEADD(day, 2, GETDATE())),
(17, 'Linda', 'Wilson', DATEADD(day, 1, GETDATE())),
(19, 'Robert', 'Anderson', DATEADD(day, 2, GETDATE())),
(13, 'Barbara', 'Thomas', DATEADD(day, 3, GETDATE()));

DELETE Hospital.Registration
SET FirstName = CASE 
    WHEN Patient_Id = 1 THEN 'Alex'
    WHEN Patient_Id = 12 THEN 'NewFirstName'
    WHEN Patient_Id = 21 THEN 'NewFirstName'
    WHEN Patient_Id =  THEN 'William'
    WHEN Patient_Id = 10 THEN 'NewFirstName'
    WHEN Patient_Id = 17 THEN 'NewFirstName'
    WHEN Patient_Id = 19 THEN 'NewFirstName'
    WHEN Patient_Id = 8 THEN 'Ashley'
    -- Add cases for each Patient_Id
    WHEN Patient_Id = 9 THEN 'Tasnim'
    ELSE FirstName
END
LastName = CASE 
    WHEN Patient_Id = 1 THEN 'Ramos'
    WHEN Patient_Id = 12 THEN 'NewLastName'
    WHEN Patient_Id = 21 THEN 'NewLastName'
    --WHEN Patient_Id = 4 THEN 'Saliba'
    WHEN Patient_Id = 10 THEN 'NewLastName'
    WHEN Patient_Id = 17 THEN 'NewLastName'
    WHEN Patient_Id = 19 THEN 'NewLastName'
    WHEN Patient_Id = 8 THEN 'Andrew'
    -- Add cases for each Patient_Id
    WHEN Patient_Id = 9 THEN 'Suleman'
    ELSE LastName
END,
WHERE Patient_Id BETWEEN 1 AND 9;



SELECT [Treatment], [Diagnosis], [Patient_Id]
FROM [Hospital].[Medicalrecord];


``````





