IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
DROP TABLE [dbo].[Users];
GO

CREATE TABLE [dbo].[Users] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Username] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [Salt] NVARCHAR(128) NOT NULL,
    [FirstName] NVARCHAR(50),
    [LastName] NVARCHAR(50),
    [Phone] NVARCHAR(20),
    [Address] NVARCHAR(255),
    [IsActive] BIT NOT NULL DEFAULT 1,
    [Role] NVARCHAR(20) NOT NULL DEFAULT 'User',
    [LastLogin] DATETIME,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME
    );
GO

ALTER TABLE [dbo].[Users] ADD CONSTRAINT UQ_Users_Username UNIQUE ([Username]);
ALTER TABLE [dbo].[Users] ADD CONSTRAINT UQ_Users_Email UNIQUE ([Email]);
GO

CREATE NONCLUSTERED INDEX IX_Users_Username ON [dbo].[Users]([Username]);
CREATE NONCLUSTERED INDEX IX_Users_Email ON [dbo].[Users]([Email]);
GO
