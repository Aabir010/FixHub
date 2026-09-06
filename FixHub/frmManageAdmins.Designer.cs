using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmManageAdmins
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private Label lblHeading;
        private RoundedTextBox txtSearch;
        private ComboBox cmbFilter;
        private DataGridView grid;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBack = new Label();
            this.lblHeading = new Label();
            this.txtSearch = new RoundedTextBox();
            this.cmbFilter = new ComboBox();
            this.grid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();

            this.SuspendLayout();

            this.Text = "FixHub - Manage admins";
            this.ClientSize = UIHelper.StandardWindowSize;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            this.lblBack.Text = "< Back";
            this.lblBack.ForeColor = FixHubColors.PurpleDark;
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = Cursors.Hand;
            this.lblBack.Location = new Point(30, 16);
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            this.lblHeading.Text = "Manage providers (admins)";
            this.lblHeading.Font = new Font("Segoe UI Semibold", 14F);
            this.lblHeading.ForeColor = FixHubColors.TextDark;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Location = new Point(30, 42);

            this.txtSearch.Location = new Point(30, 78);
            this.txtSearch.Size = new Size(350, 32);
            this.txtSearch.InnerTextBox.Text = "";

            this.cmbFilter.Location = new Point(390, 78);
            this.cmbFilter.Size = new Size(180, 32);
            this.cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFilter.Items.AddRange(new object[] { "All Providers", "Pending Approval", "Active", "Suspended" });
            this.cmbFilter.SelectedIndex = 0;

            this.grid.Location = new Point(30, 120);
            this.grid.Size = new Size(820, 430);
            this.grid.AutoGenerateColumns = false;

            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colName", HeaderText = "Name", Width = 140 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEmail", HeaderText = "Email", Width = 160 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCategory", HeaderText = "Category", Width = 110 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRating", HeaderText = "Rating", Width = 70 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colJobs", HeaderText = "Jobs", Width = 60 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus", HeaderText = "Status", Width = 110 });
            this.grid.Columns.Add(new DataGridViewButtonColumn { Name = "colAction", HeaderText = "Action", Width = 85 });
            this.grid.Columns.Add(new DataGridViewButtonColumn { Name = "colReject", HeaderText = "", Width = 75 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAdminId", HeaderText = "Admin ID", Visible = false });

            this.grid.CellClick += new DataGridViewCellEventHandler(this.grid_CellClick);
            this.grid.CellFormatting += new DataGridViewCellFormattingEventHandler(this.grid_CellFormatting);
            UIHelper.StyleGrid(this.grid, FixHubColors.PurpleLight, FixHubColors.PurpleDark);

            this.Controls.Add(this.grid);
            this.Controls.Add(this.cmbFilter);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.lblBack);

            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
