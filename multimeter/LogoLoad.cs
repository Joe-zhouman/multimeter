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
    public partial class LogoLoad : Form {
        public bool LogoClose = false;
        public LogoLoad() {
            InitializeComponent();
        }

        private void LogoLoad_Load(object sender, EventArgs e) {
            timer1.Enabled = true;
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
