-- Create Order table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Order')
BEGIN
CREATE TABLE [dbo].[Order](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [OrderNumber] [nvarchar](50) NOT NULL,
    [UserId] [int] NOT NULL,
    [OrderDate] [datetime] NOT NULL DEFAULT GETDATE(),
    [TotalAmount] [decimal](18, 2) NOT NULL,
    [ShippingAddress] [nvarchar](500) NULL,
    [BillingAddress] [nvarchar](500) NULL,
    [Phone] [nvarchar](50) NULL,
    [Email] [nvarchar](100) NOT NULL,
    [PaymentMethod] [nvarchar](50) NOT NULL,
    [PaymentStatus] [nvarchar](50) NOT NULL DEFAULT 'Pending',
    [OrderStatus] [nvarchar](50) NOT NULL DEFAULT 'Processing',
    [TrackingNumber] [nvarchar](100) NULL,
    [Notes] [nvarchar](1000) NULL,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] [datetime] NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
END
GO

-- Create OrderItem table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItem')
BEGIN
CREATE TABLE [dbo].[OrderItem](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [OrderId] [int] NOT NULL,
    [ProductId] [int] NOT NULL,
    [Quantity] [int] NOT NULL DEFAULT 1,
    [Price] [decimal](18, 2) NOT NULL,
    [Discount] [decimal](18, 2) NULL DEFAULT 0,
    [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id]),
    CONSTRAINT [FK_OrderItem_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
    )
END
GO

