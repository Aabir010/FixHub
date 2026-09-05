using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlLeft;
        private LogoBadge logoBadge;
        private Label lblBrand;
        private Label lblTagline;

        private Panel pnlRight;
        private Label lblHeading;
        private Label lblEmail;
        private RoundedTextBox txtEmail;
        private Label lblPassword;
        private RoundedTextBox txtPassword;
        private Label lblLoginAs;
        private RadioButton radCustomer;
        private RadioButton radAdmin;
        private RoundedButton btnLogin;
        private Label lblNoAccount;
        private LinkLabel lnkSignup;
        private Label lblError;
        private LinkLabel lnkSuperAdmin;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.logoBadge = new FixHub.Helpers.LogoBadge();
            this.lblBrand = new System.Windows.Forms.Label();
            this.lblTagline = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lblHeading = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new FixHub.Helpers.RoundedTextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new FixHub.Helpers.RoundedTextBox();
            this.lblLoginAs = new System.Windows.Forms.Label();
            this.radCustomer = new System.Windows.Forms.RadioButton();
            this.radAdmin = new System.Windows.Forms.RadioButton();
            this.lblError = new System.Windows.Forms.Label();
            this.btnLogin = new FixHub.Helpers.RoundedButton();
            this.lblNoAccount = new System.Windows.Forms.Label();
            this.lnkSignup = new System.Windows.Forms.LinkLabel();
            this.lnkSuperAdmin = new System.Windows.Forms.LinkLabel();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(110)))), ((int)(((byte)(86)))));
            this.pnlLeft.Controls.Add(this.logoBadge);
            this.pnlLeft.Controls.Add(this.lblBrand);
            this.pnlLeft.Controls.Add(this.lblTagline);
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(260, 440);
            this.pnlLeft.TabIndex = 1;
            // 
            // logoBadge
            // 
            this.logoBadge.BackColor = System.Drawing.Color.Transparent;
            this.logoBadge.BadgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(110)))), ((int)(((byte)(86)))));
            this.logoBadge.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(202)))), ((int)(((byte)(165)))));
            this.logoBadge.Location = new System.Drawing.Point(98, 150);
            this.logoBadge.Name = "logoBadge";
            this.logoBadge.Size = new System.Drawing.Size(64, 64);
            this.logoBadge.TabIndex = 0;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            this.lblBrand.ForeColor = System.Drawing.Color.White;
            this.lblBrand.Location = new System.Drawing.Point(90, 226);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(104, 37);
            this.lblBrand.TabIndex = 1;
            this.lblBrand.Text = "FixHub";
            // 
            // lblTagline
            // 
            this.lblTagline.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTagline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(245)))), ((int)(((byte)(238)))));
            this.lblTagline.Location = new System.Drawing.Point(20, 270);
            this.lblTagline.Name = "lblTagline";
            this.lblTagline.Size = new System.Drawing.Size(220, 40);
            this.lblTagline.TabIndex = 2;
            this.lblTagline.Text = "Home services,\nbooked in minutes";
            this.lblTagline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Controls.Add(this.lblHeading);
            this.pnlRight.Controls.Add(this.lblEmail);
            this.pnlRight.Controls.Add(this.txtEmail);
            this.pnlRight.Controls.Add(this.lblPassword);
            this.pnlRight.Controls.Add(this.txtPassword);
            this.pnlRight.Controls.Add(this.lblLoginAs);
            this.pnlRight.Controls.Add(this.radCustomer);
            this.pnlRight.Controls.Add(this.radAdmin);
            this.pnlRight.Controls.Add(this.lblError);
            this.pnlRight.Controls.Add(this.btnLogin);
            this.pnlRight.Controls.Add(this.lblNoAccount);
            this.pnlRight.Controls.Add(this.lnkSignup);
            this.pnlRight.Controls.Add(this.lnkSuperAdmin);
            this.pnlRight.Location = new System.Drawing.Point(260, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(440, 440);
            this.pnlRight.TabIndex = 0;
            this.pnlRight.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlRight_Paint);
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblHeading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.lblHeading.Location = new System.Drawing.Point(50, 50);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(260, 32);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Log in to your account";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(94)))), ((int)(((byte)(90)))));
            this.lblEmail.Location = new System.Drawing.Point(50, 100);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(46, 20);
            this.lblEmail.TabIndex = 1;
            this.lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.White;
            this.txtEmail.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(209)))), ((int)(((byte)(199)))));
            this.txtEmail.BorderThickness = 1;
            this.txtEmail.CornerRadius = 8;
            this.txtEmail.Location = new System.Drawing.Point(50, 122);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.txtEmail.ShowBorder = true;
            this.txtEmail.Size = new System.Drawing.Size(340, 36);
            this.txtEmail.TabIndex = 2;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(94)))), ((int)(((byte)(90)))));
            this.lblPassword.Location = new System.Drawing.Point(50, 172);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(70, 20);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(209)))), ((int)(((byte)(199)))));
            this.txtPassword.BorderThickness = 1;
            this.txtPassword.CornerRadius = 8;
            this.txtPassword.Location = new System.Drawing.Point(50, 194);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.txtPassword.ShowBorder = true;
            this.txtPassword.Size = new System.Drawing.Size(340, 36);
            this.txtPassword.TabIndex = 4;
            // 
            // lblLoginAs
            // 
            this.lblLoginAs.AutoSize = true;
            this.lblLoginAs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(94)))), ((int)(((byte)(90)))));
            this.lblLoginAs.Location = new System.Drawing.Point(50, 244);
            this.lblLoginAs.Name = "lblLoginAs";
            this.lblLoginAs.Size = new System.Drawing.Size(64, 20);
            this.lblLoginAs.TabIndex = 5;
            this.lblLoginAs.Text = "Login as";
            // 
            // radCustomer
            // 
            this.radCustomer.AutoSize = true;
            this.radCustomer.Checked = true;
            this.radCustomer.Location = new System.Drawing.Point(50, 266);
            this.radCustomer.Name = "radCustomer";
            this.radCustomer.Size = new System.Drawing.Size(93, 24);
            this.radCustomer.TabIndex = 6;
            this.radCustomer.TabStop = true;
            this.radCustomer.Text = "Customer";
            // 
            // radAdmin
            // 
            this.radAdmin.AutoSize = true;
            this.radAdmin.Location = new System.Drawing.Point(170, 266);
            this.radAdmin.Name = "radAdmin";
            this.radAdmin.Size = new System.Drawing.Size(74, 24);
            this.radAdmin.TabIndex = 7;
            this.radAdmin.Text = "Admin";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Firebrick;
            this.lblError.Location = new System.Drawing.Point(50, 296);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 20);
            this.lblError.TabIndex = 8;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(110)))), ((int)(((byte)(86)))));
            this.btnLogin.CornerRadius = 8;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(50, 320);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(340, 40);
            this.btnLogin.TabIndex = 9;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblNoAccount
            // 
            this.lblNoAccount.AutoSize = true;
            this.lblNoAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(94)))), ((int)(((byte)(90)))));
            this.lblNoAccount.Location = new System.Drawing.Point(160, 374);
            this.lblNoAccount.Name = "lblNoAccount";
            this.lblNoAccount.Size = new System.Drawing.Size(92, 20);
            this.lblNoAccount.TabIndex = 10;
            this.lblNoAccount.Text = "No account?";
            // 
            // lnkSignup
            // 
            this.lnkSignup.AutoSize = true;
            this.lnkSignup.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(110)))), ((int)(((byte)(86)))));
            this.lnkSignup.Location = new System.Drawing.Point(240, 374);
            this.lnkSignup.Name = "lnkSignup";
            this.lnkSignup.Size = new System.Drawing.Size(59, 20);
            this.lnkSignup.TabIndex = 11;
            this.lnkSignup.TabStop = true;
            this.lnkSignup.Text = "Sign up";
            this.lnkSignup.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSignup_LinkClicked);
            // 
            // lnkSuperAdmin
            // 
            this.lnkSuperAdmin.AutoSize = true;
            this.lnkSuperAdmin.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(94)))), ((int)(((byte)(90)))));
            this.lnkSuperAdmin.Location = new System.Drawing.Point(50, 400);
            this.lnkSuperAdmin.Name = "lnkSuperAdmin";
            this.lnkSuperAdmin.Size = new System.Drawing.Size(133, 20);
            this.lnkSuperAdmin.TabIndex = 12;
            this.lnkSuperAdmin.TabStop = true;
            this.lnkSuperAdmin.Text = "Super Admin login";
            this.lnkSuperAdmin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSuperAdmin_LinkClicked);
            // 
            // frmLogin
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(700, 440);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FixHub - Login";
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
