using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmSuperAdminDashboard : Form
    {
        private readonly int _superAdminId;

        public frmSuperAdminDashboard(int superAdminId)
        {
            _superAdminId = superAdminId;
            InitializeComponent();
            LoadStats();

            navManageAdmins.Click += (s, e) => OpenManageAdmins();
            navCategories.Click += (s, e) => OpenCategories();
            navComplaints.Click += (s, e) => OpenComplaints();
            navReports.Click += (s, e) => OpenReports();
            navLogout.Click += (s, e) => Logout();

            cardPending.Click += (s, e) => OpenManageAdmins();
            cardAdmins.Click += (s, e) => OpenManageAdmins();
            cardOpenComplaints.Click += (s, e) => OpenComplaints();
            cardCommission.Click += (s, e) => OpenReports();
        }

        private void LoadStats()
        {
            try
            {
                int pendingApprovals = Convert.ToInt32(
                    DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Admins WHERE IsApproved = 0"));
                int openComplaints = Convert.ToInt32(
                    DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Complaints WHERE Status = 'Open'"));
                int activeProviders = Convert.ToInt32(
                    DbHelper.ExecuteScalar("SELECT COUNT(*) FROM Admins WHERE IsApproved = 1 AND IsSuspended = 0"));

                lblPendingValue.Text = pendingApprovals.ToString("N0");
                lblOpenComplaintsValue.Text = openComplaints.ToString("N0");
                lblAdminsValue.Text = activeProviders.ToString("N0");

                // 1. Check completed bookings this month
                object commissionMonthObj = DbHelper.ExecuteScalar(@"
SELECT ISNULL(SUM(p.CommissionAmount), 0)
FROM Payments p
JOIN Bookings b ON p.BookingId = b.BookingId
WHERE b.Status = 'Completed'
  AND MONTH(b.ScheduledDate) = MONTH(GETDATE())
  AND YEAR(b.ScheduledDate) = YEAR(GETDATE())");
                decimal monthCommission = commissionMonthObj != null && commissionMonthObj != DBNull.Value
                    ? Convert.ToDecimal(commissionMonthObj) : 0;

                if (monthCommission > 0)
                {
                    lblCommissionValue.Text = $"৳{monthCommission:N0}";
                }
                else
                {
                    // 2. Check completed bookings all-time
                    object allTimeCompletedObj = DbHelper.ExecuteScalar(@"
SELECT ISNULL(SUM(p.CommissionAmount), 0)
FROM Payments p
JOIN Bookings b ON p.BookingId = b.BookingId
WHERE b.Status = 'Completed'");
                    decimal allTimeCompleted = allTimeCompletedObj != null && allTimeCompletedObj != DBNull.Value
                        ? Convert.ToDecimal(allTimeCompletedObj) : 0;

                    if (allTimeCompleted > 0)
                    {
                        lblCommissionValue.Text = $"৳{allTimeCompleted:N0} (all time)";
                    }
                    else
                    {
                        // 3. Check all paid commissions across all active bookings (not cancelled/declined)
                        object allPaidObj = DbHelper.ExecuteScalar(@"
SELECT ISNULL(SUM(p.CommissionAmount), 0)
FROM Payments p
JOIN Bookings b ON p.BookingId = b.BookingId
WHERE b.Status NOT IN ('Cancelled', 'Declined')");
                        decimal allPaid = allPaidObj != null && allPaidObj != DBNull.Value
                            ? Convert.ToDecimal(allPaidObj) : 0;

                        lblCommissionValue.Text = allPaid > 0
                            ? $"৳{allPaid:N0} (pending)"
                            : "৳0";
                    }
                }

                navManageAdmins.Text = pendingApprovals > 0
                    ? $"Manage Admins ({pendingApprovals} pending)"
                    : "Manage Admins";
                navComplaints.Text = openComplaints > 0
                    ? $"Complaints ({openComplaints} open)"
                    : "Complaints";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load dashboard statistics: {ex.Message}");
                lblPendingValue.Text = "0";
                lblOpenComplaintsValue.Text = "0";
                lblAdminsValue.Text = "0";
                lblCommissionValue.Text = "৳0";
            }
        }

        private void OpenManageAdmins()
        {
            var form = new frmManageAdmins();
            UIHelper.NavigateTo(this, form, () => LoadStats());
        }

        private void OpenCategories()
        {
            var form = new frmManageCategories();
            UIHelper.NavigateTo(this, form);
        }

        private void OpenComplaints()
        {
            var form = new frmManageComplaints(_superAdminId);
            UIHelper.NavigateTo(this, form, () => LoadStats());
        }

        private void OpenReports()
        {
            var form = new frmReports();
            UIHelper.NavigateTo(this, form);
        }

        private void Logout()
        {
            this.Hide();
            var login = new frmSuperAdminLogin();
            login.FormClosed += (s, args) => this.Close();
            login.Show();
        }
    }
}
