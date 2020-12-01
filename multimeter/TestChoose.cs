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
    public partial class TestChoose : Form {
        public TestChoose() {
            InitializeComponent();
        }
        private void testchoose1_Click(object sender, EventArgs e) {
            SetupTest setupTest = (SetupTest)this.Owner;
            setupTest.Enabled = true;
            setupTest.button1_Click(sender, e);
            Close();
        }

        private void testchoose2_Click(object sender, EventArgs e) {
            SetupTest setupTest = (SetupTest)this.Owner;
            setupTest.Enabled = true;
            setupTest.TestChoose2_Click(sender, e);
            Close();
        }

        private void testchoose3_Click(object sender, EventArgs e) {
            SetupTest setupTest = (SetupTest)this.Owner;
            setupTest.Enabled = true;
            setupTest.TestChoose3_Click(sender, e);
            Close();
        }

        private void testchoose4_Click(object sender, EventArgs e) {
            SetupTest setupTest = (SetupTest)this.Owner;
            setupTest.Enabled = true;
            setupTest.TestChoose4_Click(sender, e);
            Close();
        }

        private void TestChoose_Load(object sender, EventArgs e) {

        }

        private void testchoose1_MouseMove(object sender, MouseEventArgs e) {
            test1.BackColor=Color.Lime;
        }

        private void testchoose1_MouseLeave(object sender, EventArgs e) {
            test1.BackColor = Color.White;
        }

        private void testchoose2_MouseMove(object sender, MouseEventArgs e) {
            test2.BackColor = Color.Lime;
        }

        private void testchoose2_MouseLeave(object sender, EventArgs e) {
            test2.BackColor = Color.White;
        }

        private void testchoose3_MouseMove(object sender, MouseEventArgs e) {
            test3.BackColor = Color.Lime;
        }

        private void testchoose3_MouseLeave(object sender, EventArgs e) {
            test3.BackColor = Color.White;
        }

        private void testchoose4_MouseMove(object sender, MouseEventArgs e) {
            test4.BackColor = Color.Lime;
        }

        private void testchoose4_MouseLeave(object sender, EventArgs e) {
            test4.BackColor = Color.White;
        }
    }
}
