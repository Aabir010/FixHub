using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmCustomerDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlSidebar;
        private LogoBadge logoBadge;
        private Label lblBrand;
        private Label navHome;
        private Label navBrowse;
        private Label navBookings;
        private Label navAccount;
        private Label navLogout;

        private Panel pnlContent;
        private Label lblWelcome;
        private RoundedPanel cardActiveBooking;
        private FlowLayoutPanel flowActiveHeader; // FIXED: Added flow wrapper for collision prevention
        private Label lblActiveTitle;
        private Label lblActiveStatus;
        private Label lblActiveDetails;
        private RoundedButton btnQuickViewBooking;
        private Label lblChooseCategory;
        private FlowLayoutPanel flowCategories;

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
            this.navHome = new Label();
            this.navBrowse = new Label();
            this.navBookings = new Label();
            this.navAccount = new Label();
            this.navLogout = new Label();

            this.pnlContent = new Panel();
            this.lblWelcome = new Label();
            this.cardActiveBooking = new RoundedPanel();
            this.flowActiveHeader = new FlowLayoutPanel(); // FIXED
            this.lblActiveTitle = new Label();
            this.lblActiveStatus = new Label();
            this.lblActiveDetails = new Label();
            this.btnQuickViewBooking = new RoundedButton();
            this.lblChooseCategory = new Label();
            this.flowCategories = new FlowLayoutPanel();

            this.SuspendLayout();

            this.Text = "FixHub - Dashboard";
            this.ClientSize = UIHelper.StandardWindowSize;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            // ---- Sidebar ----
            this.pnlSidebar.BackColor = FixHubColors.TealDark;
            this.pnlSidebar.Dock = DockStyle.Left;
            this.pnlSidebar.Width = 190;

            this.logoBadge.BadgeColor = FixHubColors.TealDark;
            this.logoBadge.IconColor = FixHubColors.TealMid;
            this.logoBadge.Size = new Size(40, 40);
            this.logoBadge.Location = new Point(20, 24);
            this.pnlSidebar.Controls.Add(this.logoBadge);

            this.lblBrand.Text = "FixHub";
            this.lblBrand.Font = new Font("Segoe UI Semibold", 12F);
            this.lblBrand.ForeColor = Color.White;
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new Point(70, 34);
            this.pnlSidebar.Controls.Add(this.lblBrand);

            AddNavItem(this.navHome, "Home", 90);
            AddNavItem(this.navBrowse, "Browse", 130);
            AddNavItem(this.navBookings, "My Bookings", 170);
            AddNavItem(this.navAccount, "Account", 210);
            AddNavItem(this.navLogout, "Logout", 510);

            // ---- Content ----
            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = FixHubColors.PageBg;
            this.pnlContent.Padding = new Padding(40, 30, 40, 30);

            this.lblWelcome.Text = "Welcome back";
            this.lblWelcome.Font = new Font("Segoe UI Semibold", 16F);
            this.lblWelcome.ForeColor = FixHubColors.TextDark;
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new Point(40, 22);
            this.pnlContent.Controls.Add(this.lblWelcome);

            // Active Booking Quick Card
            this.cardActiveBooking.Location = new Point(40, 62);
            this.cardActiveBooking.Size = new Size(610, 82);
            this.cardActiveBooking.BackColor = Color.White;

            // FIXED: Header Flow Container setup to prevent text elements colliding
            this.flowActiveHeader.Location = new Point(16, 12);
            this.flowActiveHeader.Size = new Size(440, 26);
            this.flowActiveHeader.FlowDirection = FlowDirection.LeftToRight;
            this.flowActiveHeader.WrapContents = false;
            this.flowActiveHeader.BackColor = Color.Transparent;

            this.lblActiveTitle.Text = "Upcoming Service";
            this.lblActiveTitle.Font = new Font("Segoe UI Semibold", 10F);
            this.lblActiveTitle.ForeColor = FixHubColors.TealDark;
            this.lblActiveTitle.AutoSize = true;
            this.lblActiveTitle.Margin = new Padding(0, 2, 8, 0); // Adds standard space on the right side of the text

            this.lblActiveStatus.Size = new Size(95, 22);
            this.lblActiveStatus.Margin = new Padding(0);

            // Add text and status badge into the layout container instead of pasting directly on the raw panel background
            this.flowActiveHeader.Controls.Add(this.lblActiveTitle);
            this.flowActiveHeader.Controls.Add(this.lblActiveStatus);
            this.cardActiveBooking.Controls.Add(this.flowActiveHeader);

            this.lblActiveDetails.Font = new Font("Segoe UI", 9.5F);
            this.lblActiveDetails.ForeColor = FixHubColors.TextDark;
            this.lblActiveDetails.AutoSize = true;
            this.lblActiveDetails.Location = new Point(16, 44);
            this.cardActiveBooking.Controls.Add(this.lblActiveDetails);

            this.btnQuickViewBooking.Text = "View Booking";
            this.btnQuickViewBooking.Font = new Font("Segoe UI Semibold", 9F);
            this.btnQuickViewBooking.BackColor = FixHubColors.TealDark;
            this.btnQuickViewBooking.ForeColor = Color.White;
            this.btnQuickViewBooking.Location = new Point(475, 24);
            this.btnQuickViewBooking.Size = new Size(118, 34);
            this.cardActiveBooking.Controls.Add(this.btnQuickViewBooking);

            this.pnlContent.Controls.Add(this.cardActiveBooking);

            this.lblChooseCategory.Text = "What do you need help with?";
            this.lblChooseCategory.Font = new Font("Segoe UI Semibold", 11F);
            this.lblChooseCategory.ForeColor = FixHubColors.TextDark;
            this.lblChooseCategory.AutoSize = true;
            this.lblChooseCategory.Location = new Point(40, 156);
            this.pnlContent.Controls.Add(this.lblChooseCategory);

            this.flowCategories.Location = new Point(40, 186);
            this.flowCategories.Size = new Size(610, 345);
            this.flowCategories.FlowDirection = FlowDirection.LeftToRight;
            this.flowCategories.WrapContents = true;
            this.flowCategories.AutoScroll = true;
            this.pnlContent.Controls.Add(this.flowCategories);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);

            this.ResumeLayout(false);
        }

        private void AddNavItem(Label lbl, string text, int y)
        {
            lbl.Text = text;
            lbl.ForeColor = FixHubColors.TealLight;
            lbl.Font = new Font("Segoe UI", 10F);
            lbl.AutoSize = true;
            lbl.Cursor = Cursors.Hand;
            lbl.Location = new Point(28, y);
            this.pnlSidebar.Controls.Add(lbl);
        }
    }
}
