using System;
using System.Drawing;

namespace multimeter {
    public partial class SetupTest {
        private void StatusTextBox_MouseLeave(object sender, EventArgs e) {
            StatusTextBox.Height = 82;
            StatusTextBox_TextChanged(sender, e);
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

        private void StatusTextBox_TextChanged(object sender, EventArgs e) {
            StatusTextBox.SelectionStart = StatusTextBox.Text.Length;
            StatusTextBox.ScrollToCaret();
        }
    }
}
