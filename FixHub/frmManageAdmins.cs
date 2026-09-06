using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public class AdminManageItem
    {
        public int AdminId;
        public string Name;
        public string Email;
        public string Phone;
        public string Category;
        public decimal Rating;
        public int JobsCompleted;
        public bool IsApproved;
        public bool IsSuspended;

        public string Status => !IsApproved ? "Pending approval" : (IsSuspended ? "Suspended" : "Active");
        public string ActionText => !IsApproved ? "Approve" : (IsSuspended ? "Reactivate" : "Suspend");
        public string RejectText => !IsApproved ? "Reject" : "";
    }

    public partial class frmManageAdmins : Form
    {
        private List<AdminManageItem> _allAdmins = new List<AdminManageItem>();

        public frmManageAdmins()
        {
            InitializeComponent();

            cmbFilter.SelectedIndexChanged += (s, e) => ApplyFilter();
            txtSearch.InnerTextBox.TextChanged += (s, e) => ApplyFilter();

            LoadAdmins();
        }

        private void LoadAdmins()
        {
            _allAdmins = new List<AdminManageItem>();

            string sql = @"
                SELECT 
                    a.AdminId, 
                    a.Name, 
                    ISNULL(a.Email, '') AS Email,
                    ISNULL(a.Phone, '') AS Phone,
                    ISNULL(c.CategoryName, 'Unassigned') AS CategoryName,
                    ISNULL((
                        SELECT AVG(r.Rating * 1.0) 
                        FROM Reviews r 
                        JOIN Bookings b ON r.BookingId = b.BookingId 
                        WHERE b.AdminId = a.AdminId
                    ), 0) AS Rating,
                    (SELECT COUNT(*) FROM Bookings b WHERE b.AdminId = a.AdminId AND b.Status = 'Completed') AS JobsCompleted,
                    ISNULL(a.IsApproved, 0) AS IsApproved,
                    ISNULL(a.IsSuspended, 0) AS IsSuspended
                FROM Admins a
                LEFT JOIN ServiceCategories c ON a.CategoryId = c.CategoryId
                ORDER BY a.IsApproved ASC, a.Name;";

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
                            _allAdmins.Add(new AdminManageItem
                            {
                                AdminId = Convert.ToInt32(reader[0]),
                                Name = reader[1]?.ToString() ?? "",
                                Email = reader[2]?.ToString() ?? "",
                                Phone = reader[3]?.ToString() ?? "",
                                Category = reader[4]?.ToString() ?? "Unassigned",
                                Rating = reader.IsDBNull(5) ? 0m : Convert.ToDecimal(reader[5]),
                                JobsCompleted = reader.IsDBNull(6) ? 0 : Convert.ToInt32(reader[6]),
                                IsApproved = !reader.IsDBNull(7) && Convert.ToBoolean(reader[7]),
                                IsSuspended = !reader.IsDBNull(8) && Convert.ToBoolean(reader[8])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load providers: " + ex.Message, "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"LoadAdmins error: {ex.Message}");
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allAdmins == null) return;

            string keyword = txtSearch.Text.Trim().ToLower();
            string filter = cmbFilter.SelectedItem?.ToString() ?? "All Providers";

            var filtered = _allAdmins.Where(a =>
                (string.IsNullOrEmpty(keyword) ||
                 a.Name.ToLower().Contains(keyword) ||
                 a.Email.ToLower().Contains(keyword) ||
                 a.Category.ToLower().Contains(keyword) ||
                 a.Phone.Contains(keyword))
            );

            if (filter == "Pending Approval")
                filtered = filtered.Where(a => !a.IsApproved);
            else if (filter == "Active")
                filtered = filtered.Where(a => a.IsApproved && !a.IsSuspended);
            else if (filter == "Suspended")
                filtered = filtered.Where(a => a.IsApproved && a.IsSuspended);

            var list = filtered.ToList();

            grid.Rows.Clear();
            foreach (var a in list)
            {
                string ratingText = a.Rating > 0 ? $"★ {a.Rating:0.0}" : "★ New";
                grid.Rows.Add(a.Name, a.Email, a.Category, ratingText, a.JobsCompleted, a.Status, a.ActionText, a.RejectText, a.AdminId);
            }
        }

        private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (grid.Columns[e.ColumnIndex].Name == "colStatus" && e.Value != null)
            {
                Color bg, fg;
                string statusText = e.Value.ToString();
                if (statusText == "Pending approval")
                    statusText = "Pending";
                else if (statusText == "Active")
                    statusText = "Completed";
                else if (statusText == "Suspended")
                    statusText = "Cancelled";

                UIHelper.ColorsForStatus(statusText, out bg, out fg);
                e.CellStyle.ForeColor = fg;
                e.CellStyle.BackColor = bg;
                e.CellStyle.Font = new Font("Segoe UI Semibold", 8.5F);
            }
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int adminId = Convert.ToInt32(grid.Rows[e.RowIndex].Cells["colAdminId"].Value);
            string status = grid.Rows[e.RowIndex].Cells["colStatus"].Value?.ToString() ?? "";
            string name = grid.Rows[e.RowIndex].Cells["colName"].Value?.ToString() ?? "";
            string colName = grid.Columns[e.ColumnIndex].Name;

            // Handle Primary Action (Approve / Suspend / Reactivate)
            if (colName == "colAction")
            {
                if (status == "Pending approval")
                {
                    var confirm = MessageBox.Show($"Approve provider \"{name}\"?\nThey will be able to log in and accept customer bookings.",
                        "Approve Provider", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirm != DialogResult.Yes) return;

                    DbHelper.ExecuteNonQuery("UPDATE Admins SET IsApproved = 1, IsSuspended = 0 WHERE AdminId = @id",
                        new SqlParameter("@id", adminId));

                    MessageBox.Show($"\"{name}\" has been approved.", "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (status == "Active")
                {
                    var confirm = MessageBox.Show($"Suspend provider \"{name}\"?\nThey will not appear in search results or receive new bookings.",
                        "Confirm Suspension", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirm != DialogResult.Yes) return;

                    DbHelper.ExecuteNonQuery("UPDATE Admins SET IsSuspended = 1 WHERE AdminId = @id",
                        new SqlParameter("@id", adminId));

                    MessageBox.Show($"\"{name}\" has been suspended.", "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (status == "Suspended")
                {
                    var confirm = MessageBox.Show($"Reactivate provider \"{name}\"?",
                        "Reactivate Provider", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirm != DialogResult.Yes) return;

                    DbHelper.ExecuteNonQuery("UPDATE Admins SET IsSuspended = 0 WHERE AdminId = @id",
                        new SqlParameter("@id", adminId));

                    MessageBox.Show($"\"{name}\" has been reactivated.", "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadAdmins();
            }
            // Handle Reject Action for pending applicants
            else if (colName == "colReject")
            {
                if (status == "Pending approval")
                {
                    var confirm = MessageBox.Show($"Reject and remove application for \"{name}\"?",
                        "Reject Application", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirm != DialogResult.Yes) return;

                    try
                    {
                        DbHelper.ExecuteNonQuery("DELETE FROM Admins WHERE AdminId = @id AND IsApproved = 0",
                            new SqlParameter("@id", adminId));

                        MessageBox.Show($"Application for \"{name}\" was rejected.", "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to remove application: " + ex.Message, "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    LoadAdmins();
                }
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
