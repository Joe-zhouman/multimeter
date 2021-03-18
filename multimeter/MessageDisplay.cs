using System;
using System.Drawing;

namespace multimeter {

    public partial class SetupTest {
        private int _textLineLength;
        private void StatusTextBox_MouseLeave(object sender, EventArgs e) {
            if (StatusTextBox.PointToClient(MousePosition).X >= (0.8*StatusTextBox.Width)) return;
            StatusTextBox.Height = 82;
        }

        private void StatusTextBox_MouseEnter(object sender, EventArgs e) {
            StatusTextBox.Height = 500;
        }

        public void StatusTextBox_Init() {
            StatusTextBox.Text = "";
            StatusTextBox.Visible = true;
            StatusTextBox.BringToFront();
            StatusTextBox.Location = new Point(903, 2);
            StatusTextBox.Size = new Size(353, 82);
        }

        public void StatusTextBox_AddText(string str) {
            int textLength = StatusTextBox.Text.Length;
            StatusTextBox.AppendText(str + "\r\n");
            if (StatusTextBox.Lines[_textLineLength].Contains("![WARNING]")) {
                StatusTextBox.Select(textLength
                                     + StatusTextBox.Lines[_textLineLength].IndexOf("![WARNING]"), 10);
                StatusTextBox.SelectionBackColor = Color.Yellow;
                
            }
            else if (StatusTextBox.Lines[_textLineLength].Contains("![ERROR]")) {
                StatusTextBox.Select(textLength
                                     + StatusTextBox.Lines[_textLineLength].IndexOf("![ERROR]"), 8);
                StatusTextBox.SelectionBackColor = Color.Red;
            }
            else if (StatusTextBox.Lines[_textLineLength].Contains("![Info]")) {
                StatusTextBox.Select(textLength
                                     + StatusTextBox.Lines[_textLineLength].IndexOf("![Info]"), 7);
                StatusTextBox.SelectionBackColor = Color.Green;
            }
            StatusTextBox.ScrollToCaret();
            _textLineLength++;
        }

    }
}
