using System;
using System.Windows.Forms;
using FixHub.Helpers;
using System.Data.SqlClient;

namespace FixHub.Forms
{
    public partial class frmBookingDetails : Form
    {
        private readonly int _customerId;
        private readonly int _bookingId;
        private string _status;

        public frmBookingDetails(int customerId, int bookingId)
        {
            InitializeComponent();
            _customerId = customerId;
            _bookingId = bookingId;
            LoadBooking();
        }

        private void LoadBooking()
        {
            try
            {
                string sql = @"SELECT b.ServiceName, a.Name AS AdminName, b.ScheduledDate,
                                      b.Address, b.Status, p.Amount
                                  FROM Bookings b
                                  JOIN Admins a ON b.AdminId = a.AdminId
                                  LEFT JOIN Payments p ON b.BookingId = p.BookingId
                                  WHERE b.BookingId = @bookingId";
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
                                lblService.Text = reader.GetString(0) + " — " + reader.GetString(1);
                                lblMeta.Text = reader.GetDateTime(2).ToString("dd MMM, h:mm tt") + "  ·  " + reader.GetString(3);
                                lblAmount.Text = "৳" + (reader.IsDBNull(5) ? 0 : reader.GetDecimal(5)).ToString("0");
                                _status = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BookingDetails error: {ex.Message}");
            }

            HighlightCurrentStep();
            btnReview.Enabled = (_status == "Completed");
            if (!btnReview.Enabled) btnReview.BackColor = FixHubColors.BorderGray;
            btnCancelBooking.Visible = (_status == "Pending");
        }

        private void HighlightCurrentStep()
        {
            var steps = new[] { ("Pending", lblStepPending), ("Accepted", lblStepAccepted),
                                 ("In Progress", lblStepProgress), ("Completed", lblStepCompleted) };

            if (_status == "Cancelled" || _status == "Declined")
            {
                foreach (var (_, lbl) in steps)
                {
                    lbl.BackColor = FixHubColors.BorderGray;
                    lbl.ForeColor = FixHubColors.TextMuted;
                }
                lblStepPending.Text = _status;
                lblStepPending.BackColor = _status == "Cancelled" ? FixHubColors.StatusCancelledBg : FixHubColors.StatusDeclinedBg;
                lblStepPending.ForeColor = _status == "Cancelled" ? FixHubColors.StatusCancelledText : FixHubColors.StatusDeclinedText;
                return;
            }

            lblStepPending.Text = "Pending";
            bool reached = true;
            foreach (var (name, lbl) in steps)
            {
                if (reached)
                {
                    lbl.BackColor = FixHubColors.TealLight;
                    lbl.ForeColor = FixHubColors.TealDark;
                }
                else
                {
                    lbl.BackColor = FixHubColors.BorderGray;
                    lbl.ForeColor = FixHubColors.TextMuted;
                }
                if (name == _status) reached = false;
            }
        }

        private void btnCancelBooking_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to cancel this booking?",
                "Cancel Booking", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                string sql = "UPDATE Bookings SET Status = 'Cancelled' WHERE BookingId = @bookingId";
                DbHelper.ExecuteNonQuery(sql, new SqlParameter("@bookingId", _bookingId));

                MessageBox.Show("Booking has been cancelled.", "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadBooking();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cancel booking: " + ex.Message, "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReview_Click(object sender, EventArgs e)
        {
            using (var review = new frmLeaveReview(_customerId, _bookingId))
            {
                UIHelper.ShowModal(this, review);
            }
            LoadBooking();
        }

        private void btnComplaint_Click(object sender, EventArgs e)
        {
            string reason = InputBoxHelper.Show(
                "Describe the issue with this booking:", "File a complaint", "");
            if (string.IsNullOrWhiteSpace(reason)) return;

            try
            {
                string sql = @"INSERT INTO Complaints (BookingId, Description, Status) VALUES (@bookingId, @reason, 'Open')";
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@bookingId", _bookingId);
                        cmd.Parameters.AddWithValue("@reason", reason);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Your complaint has been submitted. Our team will review it.",
                    "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to file complaint: " + ex.Message, "FixHub",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
