using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmSuperAdminDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlSidebar;
        private LogoBadge logoBadge;
        private Label lblBrand;
        private Label navDashboard;
        private Label navManageAdmins;
        private Label navCategories;
        private Label navComplaints;
        private Label navReports;
        private Label navLogout;

        private Panel pnlContent;
        private Label lblHeading;

        private RoundedPanel cardPending;
        private Label lblPendingValue;
        private RoundedPanel cardOpenComplaints;
        private Label lblOpenComplaintsValue;
        private RoundedPanel cardAdmins;
        private Label lblAdminsValue;
        private RoundedPanel cardCommission;
        private Label lblCommissionValue;

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
            this.navManageAdmins = new Label();
            this.navCategories = new Label();
            this.navComplaints = new Label();
            this.navReports = new Label();
            this.navLogout = new Label();

            this.pnlContent = new Panel();
            this.lblHeading = new Label();

            this.cardPending = new RoundedPanel();
            this.lblPendingValue = new Label();
            this.cardOpenComplaints = new RoundedPanel();
            this.lblOpenComplaintsValue = new Label();
            this.cardAdmins = new RoundedPanel();
            this.lblAdminsValue = new Label();
            this.cardCommission = new RoundedPanel();
            this.lblCommissionValue = new Label();

            this.SuspendLayout();

            this.Text = "FixHub - SuperAdmin dashboard";
            this.ClientSize = UIHelper.StandardWindowSize;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            // ---- Sidebar ----
            this.pnlSidebar.BackColor = FixHubColors.PurpleDark;
            this.pnlSidebar.Dock = DockStyle.Left;
            this.pnlSidebar.Width = 190;

            this.logoBadge.BadgeColor = FixHubColors.PurpleDark;
            this.logoBadge.IconColor = FixHubColors.PurpleMid;
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
            AddNavItem(this.navManageAdmins, "Manage Admins", 130);
            AddNavItem(this.navCategories, "Categories", 170);
            AddNavItem(this.navComplaints, "Complaints", 210);
            AddNavItem(this.navReports, "Reports", 250);
            AddNavItem(this.navLogout, "Logout", 510);

            // ---- Content ----
            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = FixHubColors.PageBg;

            this.lblHeading.Text = "Platform overview";
            this.lblHeading.Font = new Font("Segoe UI Semibold", 15F);
            this.lblHeading.ForeColor = FixHubColors.TextDark;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Location = new Point(36, 30);
            this.pnlContent.Controls.Add(this.lblHeading);

            BuildStatCard(this.cardPending, this.lblPendingValue, "Pending approvals", 36, Color.White);
            BuildStatCard(this.cardOpenComplaints, this.lblOpenComplaintsValue, "Open complaints", 196, Color.White);
            BuildStatCard(this.cardAdmins, this.lblAdminsValue, "Active providers", 356, Color.White);
            BuildStatCard(this.cardCommission, this.lblCommissionValue, "This month commission", 516, FixHubColors.PurpleLight);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);

            this.ResumeLayout(false);
        }

        private void AddNavItem(Label lbl, string text, int y)
        {
            lbl.Text = text;
            lbl.ForeColor = FixHubColors.PurpleLight;
            lbl.Font = new Font("Segoe UI", 10F);
            lbl.AutoSize = true;
            lbl.Cursor = Cursors.Hand;
            lbl.Location = new Point(28, y);
            this.pnlSidebar.Controls.Add(lbl);
        }

        private void BuildStatCard(RoundedPanel card, Label valueLabel, string caption, int x, Color bg)
        {
            card.BackColor = bg;
            card.Size = new Size(160, 90);
            card.Location = new Point(x, 80);

            var lblCaption = new Label
            {
                Text = caption,
                ForeColor = bg == FixHubColors.PurpleLight ? FixHubColors.PurpleDark : FixHubColors.TextMuted,
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                Location = new Point(14, 14)
            };
            card.Controls.Add(lblCaption);

            valueLabel.Font = new Font("Segoe UI Semibold", 17F);
            valueLabel.ForeColor = FixHubColors.TextDark;
            valueLabel.AutoSize = true;
            valueLabel.Location = new Point(14, 38);
            card.Controls.Add(valueLabel);

            this.pnlContent.Controls.Add(card);
        }
    }
}
