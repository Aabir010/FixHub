using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmAdminSignUp
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblHeaderText;
        private Label lblBack;
        private RoundedTextBox txtName;
        private RoundedTextBox txtEmail;
        private RoundedTextBox txtPhone;
        private ComboBox cmbCategory;
        private RoundedTextBox txtPrice;
        private TextBox txtBio;
        private RoundedTextBox txtPassword;
        private RoundedTextBox txtConfirmPassword;
        private Label lblError;
        private RoundedButton btnSignUp;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblHeaderText = new Label();
            this.lblBack = new Label();
            this.txtName = new RoundedTextBox();
            this.txtEmail = new RoundedTextBox();
            this.txtPhone = new RoundedTextBox();
            this.cmbCategory = new ComboBox();
            this.txtPrice = new RoundedTextBox();
            this.txtBio = new TextBox();
            this.txtPassword = new RoundedTextBox();
            this.txtConfirmPassword = new RoundedTextBox();
            this.lblError = new Label();
            this.btnSignUp = new RoundedButton();

            this.SuspendLayout();

            this.Text = "FixHub - Join as a provider";
            this.ClientSize = new Size(420, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = Color.White;

            this.pnlHeader.BackColor = FixHubColors.CoralDark;
            this.pnlHeader.Location = new Point(0, 0);
            this.pnlHeader.Size = new Size(420, 56);

            this.lblHeaderText.Text = "Join FixHub as a provider";
            this.lblHeaderText.Font = new Font("Segoe UI Semibold", 11F);
            this.lblHeaderText.ForeColor = Color.White;
            this.lblHeaderText.TextAlign = ContentAlignment.MiddleCenter;
            this.lblHeaderText.Dock = DockStyle.Fill;
            this.pnlHeader.Controls.Add(this.lblHeaderText);

            this.lblBack.Text = "< Back to login";
            this.lblBack.ForeColor = FixHubColors.CoralDark;
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = Cursors.Hand;
            this.lblBack.Location = new Point(30, 66);
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            int y = 96;
            AddField("Full name", this.txtName, ref y);
            AddField("Email / phone", this.txtEmail, ref y);

            AddPlainLabel("Service category", y); y += 20;
            this.cmbCategory.Location = new Point(30, y);
            this.cmbCategory.Size = new Size(360, 32);
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(this.cmbCategory);
            y += 48;

            AddField("Starting price (৳)", this.txtPrice, ref y);

            AddPlainLabel("Bio", y); y += 20;
            this.txtBio.Multiline = true;
            this.txtBio.Location = new Point(30, y);
            this.txtBio.Size = new Size(360, 60);
            this.txtBio.Font = new Font("Segoe UI", 9.5F);
            this.Controls.Add(this.txtBio);
            y += 76;

            AddField("Password", this.txtPassword, ref y);
            this.txtPassword.SetAsPassword();
            AddField("Confirm password", this.txtConfirmPassword, ref y);
            this.txtConfirmPassword.SetAsPassword();

            this.lblError.ForeColor = Color.Firebrick;
            this.lblError.AutoSize = false;
            this.lblError.Size = new Size(360, 18);
            this.lblError.Location = new Point(30, y + 6);
            y += 30;

            this.btnSignUp.Text = "Sign up";
            this.btnSignUp.BackColor = FixHubColors.CoralDark;
            this.btnSignUp.Location = new Point(30, y);
            this.btnSignUp.Size = new Size(360, 42);
            this.btnSignUp.Click += new System.EventHandler(this.btnSignUp_Click);

            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnSignUp);
            this.Controls.Add(this.lblBack);
            this.Controls.Add(this.pnlHeader);

            this.ResumeLayout(false);
        }

        private void AddField(string labelText, RoundedTextBox box, ref int y)
        {
            AddPlainLabel(labelText, y);
            y += 20;
            box.Location = new Point(30, y);
            box.Size = new Size(360, 36);
            this.Controls.Add(box);
            y += 48;
        }

        private void AddPlainLabel(string text, int y)
        {
            var lbl = new Label { Text = text, ForeColor = FixHubColors.TextMuted, AutoSize = true, Location = new Point(30, y) };
            this.Controls.Add(lbl);
        }
    }
}
