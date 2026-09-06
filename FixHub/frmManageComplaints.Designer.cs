using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmManageComplaints
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private Label lblHeading;
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
            this.grid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();

            this.SuspendLayout();

            this.Text = "FixHub - Manage complaints";
            this.ClientSize = UIHelper.StandardWindowSize;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            this.lblBack.Text = "< Back";
            this.lblBack.ForeColor = FixHubColors.PurpleDark;
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = Cursors.Hand;
            this.lblBack.Location = new Point(30, 20);
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            this.lblHeading.Text = "Open complaints";
            this.lblHeading.Font = new Font("Segoe UI Semibold", 14F);
            this.lblHeading.ForeColor = FixHubColors.TextDark;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Location = new Point(30, 50);

            this.grid.Location = new Point(30, 96);
            this.grid.Size = new Size(820, 440);
            this.grid.AutoGenerateColumns = false;
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colBooking", HeaderText = "Booking", Width = 110 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCustomer", HeaderText = "Customer", Width = 140 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProvider", HeaderText = "Provider", Width = 140 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDescription", HeaderText = "Description", Width = 320 });
            this.grid.Columns.Add(new DataGridViewButtonColumn { Name = "colResolve", HeaderText = "", Text = "Resolve", UseColumnTextForButtonValue = true, Width = 90 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colComplaintId", HeaderText = "ComplaintId", Visible = false });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProviderAdminId", HeaderText = "ProviderAdminId", Visible = false });
            this.grid.CellClick += new DataGridViewCellEventHandler(this.grid_CellClick);
            UIHelper.StyleGrid(this.grid, FixHubColors.PurpleLight, FixHubColors.PurpleDark);

            this.Controls.Add(this.grid);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.lblBack);

            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
