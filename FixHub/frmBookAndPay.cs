using FixHub.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FixHub.Forms
{
    public partial class frmBookAndPay : Form
    {
        private readonly int _customerId;
        private readonly int _adminId;
        private decimal _basePrice = 0m;
        private decimal _discountPercent = 0m;
        private string _serviceName = "";

        public frmBookAndPay(int customerId, int adminId)
        {
            InitializeComponent();
            _customerId = customerId;
            _adminId = adminId;
            LoadProvider();
            LoadCustomerAddress();
            UpdateTotal();
        }

        private void LoadCustomerAddress()
        {
            try
            {
                // 1. Try customer's profile address
                object addrObj = DbHelper.ExecuteScalar(
                    "SELECT Address FROM Customers WHERE CustomerId = @cId",
                    new SqlParameter("@cId", _customerId));
                string address = addrObj != null && addrObj != DBNull.Value ? addrObj.ToString().Trim() : "";

                // 2. If profile address is empty, look up the customer's most recent booking address
                if (string.IsNullOrEmpty(address))
                {
                    object recentObj = DbHelper.ExecuteScalar(
                        "SELECT TOP 1 Address FROM Bookings WHERE CustomerId = @cId AND Address IS NOT NULL AND Address <> '' ORDER BY BookingId DESC",
                        new SqlParameter("@cId", _customerId));
                    if (recentObj != null && recentObj != DBNull.Value)
                        address = recentObj.ToString().Trim();
                }

                if (!string.IsNullOrEmpty(address))
                {
                    txtAddress.InnerTextBox.Text = address;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadCustomerAddress error: {ex.Message}");
            }
        }

        private void LoadProvider()
        {
            try
            {
                string sql = @"SELECT a.Name, ISNULL(c.CategoryName, 'Other'), a.Price
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
                                string name = reader.GetString(0);
                                string categoryText = reader.GetString(1);
                                _basePrice = reader.IsDBNull(2) ? 0m : reader.GetDecimal(2);

                                _serviceName = categoryText;
                                lblHeading.Text = $"Book {name} — {categoryText}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblHeading.Text = "Book Service";
                System.Diagnostics.Debug.WriteLine($"LoadProvider error: {ex.Message}");
            }
        }

        private void btnApplyCoupon_Click(object sender, EventArgs e)
        {
            string code = txtCoupon.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(code))
            {
                _discountPercent = 0m;
                lblError.Text = "";
                UpdateTotal();
                return;
            }

            try
            {
                // Optional coupon table lookups — handled with high integrity catch logic
                string sql = @"SELECT DiscountPercent, ExpiryDate, UsageLimit, UsedCount, IsActive
                              FROM Coupons WHERE Code = @code";
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@code", code);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                _discountPercent = 0m;
                                lblError.ForeColor = System.Drawing.Color.Firebrick;
                                lblError.Text = "Invalid coupon code.";
                            }
                            else
                            {
                                bool isActive = reader.GetBoolean(4);
                                DateTime? expiry = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1);
                                int usageLimit = reader.GetInt32(2);
                                int usedCount = reader.GetInt32(3);

                                if (!isActive || (expiry.HasValue && expiry.Value < DateTime.Today) || usedCount >= usageLimit)
                                {
                                    _discountPercent = 0m;
                                    lblError.ForeColor = System.Drawing.Color.Firebrick;
                                    lblError.Text = "Coupon expired or invalid.";
                                }
                                else
                                {
                                    _discountPercent = reader.GetDecimal(0);
                                    lblError.ForeColor = System.Drawing.Color.SeaGreen;
                                    lblError.Text = $"Coupon applied - {_discountPercent:0}% off.";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _discountPercent = 0m;
                lblError.ForeColor = System.Drawing.Color.Firebrick;
                lblError.Text = "Coupon table unavailable. Passing standard value.";
                System.Diagnostics.Debug.WriteLine($"Coupon table trace error: {ex.Message}");
            }
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = _basePrice - (_basePrice * _discountPercent / 100m);
            lblTotalAmount.Text = $"৳{total:0}";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            lblError.ForeColor = System.Drawing.Color.Firebrick;

            string addressInput = txtAddress.Text.Trim();

            if (string.IsNullOrEmpty(addressInput))
            {
                lblError.Text = "Please fill in an address.";
                return;
            }

            // Pulls the pre-validated date value directly from the calendar layout dropdown
            DateTime scheduledDate = dtpScheduledDate.Value;

            decimal finalAmount = _basePrice - (_basePrice * _discountPercent / 100m);

            try
            {
                // 1. Resolve UI selections into clean string variables early
                string paymentMethodText = radPayNow.Checked ? "PayNow" : "PayAfter";
                string paymentStatusText = radPayNow.Checked ? "Paid" : "Unpaid";

                // 2. Insert booking record (REVERTED: Removed PaymentMethod column from this table)
                string bookingSql = @"INSERT INTO Bookings (CustomerId, AdminId, ServiceName, ScheduledDate, Address, Status)
                              VALUES (@customerId, @adminId, @serviceName, @scheduledDate, @address, 'Pending');
                              SELECT SCOPE_IDENTITY();";

                object bookingScalarResult = DbHelper.ExecuteScalar(bookingSql,
                    new SqlParameter("@customerId", _customerId),
                    new SqlParameter("@adminId", _adminId),
                    new SqlParameter("@serviceName", string.IsNullOrEmpty(_serviceName) ? "General Service" : _serviceName),
                    new SqlParameter("@scheduledDate", scheduledDate),
                    new SqlParameter("@address", addressInput));

                int bookingId = Convert.ToInt32(bookingScalarResult);

                // 3. Insert payment details ledger records safely (This is where PaymentMethod lives!)
                decimal commissionRate = 0.15m;
                decimal commission = _basePrice * commissionRate;

                string paymentSql = @"INSERT INTO Payments (BookingId, Amount, CommissionAmount, PaymentStatus, PaymentMethod)
                              VALUES (@bookingId, @amount, @commission, @status, @method)";

                SqlParameter[] paymentParams = new SqlParameter[]
                {
            new SqlParameter("@bookingId", SqlDbType.Int) { Value = bookingId },
            new SqlParameter("@amount", SqlDbType.Decimal) { Value = finalAmount },
            new SqlParameter("@commission", SqlDbType.Decimal) { Value = commission },
            new SqlParameter("@status", SqlDbType.VarChar) { Value = paymentStatusText },
            new SqlParameter("@method", SqlDbType.VarChar) { Value = paymentMethodText }
                };

                DbHelper.ExecuteNonQuery(paymentSql, paymentParams);

                // 4. Increment coupon usage if a valid discount was applied
                string couponCode = txtCoupon.Text.Trim().ToUpper();
                if (_discountPercent > 0 && !string.IsNullOrEmpty(couponCode))
                {
                    try
                    {
                        DbHelper.ExecuteNonQuery("UPDATE Coupons SET UsedCount = UsedCount + 1 WHERE Code = @code",
                            new SqlParameter("@code", couponCode));
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to increment coupon count: {ex.Message}");
                    }
                }

                // 5. Save customer address for future auto-fill if not already set
                try
                {
                    DbHelper.ExecuteNonQuery(
                        "UPDATE Customers SET Address = @addr WHERE CustomerId = @cId AND (Address IS NULL OR Address = '')",
                        new SqlParameter("@addr", addressInput),
                        new SqlParameter("@cId", _customerId));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to auto-save customer address: {ex.Message}");
                }

                MessageBox.Show("Booking confirmed successfully!", "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                lblError.ForeColor = System.Drawing.Color.Firebrick;
                lblError.Text = "Something went wrong while confirming your booking. Please try again.";
                System.Diagnostics.Debug.WriteLine($"Booking Insertion Failure Trace: {ex.Message}");
            }
        }




        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
