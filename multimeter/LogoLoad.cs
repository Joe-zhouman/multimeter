using System;
using System.Drawing;
using System.Windows.Forms;

namespace multimeter {
    public partial class LogoLoad : Form {
        private ProgressBar _progress = new ProgressBar();
        public bool LogoClose;

        public LogoLoad() {
            InitializeComponent();
        }

        private void LogoLoad_Load(object sender, EventArgs e) {
            timer1.Enabled = true;
            Opacity = 0.7; //窗体透明度0-1
            LoadingLabel.Parent = CompanyLogo;
            LoadingLabel.Location = new Point(0, 30); //LoadingLabel相对于 CompanyLogo的位置
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (LogoClose) {
                var setupTest = new SetupTest();
                setupTest.Show();
                timer1.Enabled = false;
                Hide();
            }
            else {
                LogoClose = true;
            }
        }
    }
}