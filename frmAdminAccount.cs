using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmAdminAccount : Form
    {
        private readonly int _adminId;

        public frmAdminAccount(int adminId)
        {
            InitializeComponent();
            _adminId = adminId;
            LoadAccount();
        }

        private void LoadAccount()
        {
            string sql = "SELECT Phone FROM Admins WHERE AdminId = @adminId";
            string phone = DbHelper.ExecuteScalar<string>(sql, new SqlParameter("@adminId", _adminId));
            txtPhone.Text = phone ?? "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            string phone = txtPhone.Text.Trim();
            string current = txtCurrentPassword.Text;
            string newPass = txtNewPassword.Text;

            if (string.IsNullOrEmpty(phone))
            {
                lblError.ForeColor = System.Drawing.Color.Crimson;
                lblError.Text = "Phone number can't be empty.";
                return;
            }

            try
            {
                DbHelper.ExecuteNonQuery(
                    "UPDATE Admins SET Phone = @phone WHERE AdminId = @adminId",
                    new SqlParameter("@phone", phone),
                    new SqlParameter("@adminId", _adminId));

                if (!string.IsNullOrEmpty(newPass))
                {
                    if (string.IsNullOrEmpty(current))
                    {
                        lblError.ForeColor = System.Drawing.Color.Crimson;
                        lblError.Text = "Enter your current password to change it.";
                        return;
                    }

                    string storedHash = DbHelper.ExecuteScalar<string>(
                        "SELECT PasswordHash FROM Admins WHERE AdminId = @adminId",
                        new SqlParameter("@adminId", _adminId));

                    if (string.IsNullOrEmpty(storedHash) || !PasswordHasher.VerifyPassword(current, storedHash))
                    {
                        lblError.ForeColor = System.Drawing.Color.Crimson;
                        lblError.Text = "Current password is incorrect.";
                        return;
                    }

                    string newHash = PasswordHasher.HashPassword(newPass);
                    DbHelper.ExecuteNonQuery(
                        "UPDATE Admins SET PasswordHash = @hash WHERE AdminId = @adminId",
                        new SqlParameter("@hash", newHash),
                        new SqlParameter("@adminId", _adminId));
                }

                lblError.ForeColor = System.Drawing.Color.SeaGreen;
                lblError.Text = "Account updated.";
                txtCurrentPassword.Text = "";
                txtNewPassword.Text = "";
            }
            catch (Exception ex)
            {
                lblError.ForeColor = System.Drawing.Color.Crimson;
                lblError.Text = "Update failed: " + ex.Message;
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}