-- Create indexes for faster lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Order_UserId' AND object_id = OBJECT_ID('dbo.Order'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Order_UserId] ON [dbo].[Order]([UserId] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Order_OrderNumber' AND object_id = OBJECT_ID('dbo.Order'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Order_OrderNumber] ON [dbo].[Order]([OrderNumber] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Order_OrderStatus' AND object_id = OBJECT_ID('dbo.Order'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Order_OrderStatus] ON [dbo].[Order]([OrderStatus] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrderItem_OrderId' AND object_id = OBJECT_ID('dbo.OrderItem'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_OrderItem_OrderId] ON [dbo].[OrderItem]([OrderId] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrderItem_ProductId' AND object_id = OBJECT_ID('dbo.OrderItem'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_OrderItem_ProductId] ON [dbo].[OrderItem]([ProductId] ASC)
END
GO

-- Create stored procedures for common operations

-- Get all orders
IF OBJECT_ID('sp_GetAllOrders', 'P') IS NOT NULL
DROP PROCEDURE sp_GetAllOrders
    GO

CREATE PROCEDURE sp_GetAllOrders
    AS
BEGIN
SELECT * FROM [dbo].[Order]
WHERE [IsActive] = 1
ORDER BY [OrderDate] DESC
END
GO

-- Get order by ID
IF OBJECT_ID('sp_GetOrderById', 'P') IS NOT NULL
DROP PROCEDURE sp_GetOrderById
    GO

CREATE PROCEDURE sp_GetOrderById
    @Id INT
AS
BEGIN
SELECT * FROM [dbo].[Order]
WHERE [Id] = @Id AND [IsActive] = 1
END
GO

-- Get orders by user ID
IF OBJECT_ID('sp_GetOrdersByUserId', 'P') IS NOT NULL
DROP PROCEDURE sp_GetOrdersByUserId
    GO

CREATE PROCEDURE sp_GetOrdersByUserId
    @UserId INT
AS
BEGIN
SELECT * FROM [dbo].[Order]
WHERE [UserId] = @UserId AND [IsActive] = 1
ORDER BY [OrderDate] DESC
END
GO

-- Get order items by order ID
IF OBJECT_ID('sp_GetOrderItemsByOrderId', 'P') IS NOT NULL
DROP PROCEDURE sp_GetOrderItemsByOrderId
    GO

CREATE PROCEDURE sp_GetOrderItemsByOrderId
    @OrderId INT
AS
BEGIN
SELECT oi.*, p.Name as ProductName, p.ImageUrl
FROM [dbo].[OrderItem] oi
    INNER JOIN [dbo].[Product] p ON oi.ProductId = p.Id
WHERE oi.OrderId = @OrderId
END
GO

-- Create new order
IF OBJECT_ID('sp_CreateOrder', 'P') IS NOT NULL
DROP PROCEDURE sp_CreateOrder
    GO

CREATE PROCEDURE sp_CreateOrder
    @OrderNumber NVARCHAR(50),
    @UserId INT,
    @TotalAmount DECIMAL(18, 2),
    @ShippingAddress NVARCHAR(500),
    @BillingAddress NVARCHAR(500),
    @Phone NVARCHAR(50),
    @Email NVARCHAR(100),
    @PaymentMethod NVARCHAR(50),
    @PaymentStatus NVARCHAR(50),
    @OrderStatus NVARCHAR(50),
    @Notes NVARCHAR(1000) = NULL
AS
BEGIN
INSERT INTO [dbo].[Order] (
    [OrderNumber],
    [UserId],
    [OrderDate],
    [TotalAmount],
    [ShippingAddress],
    [BillingAddress],
    [Phone],
    [Email],
    [PaymentMethod],
    [PaymentStatus],
    [OrderStatus],
    [Notes],
    [CreatedAt]
    )
VALUES (
    @OrderNumber,
    @UserId,
    GETDATE(),
    @TotalAmount,
    @ShippingAddress,
    @BillingAddress,
    @Phone,
    @Email,
    @PaymentMethod,
    @PaymentStatus,
    @OrderStatus,
    @Notes,
    GETDATE()
    )

SELECT SCOPE_IDENTITY() AS NewOrderId
END
GO

-- Add order item
IF OBJECT_ID('sp_AddOrderItem', 'P') IS NOT NULL
DROP PROCEDURE sp_AddOrderItem
    GO

CREATE PROCEDURE sp_AddOrderItem
    @OrderId INT,
    @ProductId INT,
    @Quantity INT,
    @Price DECIMAL(18, 2),
    @Discount DECIMAL(18, 2) = 0
AS
BEGIN
INSERT INTO [dbo].[OrderItem] (
    [OrderId],
    [ProductId],
    [Quantity],
    [Price],
    [Discount],
[CreatedAt]
)
VALUES (
    @OrderId,
    @ProductId,
    @Quantity,
    @Price,
    @Discount,
    GETDATE()
    )

SELECT SCOPE_IDENTITY() AS NewOrderItemId
END
GO

-- Update order status
IF OBJECT_ID('sp_UpdateOrderStatus', 'P') IS NOT NULL
DROP PROCEDURE sp_UpdateOrderStatus
    GO

CREATE PROCEDURE sp_UpdateOrderStatus
    @Id INT,
    @OrderStatus NVARCHAR(50)
AS
BEGIN
UPDATE [dbo].[Order]
SET
    [OrderStatus] = @OrderStatus,
    [UpdatedAt] = GETDATE()
WHERE [Id] = @Id
END
GO

-- Update payment status
IF OBJECT_ID('sp_UpdatePaymentStatus', 'P') IS NOT NULL
DROP PROCEDURE sp_UpdatePaymentStatus
    GO

CREATE PROCEDURE sp_UpdatePaymentStatus
    @Id INT,
    @PaymentStatus NVARCHAR(50)
AS
BEGIN
UPDATE [dbo].[Order]
SET
    [PaymentStatus] = @PaymentStatus,
    [UpdatedAt] = GETDATE()
WHERE [Id] = @Id
END
GO

-- Add tracking number
IF OBJECT_ID('sp_AddTrackingNumber', 'P') IS NOT NULL
DROP PROCEDURE sp_AddTrackingNumber
    GO

CREATE PROCEDURE sp_AddTrackingNumber
    @Id INT,
    @TrackingNumber NVARCHAR(100)
AS
BEGIN
UPDATE [dbo].[Order]
SET
    [TrackingNumber] = @TrackingNumber,
    [UpdatedAt] = GETDATE()
WHERE [Id] = @Id
END
GO

-- Cancel order
IF OBJECT_ID('sp_CancelOrder', 'P') IS NOT NULL
DROP PROCEDURE sp_CancelOrder
    GO

CREATE PROCEDURE sp_CancelOrder
    @Id INT
AS
BEGIN
UPDATE [dbo].[Order]
SET
    [OrderStatus] = 'Cancelled',
    [UpdatedAt] = GETDATE()
WHERE [Id] = @Id
END
GO

-- Delete order (soft delete)
IF OBJECT_ID('sp_DeleteOrder', 'P') IS NOT NULL
DROP PROCEDURE sp_DeleteOrder
    GO

CREATE PROCEDURE sp_DeleteOrder
    @Id INT
AS
BEGIN
UPDATE [dbo].[Order]
SET
    [IsActive] = 0,
    [UpdatedAt] = GETDATE()
WHERE [Id] = @Id
END
GO

-- Get orders by status
IF OBJECT_ID('sp_GetOrdersByStatus', 'P') IS NOT NULL
DROP PROCEDURE sp_GetOrdersByStatus
    GO

CREATE PROCEDURE sp_GetOrdersByStatus
    @OrderStatus NVARCHAR(50)
AS
BEGIN
SELECT * FROM [dbo].[Order]
WHERE [OrderStatus] = @OrderStatus AND [IsActive] = 1
ORDER BY [OrderDate] DESC
END
GO

-- Get recent orders
IF OBJECT_ID('sp_GetRecentOrders', 'P') IS NOT NULL
DROP PROCEDURE sp_GetRecentOrders
    GO

CREATE PROCEDURE sp_GetRecentOrders
    @Count INT = 10
AS
BEGIN
SELECT TOP (@Count) * FROM [dbo].[Order]
WHERE [IsActive] = 1
ORDER BY [OrderDate] DESC
END
GO

-- Get order by order number
IF OBJECT_ID('sp_GetOrderByOrderNumber', 'P') IS NOT NULL
DROP PROCEDURE sp_GetOrderByOrderNumber
    GO

CREATE PROCEDURE sp_GetOrderByOrderNumber
    @OrderNumber NVARCHAR(50)
AS
BEGIN
SELECT * FROM [dbo].[Order]
WHERE [OrderNumber] = @OrderNumber AND [IsActive] = 1
END
GO

-- Update order
IF OBJECT_ID('sp_UpdateOrder', 'P') IS NOT NULL
DROP PROCEDURE sp_UpdateOrder
    GO

CREATE PROCEDURE sp_UpdateOrder
    @Id INT,
    @ShippingAddress NVARCHAR(500),
    @BillingAddress NVARCHAR(500),
    @Phone NVARCHAR(50),
    @Email NVARCHAR(100),
    @Notes NVARCHAR(1000) = NULL
AS
BEGIN
UPDATE [dbo].[Order]
SET
    [ShippingAddress] = @ShippingAddress,
    [BillingAddress] = @BillingAddress,
    [Phone] = @Phone,
    [Email] = @Email,
    [Notes] = @Notes,
    [UpdatedAt] = GETDATE()
WHERE [Id] = @Id
END
GO