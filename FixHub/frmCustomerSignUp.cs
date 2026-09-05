using System;
using System.Windows.Forms;
using FixHub.Helpers;
using System.Data.SqlClient;

namespace FixHub.Forms
{
    public partial class frmCustomerSignUp : Form
    {
        public frmCustomerSignUp()
        {
            InitializeComponent();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string password = txtPassword.Text;
            string confirm = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address) ||
                string.IsNullOrEmpty(password))
            {
                lblError.Text = "Please fill in every field.";
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                lblError.Text = "Enter a valid email address.";
                return;
            }

            if (password != confirm)
            {
                lblError.Text = "Passwords do not match.";
                return;
            }

            if (password.Length < 6)
            {
                lblError.Text = "Password must be at least 6 characters.";
                return;
            }

            try
            {
                // Check if email already exists
                string checkSql = "SELECT TOP 1 CustomerId FROM Customers WHERE Email = @email";
                var existing = DbHelper.ExecuteScalar(checkSql, new SqlParameter("@email", email));
                if (existing != null)
                {
                    lblError.Text = "An account with this email already exists.";
                    return;
                }

                // Hash the password and insert
                string hashedPassword = PasswordHasher.HashPassword(password);
                string insertSql = @"INSERT INTO Customers (Name, Email, Phone, Address, PasswordHash)
                                    VALUES (@name, @email, @phone, @address, @passwordHash);
                                    SELECT SCOPE_IDENTITY();";

                int customerId = DbHelper.ExecuteScalar<int>(insertSql,
                    new SqlParameter("@name", name),
                    new SqlParameter("@email", email),
                    new SqlParameter("@phone", phone),
                    new SqlParameter("@address", address),
                    new SqlParameter("@passwordHash", hashedPassword));

                MessageBox.Show("Account created! You can now log in.", "FixHub",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                var login = new frmLogin();
                login.FormClosed += (s, args) => this.Close();
                login.Show();
            }
            catch (Exception ex)
            {
                lblError.Text = "Registration failed. Please try again.";
                System.Diagnostics.Debug.WriteLine($"Signup error: {ex.Message}");
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            var login = new frmLogin();
            login.FormClosed += (s, args) => this.Close();
            login.Show();
        }
    }
}
