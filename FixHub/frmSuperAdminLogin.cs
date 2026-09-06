using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmSuperAdminLogin : Form
    {
        public frmSuperAdminLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Enter your username and password.";
                return;
            }

            var authResult = Authenticate(username, password);
            if (!authResult.success)
            {
                lblError.Text = authResult.errorMessage;
                return;
            }

            this.Hide();
            var dash = new frmSuperAdminDashboard(authResult.superAdminId);
            dash.FormClosed += (s, args) => this.Close();
            dash.Show();
        }

        private (bool success, int superAdminId, string errorMessage) Authenticate(string username, string password)
        {
            try
            {
                string sql = "SELECT SuperAdminId, PasswordHash FROM SuperAdmins WHERE Username = @username";
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                return (false, 0, "Incorrect username or password.");

                            int superAdminId = reader.GetInt32(0);
                            string storedHash = reader.GetString(1);

                            if (!PasswordHasher.VerifyPassword(password, storedHash))
                                return (false, 0, "Incorrect username or password.");

                            return (true, superAdminId, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SuperAdmin login error: {ex.Message}");
                return (false, 0, "Unable to connect to the database.");
            }
        }
    }
}
