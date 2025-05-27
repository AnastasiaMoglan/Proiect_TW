-- medical_consultations.sql
-- Create MedicalConsultation table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MedicalConsultation')
BEGIN
CREATE TABLE [dbo].[MedicalConsultation](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [FullName] [nvarchar](100) NOT NULL,
    [Email] [nvarchar](100) NOT NULL,
    [PhoneNumber] [nvarchar](20) NOT NULL,
    [PreferredDate] [datetime] NOT NULL,
    [ConsultationType] [nvarchar](50) NOT NULL,
    [AdditionalNotes] [nvarchar](500) NULL,
    [Status] [nvarchar](50) NOT NULL DEFAULT 'Pending',
    [UserId] [int] NULL,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] [datetime] NULL,
    CONSTRAINT [PK_MedicalConsultation] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
END
GO

-- Create indexes for faster lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_MedicalConsultation_UserId' AND object_id = OBJECT_ID('dbo.MedicalConsultation'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_MedicalConsultation_UserId] ON [dbo].[MedicalConsultation]([UserId] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_MedicalConsultation_Status' AND object_id = OBJECT_ID('dbo.MedicalConsultation'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_MedicalConsultation_Status] ON [dbo].[MedicalConsultation]([Status] ASC)
END
GO

-- Create stored procedures for common operations

-- Get all consultations
IF OBJECT_ID('sp_GetAllConsultations', 'P') IS NOT NULL
DROP PROCEDURE sp_GetAllConsultations
    GO

CREATE PROCEDURE sp_GetAllConsultations
    AS
BEGIN
SELECT * FROM [dbo].[MedicalConsultation]
WHERE [IsActive] = 1
ORDER BY [CreatedAt] DESC
END
GO

-- Get consultation by ID
IF OBJECT_ID('sp_GetConsultationById', 'P') IS NOT NULL
DROP PROCEDURE sp_GetConsultationById
    GO

CREATE PROCEDURE sp_GetConsultationById
    @Id INT
AS
BEGIN
SELECT * FROM [dbo].[MedicalConsultation]
WHERE [Id] = @Id AND [IsActive] = 1
END
GO

-- Get consultations by user ID
IF OBJECT_ID('sp_GetConsultationsByUserId', 'P') IS NOT NULL
DROP PROCEDURE sp_GetConsultationsByUserId
    GO

CREATE PROCEDURE sp_GetConsultationsByUserId
    @UserId INT
AS
BEGIN
SELECT * FROM [dbo].[MedicalConsultation]
WHERE [UserId] = @UserId AND [IsActive] = 1
ORDER BY [CreatedAt] DESC
END
GO

-- Get consultations by status
IF OBJECT_ID('sp_GetConsultationsByStatus', 'P') IS NOT NULL
DROP PROCEDURE sp_GetConsultationsByStatus
    GO

CREATE PROCEDURE sp_GetConsultationsByStatus
    @Status NVARCHAR(50)
AS
BEGIN
SELECT * FROM [dbo].[MedicalConsultation]
WHERE [Status] = @Status AND [IsActive] = 1
ORDER BY [PreferredDate] ASC
END
GO

-- Create new consultation
IF OBJECT_ID('sp_CreateConsultation', 'P') IS NOT NULL
DROP PROCEDURE sp_CreateConsultation
    GO

CREATE PROCEDURE sp_CreateConsultation
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(20),
    @PreferredDate DATETIME,
    @ConsultationType NVARCHAR(50),
    @AdditionalNotes NVARCHAR(500) = NULL,
    @UserId INT = NULL
AS
BEGIN
INSERT INTO [dbo].[MedicalConsultation] (
    [FullName],
    [Email],
    [PhoneNumber],
    [PreferredDate],
    [ConsultationType],
    [AdditionalNotes],
    [Status],
    [UserId],
[CreatedAt]
)
VALUES (
    @FullName,
    @Email,
    @PhoneNumber,
    @PreferredDate,
    @ConsultationType,
    @AdditionalNotes,
    'Pending',
    @UserId,
    GETDATE()
    )

SELECT SCOPE_IDENTITY() AS NewConsultationId
END
GO

-- Update consultation
IF OBJECT_ID('sp_UpdateConsultation', 'P') IS NOT NULL
DROP PROCEDURE sp_UpdateConsultation
    GO

CREATE PROCEDURE sp_UpdateConsultation
    @Id INT,
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(20),
    @PreferredDate DATETIME,
    @ConsultationType NVARCHAR(50),
    @AdditionalNotes NVARCHAR(500) = NULL,
    @Status NVARCHAR(50)
AS
BEGIN
UPDATE [dbo].[MedicalConsultation]
SET
    [FullName] = @FullName,
    [Email] = @Email,
    [PhoneNumber] = @PhoneNumber,
    [PreferredDate] = @PreferredDate,
    [ConsultationType] = @ConsultationType,
    [AdditionalNotes] = @AdditionalNotes,
    [Status] = @Status,
    [UpdatedAt] = GETDATE()
WHERE [Id] = @Id
END
GO

-- Update consultation status
IF OBJECT_ID('sp_UpdateConsultationStatus', 'P') IS NOT NULL
DROP PROCEDURE sp_UpdateConsultationStatus
    GO

CREATE PROCEDURE sp_UpdateConsultationStatus
    @Id INT,
    @Status NVARCHAR(50)
AS
BEGIN
UPDATE [dbo].[MedicalConsultation]
SET
    [Status] = @Status,
    [UpdatedAt] = GETDATE()
WHERE [Id] = @Id
END
GO

-- Delete consultation (soft delete)
IF OBJECT_ID('sp_DeleteConsultation', 'P') IS NOT NULL
DROP PROCEDURE sp_DeleteConsultation
    GO

CREATE PROCEDURE sp_DeleteConsultation
    @Id INT
AS
BEGIN
UPDATE [dbo].[MedicalConsultation]
SET
    [IsActive] = 0,
    [UpdatedAt] = GETDATE()
WHERE [Id] = @Id
END
GO