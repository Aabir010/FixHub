using System.Drawing;
using System.Windows.Forms;

namespace FixHub.Helpers
{
    /// <summary>
    /// Simple utility to prompt the user for a single line of text input.
    /// </summary>
    public static class InputBoxHelper
    {
        public static string Show(string prompt, string title = "Input", string defaultValue = "")
        {
            Form form = new Form();
            Label lblPrompt = new Label();
            RoundedTextBox txtInput = new RoundedTextBox();
            RoundedButton btnOk = new RoundedButton();
            RoundedButton btnCancel = new RoundedButton();

            form.Text = title;
            form.ClientSize = new Size(380, 150);
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Font = new Font("Segoe UI", 9F);
            form.BackColor = FixHubColors.PageBg;

            lblPrompt.Text = prompt;
            lblPrompt.AutoSize = false;
            lblPrompt.Size = new Size(340, 34);
            lblPrompt.Location = new Point(20, 14);
            form.Controls.Add(lblPrompt);

            txtInput.Location = new Point(20, 48);
            txtInput.Size = new Size(340, 34);
            txtInput.InnerTextBox.Text = defaultValue;
            form.Controls.Add(txtInput);

            btnOk.Text = "OK";
            btnOk.BackColor = FixHubColors.PurpleDark;
            btnOk.Location = new Point(180, 104);
            btnOk.Size = new Size(80, 30);
            btnOk.Click += (s, e) => { form.DialogResult = DialogResult.OK; };
            form.Controls.Add(btnOk);

            btnCancel.Text = "Cancel";
            btnCancel.BackColor = FixHubColors.TextMuted;
            btnCancel.Location = new Point(278, 104);
            btnCancel.Size = new Size(80, 30);
            btnCancel.Click += (s, e) => { form.DialogResult = DialogResult.Cancel; };
            form.Controls.Add(btnCancel);

            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            if (form.ShowDialog() == DialogResult.OK)
                return txtInput.Text.Trim();

            return null;
        }
    }
}

