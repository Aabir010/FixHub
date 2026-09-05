using System;
using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    partial class frmBookAndPay
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblBack;
        private RoundedPanel pnlCard;
        private Label lblHeading;
        private DateTimePicker dtpScheduledDate; // FIXED: Swapped RoundedTextBox out for DateTimePicker
        private RoundedTextBox txtAddress;
        private RoundedTextBox txtCoupon;
        private RoundedButton btnApplyCoupon;
        private RadioButton radPayNow;
        private RadioButton radPayAfter;
        private Label lblTotalCaption;
        private Label lblTotalAmount;
        private Label lblError;
        private RoundedButton btnConfirm;

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
            this.dtpScheduledDate = new DateTimePicker(); // FIXED
            this.txtAddress = new RoundedTextBox();
            this.txtCoupon = new RoundedTextBox();
            this.btnApplyCoupon = new RoundedButton();
            this.radPayNow = new RadioButton();
            this.radPayAfter = new RadioButton();
            this.lblTotalCaption = new Label();
            this.lblTotalAmount = new Label();
            this.lblError = new Label();
            this.btnConfirm = new RoundedButton();

            this.SuspendLayout();

            this.Text = "FixHub - Book & pay";
            this.ClientSize = new Size(460, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = FixHubColors.PageBg;

            this.lblBack.Text = "< Back";
            this.lblBack.ForeColor = FixHubColors.TealDark;
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = Cursors.Hand;
            this.lblBack.Location = new Point(30, 20);
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            this.pnlCard.Location = new Point(30, 50);
            this.pnlCard.Size = new Size(400, 540);

            this.lblHeading.Font = new Font("Segoe UI Semibold", 12F);
            this.lblHeading.ForeColor = FixHubColors.TextDark;
            this.lblHeading.AutoSize = true;
            this.lblHeading.Location = new Point(24, 20);
            this.pnlCard.Controls.Add(this.lblHeading);

            int y = 60;
            AddLabel("Date & time", y);

            // FIXED: Dropdown Calendar configurations initialization block
            this.dtpScheduledDate.Location = new Point(24, y + 20);
            this.dtpScheduledDate.Size = new Size(352, 36);
            this.dtpScheduledDate.Format = DateTimePickerFormat.Custom;
            this.dtpScheduledDate.CustomFormat = "dd-MM-yyyy";
            this.dtpScheduledDate.MinDate = DateTime.Today; // Prevents selection of past historic days safely
            this.dtpScheduledDate.Font = new Font("Segoe UI", 11F);
            this.pnlCard.Controls.Add(this.dtpScheduledDate);

            y += 68;
            AddLabel("Address", y); this.txtAddress.Location = new Point(24, y + 20); this.txtAddress.Size = new Size(352, 36); this.pnlCard.Controls.Add(this.txtAddress);
            y += 68;
            AddLabel("Coupon code (optional)", y);
            this.txtCoupon.Location = new Point(24, y + 20); this.txtCoupon.Size = new Size(240, 36); this.pnlCard.Controls.Add(this.txtCoupon);
            this.btnApplyCoupon.Text = "Apply";
            this.btnApplyCoupon.BackColor = FixHubColors.TealMid;
            this.btnApplyCoupon.Font = new Font("Segoe UI", 9F);
            this.btnApplyCoupon.Location = new Point(272, y + 20); this.btnApplyCoupon.Size = new Size(104, 36);
            this.btnApplyCoupon.Click += new System.EventHandler(this.btnApplyCoupon_Click);
            this.pnlCard.Controls.Add(this.btnApplyCoupon);
            y += 68;

            var lblPayHeader = new Label { Text = "Payment", ForeColor = FixHubColors.TextMuted, AutoSize = true, Location = new Point(24, y) };
            this.pnlCard.Controls.Add(lblPayHeader);
            y += 22;

            this.radPayNow.Text = "Pay now";
            this.radPayNow.Checked = true;
            this.radPayNow.AutoSize = true;
            this.radPayNow.Location = new Point(24, y);
            this.pnlCard.Controls.Add(this.radPayNow);

            this.radPayAfter.Text = "Pay after service";
            this.radPayAfter.AutoSize = true;
            this.radPayAfter.Location = new Point(140, y);
            this.pnlCard.Controls.Add(this.radPayAfter);
            y += 44;

            var divider = new Panel { BackColor = FixHubColors.BorderGray, Size = new Size(352, 1), Location = new Point(24, y) };
            this.pnlCard.Controls.Add(divider);
            y += 16;

            this.lblTotalCaption.Text = "Total";
            this.lblTotalCaption.ForeColor = FixHubColors.TextMuted;
            this.lblTotalCaption.AutoSize = true;
            this.lblTotalCaption.Location = new Point(24, y);
            this.pnlCard.Controls.Add(this.lblTotalCaption);

            this.lblTotalAmount.Font = new Font("Segoe UI Semibold", 12F);
            this.lblTotalAmount.ForeColor = FixHubColors.TextDark;
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Location = new Point(300, y - 2);
            this.pnlCard.Controls.Add(this.lblTotalAmount);
            y += 36;

            this.lblError.ForeColor = Color.Firebrick;
            this.lblError.AutoSize = false;
            this.lblError.Size = new Size(352, 18);
            this.lblError.Location = new Point(24, y);
            this.pnlCard.Controls.Add(this.lblError);
            y += 26;

            this.btnConfirm.Text = "Confirm booking";
            this.btnConfirm.BackColor = FixHubColors.TealDark;
            this.btnConfirm.Location = new Point(24, y);
            this.btnConfirm.Size = new Size(352, 42);
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            this.pnlCard.Controls.Add(this.btnConfirm);

            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblBack);

            this.ResumeLayout(false);
        }

        private void AddLabel(string text, int y)
        {
            var lbl = new Label { Text = text, ForeColor = FixHubColors.TextMuted, AutoSize = true, Location = new Point(24, y) };
            this.pnlCard.Controls.Add(lbl);
        }
    }
}
