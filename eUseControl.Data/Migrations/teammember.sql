-- team_members.sql
-- Create TeamMember table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TeamMember')
BEGIN
CREATE TABLE [dbo].[TeamMember](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Position] [nvarchar](100) NOT NULL,
    [ImageUrl] [nvarchar](255) NOT NULL,
    [Description] [nvarchar](500) NULL,
    [Email] [nvarchar](100) NULL,
    [DisplayOrder] [int] NOT NULL DEFAULT 0,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] [datetime] NULL,
    CONSTRAINT [PK_TeamMember] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
END
GO

-- Check if data already exists to avoid duplicate inserts
IF NOT EXISTS (SELECT TOP 1 * FROM [dbo].[TeamMember])
BEGIN
    -- Insert sample data for TeamMembers
INSERT INTO [dbo].[TeamMember] (
    [Name],
    [Position],
    [ImageUrl],
    [Description],
    [Email],
    [DisplayOrder],
    [IsActive],
[CreatedAt]
)
VALUES
    (
    'Dr. Jane Smith',
    'Chief Ophthalmologist',
    '~/Content/Team/oftalmolog1.jpg',
    'Dr. Smith has over 15 years of experience in treating various eye conditions and specializes in laser eye surgery.',
    'jane.smith@eyecare.com',
    1,
    1,
    GETDATE()
    ),
    (
    'Dr. Lara Davis',
    'Optometrist',
    '~/Content/Team/oftalmolog2.jpg',
    'Dr. Davis specializes in contact lens fitting and has helped thousands of patients find the perfect lenses for their needs.',
    'lara.davis@eyecare.com',
    2,
    1,
    GETDATE()
    ),
    (
    'Sarah Johnson',
    'Optical Technician',
    '~/Content/Team/oftalmolog3.jpg',
    'Sarah has been with our team for 8 years and is an expert in frame styling and lens technology.',
    'sarah.johnson@eyecare.com',
    3,
    1,
    GETDATE()
    );
END
GO

-- Create stored procedures for common operations

-- Get all team members
IF OBJECT_ID('sp_GetAllTeamMembers', 'P') IS NOT NULL
DROP PROCEDURE sp_GetAllTeamMembers
    GO

CREATE PROCEDURE sp_GetAllTeamMembers
    AS
BEGIN
SELECT * FROM [dbo].[TeamMember]
ORDER BY [DisplayOrder] ASC
END
GO

-- Get active team members
IF OBJECT_ID('sp_GetActiveTeamMembers', 'P') IS NOT NULL
DROP PROCEDURE sp_GetActiveTeamMembers
    GO

CREATE PROCEDURE sp_GetActiveTeamMembers
    AS
BEGIN
SELECT * FROM [dbo].[TeamMember]
WHERE [IsActive] = 1
ORDER BY [DisplayOrder] ASC
END
GO