using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmManageCoupons : Form
    {
        public frmManageCoupons()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim().ToUpper();
            string percentText = txtDiscount.Text.Trim();
            DateTime expiry = dtpExpiry.Value;
            int usageLimit = (int)numLimit.Value;

            if (string.IsNullOrEmpty(code) || !decimal.TryParse(percentText, out decimal percent) || percent <= 0 || percent > 100)
            {
                MessageBox.Show("Please enter a valid coupon code and a discount percentage between 1 and 100.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sql = "INSERT INTO Coupons (Code, DiscountPercent, ExpiryDate, UsageLimit) VALUES (@code, @percent, @expiry, @limit)";
                DbHelper.ExecuteNonQuery(sql,
                    new SqlParameter("@code", code),
                    new SqlParameter("@percent", percent),
                    new SqlParameter("@expiry", expiry),
                    new SqlParameter("@limit", usageLimit));

                MessageBox.Show("Coupon generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate coupon: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
