using System;
using System.Drawing;
using System.Windows.Forms;
using Model;

namespace multimeter {

    public partial class SetupTest {
        private void StatusTextBox_MouseLeave(object sender, EventArgs e) {
            if (StatusTextBox.PointToClient(MousePosition).X >= (0.8 * StatusTextBox.Width)) return;
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

        public void StatusTextBox_AddText(PromptType promptType,string str) {
            int textLength = StatusTextBox.Text.Length;
            StatusTextBox.AppendText("!["+promptType+"]"
                                     +str + "\r\n");
            switch (promptType) {
                case PromptType.INFO: {
                    StatusTextBox.Select(textLength, 7);
                    StatusTextBox.SelectionBackColor = Color.Green;
                    } break;
                case PromptType.WARNING: {
                    StatusTextBox.Select(textLength, 10);
                    StatusTextBox.SelectionBackColor = Color.Yellow;
                    }
                    break;
                case PromptType.ERROR: {
                    StatusTextBox.Select(textLength, 8);
                    StatusTextBox.SelectionBackColor = Color.Red;
                    }
                    break;
                default:
                    break;
            }
            StatusTextBox.ScrollToCaret();
        }
    }
}
