using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;

namespace multimeter {
    public partial class LogoLoad : Form {
        public bool LogoClose = false;
        private ProgressBar _progress = new ProgressBar();
        public LogoLoad() {
            InitializeComponent();
        }

        private void LogoLoad_Load(object sender, EventArgs e) {
            timer1.Enabled = true;
            this.Opacity = 0.7; //窗体透明度0-1
            LoadingLabel.Parent = CompanyLogo;
            LoadingLabel.Location = new Point(0, 30);//LoadingLabel相对于 CompanyLogo的位置
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (LogoClose) {
                SetupTest setupTest = new SetupTest();
                setupTest.Show();
                timer1.Enabled = false;
                this.Hide();
            }
            else LogoClose = true;
        }

    }
}
