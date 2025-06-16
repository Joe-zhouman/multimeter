using BusinessLogic;
using System;

namespace multimeter {
    public partial class SetupTest {
        private int _countDownNum;

        private void ConvergentHolding_Timer_Tick(object sender, EventArgs e) {
            _countDownNum--;

            if(!IsAutoStop.Checked)
                IsAutoStop.Text = "数据稳定自动停止";
            else
                IsAutoStop.Text = "自动停止倒计时 " + SecToTimeSpan(_countDownNum);

            if(_countDownNum <= 0) {
                ConvergentHolding_Timer.Enabled = false;
                if(IsAutoStop.Checked) {
                    IsAutoStop.Text = "数据稳定自动停止";
                    TestRun_Click(sender, e);     //自动关闭停止按钮
                    CurrentTestResult_Click(sender, e);   //自动计算当前测试结果
                    HideChart_Click(sender, e);      //自动隐藏实时图表 
                }
            }//触发自动停止事件   
        }

        private void IsAutoStop_CheckedChanged(object sender, EventArgs e) {
            //if(!IsAutoStop.Checked) {
            //    IsAutoStop.Text = "数据稳定自动停止";
            //}
        }
        private void IsConvergent(object sender, EventArgs e) {
            if(!IsAutoStop.Checked) {
                return;
            }
            double[] lastTempArray = Solution.AveTemp(_lastTemp);
            double[] currentTempArray = Solution.AveTemp(_temp);
            for(int i = 0; i < _multiMeter.TotalNum; i++) {
                if(Math.Abs(lastTempArray[i] - currentTempArray[i]) > _appCfg.SysPara.ConvergentLim) {
                    _convergent = false;
                    return;
                }
            }

            _convergent = true;

            TestRun_Click(sender, e);     //自动关闭停止按钮
            CurrentTestResult_Click(sender, e);   //自动计算当前测试结果
            HideChart_Click(sender, e);      //自动隐藏实时图表 



            //            if(ConvergentHolding_Timer.Enabled) { return; }
            //            ConvergentHolding_Timer.Enabled = true;
            //            string countDown = SecToTimeSpan(_appCfg.SysPara.AutoCloseInterval);
            //            MessageBox.Show($@"所有通道数据已经稳定
            //自动停止测试倒计时长{countDown}", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}