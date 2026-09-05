using System;
using System.Windows.Forms;
using FixHub.Helpers;
using System.Drawing;
using System.Data.SqlClient;

namespace FixHub.Forms
{
    public partial class frmLeaveReview : Form
    {
        private readonly int _customerId;
        private readonly int _bookingId;
        private int _selectedStars = 0;
        private int _adminId = 0;
        private string _providerName = "";

        public frmLeaveReview(int customerId, int bookingId)
        {
            InitializeComponent();
            _customerId = customerId;
            _bookingId = bookingId;
            LoadProviderName();
        }

        private void LoadProviderName()
        {
            try
            {
                string sql = @"SELECT a.AdminId, a.Name FROM Bookings b JOIN Admins a ON b.AdminId = a.AdminId WHERE b.BookingId = @bookingId";
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@bookingId", _bookingId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _adminId = reader.GetInt32(0);
                                _providerName = reader.GetString(1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadProviderName error: {ex.Message}");
            }
            lblHeading.Text = string.IsNullOrEmpty(_providerName) ? "Rate Provider" : $"Rate {_providerName}";
        }

        private void star_Click(object sender, EventArgs e)
        {
            _selectedStars = (int)((Label)sender).Tag;
            for (int i = 0; i < starLabels.Length; i++)
                starLabels[i].ForeColor = i < _selectedStars ? Color.Orange : FixHubColors.BorderGray;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_selectedStars == 0)
            {
                lblError.Text = "Please select a star rating.";
                return;
            }

            try
            {
                string sql = @"INSERT INTO Reviews (BookingId, CustomerId, Rating, Comment) 
                               VALUES (@bookingId, @customerId, @rating, @comment)";
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@bookingId", _bookingId);
                        cmd.Parameters.AddWithValue("@customerId", _customerId);
                        cmd.Parameters.AddWithValue("@rating", _selectedStars);
                        cmd.Parameters.AddWithValue("@comment", txtComment.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }

                    // Recalculate and update provider's rating
                    if (_adminId > 0)
                    {
                        try
                        {
                            string updateRatingSql = @"
                                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Admins') AND name = 'Rating')
                                BEGIN
                                    UPDATE Admins 
                                    SET Rating = ISNULL((
                                        SELECT AVG(r.Rating * 1.0) 
                                        FROM Reviews r 
                                        JOIN Bookings b ON r.BookingId = b.BookingId 
                                        WHERE b.AdminId = @adminId
                                    ), 0)
                                    WHERE AdminId = @adminId;
                                END";

                            using (var cmd = new SqlCommand(updateRatingSql, conn))
                            {
                                cmd.Parameters.AddWithValue("@adminId", _adminId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Update provider rating note: {ex.Message}");
                        }
                    }
                }

                MessageBox.Show("Thanks for your feedback!", "FixHub",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to submit review: " + ex.Message, "FixHub",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
