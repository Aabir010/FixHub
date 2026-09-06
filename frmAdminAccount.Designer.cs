using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmAdminAccount
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private RoundedPanel pnlCard;
        private Label lblHeading;
        private Label lblPhoneCaption;
        private RoundedTextBox txtPhone;
        private Label lblCurrentCaption;
        private RoundedTextBox txtCurrentPassword;
        private Label lblNewCaption;
        private RoundedTextBox txtNewPassword;
        private Label lblError;
        private RoundedButton btnUpdate;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBack = new Label();
            this.pnlCard = new RoundedPanel();
            this.lblHeading = new Label();
            this.lblPhoneCaption = new Label();
            this.txtPhone = new RoundedTextBox();
            this.lblCurrentCaption = new Label();
            this.txtCurrentPassword = new RoundedTextBox();
            this.lblNewCaption = new Label();
            this.txtNewPassword = new RoundedTextBox();
            this.lblError = new Label();
            this.btnUpdate = new RoundedButton();

            this.SuspendLayout();

            this.Text = "FixHub - Account settings";
            this.ClientSize = new Size(420, 460);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            this.lblBack.Text = "< Back";
            this.lblBack.ForeColor = FixHubColors.CoralDark;
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = Cursors.Hand;
            this.lblBack.Location = new Point(30, 20);
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            this.pnlCard.Location = new Point(30, 50);
            this.pnlCard.Size = new Size(360, 360);

            this.lblHeading.Text = "Account settings";
            this.lblHeading.Font = new Font("Segoe UI Semibold", 12F);
            this.lblHeading.ForeColor = FixHubColors.TextDark;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Location = new Point(24, 20);
            this.pnlCard.Controls.Add(this.lblHeading);

            this.lblPhoneCaption.Text = "Phone";
            this.lblPhoneCaption.ForeColor = FixHubColors.TextMuted;
            this.lblPhoneCaption.AutoSize = true;
            this.lblPhoneCaption.Location = new Point(24, 62);
            this.pnlCard.Controls.Add(this.lblPhoneCaption);

            this.txtPhone.Location = new Point(24, 84);
            this.txtPhone.Size = new Size(310, 36);
            this.pnlCard.Controls.Add(this.txtPhone);

            this.lblCurrentCaption.Text = "Current password";
            this.lblCurrentCaption.ForeColor = FixHubColors.TextMuted;
            this.lblCurrentCaption.AutoSize = true;
            this.lblCurrentCaption.Location = new Point(24, 134);
            this.pnlCard.Controls.Add(this.lblCurrentCaption);

            this.txtCurrentPassword.Location = new Point(24, 156);
            this.txtCurrentPassword.Size = new Size(310, 36);
            this.txtCurrentPassword.SetAsPassword();
            this.pnlCard.Controls.Add(this.txtCurrentPassword);

            this.lblNewCaption.Text = "New password";
            this.lblNewCaption.ForeColor = FixHubColors.TextMuted;
            this.lblNewCaption.AutoSize = true;
            this.lblNewCaption.Location = new Point(24, 206);
            this.pnlCard.Controls.Add(this.lblNewCaption);

            this.txtNewPassword.Location = new Point(24, 228);
            this.txtNewPassword.Size = new Size(310, 36);
            this.txtNewPassword.SetAsPassword();
            this.pnlCard.Controls.Add(this.txtNewPassword);

            this.lblError.ForeColor = Color.Firebrick;
            this.lblError.AutoSize = false;
            this.lblError.Size = new Size(310, 18);
            this.lblError.Location = new Point(24, 274);
            this.pnlCard.Controls.Add(this.lblError);

            this.btnUpdate.Text = "Update account";
            this.btnUpdate.BackColor = FixHubColors.CoralDark;
            this.btnUpdate.Location = new Point(24, 300);
            this.btnUpdate.Size = new Size(310, 42);
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            this.pnlCard.Controls.Add(this.btnUpdate);

            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblBack);

            this.ResumeLayout(false);
        }
    }
}
