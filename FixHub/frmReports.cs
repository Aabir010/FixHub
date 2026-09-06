using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmReports : Form
    {
        public frmReports()
        {
            InitializeComponent();
            LoadReport();
        }

        private void LoadReport()
        {
            string sql = @"
                SELECT 
                    ISNULL(c.CategoryName, 'Other') AS CategoryName, 
                    ISNULL(SUM(p.CommissionAmount), 0) AS TotalCommission
                FROM Bookings b
                JOIN Payments p ON b.BookingId = p.BookingId
                JOIN Admins a ON b.AdminId = a.AdminId
                LEFT JOIN ServiceCategories c ON a.CategoryId = c.CategoryId
                WHERE b.Status = 'Completed'
                GROUP BY c.CategoryName
                ORDER BY TotalCommission DESC;";

            grid.Rows.Clear();
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string category = reader.GetString(0);
                            decimal commission = reader.GetDecimal(1);
                            grid.Rows.Add(category, "৳" + commission.ToString("N0"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load reports: {ex.Message}");
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
