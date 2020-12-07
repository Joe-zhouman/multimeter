using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace multimeter {
    public partial class SetupTest {
        public void TestChooseFormShow_Enable(bool enable) {
            if (enable) {
                TestChooseFormShow.Enabled = true;
                TestChooseFormShow.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "TestChooseFormShow_Enable.png"));
            }
            else {
                TestChooseFormShow.Enabled = false;
                TestChooseFormShow.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "TestChooseFormShow_disable.png"));
            }
        }

        public void TestRun_Enable(bool enable) {
            if (enable) {
                TestRun.Enabled = true;
                TestRun.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "TestRun_Enable.png"));
            }
            else {
                //TestRun.Enabled = false;
                TestRun.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "TestStop.png"));
            }
        }
        public void Monitor_Enable(bool enable) {
            if (enable) {
                Monitor.Enabled = true;
                Monitor.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "Monitor_Enable.png"));
            }
            else {
                Monitor.Enabled = false;
                Monitor.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "Monitor_Disable.png"));
            }
        }
        public void CurrentTestResult_Enable(bool enable) {
            if (enable) {
                CurrentTestResult.Enabled = true;
                CurrentTestResult.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "CurrentDataProcess_Enable.png"));
            }
            else {
                CurrentTestResult.Enabled = false;
                CurrentTestResult.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "CurrentDataProcess_Disable.png"));
            }
        }
        public void HistoryTestResult_Enable(bool enable) {
            if (enable) {
                HistoryTestResult.Enabled = true;
                HistoryTestResult.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "HistoryDataProcess_Enable.png"));
            }
            else {
                HistoryTestResult.Enabled = false;
                HistoryTestResult.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "HistoryDataProcess_Disable.png"));
            }
        }
        public void SerialPort_Enable(bool enable) {
            if (enable) {
                SerialPort.Enabled = true;
                SerialPort.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "SerialPort_Enable.png"));
            }
            else {
                SerialPort.Enabled = false;
                SerialPort.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "SerialPort_Disable.png"));
            }
        }
        public void AdvancedSetting_Enable(bool enable) {
            if (enable) {
                AdvancedSetting.Enabled = true;
                AdvancedSetting.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "AdvancedSetting_Enable.png"));
            }
            else {
                AdvancedSetting.Enabled = false;
                AdvancedSetting.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ButtonImage", "AdvancedSetting_Disable.png"));
            }
        }

            
    }
}
