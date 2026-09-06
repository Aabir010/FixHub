using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmProviderProfile : Form
    {
        private readonly int _customerId;
        private readonly int _adminId;

        public frmProviderProfile(int customerId, int adminId)
        {
            InitializeComponent();
            _customerId = customerId;
            _adminId = adminId;
            LoadProvider();
            LoadReviews();
        }

        private void LoadProvider()
        {
            string sql = @"SELECT a.Name, ISNULL(c.CategoryName, 'Other Services'), a.Price, a.Bio
                           FROM Admins a
                           LEFT JOIN ServiceCategories c ON a.CategoryId = c.CategoryId
                           WHERE a.AdminId = @adminId";
            try
            {
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
                                string categoryText = reader.GetString(1);
                                decimal price = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                                lblBio.Text = reader.IsDBNull(3) ? "No biography provided." : reader.GetString(3);

                                double avgRating = GetAverageRating();
                                int reviewCount = GetReviewCount();
                                lblMeta.Text = reviewCount == 0
                                    ? $"{categoryText}  ·  No reviews yet  ·  from ৳{price:N0}"
                                    : $"{categoryText}  ·  ★ {avgRating:F1} ({reviewCount} reviews)  ·  from ৳{price:N0}";

                                string initials = "";
                                string name = lblName.Text;
                                if (!string.IsNullOrWhiteSpace(name))
                                {
                                    var parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    initials = parts.Length >= 2 ? $"{parts[0][0]}{parts[1][0]}" : name.Substring(0, Math.Min(2, name.Length));
                                    initials = initials.ToUpper();
                                }
                                avatar.Initials = initials;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadProvider error: {ex.Message}");
            }
        }

        private int GetReviewCount()
        {
            string sql = @"SELECT COUNT(*)
                           FROM Reviews r
                           JOIN Bookings b ON r.BookingId = b.BookingId
                           WHERE b.AdminId = @adminId";
            object result = DbHelper.ExecuteScalar(sql, new SqlParameter("@adminId", _adminId));
            return result != null ? Convert.ToInt32(result) : 0;
        }

        private double GetAverageRating()
        {
            string sql = @"SELECT ISNULL(AVG(r.Rating * 1.0), 0)
                           FROM Reviews r
                           JOIN Bookings b ON r.BookingId = b.BookingId
                           WHERE b.AdminId = @adminId";
            return DbHelper.ExecuteScalar<double>(sql, new SqlParameter("@adminId", _adminId));
        }

        private void LoadReviews()
        {
            flowReviews.Controls.Clear();

            string sql = @"SELECT r.Rating, r.Comment, c.Name
                           FROM Reviews r
                           JOIN Bookings b ON r.BookingId = b.BookingId
                           JOIN Customers c ON r.CustomerId = c.CustomerId
                           WHERE b.AdminId = @adminId
                           ORDER BY r.ReviewId DESC";
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@adminId", _adminId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int stars = reader.GetInt32(0);
                                string comment = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                string reviewer = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                AddReviewRow(stars, comment, reviewer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadReviews error: {ex.Message}");
            }
        }

        private void AddReviewRow(int stars, string comment, string reviewerName)
        {
            var panel = new Panel { Size = new Size(750, 56), Margin = new Padding(0, 0, 0, 6) };
            var lblStars = new Label { Text = new string('★', Math.Max(1, Math.Min(5, stars))), ForeColor = Color.Orange, Font = new Font("Segoe UI Semibold", 9.5F), AutoSize = true, Location = new Point(0, 0) };
            panel.Controls.Add(lblStars);
            var lblText = new Label { Text = $"\"{comment}\" — {reviewerName}", ForeColor = FixHubColors.TextMuted, Font = new Font("Segoe UI", 9F), AutoSize = false, Size = new Size(750, 34), Location = new Point(0, 18) };
            panel.Controls.Add(lblText);
            var divider = new Panel { BackColor = FixHubColors.BorderGray, Size = new Size(750, 1), Location = new Point(0, 54) };
            panel.Controls.Add(divider);
            flowReviews.Controls.Add(panel);
        }

        private void btnBookNow_Click(object sender, EventArgs e)
        {
            using (var book = new frmBookAndPay(_customerId, _adminId))
            {
                if (UIHelper.ShowModal(this, book) == DialogResult.OK)
                {
                    this.Close();
                }
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            // FIXED: Closes window instantly, returning control to parent list view instead of orphaning thread loops
            this.Close();
        }

        private void frmProviderProfile_Load(object sender, EventArgs e)
        {

        }
    }
}
