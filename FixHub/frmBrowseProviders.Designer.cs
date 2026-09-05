using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmBrowseProviders
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private RoundedTextBox txtSearch;
        private ComboBox cmbCategory;
        private RoundedButton btnSearch;
        private Label lblSort;
        private ComboBox cmbSort;
        private Label lblPrice;
        private ComboBox cmbPrice;
        private Label lblRating;
        private ComboBox cmbRating;
        private FlowLayoutPanel flowProviders;
        private Label lblNoResults;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBack = new Label();
            this.txtSearch = new RoundedTextBox();
            this.cmbCategory = new ComboBox();
            this.btnSearch = new RoundedButton();
            this.lblSort = new Label();
            this.cmbSort = new ComboBox();
            this.lblPrice = new Label();
            this.cmbPrice = new ComboBox();
            this.lblRating = new Label();
            this.cmbRating = new ComboBox();
            this.flowProviders = new FlowLayoutPanel();
            this.lblNoResults = new Label();

            this.SuspendLayout();

            this.Text = "FixHub - Browse providers";
            this.ClientSize = UIHelper.StandardWindowSize;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            this.lblBack.Text = "< Back";
            this.lblBack.ForeColor = FixHubColors.TealDark;
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = Cursors.Hand;
            this.lblBack.Location = new Point(30, 16);
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            this.txtSearch.Location = new Point(30, 42);
            this.txtSearch.Size = new Size(460, 36);
            this.txtSearch.InnerTextBox.Text = "";

            this.cmbCategory.Location = new Point(500, 42);
            this.cmbCategory.Size = new Size(180, 36);
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCategory.Items.AddRange(new object[] { "All", "Plumbing", "Electrical", "Cleaning", "AC Repair" });
            this.cmbCategory.SelectedIndex = 0;

            this.btnSearch.Text = "Search";
            this.btnSearch.BackColor = FixHubColors.TealDark;
            this.btnSearch.Location = new Point(690, 42);
            this.btnSearch.Size = new Size(150, 36);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            // Filter row
            this.lblSort.Text = "Sort:";
            this.lblSort.ForeColor = FixHubColors.TextMuted;
            this.lblSort.AutoSize = true;
            this.lblSort.Location = new Point(30, 92);

            this.cmbSort.Location = new Point(66, 88);
            this.cmbSort.Size = new Size(160, 30);
            this.cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSort.Items.AddRange(new object[] { "Recommended", "Price: Low to High", "Price: High to Low", "Rating: High to Low" });
            this.cmbSort.SelectedIndex = 0;

            this.lblPrice.Text = "Budget:";
            this.lblPrice.ForeColor = FixHubColors.TextMuted;
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new Point(242, 92);

            this.cmbPrice.Location = new Point(294, 88);
            this.cmbPrice.Size = new Size(140, 30);
            this.cmbPrice.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPrice.Items.AddRange(new object[] { "Any Price", "Under ৳500", "Under ৳1,000", "Under ৳2,000", "Under ৳5,000" });
            this.cmbPrice.SelectedIndex = 0;

            this.lblRating.Text = "Rating:";
            this.lblRating.ForeColor = FixHubColors.TextMuted;
            this.lblRating.AutoSize = true;
            this.lblRating.Location = new Point(450, 92);

            this.cmbRating.Location = new Point(500, 88);
            this.cmbRating.Size = new Size(130, 30);
            this.cmbRating.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRating.Items.AddRange(new object[] { "Any Rating", "4.0+ Stars", "4.5+ Stars" });
            this.cmbRating.SelectedIndex = 0;

            this.lblNoResults.Text = "No providers found matching your filters.";
            this.lblNoResults.ForeColor = FixHubColors.TextMuted;
            this.lblNoResults.AutoSize = true;
            this.lblNoResults.Location = new Point(30, 140);
            this.lblNoResults.Visible = false;

            this.flowProviders.Location = new Point(30, 126);
            this.flowProviders.Size = new Size(820, 430);
            this.flowProviders.FlowDirection = FlowDirection.TopDown;
            this.flowProviders.WrapContents = false;
            this.flowProviders.AutoScroll = true;

            this.Controls.Add(this.lblNoResults);
            this.Controls.Add(this.flowProviders);
            this.Controls.Add(this.cmbRating);
            this.Controls.Add(this.lblRating);
            this.Controls.Add(this.cmbPrice);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.cmbSort);
            this.Controls.Add(this.lblSort);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblBack);

            this.ResumeLayout(false);
        }
    }
}
