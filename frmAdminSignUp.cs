using System;
using System.Windows.Forms;
using FixHub.Helpers;
using System.Data.SqlClient;

namespace FixHub.Forms
{
    public partial class frmAdminSignUp : Form
    {
        public frmAdminSignUp()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
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
                foreach (var cat in new[] { "Plumbing", "Electrical", "Cleaning", "AC Repair" })
                    cmbCategory.Items.Add(cat);
            }

            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string category = cmbCategory.SelectedItem?.ToString();
            string price = txtPrice.Text.Trim();
            string password = txtPassword.Text;
            string confirm = txtConfirmPassword.Text;
            string bio = txtBio.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
                category == null || string.IsNullOrEmpty(price) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Please fill in every required field.";
                return;
            }

            if (!decimal.TryParse(price, out decimal priceValue) || priceValue <= 0)
            {
                lblError.Text = "Enter a valid starting price.";
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
                string checkSql = "SELECT TOP 1 AdminId FROM Admins WHERE Email = @email";
                var existing = DbHelper.ExecuteScalar(checkSql, new SqlParameter("@email", email));
                if (existing != null)
                {
                    lblError.Text = "An account with this email already exists.";
                    return;
                }

                // Get CategoryId for the selected category
                string catSql = "SELECT CategoryId FROM ServiceCategories WHERE CategoryName = @category";
                int categoryId = DbHelper.ExecuteScalar<int>(catSql,
                    new SqlParameter("@category", category));

                // Hash the password and insert
                string hashedPassword = PasswordHasher.HashPassword(password);
                string insertSql = @"INSERT INTO Admins (Name, Email, Phone, CategoryId, Price, Bio, PasswordHash, IsApproved)
                                    VALUES (@name, @email, @phone, @categoryId, @price, @bio, @passwordHash, 0);
                                    SELECT SCOPE_IDENTITY();";

                int adminId = DbHelper.ExecuteScalar<int>(insertSql,
                    new SqlParameter("@name", name),
                    new SqlParameter("@email", email),
                    new SqlParameter("@phone", string.IsNullOrEmpty(phone) ? (object)DBNull.Value : phone),
                    new SqlParameter("@categoryId", categoryId),
                    new SqlParameter("@price", priceValue),
                    new SqlParameter("@bio", string.IsNullOrEmpty(bio) ? (object)DBNull.Value : bio),
                    new SqlParameter("@passwordHash", hashedPassword));

                MessageBox.Show("Application submitted! An admin will review your profile before it goes live.",
                    "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                var login = new frmLogin();
                login.FormClosed += (s, args) => this.Close();
                login.Show();
            }
            catch (Exception ex)
            {
                lblError.Text = "Registration failed. Please try again.";
                System.Diagnostics.Debug.WriteLine($"Admin signup error: {ex.Message}");
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
