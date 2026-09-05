using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmCustomerAccount : Form
    {
        private readonly int _customerId;

        public frmCustomerAccount(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            LoadProfile();
        }

        private void LoadProfile()
        {
            try
            {
                string sql = @"SELECT Name, Email, Phone, Address FROM Customers WHERE CustomerId = @customerId";
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", _customerId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader.GetString(0);
                                string email = reader.GetString(1);
                                string phone = reader.GetString(2);
                                string address = reader.GetString(3);

                                lblName.Text = name;
                                lblEmail.Text = email;
                                txtPhone.Text = phone;
                                txtAddress.Text = address;
                                avatar.Initials = GetInitials(name);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Failed to load profile: " + ex.Message;
            }
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";
            var parts = name.Trim().Split(' ');
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpper();
            return (parts[0][0].ToString() + parts[parts.Length - 1][0].ToString()).ToUpper();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPhone.Text) || string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                lblError.Text = "Phone and address can't be empty.";
                return;
            }

            try
            {
                string sql = @"UPDATE Customers SET Phone = @phone, Address = @address WHERE CustomerId = @customerId";
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@customerId", _customerId);
                        cmd.ExecuteNonQuery();
                    }
                }
                lblError.ForeColor = System.Drawing.Color.SeaGreen;
                lblError.Text = "Changes saved.";
            }
            catch (Exception ex)
            {
                lblError.ForeColor = System.Drawing.Color.Crimson;
                lblError.Text = "Failed to save: " + ex.Message;
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            string current = InputBoxHelper.Show("Current password:", "Change password", "");
            if (string.IsNullOrEmpty(current)) return;
            string newPass = InputBoxHelper.Show("New password:", "Change password", "");
            if (string.IsNullOrEmpty(newPass)) return;

            try
            {
                // Verify the current password
                string sql = @"SELECT PasswordHash FROM Customers WHERE CustomerId = @customerId";
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", _customerId);
                        string storedHash = (string)cmd.ExecuteScalar();
                        if (storedHash == null || !PasswordHasher.VerifyPassword(current, storedHash))
                        {
                            lblError.ForeColor = System.Drawing.Color.Crimson;
                            lblError.Text = "Current password is incorrect.";
                            return;
                        }
                    }
                }

                string newHash = PasswordHasher.HashPassword(newPass);
                sql = @"UPDATE Customers SET PasswordHash = @newHash WHERE CustomerId = @customerId";
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@newHash", newHash);
                        cmd.Parameters.AddWithValue("@customerId", _customerId);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Password updated.", "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to change password: " + ex.Message, "FixHub",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
