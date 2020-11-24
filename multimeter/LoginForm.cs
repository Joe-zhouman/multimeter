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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e)
        {
            string userName = this.usernameTBox.Text;
            string userPassword = this.userpasswordTBox.Text;
            if (comboBox.SelectedItem.ToString() != "热电偶标定系数" && comboBox.SelectedItem.ToString() != "测试参数") {
                MessageBox.Show("请输入正确修改方法！", "警告", MessageBoxButtons.OK);
                return;
            }
            if (userName.Equals("123") && userPassword.Equals("123")) {
                MessageBox.Show("登录成功！", "提示", MessageBoxButtons.OK);
                if (comboBox.SelectedItem.ToString() == "热电偶标定系数") {
                    AlphaT0Setting alphaT0Setting = new AlphaT0Setting();
                    alphaT0Setting.Show();
                    Close();
                }
                else {
                    SetupTest setupTest=(SetupTest)this.Owner;
                    setupTest.TextBoxEnable();
                    Close();
                }
               
            }
            else {
                MessageBox.Show("用户密码或密码错误！", "警告", MessageBoxButtons.OK);
            }

}

        private void cancel_Click(object sender, EventArgs e) {
            Close();
        }
        private void userpasswordTBox_TextChanged(object sender, EventArgs e) {
            userpasswordTBox.PasswordChar = '*';
        }
        private void LoginForm_Load(object sender, EventArgs e) {

        }




    }
}
