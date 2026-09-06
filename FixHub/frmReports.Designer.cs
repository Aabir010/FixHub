using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmReports
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private RoundedPanel pnlCard;
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
            this.pnlCard = new RoundedPanel();
            this.lblHeading = new Label();
            this.grid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();

            this.SuspendLayout();

            this.Text = "FixHub - Reports";
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

            this.pnlCard.Location = new Point(30, 50);
            this.pnlCard.Size = new Size(820, 500);

            this.lblHeading.Text = "Commission by category";
            this.lblHeading.Font = new Font("Segoe UI Semibold", 13F);
            this.lblHeading.ForeColor = FixHubColors.TextDark;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Location = new Point(24, 20);
            this.pnlCard.Controls.Add(this.lblHeading);

            this.grid.Location = new Point(24, 60);
            this.grid.Size = new Size(770, 410);
            this.grid.AutoGenerateColumns = false;
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCategory", HeaderText = "Category", Width = 450 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCommission", HeaderText = "Commission earned", Width = 300 });
            UIHelper.StyleGrid(this.grid, FixHubColors.PurpleLight, FixHubColors.PurpleDark);
            this.pnlCard.Controls.Add(this.grid);

            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblBack);

            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
