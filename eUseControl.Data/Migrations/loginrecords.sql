IF OBJECT_ID('dbo.LoginRecords', 'U') IS NOT NULL
DROP TABLE dbo.LoginRecords;
GO

CREATE TABLE dbo.LoginRecords (
                                  Id INT IDENTITY(1,1) PRIMARY KEY,
                                  Email NVARCHAR(100) NOT NULL,
                                  IP NVARCHAR(50) NULL,
                                  UserAgent NVARCHAR(255) NULL,
                                  IsSuccessful BIT NOT NULL,
                                  LoginTime DATETIME NOT NULL DEFAULT GETUTCDATE()
);
GO
