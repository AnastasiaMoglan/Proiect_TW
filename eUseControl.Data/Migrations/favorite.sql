-- Create Favorites table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Favorite')
BEGIN
CREATE TABLE [dbo].[Favorite](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
    [ProductId] [int] NOT NULL,
    [DateAdded] [datetime] NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_Favorite] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Favorite_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_Favorite_UserProduct] UNIQUE ([UserId], [ProductId])
    )
END
GO

-- Create index for faster lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Favorite_UserId' AND object_id = OBJECT_ID('dbo.Favorite'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Favorite_UserId] ON [dbo].[Favorite]([UserId] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Favorite_ProductId' AND object_id = OBJECT_ID('dbo.Favorite'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Favorite_ProductId] ON [dbo].[Favorite]([ProductId] ASC)
END
GO

-- Add sample data for testing (optional)
-- This only inserts if the table is empty
IF NOT EXISTS (SELECT TOP 1 * FROM [dbo].[Favorite])
BEGIN
    -- Assuming there's at least one user with ID 1 and some products
    -- Check if user with ID 1 exists
    IF EXISTS (SELECT TOP 1 * FROM [dbo].[Users] WHERE [Id] = 1)
BEGIN
        -- Add some favorites for user ID 1
        -- Get some random products to add as favorites
        DECLARE @ProductId1 INT, @ProductId2 INT
        
        -- Get first product ID
SELECT TOP 1 @ProductId1 = [Id]
FROM [dbo].[Product]
WHERE [Type] = 'Sunglasses'
ORDER BY NEWID()

-- Get second product ID
SELECT TOP 1 @ProductId2 = [Id]
FROM [dbo].[Product]
WHERE [Type] = 'OpticalFrames'
ORDER BY NEWID()

    -- Insert favorites if we found products
    IF @ProductId1 IS NOT NULL
BEGIN
INSERT INTO [dbo].[Favorite] ([UserId], [ProductId], [DateAdded])
VALUES (1, @ProductId1, GETDATE())
END
        
        IF @ProductId2 IS NOT NULL
BEGIN
INSERT INTO [dbo].[Favorite] ([UserId], [ProductId], [DateAdded])
VALUES (1, @ProductId2, GETDATE())
END
END
END
GO

-- Create stored procedures for common operations

-- Get all favorites for a user
IF OBJECT_ID('sp_GetUserFavorites', 'P') IS NOT NULL
DROP PROCEDURE sp_GetUserFavorites
    GO

CREATE PROCEDURE sp_GetUserFavorites
    @UserId INT
AS
BEGIN
SELECT f.*, p.*
FROM [dbo].[Favorite] f
    INNER JOIN [dbo].[Product] p ON f.ProductId = p.Id
WHERE f.UserId = @UserId
ORDER BY f.DateAdded DESC
END
GO

-- Add product to favorites
IF OBJECT_ID('sp_AddToFavorites', 'P') IS NOT NULL
DROP PROCEDURE sp_AddToFavorites
    GO

CREATE PROCEDURE sp_AddToFavorites
    @UserId INT,
    @ProductId INT
AS
BEGIN
    -- Check if the favorite already exists
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Favorite] WHERE UserId = @UserId AND ProductId = @ProductId)
BEGIN
INSERT INTO [dbo].[Favorite] (UserId, ProductId, DateAdded)
VALUES (@UserId, @ProductId, GETDATE())

SELECT SCOPE_IDENTITY() AS NewFavoriteId
END
ELSE
BEGIN
        -- Return the existing favorite ID
SELECT Id AS NewFavoriteId FROM [dbo].[Favorite]
WHERE UserId = @UserId AND ProductId = @ProductId
END
END
GO

-- Remove from favorites
IF OBJECT_ID('sp_RemoveFromFavorites', 'P') IS NOT NULL
DROP PROCEDURE sp_RemoveFromFavorites
    GO

CREATE PROCEDURE sp_RemoveFromFavorites
    @FavoriteId INT
AS
BEGIN
DELETE FROM [dbo].[Favorite]
WHERE Id = @FavoriteId
END
GO

-- Check if a product is in user's favorites
IF OBJECT_ID('sp_IsFavorite', 'P') IS NOT NULL
DROP PROCEDURE sp_IsFavorite
    GO

CREATE PROCEDURE sp_IsFavorite
    @UserId INT,
    @ProductId INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM [dbo].[Favorite] WHERE UserId = @UserId AND ProductId = @ProductId)
SELECT 1 AS IsFavorite
    ELSE
SELECT 0 AS IsFavorite
END
GO

-- Toggle favorite status
IF OBJECT_ID('sp_ToggleFavorite', 'P') IS NOT NULL
DROP PROCEDURE sp_ToggleFavorite
    GO

CREATE PROCEDURE sp_ToggleFavorite
    @UserId INT,
    @ProductId INT
AS
BEGIN
    DECLARE @FavoriteId INT
    
    -- Check if the favorite exists
SELECT @FavoriteId = Id FROM [dbo].[Favorite]
WHERE UserId = @UserId AND ProductId = @ProductId

    -- If it exists, remove it
    IF @FavoriteId IS NOT NULL
BEGIN
DELETE FROM [dbo].[Favorite] WHERE Id = @FavoriteId
SELECT 0 AS IsFavorite
END
    -- If it doesn't exist, add it
ELSE
BEGIN
INSERT INTO [dbo].[Favorite] (UserId, ProductId, DateAdded)
VALUES (@UserId, @ProductId, GETDATE())
SELECT 1 AS IsFavorite
END
END
GO