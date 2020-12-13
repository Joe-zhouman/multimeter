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
    public partial class LoginForm : Form {
        public LoginForm() {
            InitializeComponent();
        }
        private void login_Click(object sender, EventArgs e) {
            string userName = this.usernameTBox.Text;
            string userPassword = this.userpasswordTBox.Text;
            SetupTest setupTest = (SetupTest)this.Owner;
            switch (comboBox.SelectedIndex) {
                case 0:
                    setupTest._user = User.NORMAL;
                    setupTest.TestChooseFormShow_Click(sender, e);
                    setupTest.SerialPort.Visible = false;
                    setupTest.SerialPortLabel.Visible = false;
                    setupTest.AdvancedSetting.Visible = false;
                    setupTest.AdvancedSettingLabel.Visible = false;
                    setupTest.AdvancedLabel.Visible = false;
                    Close();
                    break;
                case 1 when userName.Equals("admin") && userPassword.Equals("admin"):
                    setupTest._user = User.ADVANCE;
                    setupTest.TestChooseFormShow_Click(sender, e);
                    setupTest.AllTextBoxEnable();
                    setupTest.SerialPort.Visible = true;
                    setupTest.SerialPortLabel.Visible = true;
                    setupTest.AdvancedSetting.Visible = true;
                    setupTest.AdvancedSettingLabel.Visible = true;
                    setupTest.AdvancedLabel.Visible = true;
                    Close();
                    break;
                default:
                    MessageBox.Show(@"用户密码或密码错误！", @"警告", MessageBoxButtons.OK);
                    break;
            }

        }
        private void LoginForm_KeyDown(object sender, KeyEventArgs e) {
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

        private void LoginForm_Load(object sender, EventArgs e) {
            comboBox.SelectedItem = "普通用户";
        }


    }
}
