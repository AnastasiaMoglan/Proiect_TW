-- Database Creation Script for User Authentication System

USE [master]
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'eUseControlDB')
BEGIN
    CREATE DATABASE [eUseControlDB]
END
GO

USE [eUseControlDB]
GO

-- Create Users table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
    [UserId] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
    [Username] [nvarchar](50) NOT NULL,
    [Password] [nvarchar](100) NOT NULL,
    [Email] [nvarchar](100) NOT NULL,
    [FirstName] [nvarchar](50) NULL,
    [LastName] [nvarchar](50) NULL,
    [PhoneNumber] [nvarchar](20) NULL,
    [Address] [nvarchar](255) NULL,
    [UserRole] [nvarchar](20) NOT NULL DEFAULT 'User',
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
    [LastLogin] [datetime] NULL,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [UQ_User_Username] UNIQUE ([Username]),
    CONSTRAINT [UQ_User_Email] UNIQUE ([Email])
    )
END
GO

-- Create LoginRecords table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LoginRecord]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LoginRecord](
    [LoginId] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
    [UserId] [uniqueidentifier] NOT NULL,
    [LoginDateTime] [datetime] NOT NULL,
    [IpAddress] [nvarchar](50) NULL,
    [UserAgent] [nvarchar](255) NULL,
    [IsSuccessful] [bit] NOT NULL,
    CONSTRAINT [PK_LoginRecord] PRIMARY KEY CLUSTERED ([LoginId] ASC),
    CONSTRAINT [FK_LoginRecord_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
    )
END
GO

-- Create UserSessions table for maintaining active sessions
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSession]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserSession](
    [SessionId] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
    [UserId] [uniqueidentifier] NOT NULL,
    [Token] [nvarchar](255) NOT NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
    [ExpiresAt] [datetime] NOT NULL,
    [LastActivity] [datetime] NOT NULL,
    [IpAddress] [nvarchar](50) NULL,
    [UserAgent] [nvarchar](255) NULL,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    CONSTRAINT [PK_UserSession] PRIMARY KEY CLUSTERED ([SessionId] ASC),
    CONSTRAINT [FK_UserSession_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
    )
END
GO

-- Insert admin user with password "Admin123!" (you should use proper password hashing in production)
IF NOT EXISTS (SELECT * FROM [dbo].[User] WHERE [Username] = 'admin')
BEGIN
INSERT INTO [dbo].[User] ([Username], [Password], [Email], [FirstName], [LastName], [UserRole])
VALUES ('admin', 'AQAAAAEAACcQAAAAEKXxXGA5dn9pJox5ACd/zl9QwQDYMjOvn7G8UHtx9hv/gQUBW40h1rB3GXy4qPPDOA==', 'admin@example.com', 'System', 'Administrator', 'Admin')
END
GO

-- Insert regular user with password "User123!" (you should use proper password hashing in production)
IF NOT EXISTS (SELECT * FROM [dbo].[User] WHERE [Username] = 'user')
BEGIN
INSERT INTO [dbo].[User] ([Username], [Password], [Email], [FirstName], [LastName], [UserRole])
VALUES ('user', 'AQAAAAEAACcQAAAAECDNIrUelNXl5VDLqKe0Zr4jS2MXZVIwHyoLEsKvPg3vUJcTYeT1Vkz+oGZ3iZOGSA==', 'user@example.com', 'Regular', 'User', 'User')
END
GO