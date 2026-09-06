using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmAdminProfile
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private RoundedPanel pnlCard;
        private AvatarBadge avatar;
        private Label lblName;
        private Label lblMeta;
        private CheckBox chkAvailable;
        private Label lblBioCaption;
        private TextBox txtBio;
        private Label lblPriceCaption;
        private RoundedTextBox txtPrice;
        private Label lblError;
        private RoundedButton btnSave;

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
            this.lblMeta = new Label();
            this.chkAvailable = new CheckBox();
            this.lblBioCaption = new Label();
            this.txtBio = new TextBox();
            this.lblPriceCaption = new Label();
            this.txtPrice = new RoundedTextBox();
            this.lblError = new Label();
            this.btnSave = new RoundedButton();

            this.SuspendLayout();

            this.Text = "FixHub - My profile";
            this.ClientSize = new Size(460, 460);
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
            this.pnlCard.Size = new Size(400, 380);

            this.avatar.Size = new Size(48, 48);
            this.avatar.BadgeColor = FixHubColors.CoralMid;
            this.avatar.TextColor = FixHubColors.CoralDark;
            this.avatar.Location = new Point(24, 20);
            this.pnlCard.Controls.Add(this.avatar);

            this.lblName.Font = new Font("Segoe UI Semibold", 11F);
            this.lblName.ForeColor = FixHubColors.TextDark;
            this.lblName.AutoSize = true;
            this.lblName.Location = new Point(84, 20);
            this.pnlCard.Controls.Add(this.lblName);

            this.lblMeta.Font = new Font("Segoe UI", 9F);
            this.lblMeta.ForeColor = FixHubColors.TextMuted;
            this.lblMeta.AutoSize = true;
            this.lblMeta.Location = new Point(84, 42);
            this.pnlCard.Controls.Add(this.lblMeta);

            this.chkAvailable.Text = "Available for new bookings";
            this.chkAvailable.Checked = true;
            this.chkAvailable.AutoSize = true;
            this.chkAvailable.Location = new Point(24, 76);
            this.pnlCard.Controls.Add(this.chkAvailable);

            this.lblBioCaption.Text = "Bio";
            this.lblBioCaption.ForeColor = FixHubColors.TextMuted;
            this.lblBioCaption.AutoSize = true;
            this.lblBioCaption.Location = new Point(24, 116);
            this.pnlCard.Controls.Add(this.lblBioCaption);

            this.txtBio.Multiline = true;
            this.txtBio.Location = new Point(24, 138);
            this.txtBio.Size = new Size(350, 80);
            this.txtBio.Font = new Font("Segoe UI", 9.5F);
            this.pnlCard.Controls.Add(this.txtBio);

            this.lblPriceCaption.Text = "Starting price (৳)";
            this.lblPriceCaption.ForeColor = FixHubColors.TextMuted;
            this.lblPriceCaption.AutoSize = true;
            this.lblPriceCaption.Location = new Point(24, 232);
            this.pnlCard.Controls.Add(this.lblPriceCaption);

            this.txtPrice.Location = new Point(24, 254);
            this.txtPrice.Size = new Size(200, 36);
            this.pnlCard.Controls.Add(this.txtPrice);

            this.lblError.ForeColor = Color.Firebrick;
            this.lblError.AutoSize = false;
            this.lblError.Size = new Size(350, 18);
            this.lblError.Location = new Point(24, 302);
            this.pnlCard.Controls.Add(this.lblError);

            this.btnSave.Text = "Save profile";
            this.btnSave.BackColor = FixHubColors.CoralDark;
            this.btnSave.Location = new Point(24, 328);
            this.btnSave.Size = new Size(350, 42);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.pnlCard.Controls.Add(this.btnSave);

            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblBack);

            this.ResumeLayout(false);
        }
    }
}
