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
            if (comboBox.SelectedItem.ToString() != "普通用户" && comboBox.SelectedItem.ToString() != "高级用户") {
                MessageBox.Show("请输入用户类型！", "警告", MessageBoxButtons.OK);
                return;
            }
            if (comboBox.SelectedItem.ToString() == "普通用户") {
                TestChooseFormShow_Click(sender, e);
                AdvancedSetting.Visible = false;
                AdvancedSettingLabel.Visible = false;
                LoginGroupBox.Visible = false;
            }//普通用户登录操作
            else if (userName.Equals("admin") && userPassword.Equals("admin") && comboBox.SelectedItem.ToString() == "高级用户"){
                TestChooseFormShow_Click(sender, e);
                TextBoxEnable();
                AdvancedSetting.Visible = true;
                AdvancedSettingLabel.Visible = true;
                LoginGroupBox.Visible = false;
            }//高级用户登录操作

            else {
                MessageBox.Show("用户密码或密码错误！", "警告", MessageBoxButtons.OK);
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
