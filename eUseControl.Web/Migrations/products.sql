-- Create Product table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Product')
BEGIN
CREATE TABLE [dbo].[Product](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Type] [nvarchar](50) NOT NULL,
    [Price] [decimal](18, 2) NOT NULL,
    [DiscountedPrice] [decimal](18, 2) NULL,
    [ImageUrl] [nvarchar](255) NOT NULL,
    [Description] [nvarchar](500) NULL,
    [Gender] [nvarchar](50) NULL,
    [VisionType] [nvarchar](50) NULL,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] [datetime] NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
END
GO

-- Check if data already exists to avoid duplicate inserts
IF NOT EXISTS (SELECT TOP 1 * FROM [dbo].[Product])
BEGIN
    -- Insert sample data for Sunglasses
INSERT INTO [dbo].[Product] ([Name], [Type], [Price], [DiscountedPrice], [ImageUrl], [Description], [Gender], [IsActive], [CreatedAt])
VALUES
    ('Classic Aviator', 'Sunglasses', 100.00, NULL, '~/Content/Sunglasses/soare1.jpg', 'Timeless aviator style with UV protection.', 'Men', 1, GETDATE()),
    ('Retro Round', 'Sunglasses', 120.00, NULL, '~/Content/Sunglasses/soare2.jpg', 'Vintage round frames for a classic look.', 'Women', 1, GETDATE()),
    ('Modern Square', 'Sunglasses', 124.00, NULL, '~/Content/Sunglasses/soare3.jpg', 'Bold square design, perfect for any outfit.', 'Men', 1, GETDATE()),
    ('Sporty Wrap', 'Sunglasses', 156.00, NULL, '~/Content/Sunglasses/soare4.jpg', 'Sporty wrap-around for active lifestyles.', 'Men', 1, GETDATE()),
    ('Vintage Cat Eye', 'Sunglasses', 434.00, NULL, '~/Content/Sunglasses/soare5.jpg', 'Chic cat eye frames for a retro vibe.', 'Women', 1, GETDATE()),
    ('Trendy Oversized', 'Sunglasses', 123.00, NULL, '~/Content/Sunglasses/soare6.jpg', 'Oversized lenses for maximum coverage.', 'Women', 1, GETDATE());

-- Insert sample data for Optical Frames
INSERT INTO [dbo].[Product] ([Name], [Type], [Price], [DiscountedPrice], [ImageUrl], [Description], [VisionType], [IsActive], [CreatedAt])
VALUES
    ('Urban Classic', 'OpticalFrames', 140.00, NULL, '~/Content/Frames/rame1.jpg', 'Sleek and timeless for everyday wear.', 'Near', 1, GETDATE()),
    ('Elegant Oval', 'OpticalFrames', 178.00, NULL, '~/Content/Frames/rame2.jpg', 'Elegant oval frames for a refined look.', 'Distance', 1, GETDATE()),
    ('Bold Rectangle', 'OpticalFrames', 167.00, NULL, '~/Content/Frames/rame3.jpg', 'Bold rectangular design for a modern style.', 'Near', 1, GETDATE()),
    ('Chic Cat Eye', 'OpticalFrames', 189.00, NULL, '~/Content/Frames/rame4.jpg', 'Chic cat eye frames for a fashionable statement.', 'Distance', 1, GETDATE()),
    ('Minimalist Round', 'OpticalFrames', 177.00, NULL, '~/Content/Frames/rame5.jpg', 'Minimalist round frames for a subtle touch.', 'Near', 1, GETDATE()),
    ('Trendy Square', 'OpticalFrames', 134.00, NULL, '~/Content/Frames/rame6.jpg', 'Trendy square frames for a bold impression.', 'Distance', 1, GETDATE());

-- Insert sample data for Lenses
INSERT INTO [dbo].[Product] ([Name], [Type], [Price], [DiscountedPrice], [ImageUrl], [Description], [IsActive], [CreatedAt])
VALUES
    ('Essilor Single Vision', 'Lenses', 120.00, 60.00, '~/Content/Lenses/Lentila1.png', 'Clear single vision lenses with UV protection', 1, GETDATE()),
    ('Zeiss Progressive', 'Lenses', 280.00, 280.00, '~/Content/Lenses/Lentila2.png', 'Premium progressive lenses with digital design', 1, GETDATE()),
    ('Hoya Bifocal', 'Lenses', 160.00, 160.00, '~/Content/Lenses/Lentila3.png', 'Traditional bifocal lenses with clear segment', 1, GETDATE()),
    ('Varilux Progressive', 'Lenses', 320.00, 320.00, '~/Content/Lenses/Lentila4.png', 'Advanced progressive lenses with smooth transitions', 1, GETDATE()),
    ('Eyezen Single Vision', 'Lenses', 140.00, 70.00, '~/Content/Lenses/Lentila6.png', 'Digital-friendly lenses for daily eye comfort', 1, GETDATE());
END
GO
