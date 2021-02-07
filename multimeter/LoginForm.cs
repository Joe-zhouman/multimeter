using System;
using System.Windows.Forms;
using DataAccess;
using Model;

namespace multimeter {
    public partial class LoginForm : Form {
        private User _user;

        public LoginForm() {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e) {
            var setupTest = (SetupTest) Owner;
            _user.Name = usernameTBox.Text;
            _user.Password = userpasswordTBox.Text;
            switch (comboBox.SelectedIndex) {
                case 0 when CheckUserInfo.CheckNormalUser(_user):
                    setupTest.User = _user.Type;
                    setupTest.TestChooseFormShow_Click(sender, e);
                    setupTest.SerialPort.Visible = false;
                    setupTest.SerialPortLabel.Visible = false;
                    setupTest.AdvancedSetting.Visible = false;
                    setupTest.AdvancedSettingLabel.Visible = false;
                    setupTest.AdvancedLabel.Visible = false;
                    Close();
                    break;
                case 1 when CheckUserInfo.CheckAdvanceUser(_user):
                    setupTest.User = _user.Type;
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

        private void userPasswordTBox_TextChanged(object sender, EventArgs e) {
            userpasswordTBox.PasswordChar = '*';
        }

        private void LoginForm_Load(object sender, EventArgs e) {
            _user = new User {Type = UserType.NORMAL};
            comboBox.SelectedIndex = 0;
        }

        private void UserType_SelectedIndexChanged(object sender, EventArgs e) {
            switch (comboBox.SelectedIndex) {
                case 0:
                    _user.Type = UserType.NORMAL;
                    break;
                case 1:
                    _user.Type = UserType.ADVANCE;
                    break;
            }
        }

        private void button4_Click(object sender, EventArgs e) { }

        private void LoginGroupBox_Enter(object sender, EventArgs e) { }
    }
}