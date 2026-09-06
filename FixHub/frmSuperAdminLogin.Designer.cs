using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmSuperAdminLogin
    {
        private System.ComponentModel.IContainer components = null;

        private RoundedPanel pnlCard;
        private LogoBadge logoBadge;
        private Label lblBrand;
        private Label lblUsername;
        private RoundedTextBox txtUsername;
        private Label lblPassword;
        private RoundedTextBox txtPassword;
        private RoundedButton btnLogin;
        private Label lblError;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlCard = new RoundedPanel();
            this.logoBadge = new LogoBadge();
            this.lblBrand = new Label();
            this.lblUsername = new Label();
            this.txtUsername = new RoundedTextBox();
            this.lblPassword = new Label();
            this.txtPassword = new RoundedTextBox();
            this.btnLogin = new RoundedButton();
            this.lblError = new Label();

            this.SuspendLayout();

            // ---- Form ----
            this.Text = "FixHub - SuperAdmin";
            this.ClientSize = new Size(420, 440);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            // ---- Card ----
            this.pnlCard.BorderColor = FixHubColors.BorderGray;
            this.pnlCard.CornerRadius = 14;
            this.pnlCard.Location = new Point(30, 30);
            this.pnlCard.Size = new Size(360, 380);

            this.logoBadge.BadgeColor = FixHubColors.PurpleDark;
            this.logoBadge.IconColor = FixHubColors.PurpleMid;
            this.logoBadge.Location = new Point(148, 30);
            this.pnlCard.Controls.Add(this.logoBadge);

            this.lblBrand.Text = "FixHub SuperAdmin";
            this.lblBrand.Font = new Font("Segoe UI Semibold", 12F);
            this.lblBrand.ForeColor = FixHubColors.TextDark;
            this.lblBrand.AutoSize = false;
            this.lblBrand.TextAlign = ContentAlignment.MiddleCenter;
            this.lblBrand.Size = new Size(360, 24);
            this.lblBrand.Location = new Point(0, 104);
            this.pnlCard.Controls.Add(this.lblBrand);

            this.lblUsername.Text = "Username";
            this.lblUsername.ForeColor = FixHubColors.TextMuted;
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new Point(30, 156);
            this.pnlCard.Controls.Add(this.lblUsername);

            this.txtUsername.Location = new Point(30, 178);
            this.txtUsername.Size = new Size(300, 36);
            this.pnlCard.Controls.Add(this.txtUsername);

            this.lblPassword.Text = "Password";
            this.lblPassword.ForeColor = FixHubColors.TextMuted;
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new Point(30, 228);
            this.pnlCard.Controls.Add(this.lblPassword);

            this.txtPassword.Location = new Point(30, 250);
            this.txtPassword.Size = new Size(300, 36);
            this.txtPassword.SetAsPassword();
            this.pnlCard.Controls.Add(this.txtPassword);

            this.lblError.Text = "";
            this.lblError.ForeColor = Color.Firebrick;
            this.lblError.AutoSize = true;
            this.lblError.Location = new Point(30, 292);
            this.lblError.Size = new Size(300, 18);
            this.pnlCard.Controls.Add(this.lblError);

            this.btnLogin.Text = "Login";
            this.btnLogin.BackColor = FixHubColors.PurpleDark;
            this.btnLogin.Location = new Point(30, 316);
            this.btnLogin.Size = new Size(300, 40);
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.pnlCard.Controls.Add(this.btnLogin);

            this.Controls.Add(this.pnlCard);

            this.ResumeLayout(false);
        }
    }
}
