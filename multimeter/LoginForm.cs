using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace multimeter
{
    public partial class LoginForm : Form {
        public LoginForm() {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e) {
            string userName = this.usernameTBox.Text; 
                
            string userPassword = this.userpasswordTBox.Text;
            if (comboBox.SelectedItem.ToString() != "普通用户" && comboBox.SelectedItem.ToString() != "高级用户") {
                MessageBox.Show("请输入用户类型！", "警告", MessageBoxButtons.OK);
                return;
            }
            if (userName.Equals("123") && userPassword.Equals("123") && comboBox.SelectedItem.ToString() == "普通用户") {
                // MessageBox.Show("登录成功！", "提示", MessageBoxButtons.OK);
                SetupTest setupTest = (SetupTest)this.Owner;
                setupTest.TestChooseFormShow_Click(sender,e);
                setupTest.AdvancedSetting.Visible = false;
                setupTest.AdvancedSettingLabel.Visible = false;
                Hide();
            }//普通用户登录操作
            else if (userName.Equals("admin") && userPassword.Equals("admin") && comboBox.SelectedItem.ToString() == "高级用户") {
                // MessageBox.Show("登录成功！", "提示", MessageBoxButtons.OK);
                SetupTest setupTest = (SetupTest)this.Owner;
                setupTest.TestChooseFormShow_Click(sender, e);
                setupTest.TextBoxEnable();
                setupTest.AdvancedSetting.Visible = true;
                setupTest.AdvancedSettingLabel.Visible = true;
                Hide();
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
        private void LoginForm_Load(object sender, EventArgs e) {
            comboBox.SelectedItem= "普通用户";
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e) {
            Application.Exit();
        }
    }
}
