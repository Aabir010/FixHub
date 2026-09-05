using System;
using System.Windows.Forms;
using FixHub.Helpers;
using System.Data.SqlClient;

namespace FixHub.Forms
{
    public partial class frmLogin : Form
    {
        private int loggedInUserId;

        public frmLogin()
        {
            InitializeComponent();

            this.txtPassword.SetAsPassword();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Enter your email and password.";
                return;
            }

            bool isCustomer = radCustomer.Checked;

            var authResult = Authenticate(email, password, isCustomer);
            if (!authResult.success)
            {
                lblError.Text = authResult.errorMessage;
                return;
            }

            loggedInUserId = authResult.userId;

            this.Hide();
            if (isCustomer)
            {
                var dash = new frmCustomerDashboard(loggedInUserId);
                dash.FormClosed += (s, args) => this.Close();
                dash.Show();
            }
            else
            {
                var dash = new frmAdminDashboard(loggedInUserId);
                dash.FormClosed += (s, args) => this.Close();
                dash.Show();
            }
        }

        private (bool success, int userId, string errorMessage) Authenticate(string email, string password, bool isCustomer)
        {
            try
            {
                if (isCustomer)
                {
                    // Authenticate customer
                    string sql = @"SELECT CustomerId, PasswordHash FROM Customers WHERE Email = @email";
                    using (var conn = DbHelper.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@email", email);
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    return (false, 0, "Incorrect email or password.");
                                }
                                int customerId = reader.GetInt32(0);
                                string storedHash = reader.GetString(1);
                                reader.Close();

                                if (!PasswordHasher.VerifyPassword(password, storedHash))
                                {
                                    return (false, 0, "Incorrect email or password.");
                                }
                                return (true, customerId, null);
                            }
                        }
                    }
                }
                else
                {
                    // Authenticate admin/provider
                    string sql = @"SELECT AdminId, PasswordHash, IsApproved, IsSuspended
                                   FROM Admins WHERE Email = @email";
                    using (var conn = DbHelper.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@email", email);
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    return (false, 0, "Incorrect email or password.");
                                }
                                int adminId = reader.GetInt32(0);
                                string storedHash = reader.GetString(1);
                                bool isApproved = reader.GetBoolean(2);
                                bool isSuspended = reader.GetBoolean(3);
                                reader.Close();

                                if (!PasswordHasher.VerifyPassword(password, storedHash))
                                {
                                    return (false, 0, "Incorrect email or password.");
                                }

                                if (!isApproved)
                                {
                                    return (false, 0, "Your account is pending approval.");
                                }

                                if (isSuspended)
                                {
                                    return (false, 0, "Your account has been suspended.");
                                }

                                return (true, adminId, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                return (false, 0, "Login failed. Please try again.");
            }
        }

        private void lnkSignup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            if (radCustomer.Checked)
            {
                var signup = new frmCustomerSignUp();
                signup.FormClosed += (s, args) => this.Show();
                signup.Show();
            }
            else
            {
                var signup = new frmAdminSignUp();
                signup.FormClosed += (s, args) => this.Show();
                signup.Show();
            }
        }

        private void lnkSuperAdmin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var superAdminLogin = new frmSuperAdminLogin();
            superAdminLogin.FormClosed += (s, args) => this.Close();
            superAdminLogin.Show();
        }

        private void pnlRight_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
