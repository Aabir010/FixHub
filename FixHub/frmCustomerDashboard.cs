using System;
using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmCustomerDashboard : Form
    {
        private readonly int _customerId;
        private int _currentActiveBookingId = 0;

        public frmCustomerDashboard(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            LoadCustomerProfile();
            LoadActiveBooking();
            LoadCategories();

            navBrowse.Click += (s, e) => OpenBrowseProviders(null);
            navBookings.Click += (s, e) => OpenMyBookings();
            navAccount.Click += (s, e) => OpenAccount();
            navLogout.Click += (s, e) => Logout();
        }

        private void LoadCustomerProfile()
        {
            try
            {
                object nameObj = DbHelper.ExecuteScalar(
                    "SELECT Name FROM Customers WHERE CustomerId = @id",
                    new System.Data.SqlClient.SqlParameter("@id", _customerId));
                if (nameObj != null && nameObj != DBNull.Value)
                {
                    string fullName = nameObj.ToString().Trim();
                    string firstName = fullName.Split(' ')[0];
                    lblWelcome.Text = $"Welcome back, {firstName}";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadCustomerProfile error: {ex.Message}");
            }
        }

        private void LoadActiveBooking()
        {
            try
            {
                string sql = @"
                    SELECT TOP 1 b.BookingId, b.ServiceName, b.ScheduledDate, b.Status, a.Name AS ProviderName
                    FROM Bookings b
                    JOIN Admins a ON b.AdminId = a.AdminId
                    WHERE b.CustomerId = @customerId
                      AND b.Status IN ('Pending', 'Accepted', 'In Progress')
                    ORDER BY b.BookingId DESC";

                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", _customerId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _currentActiveBookingId = reader.GetInt32(0);
                                string service = reader.GetString(1);
                                DateTime dt = reader.GetDateTime(2);
                                string status = reader.GetString(3);
                                string providerName = reader.GetString(4);

                                lblActiveTitle.Text = "Active Service Request";
                                lblActiveDetails.Text = $"{service} with {providerName}  ·  {dt:dd MMM, yyyy}";
                                lblActiveStatus.Visible = true;
                                lblActiveStatus.Text = status;
                                Color bg, fg;
                                UIHelper.ColorsForStatus(status, out bg, out fg);
                                UIHelper.MakeStatusPill(lblActiveStatus, bg, fg);

                                btnQuickViewBooking.Text = "View Details";
                                btnQuickViewBooking.Click -= QuickAction_Click;
                                btnQuickViewBooking.Click += QuickAction_Click;
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadActiveBooking error: {ex.Message}");
            }

            // Fallback when no active bookings exist
            _currentActiveBookingId = 0;
            lblActiveTitle.Text = "No Active Bookings";
            lblActiveStatus.Visible = false;
            lblActiveDetails.Text = "Ready to schedule a service? Browse verified professionals below.";
            btnQuickViewBooking.Text = "Browse";
            btnQuickViewBooking.Click -= QuickAction_Click;
            btnQuickViewBooking.Click += QuickAction_Click;
        }

        private void QuickAction_Click(object sender, EventArgs e)
        {
            if (_currentActiveBookingId > 0)
            {
                using (var details = new frmBookingDetails(_customerId, _currentActiveBookingId))
                {
                    UIHelper.ShowModal(this, details);
                }
                LoadActiveBooking();
            }
            else
            {
                OpenBrowseProviders(null);
            }
        }

        private void LoadCategories()
        {
            flowCategories.Controls.Clear();

            try
            {
                var categories = new System.Collections.Generic.List<string>();
                string sql = "SELECT CategoryName FROM ServiceCategories ORDER BY CategoryName";
                using (var conn = FixHub.Helpers.DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            categories.Add(reader.GetString(0));
                    }
                }

                // Fallback if table is empty
                if (categories.Count == 0)
                    categories.AddRange(new[] { "Plumbing", "Electrical", "Cleaning", "AC Repair" });

                for (int i = 0; i < categories.Count; i++)
                {
                    var tile = new CategoryTile { CategoryText = categories[i] };
                    tile.SetTheme(i % 2 == 0 ? FixHubColors.TealLight : FixHubColors.CoralLight,
                                  i % 2 == 0 ? FixHubColors.TealDark : FixHubColors.CoralDark);
                    tile.Margin = new Padding(0, 0, 16, 16);

                    string category = categories[i];
                    tile.Click += (s, e) => OpenBrowseProviders(category);

                    flowCategories.Controls.Add(tile);
                }
            }
            catch
            {
                // Fallback on DB error
                var fallback = new[] { "Plumbing", "Electrical", "Cleaning", "AC Repair" };
                for (int i = 0; i < fallback.Length; i++)
                {
                    var tile = new CategoryTile { CategoryText = fallback[i] };
                    tile.SetTheme(i % 2 == 0 ? FixHubColors.TealLight : FixHubColors.CoralLight,
                                  i % 2 == 0 ? FixHubColors.TealDark : FixHubColors.CoralDark);
                    tile.Margin = new Padding(0, 0, 16, 16);
                    string category = fallback[i];
                    tile.Click += (s, e) => OpenBrowseProviders(category);
                    flowCategories.Controls.Add(tile);
                }
            }
        }

        private void OpenBrowseProviders(string presetCategory)
        {
            var browse = new frmBrowseProviders(_customerId, presetCategory);
            UIHelper.NavigateTo(this, browse, () => { LoadCustomerProfile(); LoadActiveBooking(); });
        }

        private void OpenMyBookings()
        {
            var bookings = new frmMyBookings(_customerId);
            UIHelper.NavigateTo(this, bookings, () => { LoadCustomerProfile(); LoadActiveBooking(); });
        }

        private void OpenAccount()
        {
            using (var account = new frmCustomerAccount(_customerId))
            {
                UIHelper.ShowModal(this, account);
            }
            LoadCustomerProfile();
            LoadActiveBooking();
        }

        private void Logout()
        {
            this.Hide();
            var login = new frmLogin();
            login.FormClosed += (s, args) => this.Close(); // Safely close app domain context on literal user logout
            login.Show();
        }
    }
}
