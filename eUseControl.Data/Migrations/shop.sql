-- Create Shop table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Shop')
BEGIN
CREATE TABLE [dbo].[Shop](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Address] [nvarchar](200) NOT NULL,
    [City] [nvarchar](50) NOT NULL,
    [PhoneNumber] [nvarchar](20) NOT NULL,
    [Email] [nvarchar](100) NULL,
    [Description] [nvarchar](255) NULL,
    [ImageUrl] [nvarchar](255) NULL,
    [OpeningHours] [nvarchar](100) NOT NULL,
    [Latitude] [decimal](10, 6) NOT NULL,
    [Longitude] [decimal](10, 6) NOT NULL,
    [HasParkingAvailable] [bit] NOT NULL DEFAULT 0,
    [IsServiceCenter] [bit] NOT NULL DEFAULT 0,
    [HasOptician] [bit] NOT NULL DEFAULT 0,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] [datetime] NULL,
    CONSTRAINT [PK_Shop] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
END
GO

-- Check if data already exists to avoid duplicate inserts
IF NOT EXISTS (SELECT TOP 1 * FROM [dbo].[Shop])
BEGIN
    -- Insert sample data for Shops
INSERT INTO [dbo].[Shop] (
    [Name], [Address], [City], [PhoneNumber], [Email], [Description],
    [ImageUrl], [OpeningHours], [Latitude], [Longitude],
    [HasParkingAvailable], [IsServiceCenter], [HasOptician], [IsActive]
)
VALUES
    (
    'EyeCare Center Downtown',
    '123 Main Street',
    'New York',
    '(212) 555-1234',
    'downtown@eyecare.com',
    'Our flagship store with full optical services and the latest designer frames.',
    '~/Content/Images/Shops/shop1.jpg',
    'Mon-Fri: 9AM-7PM, Sat: 10AM-5PM, Sun: Closed',
    40.712776,
    -74.005974,
    1, 1, 1, 1
    ),
    (
    'EyeCare Express Midtown',
    '456 Park Avenue',
    'New York',
    '(212) 555-5678',
    'midtown@eyecare.com',
    'Express service for busy professionals. Get your glasses in under an hour!',
    '~/Content/Images/Shops/shop2.jpg',
    'Mon-Fri: 8AM-8PM, Sat-Sun: 10AM-6PM',
    40.754932,
    -73.984016,
    0, 1, 1, 1
    ),
    (
    'EyeCare Luxury Collection',
    '789 Fifth Avenue',
    'New York',
    '(212) 555-9012',
    'luxury@eyecare.com',
    'Premium designer eyewear and personalized styling consultations.',
    '~/Content/Images/Shops/shop3.jpg',
    'Mon-Sat: 10AM-7PM, Sun: 12PM-5PM',
    40.763841,
    -73.973388,
    1, 0, 1, 1
    ),
    (
    'EyeCare Family Center',
    '321 Maple Street',
    'Brooklyn',
    '(718) 555-3456',
    'brooklyn@eyecare.com',
    'Family-friendly store with specialized services for children and teens.',
    '~/Content/Images/Shops/shop4.jpg',
    'Mon-Fri: 9AM-6PM, Sat: 9AM-5PM, Sun: Closed',
    40.650002,
    -73.949997,
    1, 1, 1, 1
    ),
    (
    'EyeCare Quick Service',
    '654 Broadway',
    'Queens',
    '(718) 555-7890',
    'queens@eyecare.com',
    'Rapid optical services with affordable options for the whole family.',
    '~/Content/Images/Shops/shop5.jpg',
    'Mon-Sun: 10AM-9PM',
    40.742054,
    -73.769417,
    1, 0, 1, 1
    ),
    (
    'EyeCare Contact Lens Specialist',
    '987 Hudson Street',
    'Jersey City',
    '(201) 555-2345',
    'contacts@eyecare.com',
    'Specializing in all types of contact lenses with expert fitting services.',
    '~/Content/Images/Shops/shop6.jpg',
    'Mon-Fri: 9AM-7PM, Sat: 10AM-4PM, Sun: Closed',
    40.728157,
    -74.077642,
    0, 0, 1, 1
    );
END
GO