using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmLeaveReview
    {
        private System.ComponentModel.IContainer components = null;

        private RoundedPanel pnlCard;
        private Label lblHeading;
        private FlowLayoutPanel flowStars;
        private Label[] starLabels;
        private TextBox txtComment;
        private RoundedButton btnSubmit;
        private Label lblError;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlCard = new RoundedPanel();
            this.lblHeading = new Label();
            this.flowStars = new FlowLayoutPanel();
            this.txtComment = new TextBox();
            this.btnSubmit = new RoundedButton();
            this.lblError = new Label();

            this.SuspendLayout();

            this.Text = "FixHub - Leave a review";
            this.ClientSize = new Size(420, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            this.pnlCard.Location = new Point(30, 30);
            this.pnlCard.Size = new Size(360, 340);

            this.lblHeading.Font = new Font("Segoe UI Semibold", 12F);
            this.lblHeading.ForeColor = FixHubColors.TextDark;
            this.lblHeading.TextAlign = ContentAlignment.MiddleCenter;
            this.lblHeading.AutoSize = false;
            this.lblHeading.Size = new Size(360, 26);
            this.lblHeading.Location = new Point(0, 24);
            this.pnlCard.Controls.Add(this.lblHeading);

            this.flowStars.FlowDirection = FlowDirection.LeftToRight;
            this.flowStars.Size = new Size(280, 44);
            this.flowStars.Location = new Point(40, 66);
            this.starLabels = new Label[5];
            for (int i = 0; i < 5; i++)
            {
                var star = new Label
                {
                    Text = "*",
                    Font = new Font("Segoe UI", 20F),
                    ForeColor = FixHubColors.BorderGray,
                    AutoSize = true,
                    Cursor = Cursors.Hand,
                    Tag = i + 1
                };
                star.Click += new System.EventHandler(this.star_Click);
                this.starLabels[i] = star;
                this.flowStars.Controls.Add(star);
            }
            this.pnlCard.Controls.Add(this.flowStars);

            this.txtComment.Multiline = true;
            this.txtComment.Location = new Point(30, 130);
            this.txtComment.Size = new Size(300, 90);
            this.txtComment.Font = new Font("Segoe UI", 9.5F);
            this.pnlCard.Controls.Add(this.txtComment);

            this.lblError.ForeColor = Color.Firebrick;
            this.lblError.AutoSize = false;
            this.lblError.Size = new Size(300, 18);
            this.lblError.Location = new Point(30, 228);
            this.pnlCard.Controls.Add(this.lblError);

            this.btnSubmit.Text = "Submit review";
            this.btnSubmit.BackColor = FixHubColors.TealDark;
            this.btnSubmit.Location = new Point(30, 256);
            this.btnSubmit.Size = new Size(300, 42);
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.pnlCard.Controls.Add(this.btnSubmit);

            this.Controls.Add(this.pnlCard);

            this.ResumeLayout(false);
        }
    }
}
