-- Create Accessory table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Accessory')
BEGIN
CREATE TABLE [dbo].[Accessory](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Category] [nvarchar](50) NOT NULL,
    [Price] [decimal](18, 2) NOT NULL,
    [ImagePath] [nvarchar](255) NOT NULL,
    [Description] [nvarchar](500) NULL,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] [datetime] NULL,
    CONSTRAINT [PK_Accessory] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
END
GO

-- Check if data already exists to avoid duplicate inserts
IF NOT EXISTS (SELECT TOP 1 * FROM [dbo].[Accessory])
BEGIN
        -- Insert sample data for Eyewear Cases
INSERT INTO [dbo].[Accessory] ([Name], [Category], [Price], [ImagePath], [Description], [IsActive], [CreatedAt])
VALUES
    ('Premium Hard Shell Eyewear Case', 'case', 24.99, '/Content/Images/Accessories/case1.jpg', 'Durable hard shell case with soft interior lining to protect your glasses from scratches and damage. Includes cleaning cloth.', 1, GETDATE()),
    ('Leather Glasses Case', 'case', 34.99, '/Content/Images/Accessories/case2.jpg', 'Elegant leather case with magnetic closure. Available in black, brown, and burgundy colors.', 1, GETDATE()),
    ('Slim Soft Pouch', 'case', 12.99, '/Content/Images/Accessories/case3.jpg', 'Lightweight microfiber pouch that doubles as a cleaning cloth. Perfect for on-the-go protection.', 1, GETDATE()),
    ('Designer Hard Case', 'case', 39.99, '/Content/Images/Accessories/case4.jpg', 'Stylish designer hard case with embossed pattern and premium finish. Makes a great gift.', 1, GETDATE());

-- Insert sample data for Cleaning Sprays
INSERT INTO [dbo].[Accessory] ([Name], [Category], [Price], [ImagePath], [Description], [IsActive], [CreatedAt])
VALUES
    ('Anti-Fog Lens Spray', 'spray', 9.99, '/Content/Images/Accessories/spray1.jpg', 'Prevents fogging on all types of lenses. Perfect for mask wearers and cold weather. 60ml bottle.', 1, GETDATE()),
    ('Premium Lens Cleaner Spray', 'spray', 8.99, '/Content/Images/Accessories/spray2.jpg', 'Streak-free formula safe for all lens coatings. Removes fingerprints, oil, and dust effectively. 120ml bottle.', 1, GETDATE()),
    ('Travel Size Cleaning Kit', 'spray', 12.99, '/Content/Images/Accessories/spray3.jpg', 'Compact cleaning kit with 30ml spray bottle and microfiber cloth. TSA-approved size for travel.', 1, GETDATE()),
    ('Eco-Friendly Lens Cleaner', 'spray', 11.99, '/Content/Images/Accessories/spray4.jpg', 'Plant-based, biodegradable formula in recycled packaging. Gentle yet effective cleaning. 200ml bottle.', 1, GETDATE());

-- Insert sample data for Cleaning Cloths
INSERT INTO [dbo].[Accessory] ([Name], [Category], [Price], [ImagePath], [Description], [IsActive], [CreatedAt])
VALUES
    ('Microfiber Cleaning Cloth Pack', 'cloth', 7.99, '/Content/Images/Accessories/cloth1.jpg', 'Set of 5 ultra-soft microfiber cloths in different colors. Machine washable and lint-free.', 1, GETDATE()),
    ('Premium Chamois Cleaning Cloth', 'cloth', 9.99, '/Content/Images/Accessories/cloth2.jpg', 'Natural chamois leather cloth that cleans without scratching. Superior absorption and durability.', 1, GETDATE()),
    ('XL Cleaning Cloth', 'cloth', 6.99, '/Content/Images/Accessories/cloth3.jpg', 'Extra large (12"x12") microfiber cloth for quick and efficient cleaning of all lens types.', 1, GETDATE()),
    ('Designer Pattern Cloths', 'cloth', 14.99, '/Content/Images/Accessories/cloth4.jpg', 'Set of 3 decorative microfiber cloths with stylish patterns. Functional and fashionable.', 1, GETDATE());

-- Insert sample data for Cords & Chains
INSERT INTO [dbo].[Accessory] ([Name], [Category], [Price], [ImagePath], [Description], [IsActive], [CreatedAt])
VALUES
    ('Adjustable Eyewear Retainer', 'cord', 8.99, '/Content/Images/Accessories/cord1.jpg', 'Comfortable neoprene strap with adjustable fit. Keeps glasses secure during activities.', 1, GETDATE()),
    ('Metal Chain Strap', 'cord', 19.99, '/Content/Images/Accessories/cord2.jpg', 'Elegant gold or silver-toned metal chain with secure silicone ends. Perfect for reading glasses.', 1, GETDATE()),
    ('Sports Glasses Strap', 'cord', 12.99, '/Content/Images/Accessories/cord3.jpg', 'High-grip silicone strap designed for athletic activities. Floats in water and includes quick-release feature.', 1, GETDATE()),
    ('Beaded Eyewear Chain', 'cord', 24.99, '/Content/Images/Accessories/cord4.jpg', 'Fashionable beaded chain that transforms your glasses into a stylish accessory. Multiple color options available.', 1, GETDATE());
END
GO
