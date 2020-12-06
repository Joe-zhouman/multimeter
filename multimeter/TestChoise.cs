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
        private void testchoose1_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.KAPPA;
            ParSetting_Click(sender, e);
        }

        private void testchoose2_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITC;
            ParSetting_Click(sender, e);
        }

        private void testchoose3_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITM;
            ParSetting_Click(sender, e);
        }

        private void testchoose4_Click(object sender, EventArgs e) {
            ButtonEnable();
            _method = TestMethod.ITMS;
            ParSetting_Click(sender, e);
        }


        private void testchoose1_MouseMove(object sender, MouseEventArgs e) {
            test1.BackColor = Color.Lime;
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

        private void ButtonEnable() {
            TestRun_Enable(true);
            Monitor_Enable(false);
            CurrentTestResult_Enable(false);
            HistoryTestResult_Enable(true);
            SerialPort_Enable(true);
            AdvancedSetting_Enable(true);
            MenuGroupBox.Visible = true;
            TestChoiseGroupBox.Visible = false;
        }
    }
}
