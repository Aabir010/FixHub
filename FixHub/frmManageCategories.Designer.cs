using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmManageCategories
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private RoundedPanel pnlCard;
        private Label lblHeading;
        private RoundedTextBox txtNewCategory;
        private RoundedButton btnAddCategory;
        private Label lblError;
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
            this.txtNewCategory = new RoundedTextBox();
            this.btnAddCategory = new RoundedButton();
            this.lblError = new Label();
            this.grid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();

            this.SuspendLayout();

            this.Text = "FixHub - Manage Categories";
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

            this.pnlCard.Location = new Point(30, 45);
            this.pnlCard.Size = new Size(820, 505);

            this.lblHeading.Text = "Service categories";
            this.lblHeading.Font = new Font("Segoe UI Semibold", 13F);
            this.lblHeading.ForeColor = FixHubColors.TextDark;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Location = new Point(24, 20);
            this.pnlCard.Controls.Add(this.lblHeading);

            this.txtNewCategory.Location = new Point(24, 58);
            this.txtNewCategory.Size = new Size(580, 36);
            this.txtNewCategory.InnerTextBox.Text = "";
            this.pnlCard.Controls.Add(this.txtNewCategory);

            this.btnAddCategory.Text = "+ Add Category";
            this.btnAddCategory.BackColor = FixHubColors.PurpleDark;
            this.btnAddCategory.Location = new Point(620, 58);
            this.btnAddCategory.Size = new Size(170, 36);
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            this.pnlCard.Controls.Add(this.btnAddCategory);

            this.lblError.ForeColor = Color.Firebrick;
            this.lblError.Font = new Font("Segoe UI", 8.5F);
            this.lblError.AutoSize = true;
            this.lblError.Location = new Point(24, 98);
            this.pnlCard.Controls.Add(this.lblError);

            this.grid.Location = new Point(24, 122);
            this.grid.Size = new Size(766, 355);
            this.grid.AutoGenerateColumns = false;
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCategory", HeaderText = "Category Name", Width = 420 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProviders", HeaderText = "Active Pros", Width = 150 });
            this.grid.Columns.Add(new DataGridViewButtonColumn { Name = "colEdit", HeaderText = "", Width = 95 });
            this.grid.Columns.Add(new DataGridViewButtonColumn { Name = "colDelete", HeaderText = "", Width = 95 });
            this.grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCategoryId", HeaderText = "ID", Visible = false });

            this.grid.CellClick += new DataGridViewCellEventHandler(this.grid_CellClick);
            UIHelper.StyleGrid(this.grid, FixHubColors.PurpleLight, FixHubColors.PurpleDark);
            this.pnlCard.Controls.Add(this.grid);

            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblBack);

            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
