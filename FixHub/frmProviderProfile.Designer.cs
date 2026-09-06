using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmProviderProfile
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private RoundedPanel pnlCard;
        private AvatarBadge avatar;
        private Label lblName;
        private Label lblMeta;
        private RoundedButton btnBookNow;
        private Label lblBio;
        private Label lblReviewsHeader;
        private FlowLayoutPanel flowReviews;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBack = new System.Windows.Forms.Label();
            this.pnlCard = new FixHub.Helpers.RoundedPanel();
            this.avatar = new FixHub.Helpers.AvatarBadge();
            this.lblName = new System.Windows.Forms.Label();
            this.lblMeta = new System.Windows.Forms.Label();
            this.btnBookNow = new FixHub.Helpers.RoundedButton();
            this.lblBio = new System.Windows.Forms.Label();
            this.lblReviewsHeader = new System.Windows.Forms.Label();
            this.flowReviews = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();

            // lblBack
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblBack.ForeColor = FixHubColors.TealDark;
            this.lblBack.Location = new System.Drawing.Point(30, 16);
            this.lblBack.Name = "lblBack";
            this.lblBack.Size = new System.Drawing.Size(54, 20);
            this.lblBack.TabIndex = 1;
            this.lblBack.Text = "< Back";
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            // pnlCard
            this.pnlCard.BackColor = System.Drawing.Color.White;
            this.pnlCard.BorderColor = FixHubColors.BorderGray;
            this.pnlCard.BorderThickness = 1;
            this.pnlCard.Controls.Add(this.avatar);
            this.pnlCard.Controls.Add(this.lblName);
            this.pnlCard.Controls.Add(this.lblMeta);
            this.pnlCard.Controls.Add(this.btnBookNow);
            this.pnlCard.Controls.Add(this.lblBio);
            this.pnlCard.Controls.Add(this.lblReviewsHeader);
            this.pnlCard.Controls.Add(this.flowReviews);
            this.pnlCard.CornerRadius = 12;
            this.pnlCard.Location = new System.Drawing.Point(30, 45);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.ShowBorder = true;
            this.pnlCard.Size = new System.Drawing.Size(820, 505);
            this.pnlCard.TabIndex = 0;

            // avatar
            this.avatar.BadgeColor = FixHubColors.TealMid;
            this.avatar.Initials = "??";
            this.avatar.Location = new System.Drawing.Point(24, 20);
            this.avatar.Name = "avatar";
            this.avatar.Size = new System.Drawing.Size(56, 56);
            this.avatar.TabIndex = 0;
            this.avatar.TextColor = FixHubColors.TealDark;

            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.lblName.ForeColor = FixHubColors.TextDark;
            this.lblName.Location = new System.Drawing.Point(92, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(0, 30);
            this.lblName.TabIndex = 1;

            // lblMeta
            this.lblMeta.AutoSize = true;
            this.lblMeta.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMeta.ForeColor = FixHubColors.TextMuted;
            this.lblMeta.Location = new System.Drawing.Point(92, 48);
            this.lblMeta.Name = "lblMeta";
            this.lblMeta.Size = new System.Drawing.Size(0, 21);
            this.lblMeta.TabIndex = 2;

            // btnBookNow
            this.btnBookNow.BackColor = FixHubColors.TealDark;
            this.btnBookNow.CornerRadius = 8;
            this.btnBookNow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBookNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBookNow.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnBookNow.ForeColor = System.Drawing.Color.White;
            this.btnBookNow.Location = new System.Drawing.Point(670, 24);
            this.btnBookNow.Name = "btnBookNow";
            this.btnBookNow.Size = new System.Drawing.Size(126, 38);
            this.btnBookNow.TabIndex = 3;
            this.btnBookNow.Text = "Book now";
            this.btnBookNow.UseVisualStyleBackColor = false;
            this.btnBookNow.Click += new System.EventHandler(this.btnBookNow_Click);

            // lblBio
            this.lblBio.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblBio.ForeColor = FixHubColors.TextMuted;
            this.lblBio.Location = new System.Drawing.Point(24, 88);
            this.lblBio.Name = "lblBio";
            this.lblBio.Size = new System.Drawing.Size(772, 44);
            this.lblBio.TabIndex = 4;

            // lblReviewsHeader
            this.lblReviewsHeader.AutoSize = true;
            this.lblReviewsHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.lblReviewsHeader.ForeColor = FixHubColors.TextDark;
            this.lblReviewsHeader.Location = new System.Drawing.Point(24, 140);
            this.lblReviewsHeader.Name = "lblReviewsHeader";
            this.lblReviewsHeader.Size = new System.Drawing.Size(79, 25);
            this.lblReviewsHeader.TabIndex = 5;
            this.lblReviewsHeader.Text = "Reviews";

            // flowReviews
            this.flowReviews.AutoScroll = true;
            this.flowReviews.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowReviews.Location = new System.Drawing.Point(24, 168);
            this.flowReviews.Name = "flowReviews";
            this.flowReviews.Size = new System.Drawing.Size(772, 315);
            this.flowReviews.TabIndex = 6;
            this.flowReviews.WrapContents = false;

            // frmProviderProfile
            this.BackColor = FixHubColors.PageBg;
            this.ClientSize = UIHelper.StandardWindowSize;
            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblBack);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "frmProviderProfile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FixHub - Provider profile";
            this.Load += new System.EventHandler(this.frmProviderProfile_Load);
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

