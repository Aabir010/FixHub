using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;
using System.Data.SqlClient;

namespace FixHub.Forms
{
    public class BookingListItem
    {
        public int BookingId;
        public string Date;
        public string ProviderName;
        public string ServiceName;
        public string Status; // Pending, Accepted, In Progress, Completed
    }

    public partial class frmMyBookings : Form
    {
        private readonly int _customerId;
        private List<BookingListItem> _bookings;

        public frmMyBookings(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            grid.CellFormatting += grid_CellFormatting;
            LoadBookings();
        }

        private void LoadBookings()
        {
            _bookings = new List<BookingListItem>();

            try
            {
                string sql = @"SELECT b.BookingId, b.ScheduledDate, a.Name AS ProviderName,
                                     ISNULL(b.ServiceName, c.CategoryName) AS ServiceName, b.Status
                              FROM Bookings b
                              JOIN Admins a ON b.AdminId = a.AdminId
                              LEFT JOIN ServiceCategories c ON a.CategoryId = c.CategoryId
                              WHERE b.CustomerId = @customerId
                              ORDER BY b.ScheduledDate DESC";

                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", _customerId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _bookings.Add(new BookingListItem
                                {
                                    BookingId = reader.GetInt32(0),
                                    Date = reader.GetDateTime(1).ToString("dd MMM"),
                                    ProviderName = reader.GetString(2),
                                    ServiceName = reader.GetString(3),
                                    Status = reader.GetString(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadBookings error: {ex.Message}");
            }

            grid.Rows.Clear();
            foreach (var b in _bookings)
                grid.Rows.Add(b.Date, b.ProviderName, b.ServiceName, b.Status, "View");
        }

        // Colors the Status column's text to match the booking's state,
        // giving a lightweight pill-like effect without owner-drawing the whole grid.
        private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (grid.Columns[e.ColumnIndex].Name != "colStatus" || e.Value == null) return;

            Color bg, fg;
            UIHelper.ColorsForStatus(e.Value.ToString(), out bg, out fg);
            e.CellStyle.ForeColor = fg;
            e.CellStyle.BackColor = bg;
            e.CellStyle.Font = new Font("Segoe UI Semibold", 8.5F);
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (grid.Columns[e.ColumnIndex].Name != "colView") return;

            int bookingId = _bookings[e.RowIndex].BookingId;
            using (var details = new frmBookingDetails(_customerId, bookingId))
            {
                UIHelper.ShowModal(this, details);
            }
            LoadBookings();
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
