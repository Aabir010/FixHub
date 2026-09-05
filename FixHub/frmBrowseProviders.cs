using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public class ProviderListItem
    {
        public int AdminId;
        public string Name;
        public string Category;
        public double Rating;
        public decimal StartingPrice;
    }

    public partial class frmBrowseProviders : Form
    {
        private readonly int _customerId;
        private List<ProviderListItem> _allProviders;

        public frmBrowseProviders(int customerId, string presetCategory)
        {
            InitializeComponent();
            _customerId = customerId;
            LoadProviders();

            if (!string.IsNullOrEmpty(presetCategory) && cmbCategory.Items.Contains(presetCategory))
                cmbCategory.SelectedItem = presetCategory;

            // Wire reactive filtering events
            cmbCategory.SelectedIndexChanged += (s, e) => ApplyFilter();
            cmbSort.SelectedIndexChanged += (s, e) => ApplyFilter();
            cmbPrice.SelectedIndexChanged += (s, e) => ApplyFilter();
            cmbRating.SelectedIndexChanged += (s, e) => ApplyFilter();
            txtSearch.InnerTextBox.TextChanged += (s, e) => ApplyFilter();

            ApplyFilter();
        }

        private void LoadProviders()
        {
            _allProviders = new List<ProviderListItem>();

            try
            {
                string sql = @"SELECT a.AdminId, a.Name, ISNULL(c.CategoryName, 'Other'), a.Price,
                                      ISNULL((
                                          SELECT AVG(r.Rating * 1.0)
                                          FROM Reviews r
                                          JOIN Bookings b ON r.BookingId = b.BookingId
                                          WHERE b.AdminId = a.AdminId
                                      ), 0) AS Rating
                              FROM Admins a
                              LEFT JOIN ServiceCategories c ON a.CategoryId = c.CategoryId
                              WHERE a.IsApproved = 1 AND a.IsAvailable = 1 AND a.IsSuspended = 0";

                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _allProviders.Add(new ProviderListItem
                            {
                                AdminId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Category = reader.GetString(2),
                                StartingPrice = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                                Rating = reader.IsDBNull(4) ? 0.0 : Convert.ToDouble(reader.GetValue(4))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadProviders error: {ex.Message}");
            }

            LoadCategories();
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All");

            try
            {
                string sql = "SELECT CategoryName FROM ServiceCategories ORDER BY CategoryName";
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            cmbCategory.Items.Add(reader.GetString(0));
                    }
                }
            }
            catch
            {
                // Fallback if DB read fails
                foreach (var cat in new[] { "Plumbing", "Electrical", "Cleaning", "AC Repair" })
                    cmbCategory.Items.Add(cat);
            }

            cmbCategory.SelectedIndex = 0;
        }

        private void ApplyFilter()
        {
            if (_allProviders == null) return;

            string keyword = txtSearch.Text.Trim().ToLower();
            string category = cmbCategory.SelectedItem?.ToString() ?? "All";
            string priceFilter = cmbPrice.SelectedItem?.ToString() ?? "Any Price";
            string ratingFilter = cmbRating.SelectedItem?.ToString() ?? "Any Rating";
            string sortOrder = cmbSort.SelectedItem?.ToString() ?? "Recommended";

            var filtered = _allProviders.Where(p =>
                (category == "All" || p.Category == category) &&
                (string.IsNullOrEmpty(keyword) || p.Name.ToLower().Contains(keyword))
            );

            // Budget filter
            if (priceFilter == "Under ৳500")
                filtered = filtered.Where(p => p.StartingPrice <= 500);
            else if (priceFilter == "Under ৳1,000")
                filtered = filtered.Where(p => p.StartingPrice <= 1000);
            else if (priceFilter == "Under ৳2,000")
                filtered = filtered.Where(p => p.StartingPrice <= 2000);
            else if (priceFilter == "Under ৳5,000")
                filtered = filtered.Where(p => p.StartingPrice <= 5000);

            // Rating filter
            if (ratingFilter == "4.0+ Stars")
                filtered = filtered.Where(p => p.Rating >= 4.0);
            else if (ratingFilter == "4.5+ Stars")
                filtered = filtered.Where(p => p.Rating >= 4.5);

            // Sorting
            List<ProviderListItem> results;
            if (sortOrder == "Price: Low to High")
                results = filtered.OrderBy(p => p.StartingPrice).ToList();
            else if (sortOrder == "Price: High to Low")
                results = filtered.OrderByDescending(p => p.StartingPrice).ToList();
            else if (sortOrder == "Rating: High to Low")
                results = filtered.OrderByDescending(p => p.Rating).ThenBy(p => p.StartingPrice).ToList();
            else
                results = filtered.ToList();

            flowProviders.Controls.Clear();
            lblNoResults.Visible = results.Count == 0;

            foreach (var provider in results)
                flowProviders.Controls.Add(BuildProviderRow(provider));
        }

        private RoundedPanel BuildProviderRow(ProviderListItem provider)
        {
            var row = new RoundedPanel
            {
                Size = new Size(790, 66),
                Margin = new Padding(0, 0, 0, 12)
            };

            var avatar = new AvatarBadge
            {
                Initials = InitialsFrom(provider.Name),
                BadgeColor = FixHubColors.TealMid,
                TextColor = FixHubColors.TealDark,
                Location = new Point(14, 11)
            };
            row.Controls.Add(avatar);

            var lblName = new Label
            {
                Text = provider.Name,
                Font = new Font("Segoe UI Semibold", 10F),
                ForeColor = FixHubColors.TextDark,
                AutoSize = true,
                Location = new Point(70, 12)
            };
            row.Controls.Add(lblName);

            string ratingText = provider.Rating > 0 ? $"★ {provider.Rating:0.0}" : "★ New";
            var lblMeta = new Label
            {
                Text = $"{ratingText}  ·  from ৳{provider.StartingPrice:N0}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = FixHubColors.TextMuted,
                AutoSize = true,
                Location = new Point(70, 34)
            };
            row.Controls.Add(lblMeta);

            var btnView = new Button
            {
                Text = "View Profile",
                Size = new Size(110, 32),
                Location = new Point(660, 17),
                Cursor = Cursors.Hand
            };
            btnView.Click += (s, e) => OpenProfile(provider.AdminId);
            row.Controls.Add(btnView);

            return row;
        }

        private string InitialsFrom(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "P";
            var parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpper();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpper();
        }

        private void OpenProfile(int adminId)
        {
            var profileForm = new frmProviderProfile(_customerId, adminId);
            UIHelper.NavigateTo(this, profileForm, () => { LoadProviders(); ApplyFilter(); });
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }
    }
}
