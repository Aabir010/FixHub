using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmAdminDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlSidebar;
        private LogoBadge logoBadge;
        private Label lblBrand;
        private Label navDashboard;
        private Label navRequests;
        private Label navProfile;
        private Label navAccount;
        private Label navLogout;

        private Panel pnlContent;
        private Label tabOverview;
        private Label tabEarnings;

        private RoundedPanel cardPending;
        private Label lblPendingValue;
        private RoundedPanel cardMonth;
        private Label lblMonthValue;
        private RoundedPanel cardRating;
        private Label lblRatingValue;

        private DataGridView gridEarnings;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlSidebar = new Panel();
            this.logoBadge = new LogoBadge();
            this.lblBrand = new Label();
            this.navDashboard = new Label();
            this.navRequests = new Label();
            this.navProfile = new Label();
            this.navAccount = new Label();
            this.navLogout = new Label();

            this.pnlContent = new Panel();
            this.tabOverview = new Label();
            this.tabEarnings = new Label();

            this.cardPending = new RoundedPanel();
            this.lblPendingValue = new Label();
            this.cardMonth = new RoundedPanel();
            this.lblMonthValue = new Label();
            this.cardRating = new RoundedPanel();
            this.lblRatingValue = new Label();

            this.gridEarnings = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridEarnings)).BeginInit();

            this.SuspendLayout();

            this.Text = "FixHub - Admin dashboard";
            this.ClientSize = UIHelper.StandardWindowSize;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            // ---- Sidebar ----
            this.pnlSidebar.BackColor = FixHubColors.CoralDark;
            this.pnlSidebar.Dock = DockStyle.Left;
            this.pnlSidebar.Width = 190;

            this.logoBadge.BadgeColor = FixHubColors.CoralDark;
            this.logoBadge.IconColor = FixHubColors.CoralMid;
            this.logoBadge.Size = new Size(40, 40);
            this.logoBadge.Location = new Point(20, 24);
            this.pnlSidebar.Controls.Add(this.logoBadge);

            this.lblBrand.Text = "FixHub";
            this.lblBrand.Font = new Font("Segoe UI Semibold", 12F);
            this.lblBrand.ForeColor = Color.White;
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new Point(70, 34);
            this.pnlSidebar.Controls.Add(this.lblBrand);

            AddNavItem(this.navDashboard, "Dashboard", 90);
            AddNavItem(this.navRequests, "Requests", 130);
            AddNavItem(this.navProfile, "Profile", 170);
            AddNavItem(this.navAccount, "Account", 210);
            AddNavItem(this.navLogout, "Logout", 510);

            // ---- Content ----
            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = FixHubColors.PageBg;

            this.tabOverview.Text = "Overview";
            this.tabOverview.Font = new Font("Segoe UI Semibold", 9.5F);
            this.tabOverview.ForeColor = FixHubColors.CoralDark;
            this.tabOverview.BackColor = FixHubColors.CoralLight;
            this.tabOverview.TextAlign = ContentAlignment.MiddleCenter;
            this.tabOverview.Size = new Size(100, 30);
            this.tabOverview.Location = new Point(40, 30);
            this.tabOverview.Cursor = Cursors.Hand;
            this.tabOverview.Click += new System.EventHandler(this.tabOverview_Click);
            this.pnlContent.Controls.Add(this.tabOverview);

            this.tabEarnings.Text = "Earnings";
            this.tabEarnings.Font = new Font("Segoe UI", 9.5F);
            this.tabEarnings.ForeColor = FixHubColors.TextMuted;
            this.tabEarnings.TextAlign = ContentAlignment.MiddleCenter;
            this.tabEarnings.Size = new Size(100, 30);
            this.tabEarnings.Location = new Point(146, 30);
            this.tabEarnings.Cursor = Cursors.Hand;
            this.tabEarnings.Click += new System.EventHandler(this.tabEarnings_Click);
            this.pnlContent.Controls.Add(this.tabEarnings);

            BuildStatCard(this.cardPending, this.lblPendingValue, "Pending", 40, FixHubColors.CoralLight);
            BuildStatCard(this.cardMonth, this.lblMonthValue, "This month", 250, Color.White);
            BuildStatCard(this.cardRating, this.lblRatingValue, "Rating", 460, Color.White);

            this.gridEarnings.Location = new Point(40, 80);
            this.gridEarnings.Size = new Size(640, 460);
            this.gridEarnings.AutoGenerateColumns = false;
            this.gridEarnings.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDate", HeaderText = "Date", Width = 110 });
            this.gridEarnings.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCustomer", HeaderText = "Customer", Width = 160 });
            this.gridEarnings.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAmount", HeaderText = "Total Amount", Width = 120 });
            this.gridEarnings.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCommission", HeaderText = "Fee (15%)", Width = 110 });
            this.gridEarnings.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNet", HeaderText = "Net Payout", Width = 140 });
            UIHelper.StyleGrid(this.gridEarnings, FixHubColors.CoralLight, FixHubColors.CoralDark);
            this.gridEarnings.Visible = false;
            this.pnlContent.Controls.Add(this.gridEarnings);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);

            ((System.ComponentModel.ISupportInitialize)(this.gridEarnings)).EndInit();
            this.ResumeLayout(false);
        }

        private void AddNavItem(Label lbl, string text, int y)
        {
            lbl.Text = text;
            lbl.ForeColor = FixHubColors.CoralLight;
            lbl.Font = new Font("Segoe UI", 10F);
            lbl.AutoSize = true;
            lbl.Cursor = Cursors.Hand;
            lbl.Location = new Point(28, y);
            this.pnlSidebar.Controls.Add(lbl);
        }

        private void BuildStatCard(RoundedPanel card, Label valueLabel, string caption, int x, Color bg)
        {
            card.BackColor = bg;
            card.Size = new Size(180, 90);
            card.Location = new Point(x, 76);

            var lblCaption = new Label
            {
                Text = caption,
                ForeColor = FixHubColors.TextMuted,
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Location = new Point(16, 14)
            };
            card.Controls.Add(lblCaption);

            valueLabel.Font = new Font("Segoe UI Semibold", 18F);
            valueLabel.ForeColor = FixHubColors.TextDark;
            valueLabel.AutoSize = true;
            valueLabel.Location = new Point(16, 38);
            card.Controls.Add(valueLabel);

            this.pnlContent.Controls.Add(card);
        }
    }
}
