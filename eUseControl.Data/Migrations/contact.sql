-- Create Contact table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Contact')
    BEGIN
        CREATE TABLE [dbo].[Contact](
                                        [Id] [int] IDENTITY(1,1) NOT NULL,
                                        [FullName] [nvarchar](100) NOT NULL,
                                        [Email] [nvarchar](100) NOT NULL,
                                        [Subject] [nvarchar](200) NOT NULL,
                                        [Message] [nvarchar](max) NOT NULL,
                                        [IsRead] [bit] NOT NULL DEFAULT 0,
                                        [DateSubmitted] [datetime] NOT NULL DEFAULT GETDATE(),
                                        [DateRead] [datetime] NULL,
                                        [Status] [nvarchar](50) NOT NULL DEFAULT 'New',
                                        [ResponseSent] [bit] NOT NULL DEFAULT 0,
                                        [IPAddress] [nvarchar](50) NULL,
                                        CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([Id] ASC)
        )
    END
GO

-- Create index for faster lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Contact_Email' AND object_id = OBJECT_ID('dbo.Contact'))
    BEGIN
        CREATE NONCLUSTERED INDEX [IX_Contact_Email] ON [dbo].[Contact]([Email] ASC)
    END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Contact_Status' AND object_id = OBJECT_ID('dbo.Contact'))
    BEGIN
        CREATE NONCLUSTERED INDEX [IX_Contact_Status] ON [dbo].[Contact]([Status] ASC)
    END
GO

-- Create stored procedures for common operations

-- Get all contacts
IF OBJECT_ID('sp_GetAllContacts', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetAllContacts
GO

CREATE PROCEDURE sp_GetAllContacts
AS
BEGIN
    SELECT * FROM [dbo].[Contact]
    ORDER BY [DateSubmitted] DESC
END
GO

-- Get contact by ID
IF OBJECT_ID('sp_GetContactById', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetContactById
GO

CREATE PROCEDURE sp_GetContactById
@Id INT
AS
BEGIN
    SELECT * FROM [dbo].[Contact]
    WHERE [Id] = @Id
END
GO

-- Create new contact
IF OBJECT_ID('sp_CreateContact', 'P') IS NOT NULL
    DROP PROCEDURE sp_CreateContact
GO

CREATE PROCEDURE sp_CreateContact
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Subject NVARCHAR(200),
    @Message NVARCHAR(MAX),
    @IPAddress NVARCHAR(50) = NULL
AS
BEGIN
    INSERT INTO [dbo].[Contact] (
        [FullName],
        [Email],
        [Subject],
        [Message],
        [IPAddress],
        [DateSubmitted]
    )
    VALUES (
               @FullName,
               @Email,
               @Subject,
               @Message,
               @IPAddress,
               GETDATE()
           )

    SELECT SCOPE_IDENTITY() AS NewContactId
END
GO

-- Mark contact as read
IF OBJECT_ID('sp_MarkContactAsRead', 'P') IS NOT NULL
    DROP PROCEDURE sp_MarkContactAsRead
GO

CREATE PROCEDURE sp_MarkContactAsRead
@Id INT
AS
BEGIN
    UPDATE [dbo].[Contact]
    SET
        [IsRead] = 1,
        [DateRead] = GETDATE(),
        [Status] = 'Read'
    WHERE [Id] = @Id
END
GO

-- Update contact status
IF OBJECT_ID('sp_UpdateContactStatus', 'P') IS NOT NULL
    DROP PROCEDURE sp_UpdateContactStatus
GO

CREATE PROCEDURE sp_UpdateContactStatus
    @Id INT,
    @Status NVARCHAR(50)
AS
BEGIN
    UPDATE [dbo].[Contact]
    SET [Status] = @Status
    WHERE [Id] = @Id
END
GO

-- Mark response sent
IF OBJECT_ID('sp_MarkResponseSent', 'P') IS NOT NULL
    DROP PROCEDURE sp_MarkResponseSent
GO

CREATE PROCEDURE sp_MarkResponseSent
@Id INT
AS
BEGIN
    UPDATE [dbo].[Contact]
    SET
        [ResponseSent] = 1,
        [Status] = 'Responded'
    WHERE [Id] = @Id
END
GO

-- Get contacts by status
IF OBJECT_ID('sp_GetContactsByStatus', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetContactsByStatus
GO

CREATE PROCEDURE sp_GetContactsByStatus
@Status NVARCHAR(50)
AS
BEGIN
    SELECT * FROM [dbo].[Contact]
    WHERE [Status] = @Status
    ORDER BY [DateSubmitted] DESC
END
GO

-- Get unread contacts
IF OBJECT_ID('sp_GetUnreadContacts', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetUnreadContacts
GO

CREATE PROCEDURE sp_GetUnreadContacts
AS
BEGIN
    SELECT * FROM [dbo].[Contact]
    WHERE [IsRead] = 0
    ORDER BY [DateSubmitted] DESC
END
GO

-- Delete contact
IF OBJECT_ID('sp_DeleteContact', 'P') IS NOT NULL
    DROP PROCEDURE sp_DeleteContact
GO

CREATE PROCEDURE sp_DeleteContact
@Id INT
AS
BEGIN
    DELETE FROM [dbo].[Contact]
    WHERE [Id] = @Id
END
GO