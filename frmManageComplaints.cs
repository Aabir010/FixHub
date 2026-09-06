using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public class ComplaintItem
    {
        public int ComplaintId;
        public int BookingId;
        public string CustomerName;
        public string ProviderName;
        public string Description;
        public int ProviderAdminId;
    }

    public partial class frmManageComplaints : Form
    {
        private readonly int _superAdminId;
        private List<ComplaintItem> _complaints;

        public frmManageComplaints(int superAdminId)
        {
            _superAdminId = superAdminId;
            InitializeComponent();
            LoadComplaints();
        }

        private void LoadComplaints()
        {
            _complaints = new List<ComplaintItem>();

            string sql = @"
SELECT c.ComplaintId, c.BookingId, cu.Name AS CustomerName, a.Name AS ProviderName,
       c.Description, b.AdminId
FROM Complaints c
JOIN Bookings b ON c.BookingId = b.BookingId
JOIN Customers cu ON b.CustomerId = cu.CustomerId
JOIN Admins a ON b.AdminId = a.AdminId
WHERE c.Status = 'Open'
ORDER BY c.CreatedAt DESC";

            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _complaints.Add(new ComplaintItem
                        {
                            ComplaintId = reader.GetInt32(0),
                            BookingId = reader.GetInt32(1),
                            CustomerName = reader.GetString(2),
                            ProviderName = reader.GetString(3),
                            Description = reader.GetString(4),
                            ProviderAdminId = reader.GetInt32(5)
                        });
                    }
                }
            }

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            grid.Rows.Clear();
            foreach (var c in _complaints)
                grid.Rows.Add($"#{c.BookingId}", c.CustomerName, c.ProviderName, c.Description, "Resolve", c.ComplaintId, c.ProviderAdminId);
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || grid.Columns[e.ColumnIndex].Name != "colResolve") return;

            var complaint = _complaints[e.RowIndex];
            string notes = InputBoxHelper.Show(
                $"Resolution notes for complaint on booking #{complaint.BookingId}:", "Resolve complaint", "");

            if (string.IsNullOrWhiteSpace(notes)) return;

            DbHelper.ExecuteNonQuery(@"
UPDATE Complaints
SET Status = 'Resolved', ResolutionNotes = @notes,
    ResolvedBySuperAdminId = @sid, ResolvedAt = GETDATE()
WHERE ComplaintId = @id",
                new SqlParameter("@notes", notes.Trim()),
                new SqlParameter("@sid", _superAdminId),
                new SqlParameter("@id", complaint.ComplaintId));

            var suspend = MessageBox.Show(
                $"Complaint resolved.\n\nSuspend provider \"{complaint.ProviderName}\"?",
                "FixHub", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (suspend == DialogResult.Yes)
            {
                DbHelper.ExecuteNonQuery(
                    "UPDATE Admins SET IsSuspended = 1 WHERE AdminId = @id",
                    new SqlParameter("@id", complaint.ProviderAdminId));
            }

            LoadComplaints();
            MessageBox.Show("Complaint marked as resolved.", "FixHub",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
