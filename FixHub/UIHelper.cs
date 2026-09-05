using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FixHub.Helpers
{
    // Central brand palette - matches the teal/coral/purple system agreed on for
    // Customer / Admin / SuperAdmin. Keep every form pulling colors from here so
    // a palette change only has to happen in one place.
    public static class FixHubColors
    {
        // Teal - Customer
        public static readonly Color TealDark = ColorTranslator.FromHtml("#0F6E56");
        public static readonly Color TealMid = ColorTranslator.FromHtml("#5DCAA5");
        public static readonly Color TealLight = ColorTranslator.FromHtml("#E1F5EE");

        // Coral - Admin / Provider
        public static readonly Color CoralDark = ColorTranslator.FromHtml("#D85A30");
        public static readonly Color CoralMid = ColorTranslator.FromHtml("#F0997B");
        public static readonly Color CoralLight = ColorTranslator.FromHtml("#FAECE7");

        // Purple - SuperAdmin
        public static readonly Color PurpleDark = ColorTranslator.FromHtml("#534AB7");
        public static readonly Color PurpleMid = ColorTranslator.FromHtml("#AFA9EC");
        public static readonly Color PurpleLight = ColorTranslator.FromHtml("#EEEDFE");

        // Neutrals
        public static readonly Color TextDark = ColorTranslator.FromHtml("#2C2C2A");
        public static readonly Color TextMuted = ColorTranslator.FromHtml("#5F5E5A");
        public static readonly Color BorderGray = ColorTranslator.FromHtml("#D3D1C7");
        public static readonly Color CardWhite = Color.White;
        public static readonly Color PageBg = ColorTranslator.FromHtml("#F1EFE8");

        // Status colors (for pill badges on booking/status lists)
        public static readonly Color StatusPendingBg = ColorTranslator.FromHtml("#FAC775");
        public static readonly Color StatusPendingText = ColorTranslator.FromHtml("#412402");
        public static readonly Color StatusCompletedBg = ColorTranslator.FromHtml("#9FE1CB");
        public static readonly Color StatusCompletedText = ColorTranslator.FromHtml("#04342C");
        public static readonly Color StatusProgressBg = ColorTranslator.FromHtml("#F5C4B3");
        public static readonly Color StatusProgressText = ColorTranslator.FromHtml("#4A1B0C");
        public static readonly Color StatusCancelledBg = ColorTranslator.FromHtml("#F8D7DA");
        public static readonly Color StatusCancelledText = ColorTranslator.FromHtml("#721C24");
        public static readonly Color StatusDeclinedBg = ColorTranslator.FromHtml("#E2E3E5");
        public static readonly Color StatusDeclinedText = ColorTranslator.FromHtml("#383D41");
    }

    public static class UIHelper
    {
        public static readonly Size StandardWindowSize = new Size(880, 580);

        /// <summary>
        /// Seamlessly navigates from currentForm to nextForm preserving screen location and restoring upon close.
        /// </summary>
        public static void NavigateTo(Form currentForm, Form nextForm, Action onReturn = null)
        {
            if (currentForm == null || nextForm == null) return;
            nextForm.StartPosition = FormStartPosition.Manual;
            nextForm.Location = currentForm.Location;
            currentForm.Hide();
            nextForm.FormClosed += (s, e) =>
            {
                if (!currentForm.IsDisposed)
                {
                    currentForm.Location = nextForm.Location;
                    onReturn?.Invoke();
                    currentForm.Show();
                }
            };
            nextForm.Show();
        }

        /// <summary>
        /// Displays dialogForm as a modal centered over parentForm.
        /// </summary>
        public static DialogResult ShowModal(Form parentForm, Form dialogForm)
        {
            if (dialogForm == null) return DialogResult.Cancel;
            dialogForm.StartPosition = FormStartPosition.CenterParent;
            return dialogForm.ShowDialog(parentForm);
        }

        public static GraphicsPath GetRoundedPath(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void ApplyRoundedRegion(Control ctrl, int radius)
        {
            if (ctrl.Width <= 0 || ctrl.Height <= 0) return;
            var bounds = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
            using (var path = GetRoundedPath(bounds, radius))
            {
                ctrl.Region = new Region(path);
            }
        }

        // Applies a pill-shaped (fully rounded) badge look to a small Label -
        // used for status tags like "Pending" / "Completed" / "In progress".
        public static void MakeStatusPill(Label lbl, Color bg, Color fg)
        {
            lbl.BackColor = bg;
            lbl.ForeColor = fg;
            lbl.Font = new Font("Segoe UI Semibold", 8.5F);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.AutoSize = false;
            lbl.Height = 22;
            ApplyRoundedRegion(lbl, 11);
        }

        // Returns the right status pill colors for a booking status string,
        // keeping the palette consistent across every screen that shows status.
        public static void ColorsForStatus(string status, out Color bg, out Color fg)
        {
            switch (status)
            {
                case "Completed":
                    bg = FixHubColors.StatusCompletedBg; fg = FixHubColors.StatusCompletedText; break;
                case "In Progress":
                case "Accepted":
                    bg = FixHubColors.StatusProgressBg; fg = FixHubColors.StatusProgressText; break;
                case "Cancelled":
                    bg = FixHubColors.StatusCancelledBg; fg = FixHubColors.StatusCancelledText; break;
                case "Declined":
                    bg = FixHubColors.StatusDeclinedBg; fg = FixHubColors.StatusDeclinedText; break;
                default: // Pending
                    bg = FixHubColors.StatusPendingBg; fg = FixHubColors.StatusPendingText; break;
            }
        }

        // Strips the default spreadsheet look off a DataGridView and applies
        // flat borders, custom header color, and comfortable row height.
        public static void StyleGrid(DataGridView grid, Color headerBg, Color headerText)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = Color.White;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = FixHubColors.BorderGray;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToResizeRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.RowTemplate.Height = 38;
            grid.Font = new Font("Segoe UI", 9.5F);

            grid.ColumnHeadersDefaultCellStyle.BackColor = headerBg;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = headerText;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F);
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 36;
            grid.EnableHeadersVisualStyles = false;

            grid.DefaultCellStyle.SelectionBackColor = headerBg;
            grid.DefaultCellStyle.SelectionForeColor = headerText;
            grid.DefaultCellStyle.Padding = new Padding(6, 0, 0, 0);
        }
    }

    // Rounded card/panel container with a thin border - the base building block
    // for stat cards, list rows, and content panels across every form.
    public class RoundedPanel : Panel
    {
        public int CornerRadius { get; set; } = 12;
        public Color BorderColor { get; set; } = FixHubColors.BorderGray;
        public int BorderThickness { get; set; } = 1;
        public bool ShowBorder { get; set; } = true;

        public RoundedPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = FixHubColors.CardWhite;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = UIHelper.GetRoundedPath(bounds, CornerRadius))
            {
                Region = new Region(path);
                using (var fill = new SolidBrush(BackColor))
                    e.Graphics.FillPath(fill, path);
                if (ShowBorder)
                {
                    using (var pen = new Pen(BorderColor, BorderThickness))
                        e.Graphics.DrawPath(pen, path);
                }
            }
            base.OnPaint(e);
        }
    }

    // Solid-fill rounded button used for primary actions (Login, Save, Confirm).
    public class RoundedButton : Button
    {
        public int CornerRadius { get; set; } = 8;

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Font = new Font("Segoe UI Semibold", 10F);
            Cursor = Cursors.Hand;
            BackColor = FixHubColors.TealDark;
            ForeColor = Color.White;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UIHelper.ApplyRoundedRegion(this, CornerRadius);
        }
    }

    // Outline-only rounded button, for secondary actions (Cancel, Change password).
    public class RoundedOutlineButton : Button
    {
        public int CornerRadius { get; set; } = 8;
        public Color OutlineColor { get; set; } = FixHubColors.BorderGray;

        public RoundedOutlineButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 1;
            Font = new Font("Segoe UI", 10F);
            Cursor = Cursors.Hand;
            BackColor = Color.White;
            ForeColor = FixHubColors.TextDark;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            FlatAppearance.BorderColor = OutlineColor;
            UIHelper.ApplyRoundedRegion(this, CornerRadius);
        }
    }

    // Rounded, bordered wrapper around a borderless TextBox - native TextBox
    // controls can't be rounded directly, so this hosts one inside a RoundedPanel.
    public class RoundedTextBox : RoundedPanel
    {
        public TextBox InnerTextBox { get; private set; }

        public RoundedTextBox()
        {
            CornerRadius = 8;
            Height = 36;
            Padding = new Padding(10, 0, 10, 0);

            InnerTextBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10F),
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            Controls.Add(InnerTextBox);
        }

        public new string Text
        {
            get { return InnerTextBox.Text; }
            set { InnerTextBox.Text = value; }
        }

        public void SetPlaceholderStyle()
        {
            InnerTextBox.ForeColor = FixHubColors.TextMuted;
        }

        public void SetAsPassword(char maskChar = '\u25CF')
        {
            InnerTextBox.PasswordChar = maskChar;
        }
    }

    // Circular badge that draws a simplified FixHub house+wrench mark.
    // Color parameters let it match whichever role's theme (teal/coral/purple).
    public class LogoBadge : Panel
    {
        public Color BadgeColor { get; set; } = FixHubColors.TealDark;
        public Color IconColor { get; set; } = FixHubColors.TealMid;

        public LogoBadge()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Size = new Size(64, 64);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var b = new SolidBrush(BadgeColor))
                e.Graphics.FillEllipse(b, bounds);

            int w = Width, h = Height;
            Point[] house =
            {
                new Point(w / 2, (int)(h * 0.20f)),
                new Point((int)(w * 0.24f), (int)(h * 0.42f)),
                new Point((int)(w * 0.24f), (int)(h * 0.76f)),
                new Point((int)(w * 0.42f), (int)(h * 0.76f)),
                new Point((int)(w * 0.42f), (int)(h * 0.58f)),
                new Point((int)(w * 0.58f), (int)(h * 0.58f)),
                new Point((int)(w * 0.58f), (int)(h * 0.76f)),
                new Point((int)(w * 0.76f), (int)(h * 0.76f)),
                new Point((int)(w * 0.76f), (int)(h * 0.42f))
            };
            using (var b = new SolidBrush(IconColor))
                e.Graphics.FillPolygon(b, house);

            base.OnPaint(e);
        }
    }

    // Small circular badge showing a person's initials - used in provider list
    // rows, profile headers, and account screens instead of a real photo.
    public class AvatarBadge : Panel
    {
        public Color BadgeColor { get; set; } = FixHubColors.TealMid;
        public Color TextColor { get; set; } = FixHubColors.TealDark;
        public string Initials { get; set; } = "??";

        public AvatarBadge()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Size = new Size(44, 44);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var b = new SolidBrush(BadgeColor))
                e.Graphics.FillEllipse(b, bounds);

            using (var f = new Font("Segoe UI Semibold", Width / 3.2f))
            using (var b = new SolidBrush(TextColor))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(Initials, f, b, bounds, sf);
            }
            base.OnPaint(e);
        }
    }

    // Clickable, colored, rounded tile for category selection on the Customer
    // Dashboard (e.g. Plumbing, Electrical, Cleaning). Raises the normal Click event.
    public class CategoryTile : RoundedPanel
    {
        private Label lblText;
        public string CategoryText
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }

        public CategoryTile()
        {
            ShowBorder = false;
            CornerRadius = 10;
            Cursor = Cursors.Hand;
            Size = new Size(160, 60);

            lblText = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 10F),
                BackColor = Color.Transparent
            };
            lblText.Click += (s, e) => OnClick(e);
            Controls.Add(lblText);
        }

        public void SetTheme(Color bg, Color fg)
        {
            BackColor = bg;
            lblText.ForeColor = fg;
        }
    }
}
