using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmCustomerAccount
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private RoundedPanel pnlCard;
        private AvatarBadge avatar;
        private Label lblName;
        private Label lblEmail;
        private Label lblPhoneCaption;
        private RoundedTextBox txtPhone;
        private Label lblAddressCaption;
        private RoundedTextBox txtAddress;
        private RoundedButton btnSave;
        private RoundedOutlineButton btnChangePassword;
        private Label lblError;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBack = new Label();
            this.pnlCard = new RoundedPanel();
            this.avatar = new AvatarBadge();
            this.lblName = new Label();
            this.lblEmail = new Label();
            this.lblPhoneCaption = new Label();
            this.txtPhone = new RoundedTextBox();
            this.lblAddressCaption = new Label();
            this.txtAddress = new RoundedTextBox();
            this.btnSave = new RoundedButton();
            this.btnChangePassword = new RoundedOutlineButton();
            this.lblError = new Label();

            this.SuspendLayout();

            this.Text = "FixHub - My account";
            this.ClientSize = new Size(460, 460);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            this.lblBack.Text = "< Back";
            this.lblBack.ForeColor = FixHubColors.TealDark;
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = Cursors.Hand;
            this.lblBack.Location = new Point(30, 20);
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            this.pnlCard.Location = new Point(30, 50);
            this.pnlCard.Size = new Size(400, 380);

            this.avatar.Size = new Size(48, 48);
            this.avatar.BadgeColor = FixHubColors.TealMid;
            this.avatar.TextColor = FixHubColors.TealDark;
            this.avatar.Location = new Point(24, 24);
            this.pnlCard.Controls.Add(this.avatar);

            this.lblName.Font = new Font("Segoe UI Semibold", 11F);
            this.lblName.ForeColor = FixHubColors.TextDark;
            this.lblName.AutoSize = true;
            this.lblName.Location = new Point(84, 26);
            this.pnlCard.Controls.Add(this.lblName);

            this.lblEmail.Font = new Font("Segoe UI", 9F);
            this.lblEmail.ForeColor = FixHubColors.TextMuted;
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new Point(84, 48);
            this.pnlCard.Controls.Add(this.lblEmail);

            this.lblPhoneCaption.Text = "Phone";
            this.lblPhoneCaption.ForeColor = FixHubColors.TextMuted;
            this.lblPhoneCaption.AutoSize = true;
            this.lblPhoneCaption.Location = new Point(24, 100);
            this.pnlCard.Controls.Add(this.lblPhoneCaption);

            this.txtPhone.Location = new Point(24, 122);
            this.txtPhone.Size = new Size(350, 36);
            this.pnlCard.Controls.Add(this.txtPhone);

            this.lblAddressCaption.Text = "Address";
            this.lblAddressCaption.ForeColor = FixHubColors.TextMuted;
            this.lblAddressCaption.AutoSize = true;
            this.lblAddressCaption.Location = new Point(24, 172);
            this.pnlCard.Controls.Add(this.lblAddressCaption);

            this.txtAddress.Location = new Point(24, 194);
            this.txtAddress.Size = new Size(350, 36);
            this.pnlCard.Controls.Add(this.txtAddress);

            this.lblError.ForeColor = Color.Firebrick;
            this.lblError.AutoSize = false;
            this.lblError.Size = new Size(350, 18);
            this.lblError.Location = new Point(24, 244);
            this.pnlCard.Controls.Add(this.lblError);

            this.btnSave.Text = "Save changes";
            this.btnSave.BackColor = FixHubColors.TealDark;
            this.btnSave.Size = new Size(170, 40);
            this.btnSave.Location = new Point(24, 300);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.pnlCard.Controls.Add(this.btnSave);

            this.btnChangePassword.Text = "Change password";
            this.btnChangePassword.Size = new Size(170, 40);
            this.btnChangePassword.Location = new Point(204, 300);
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            this.pnlCard.Controls.Add(this.btnChangePassword);

            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblBack);

            this.ResumeLayout(false);
        }
    }
}
