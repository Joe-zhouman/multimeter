using System;
using System.Drawing;

namespace multimeter {
    public partial class SetupTest {
        private void StautsTextBox_MouseLeave(object sender, EventArgs e) {
            StautsTextBox.Height = 82;
        }

        private void StautsTextBox_MouseEnter(object sender, EventArgs e) {
            StautsTextBox.Height = 500;
        }

        public void StautsTextBox_Init() {
            StautsTextBox.Text = "";
            StautsTextBox.Visible = true;
            StautsTextBox.BringToFront();
            StautsTextBox.Location = new Point(903, 2);
            StautsTextBox.Size = new Size(353, 82);
        }
    }
}
