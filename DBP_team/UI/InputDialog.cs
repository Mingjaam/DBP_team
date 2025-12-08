using System;
using System.Drawing;
using System.Windows.Forms;

namespace DBP_team.UI
{
    public class InputDialog : Form
    {
        private TextBox _text;
        private Button _ok;
        private Button _cancel;
        public string ResultText => _text.Text;

        public InputDialog(string title, string initial)
        {
            this.Text = title ?? "입력";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ClientSize = new Size(380, 140);

            _text = new TextBox { Left = 12, Top = 12, Width = 356, Height = 24, Text = initial ?? string.Empty };            
            _ok = new Button { Text = "확인", DialogResult = DialogResult.OK, Left = 212, Top = 80, Width = 75, Height = 28 };
            _cancel = new Button { Text = "취소", DialogResult = DialogResult.Cancel, Left = 293, Top = 80, Width = 75, Height = 28 };

            this.Controls.Add(_text);
            this.Controls.Add(_ok);
            this.Controls.Add(_cancel);

            this.AcceptButton = _ok;
            this.CancelButton = _cancel;
        }
    }
}
