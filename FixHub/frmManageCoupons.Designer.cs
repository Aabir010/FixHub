using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmManageCoupons
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private Label lblHeading;
        private TextBox txtCode;
        private Label lblCode;
        private TextBox txtDiscount;
        private Label lblDiscount;
        private DateTimePicker dtpExpiry;
        private Label lblExpiry;
        private NumericUpDown numLimit;
        private Label lblLimit;
        private Button btnSave;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBack = new Label();
            this.lblHeading = new Label();
            this.txtCode = new TextBox();
            this.lblCode = new Label();
            this.txtDiscount = new TextBox();
            this.lblDiscount = new Label();
            this.dtpExpiry = new DateTimePicker();
            this.lblExpiry = new Label();
            this.numLimit = new NumericUpDown();
            this.lblLimit = new Label();
            this.btnSave = new Button();

            this.SuspendLayout();

            this.Text = "FixHub - Generate Coupon";
            this.ClientSize = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = FixHubColors.PageBg;

            this.lblBack.Text = "< Back";
            this.lblBack.ForeColor = FixHubColors.PurpleDark;
            this.lblBack.Cursor = Cursors.Hand;
            this.lblBack.Location = new Point(20, 20);
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            this.lblHeading.Text = "Generate New Coupon";
            this.lblHeading.Font = new Font("Segoe UI Semibold", 14F);
            this.lblHeading.Location = new Point(20, 50);
            this.lblHeading.AutoSize = true;

            this.lblCode.Text = "Coupon Code:";
            this.lblCode.Location = new Point(20, 90);
            this.lblCode.AutoSize = true;
            this.txtCode.Location = new Point(20, 110);
            this.txtCode.Size = new Size(350, 30);

            this.lblDiscount.Text = "Discount Percentage (1-100):";
            this.lblDiscount.Location = new Point(20, 140);
            this.lblDiscount.AutoSize = true;
            this.txtDiscount.Location = new Point(20, 160);
            this.txtDiscount.Size = new Size(350, 30);

            this.lblExpiry.Text = "Expiration Date:";
            this.lblExpiry.Location = new Point(20, 190);
            this.lblExpiry.AutoSize = true;
            this.dtpExpiry.Location = new Point(20, 210);
            this.dtpExpiry.Size = new Size(350, 30);

            this.lblLimit.Text = "Usage Limit:";
            this.lblLimit.Location = new Point(20, 240);
            this.lblLimit.AutoSize = true;
            this.numLimit.Location = new Point(20, 260);
            this.numLimit.Size = new Size(350, 30);
            this.numLimit.Minimum = 1;
            this.numLimit.Maximum = 10000;
            this.numLimit.Value = 1;

            this.btnSave.Text = "Generate Coupon";
            this.btnSave.Location = new Point(20, 300);
            this.btnSave.Size = new Size(350, 40);
            this.btnSave.BackColor = FixHubColors.PurpleDark;
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.Controls.Add(this.lblBack);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblDiscount);
            this.Controls.Add(this.txtDiscount);
            this.Controls.Add(this.lblExpiry);
            this.Controls.Add(this.dtpExpiry);
            this.Controls.Add(this.lblLimit);
            this.Controls.Add(this.numLimit);
            this.Controls.Add(this.btnSave);

            this.ResumeLayout(false);
        }
    }
}
