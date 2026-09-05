-- FixHub Database Schema
-- Run this in SSMS against your FixHubDb
-- Note: Uses LocalDB compatible syntax; adjust for full SQL Server if needed

USE FixHubDb;
GO

-- Service Categories (lookup table)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ServiceCategories')
BEGIN
    CREATE TABLE ServiceCategories (
        CategoryId INT PRIMARY KEY IDENTITY(1,1),
        CategoryName NVARCHAR(100) NOT NULL UNIQUE
    );
END
GO

-- Insert default categories if empty
IF NOT EXISTS (SELECT * FROM ServiceCategories)
BEGIN
    INSERT INTO ServiceCategories (CategoryName) VALUES
    ('Plumbing'),
    ('Electrical'),
    ('Cleaning'),
    ('AC Repair');
END
GO

-- Customers (service requesters)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
BEGIN
    CREATE TABLE Customers (
        CustomerId INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(150) NOT NULL,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        Phone NVARCHAR(20) NOT NULL,
        Address NVARCHAR(500) NOT NULL,
        PasswordHash NVARCHAR(255) NOT NULL,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
END
GO

-- Admins (service providers)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Admins')
BEGIN
    CREATE TABLE Admins (
        AdminId INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(150) NOT NULL,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        Phone NVARCHAR(20),
        CategoryId INT FOREIGN KEY REFERENCES ServiceCategories(CategoryId),
        Price DECIMAL(10,2) NOT NULL DEFAULT 0,
        Bio NVARCHAR(MAX),
        PasswordHash NVARCHAR(255) NOT NULL,
        IsApproved BIT DEFAULT 0,        -- 0 = pending, 1 = approved
        IsSuspended BIT DEFAULT 0,
        IsAvailable BIT DEFAULT 1,
        Rating DECIMAL(3,2) DEFAULT 0,   -- computed from reviews
        CreatedAt DATETIME DEFAULT GETDATE()
    );
END
GO

-- Bookings (service requests)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Bookings')
BEGIN
    CREATE TABLE Bookings (
        BookingId INT PRIMARY KEY IDENTITY(1,1),
        CustomerId INT FOREIGN KEY REFERENCES Customers(CustomerId),
        AdminId INT FOREIGN KEY REFERENCES Admins(AdminId),
        ServiceName NVARCHAR(150),       -- category/service name
        ScheduledDate DATETIME NOT NULL,
        Address NVARCHAR(500) NOT NULL,
        Status NVARCHAR(20) DEFAULT 'Pending',  -- Pending, Accepted, In Progress, Completed, Cancelled, Declined
        CreatedAt DATETIME DEFAULT GETDATE(),
        CONSTRAINT CK_Bookings_Status CHECK (Status IN ('Pending', 'Accepted', 'In Progress', 'Completed', 'Cancelled', 'Declined'))
    );
END
GO

-- Allow Cancelled and Declined on existing databases that had a narrower CHECK
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Bookings_Status')
    ALTER TABLE Bookings DROP CONSTRAINT CK_Bookings_Status;
GO
ALTER TABLE Bookings ADD CONSTRAINT CK_Bookings_Status
    CHECK (Status IN ('Pending', 'Accepted', 'In Progress', 'Completed', 'Cancelled', 'Declined'));
GO
 
-- Payments
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Payments')
BEGIN
    CREATE TABLE Payments (
        PaymentId INT PRIMARY KEY IDENTITY(1,1),
        BookingId INT FOREIGN KEY REFERENCES Bookings(BookingId),
        Amount DECIMAL(10,2) NOT NULL,
        CommissionAmount DECIMAL(10,2) NOT NULL,
        PaymentStatus NVARCHAR(20) DEFAULT 'Unpaid',  -- Paid, Unpaid
        PaymentMethod NVARCHAR(20),                    -- PayNow, PayAfter
        CreatedAt DATETIME DEFAULT GETDATE()
    );
END
GO

-- Reviews
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
BEGIN
    CREATE TABLE Reviews (
        ReviewId INT PRIMARY KEY IDENTITY(1,1),
        BookingId INT FOREIGN KEY REFERENCES Bookings(BookingId),
        CustomerId INT FOREIGN KEY REFERENCES Customers(CustomerId),
        AdminId INT FOREIGN KEY REFERENCES Admins(AdminId),
        Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
        Comment NVARCHAR(500),
        CreatedAt DATETIME DEFAULT GETDATE()
    );
END
GO

-- SuperAdmins (platform owner — separate from provider Admins)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SuperAdmins')
BEGIN
    CREATE TABLE SuperAdmins (
        SuperAdminId INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(50) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
END
GO

-- Complaints
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Complaints')
BEGIN
    CREATE TABLE Complaints (
        ComplaintId INT PRIMARY KEY IDENTITY(1,1),
        BookingId INT FOREIGN KEY REFERENCES Bookings(BookingId),
        Description NVARCHAR(MAX) NOT NULL,
        Status NVARCHAR(20) DEFAULT 'Open',  -- Open, Resolved
        ResolutionNotes NVARCHAR(MAX),
        ResolvedBySuperAdminId INT,
        ResolvedAt DATETIME,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
END
GO

-- Coupons
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Coupons')
BEGIN
    CREATE TABLE Coupons (
        CouponId INT PRIMARY KEY IDENTITY(1,1),
        Code NVARCHAR(20) NOT NULL UNIQUE,
        DiscountPercent DECIMAL(5,2) NOT NULL,
        ExpiryDate DATE,
        UsageLimit INT DEFAULT 1,
        UsedCount INT DEFAULT 0,
        IsActive BIT DEFAULT 1
    );
END
GO

-- Insert sample coupon
IF NOT EXISTS (SELECT * FROM Coupons WHERE Code = 'FIXFIRST10')
BEGIN
    INSERT INTO Coupons (Code, DiscountPercent, ExpiryDate, UsageLimit)
    VALUES ('FIXFIRST10', 10, DATEADD(MONTH, 6, GETDATE()), 100);
END
GO

-- Add missing columns if they don't exist (for existing databases)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Admins') AND name = 'Rating')
    ALTER TABLE Admins ADD Rating DECIMAL(3,2) DEFAULT 0;
GO
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Payments') AND name = 'PaymentMethod')
    ALTER TABLE Payments ADD PaymentMethod NVARCHAR(20);
GO
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Complaints') AND name = 'ResolutionNotes')
    ALTER TABLE Complaints ADD ResolutionNotes NVARCHAR(MAX);
GO
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Complaints') AND name = 'ResolvedBySuperAdminId')
    ALTER TABLE Complaints ADD ResolvedBySuperAdminId INT;
GO
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Complaints') AND name = 'ResolvedAt')
    ALTER TABLE Complaints ADD ResolvedAt DATETIME;
GO

-- Helper view: Provider with category name and computed rating
IF OBJECT_ID('dbo.vw_ProviderDetails', 'V') IS NOT NULL
    DROP VIEW dbo.vw_ProviderDetails;
GO
CREATE VIEW vw_ProviderDetails AS
SELECT
    a.AdminId,
    a.Name,
    a.Email,
    a.Phone,
    c.CategoryName,
    a.Price AS StartingPrice,
    a.Bio,
    a.IsApproved,
    a.IsSuspended,
    a.IsAvailable,
    ISNULL(a.Rating, 0) AS Rating,
    (SELECT COUNT(*) FROM Bookings b WHERE b.AdminId = a.AdminId AND b.Status = 'Completed') AS JobsCompleted
FROM Admins a
LEFT JOIN ServiceCategories c ON a.CategoryId = c.CategoryId;
GO

-- Helper view: Booking with customer & provider details
IF OBJECT_ID('dbo.vw_BookingDetails', 'V') IS NOT NULL
    DROP VIEW dbo.vw_BookingDetails;
GO
CREATE VIEW vw_BookingDetails AS
SELECT
    b.BookingId,
    b.ScheduledDate,
    b.Address,
    b.Status,
    b.ServiceName,
    b.CustomerId,
    c.Name AS CustomerName,
    c.Email AS CustomerEmail,
    b.AdminId,
    a.Name AS ProviderName,
    a.Phone AS ProviderPhone,
    p.Amount,
    p.CommissionAmount,
    p.PaymentStatus,
    p.PaymentMethod
FROM Bookings b
JOIN Customers c ON b.CustomerId = c.CustomerId
JOIN Admins a ON b.AdminId = a.AdminId
LEFT JOIN Payments p ON b.BookingId = p.BookingId;
GO

PRINT 'FixHub schema created successfully!';
GO