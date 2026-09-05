using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmCustomerSignUp
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblHeaderText;
        private RoundedTextBox txtName;
        private RoundedTextBox txtEmail;
        private RoundedTextBox txtPhone;
        private RoundedTextBox txtAddress;
        private RoundedTextBox txtPassword;
        private RoundedTextBox txtConfirmPassword;
        private RoundedButton btnSignUp;
        private Label lblError;
        private Label lblBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblHeaderText = new Label();
            this.txtName = new RoundedTextBox();
            this.txtEmail = new RoundedTextBox();
            this.txtPhone = new RoundedTextBox();
            this.txtAddress = new RoundedTextBox();
            this.txtPassword = new RoundedTextBox();
            this.txtConfirmPassword = new RoundedTextBox();
            this.btnSignUp = new RoundedButton();
            this.lblError = new Label();
            this.lblBack = new Label();

            this.SuspendLayout();

            this.Text = "FixHub - Create account";
            this.ClientSize = new Size(420, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = Color.White;

            this.pnlHeader.BackColor = FixHubColors.TealDark;
            this.pnlHeader.Location = new Point(0, 0);
            this.pnlHeader.Size = new Size(420, 56);

            this.lblHeaderText.Text = "Create your FixHub account";
            this.lblHeaderText.Font = new Font("Segoe UI Semibold", 11F);
            this.lblHeaderText.ForeColor = Color.White;
            this.lblHeaderText.TextAlign = ContentAlignment.MiddleCenter;
            this.lblHeaderText.Dock = DockStyle.Fill;
            this.pnlHeader.Controls.Add(this.lblHeaderText);

            this.lblBack.Text = "< Back to login";
            this.lblBack.ForeColor = FixHubColors.TealDark;
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = Cursors.Hand;
            this.lblBack.Location = new Point(30, 66);
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            int y = 96;
            AddField("Full name", this.txtName, ref y);
            AddField("Email", this.txtEmail, ref y);
            AddField("Phone", this.txtPhone, ref y);
            AddField("Address", this.txtAddress, ref y);
            AddField("Password", this.txtPassword, ref y);
            this.txtPassword.SetAsPassword();
            AddField("Confirm password", this.txtConfirmPassword, ref y);
            this.txtConfirmPassword.SetAsPassword();

            this.lblError.Text = "";
            this.lblError.ForeColor = Color.Firebrick;
            this.lblError.AutoSize = false;
            this.lblError.Size = new Size(360, 18);
            this.lblError.Location = new Point(30, y + 6);
            y += 30;

            this.btnSignUp.Text = "Sign up";
            this.btnSignUp.BackColor = FixHubColors.TealDark;
            this.btnSignUp.Location = new Point(30, y);
            this.btnSignUp.Size = new Size(360, 42);
            this.btnSignUp.Click += new System.EventHandler(this.btnSignUp_Click);

            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnSignUp);
            this.Controls.Add(this.lblBack);
            this.Controls.Add(this.pnlHeader);

            this.ResumeLayout(false);
        }

        // Helper used only inside InitializeComponent to lay out a
        // label + rounded textbox pair and advance the running y position.
        private void AddField(string labelText, RoundedTextBox box, ref int y)
        {
            var lbl = new Label
            {
                Text = labelText,
                ForeColor = FixHubColors.TextMuted,
                AutoSize = true,
                Location = new Point(30, y)
            };
            this.Controls.Add(lbl);
            y += 20;

            box.Location = new Point(30, y);
            box.Size = new Size(360, 36);
            this.Controls.Add(box);
            y += 48;
        }
    }
}
