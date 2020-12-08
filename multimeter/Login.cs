using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace multimeter {
    public partial class SetupTest {
        private void login_Click(object sender, EventArgs e) {
            string userName = this.usernameTBox.Text;
            string userPassword = this.userpasswordTBox.Text;
            switch (comboBox.SelectedIndex) {
                case 0:
                    _user = User.NORMAL;
                    TestChooseFormShow_Click(sender, e);
                    SerialPort.Visible = false;
                    SerialPortLabel.Visible = false;
                    AdvancedSetting.Visible = false;
                    AdvancedSettingLabel.Visible = false;
                    AdvancedLabel.Visible = false;
                    LoginGroupBox.Visible = false;
                    break;
                case 1 when userName.Equals("admin") && userPassword.Equals("admin"):
                    _user = User.ADVANCE;
                    TestChooseFormShow_Click(sender, e);
                    TextBoxEnable();
                    SerialPort.Visible = true;
                    SerialPortLabel.Visible = true;
                    AdvancedSetting.Visible = true;
                    AdvancedSettingLabel.Visible = true;
                    AdvancedLabel.Visible = true;
                    LoginGroupBox.Visible = false;
                    break;
                default:
                    MessageBox.Show(@"用户密码或密码错误！", @"警告", MessageBoxButtons.OK);
                    break;
            }

        }
        private void Login_Enter(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                login.Focus();
                login_Click(this, e);
            }
        }
        private void cancel_Click(object sender, EventArgs e) {
            Close();
            Application.Exit();
        }
        private void userpasswordTBox_TextChanged(object sender, EventArgs e) {
            userpasswordTBox.PasswordChar = '*';
        }

    }
}
