using System;
using System.Data.SqlClient;

namespace FixHub.Helpers
{
    /// <summary>
    /// Ensures SuperAdmin schema and default account exist on startup.
    /// </summary>
    public static class DatabaseInitializer
    {
        public const string DefaultUsername = "superadmin";
        public const string DefaultPassword = "FixHub@2026";

        public static void EnsureSuperAdminReady()
        {
            try
            {
                DbHelper.ExecuteNonQuery(@"
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SuperAdmins')
BEGIN
    CREATE TABLE SuperAdmins (
        SuperAdminId INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(50) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
END");

                DbHelper.ExecuteNonQuery(@"
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Admins') AND name = 'Rating')
    ALTER TABLE Admins ADD Rating DECIMAL(3,2) DEFAULT 0;
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Payments') AND name = 'PaymentMethod')
    ALTER TABLE Payments ADD PaymentMethod NVARCHAR(20);
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Complaints') AND name = 'ResolutionNotes')
    ALTER TABLE Complaints ADD ResolutionNotes NVARCHAR(MAX);
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Complaints') AND name = 'ResolvedBySuperAdminId')
    ALTER TABLE Complaints ADD ResolvedBySuperAdminId INT;
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Complaints') AND name = 'ResolvedAt')
    ALTER TABLE Complaints ADD ResolvedAt DATETIME;");



                DbHelper.ExecuteNonQuery(@"
 
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Bookings_Status')
 
    ALTER TABLE Bookings DROP CONSTRAINT CK_Bookings_Status;
 
ALTER TABLE Bookings ADD CONSTRAINT CK_Bookings_Status
 
    CHECK (Status IN ('Pending', 'Accepted', 'In Progress', 'Completed', 'Cancelled', 'Declined'));");

                if (!DbHelper.Exists("SuperAdmins", "Username = @u", new SqlParameter("@u", DefaultUsername)))
                {
                    string hash = PasswordHasher.HashPassword(DefaultPassword);
                    DbHelper.ExecuteNonQuery(
                        "INSERT INTO SuperAdmins (Username, PasswordHash) VALUES (@u, @h)",
                        new SqlParameter("@u", DefaultUsername),
                        new SqlParameter("@h", hash));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DatabaseInitializer: {ex.Message}");
            }
        }
    }
}
