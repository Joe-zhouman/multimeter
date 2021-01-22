using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DataProcessor;

namespace multimeter {
    public partial class SetupTest {

        private int _countDownNum;
        private void ConvergentHolding_Timer_Tick(object sender, EventArgs e) {
           /* _countDownNum--;

            if (!IsAutoStop.Checked) 
                IsAutoStop.Text = "数据稳定自动停止";
            else 
                IsAutoStop.Text = "自动停止倒计时 " + SecToTimeSpan(_countDownNum);

            if (_countDownNum<=0) {
                ConvergentHolding_Timer.Enabled = false;
                if (IsAutoStop.Checked) {
                    IsAutoStop.Text = "数据稳定自动停止";
                    TestRun_Click(sender, e);     //自动关闭停止按钮
                    CurrentTestResult_Click(sender, e);   //自动计算当前测试结果
                    HideChart_Click(sender, e);      //自动隐藏实时图表 
                }
            }//触发自动停止事件   */

        }
        private void IsAutoStop_CheckedChanged(object sender, EventArgs e) {
           /* if (!IsAutoStop.Checked) {
                IsAutoStop.Text = "数据稳定自动停止";
            }   */

        }
    }
}