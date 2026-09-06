using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmRequests : Form
    {
        private readonly int _adminId;

        public frmRequests(int adminId)
        {
            InitializeComponent();
            _adminId = adminId;
            grid.CellFormatting += grid_CellFormatting;
            LoadRequests();
        }

        private void LoadRequests()
        {
            string sql = @"
                SELECT b.BookingId, c.Name AS CustomerName, b.ServiceName,
                       FORMAT(b.ScheduledDate, 'dd MMM, hhtt') AS DateTime, b.Status
                FROM Bookings b
                JOIN Customers c ON b.CustomerId = c.CustomerId
                WHERE b.AdminId = @adminId AND b.Status NOT IN ('Completed', 'Cancelled', 'Declined')
                ORDER BY b.ScheduledDate";

            grid.Rows.Clear();
            using (var reader = DbHelper.ExecuteReader(sql, new SqlParameter("@adminId", _adminId)))
            {
                while (reader.Read())
                {
                    int bookingId = reader.GetInt32(0);
                    string customer = reader.GetString(1);
                    string service = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    string dt = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    string status = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    string declineText = status == "Pending" ? "Decline" : "";
                    grid.Rows.Add(customer, service, dt, status, ActionTextFor(status), declineText, bookingId);
                }
            }
        }

        private static string ActionTextFor(string status)
        {
            switch (status)
            {
                case "Pending": return "Accept";
                case "Accepted": return "Start job";
                case "In Progress": return "Mark completed";
                default: return "";
            }
        }

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
            int bookingId = (int)grid.Rows[e.RowIndex].Cells["colBookingId"].Value;
            string currentStatus = grid.Rows[e.RowIndex].Cells["colStatus"].Value.ToString();
            string customer = grid.Rows[e.RowIndex].Cells["colCustomer"].Value.ToString();

            // Handle Decline
            if (grid.Columns[e.ColumnIndex].Name == "colDecline")
            {
                if (currentStatus != "Pending") return;

                var confirm = MessageBox.Show($"Decline request from {customer}?", "Decline Request",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    DbHelper.ExecuteNonQuery("UPDATE Bookings SET Status = 'Declined' WHERE BookingId = @id",
                        new SqlParameter("@id", bookingId));
                    LoadRequests();
                }
                return;
            }

            // Handle Progress / Accept / Start / Complete
            if (grid.Columns[e.ColumnIndex].Name == "colAction")
            {
                string newStatus = "";
                switch (currentStatus)
                {
                    case "Pending": newStatus = "Accepted"; break;
                    case "Accepted": newStatus = "In Progress"; break;
                    case "In Progress": newStatus = "Completed"; break;
                }

                if (string.IsNullOrEmpty(newStatus)) return;

                DbHelper.ExecuteNonQuery("UPDATE Bookings SET Status = @status WHERE BookingId = @id",
                    new SqlParameter("@status", newStatus), new SqlParameter("@id", bookingId));

                if (newStatus == "Completed")
                    MessageBox.Show("Nice work! This job has been marked complete.", "FixHub",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadRequests();
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}