using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmAdminDashboard : Form
    {
        private readonly int _adminId;
        private Timer _refreshTimer;

        public frmAdminDashboard(int adminId)
        {
            InitializeComponent();
            _adminId = adminId;
            LoadStats();

            navRequests.Click += (s, e) => OpenRequests();
            navProfile.Click += (s, e) => OpenProfile();
            navAccount.Click += (s, e) => OpenAccount();
            navLogout.Click += (s, e) => Logout();

            // Background live polling every 10 seconds for real-time notification
            _refreshTimer = new Timer { Interval = 10000 };
            _refreshTimer.Tick += (s, e) => LoadStats();
            _refreshTimer.Start();
            this.FormClosed += (s, e) => { _refreshTimer?.Stop(); _refreshTimer?.Dispose(); };
        }

        private void LoadStats()
        {
            try
            {
                // Pending count
                string pendingSql = @"SELECT COUNT(*) FROM Bookings WHERE AdminId = @adminId AND Status = 'Pending'";
                object pendingObj = DbHelper.ExecuteScalar(pendingSql, new SqlParameter("@adminId", _adminId));
                int pendingCount = pendingObj != null ? Convert.ToInt32(pendingObj) : 0;
                lblPendingValue.Text = pendingCount.ToString();
                navRequests.Text = pendingCount > 0 ? $"Requests ({pendingCount} new)" : "Requests";

                // Net earnings from completed jobs
                string monthSql = @"
                    SELECT ISNULL(SUM(p.Amount - p.CommissionAmount), 0)
                    FROM Payments p
                    JOIN Bookings b ON p.BookingId = b.BookingId
                    WHERE b.AdminId = @adminId
                      AND b.Status = 'Completed'";
                object monthObj = DbHelper.ExecuteScalar(monthSql, new SqlParameter("@adminId", _adminId));
                decimal monthEarnings = monthObj != null && monthObj != DBNull.Value ? Convert.ToDecimal(monthObj) : 0m;
                lblMonthValue.Text = "৳" + monthEarnings.ToString("N0");

                // Average rating computed from reviews
                string ratingSql = @"
                    SELECT ISNULL(AVG(r.Rating * 1.0), 0)
                    FROM Reviews r
                    JOIN Bookings b ON r.BookingId = b.BookingId
                    WHERE b.AdminId = @adminId";
                object ratingObj = DbHelper.ExecuteScalar(ratingSql, new SqlParameter("@adminId", _adminId));
                double avgRating = ratingObj != null && ratingObj != DBNull.Value ? Convert.ToDouble(ratingObj) : 0.0;
                lblRatingValue.Text = avgRating > 0 ? "★" + avgRating.ToString("0.0") : "★ New";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadStats error: {ex.Message}");
            }

            LoadEarningsGrid();
        }

        private void LoadEarningsGrid()
        {
            string sql = @"
                SELECT 
                    b.ScheduledDate, 
                    ISNULL(c.Name, 'Customer') AS CustomerName, 
                    ISNULL(p.Amount, 0) AS Amount, 
                    ISNULL(p.CommissionAmount, 0) AS CommissionAmount,
                    ISNULL(p.Amount - p.CommissionAmount, 0) AS NetAmount,
                    b.Status
                FROM Bookings b
                LEFT JOIN Payments p ON b.BookingId = p.BookingId
                JOIN Customers c ON b.CustomerId = c.CustomerId
                WHERE b.AdminId = @adminId
                ORDER BY b.ScheduledDate DESC";

            gridEarnings.Rows.Clear();
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
                                string date = reader.IsDBNull(0) ? "-" : Convert.ToDateTime(reader[0]).ToString("dd MMM");
                                string customer = reader[1]?.ToString() ?? "Customer";
                                decimal amount = reader.IsDBNull(2) ? 0m : Convert.ToDecimal(reader[2]);
                                decimal commission = reader.IsDBNull(3) ? 0m : Convert.ToDecimal(reader[3]);
                                decimal net = reader.IsDBNull(4) ? 0m : Convert.ToDecimal(reader[4]);
                                string status = reader.IsDBNull(5) ? "" : reader[5].ToString();

                                string payoutText;
                                if (status == "Completed")
                                    payoutText = "৳" + net.ToString("N0");
                                else if (status == "Cancelled" || status == "Declined")
                                    payoutText = status;
                                else
                                    payoutText = "৳" + net.ToString("N0") + " (" + status + ")";

                                gridEarnings.Rows.Add(
                                    date, 
                                    customer, 
                                    "৳" + amount.ToString("N0"), 
                                    "৳" + commission.ToString("N0"), 
                                    payoutText);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadEarningsGrid error: {ex.Message}");
            }
        }

        private void tabOverview_Click(object sender, EventArgs e)
        {
            tabOverview.Font = new Font("Segoe UI Semibold", 9.5F);
            tabOverview.ForeColor = FixHubColors.CoralDark;
            tabOverview.BackColor = FixHubColors.CoralLight;
            tabEarnings.Font = new Font("Segoe UI", 9.5F);
            tabEarnings.ForeColor = FixHubColors.TextMuted;
            tabEarnings.BackColor = Color.Transparent;

            cardPending.Visible = cardMonth.Visible = cardRating.Visible = true;
            gridEarnings.Visible = false;
        }

        private void tabEarnings_Click(object sender, EventArgs e)
        {
            tabEarnings.Font = new Font("Segoe UI Semibold", 9.5F);
            tabEarnings.ForeColor = FixHubColors.CoralDark;
            tabEarnings.BackColor = FixHubColors.CoralLight;
            tabOverview.Font = new Font("Segoe UI", 9.5F);
            tabOverview.ForeColor = FixHubColors.TextMuted;
            tabOverview.BackColor = Color.Transparent;

            cardPending.Visible = cardMonth.Visible = cardRating.Visible = false;
            LoadEarningsGrid();
            gridEarnings.Visible = true;
        }

        private void OpenRequests()
        {
            var requests = new frmRequests(_adminId);
            UIHelper.NavigateTo(this, requests, () => LoadStats());
        }

        private void OpenProfile()
        {
            using (var profile = new frmAdminProfile(_adminId))
            {
                UIHelper.ShowModal(this, profile);
            }
            LoadStats();
        }

        private void OpenAccount()
        {
            using (var account = new frmAdminAccount(_adminId))
            {
                UIHelper.ShowModal(this, account);
            }
            LoadStats();
        }

        private void Logout()
        {
            this.Hide();
            var login = new frmLogin();
            login.FormClosed += (s, args) => this.Close();
            login.Show();
        }
    }
}