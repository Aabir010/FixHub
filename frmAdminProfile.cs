using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmAdminProfile : Form
    {
        private readonly int _adminId;

        public frmAdminProfile(int adminId)
        {
            InitializeComponent();
            _adminId = adminId;
            LoadProfile();
        }

        private void LoadProfile()
        {
            string sql = @"SELECT a.Name, c.CategoryName, a.Bio, a.Price, a.IsAvailable
                           FROM Admins a
                           LEFT JOIN ServiceCategories c ON a.CategoryId = c.CategoryId
                           WHERE a.AdminId = @adminId";
            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@adminId", _adminId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblName.Text = reader.GetString(0);
                            string category = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            double rating = GetAverageRating();
                            lblMeta.Text = $"★ {rating:F1}  ·  {category}";
                            txtBio.Text = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            txtPrice.Text = reader.IsDBNull(3) ? "0" : reader.GetDecimal(3).ToString("0");
                            chkAvailable.Checked = reader.IsDBNull(4) ? true : reader.GetBoolean(4);
                            string initials = "";
                            string name = lblName.Text;
                            if (!string.IsNullOrWhiteSpace(name))
                            {
                                var parts = name.Split(' ');
                                initials = parts.Length >= 2 ? $"{parts[0][0]}{parts[1][0]}" : name.Substring(0, Math.Min(2, name.Length));
                                initials = initials.ToUpper();
                            }
                            avatar.Initials = initials;
                        }
                    }
                }
            }
        }

        private double GetAverageRating()
        {
            string sql = @"SELECT ISNULL(AVG(r.Rating * 1.0), 0)
                           FROM Reviews r
                           JOIN Bookings b ON r.BookingId = b.BookingId
                           WHERE b.AdminId = @adminId";
            return DbHelper.ExecuteScalar<double>(sql, new SqlParameter("@adminId", _adminId));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price) || price <= 0)
            {
                lblError.Text = "Enter a valid starting price.";
                return;
            }
            string updateSql = @"UPDATE Admins SET Bio = @bio, Price = @price, IsAvailable = @available WHERE AdminId = @adminId";
            DbHelper.ExecuteNonQuery(updateSql,
                new SqlParameter("@bio", txtBio.Text.Trim()),
                new SqlParameter("@price", price),
                new SqlParameter("@available", chkAvailable.Checked),
                new SqlParameter("@adminId", _adminId));
            lblError.ForeColor = System.Drawing.Color.SeaGreen;
            lblError.Text = "Profile updated.";
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}