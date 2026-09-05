using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmBookingDetails
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private RoundedPanel pnlCard;
        private Label lblService;
        private Label lblMeta;
        private Label lblStepPending;
        private Label lblStepAccepted;
        private Label lblStepProgress;
        private Label lblStepCompleted;
        private Label lblAmountCaption;
        private Label lblAmount;
        private RoundedButton btnReview;
        private RoundedOutlineButton btnComplaint;
        private RoundedOutlineButton btnCancelBooking;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBack = new Label();
            this.pnlCard = new RoundedPanel();
            this.lblService = new Label();
            this.lblMeta = new Label();
            this.lblStepPending = new Label();
            this.lblStepAccepted = new Label();
            this.lblStepProgress = new Label();
            this.lblStepCompleted = new Label();
            this.lblAmountCaption = new Label();
            this.lblAmount = new Label();
            this.btnReview = new RoundedButton();
            this.btnComplaint = new RoundedOutlineButton();
            this.btnCancelBooking = new RoundedOutlineButton();

            this.SuspendLayout();

            this.Text = "FixHub - Booking details";
            this.ClientSize = new Size(460, 430);
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
            this.pnlCard.Size = new Size(400, 350);

            this.lblService.Font = new Font("Segoe UI Semibold", 11F);
            this.lblService.ForeColor = FixHubColors.TextDark;
            this.lblService.AutoSize = true;
            this.lblService.Location = new Point(20, 18);
            this.pnlCard.Controls.Add(this.lblService);

            this.lblMeta.Font = new Font("Segoe UI", 9F);
            this.lblMeta.ForeColor = FixHubColors.TextMuted;
            this.lblMeta.AutoSize = true;
            this.lblMeta.Location = new Point(20, 40);
            this.pnlCard.Controls.Add(this.lblMeta);

            int stepY = 80, stepW = 84;
            this.lblStepPending.Location = new Point(20, stepY);
            this.lblStepAccepted.Location = new Point(20 + stepW + 6, stepY);
            this.lblStepProgress.Location = new Point(20 + (stepW + 6) * 2, stepY);
            this.lblStepCompleted.Location = new Point(20 + (stepW + 6) * 3, stepY);
            foreach (var pair in new (Label lbl, string text)[]
            {
                (this.lblStepPending, "Pending"), (this.lblStepAccepted, "Accepted"),
                (this.lblStepProgress, "In progress"), (this.lblStepCompleted, "Completed")
            })
            {
                pair.lbl.Text = pair.text;
                pair.lbl.Size = new Size(stepW, 26);
                pair.lbl.TextAlign = ContentAlignment.MiddleCenter;
                pair.lbl.Font = new Font("Segoe UI", 8F);
                pair.lbl.BackColor = FixHubColors.BorderGray;
                pair.lbl.ForeColor = FixHubColors.TextMuted;
                this.pnlCard.Controls.Add(pair.lbl);
            }

            var divider = new Panel { BackColor = FixHubColors.BorderGray, Size = new Size(360, 1), Location = new Point(20, 130) };
            this.pnlCard.Controls.Add(divider);

            this.lblAmountCaption.Text = "Amount paid";
            this.lblAmountCaption.ForeColor = FixHubColors.TextMuted;
            this.lblAmountCaption.AutoSize = true;
            this.lblAmountCaption.Location = new Point(20, 150);
            this.pnlCard.Controls.Add(this.lblAmountCaption);

            this.lblAmount.Font = new Font("Segoe UI Semibold", 11F);
            this.lblAmount.ForeColor = FixHubColors.TextDark;
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new Point(300, 148);
            this.pnlCard.Controls.Add(this.lblAmount);

            this.btnReview.Text = "Leave a review";
            this.btnReview.BackColor = FixHubColors.TealDark;
            this.btnReview.Size = new Size(170, 40);
            this.btnReview.Location = new Point(20, 210);
            this.btnReview.Click += new System.EventHandler(this.btnReview_Click);
            this.pnlCard.Controls.Add(this.btnReview);

            this.btnComplaint.Text = "File complaint";
            this.btnComplaint.OutlineColor = Color.Firebrick;
            this.btnComplaint.ForeColor = Color.Firebrick;
            this.btnComplaint.Size = new Size(170, 40);
            this.btnComplaint.Location = new Point(210, 210);
            this.btnComplaint.Click += new System.EventHandler(this.btnComplaint_Click);
            this.pnlCard.Controls.Add(this.btnComplaint);

            this.btnCancelBooking.Text = "Cancel booking";
            this.btnCancelBooking.OutlineColor = Color.Gray;
            this.btnCancelBooking.ForeColor = FixHubColors.TextMuted;
            this.btnCancelBooking.Size = new Size(360, 36);
            this.btnCancelBooking.Location = new Point(20, 264);
            this.btnCancelBooking.Click += new System.EventHandler(this.btnCancelBooking_Click);
            this.pnlCard.Controls.Add(this.btnCancelBooking);

            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblBack);

            this.ResumeLayout(false);
        }
    }
}
