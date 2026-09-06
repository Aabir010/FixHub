using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmRequests
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

            this.Text = "FixHub - Requests";
            this.ClientSize = UIHelper.StandardWindowSize;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            this.lblBack.Text = "< Back";
            this.lblBack.ForeColor = FixHubColors.CoralDark;
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = Cursors.Hand;
            this.lblBack.Location = new Point(30, 20);
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            this.lblHeading.Text = "Your requests";
            this.lblHeading.Font = new Font("Segoe UI Semibold", 14F);
            this.lblHeading.ForeColor = FixHubColors.TextDark;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Location = new Point(30, 50);

            this.grid.Location = new Point(30, 96);
            this.grid.Size = new Size(820, 440);
            this.grid.AutoGenerateColumns = false;
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCustomer", HeaderText = "Customer", Width = 180 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colService", HeaderText = "Service", Width = 170 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDateTime", HeaderText = "Date / time", Width = 160 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus", HeaderText = "Status", Width = 110 });
            this.grid.Columns.Add(new DataGridViewButtonColumn { Name = "colAction", HeaderText = "Action", Width = 100 });
            this.grid.Columns.Add(new DataGridViewButtonColumn { Name = "colDecline", HeaderText = "", Width = 80 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colBookingId", HeaderText = "", Width = 0, Visible = false });
            this.grid.CellClick += new DataGridViewCellEventHandler(this.grid_CellClick);
            UIHelper.StyleGrid(this.grid, FixHubColors.CoralLight, FixHubColors.CoralDark);

            this.Controls.Add(this.grid);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.lblBack);

            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
