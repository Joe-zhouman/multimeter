using multimeter.Properties;

namespace multimeter {
    public partial class SetupTest {
        public void TestChooseFormShow_Enable(bool enable) {
            if (enable) {
                TestChooseFormShow.Enabled = true;
                TestChooseFormShow.BackgroundImage = Resources.TestChooseFormShow_Enable;
            }
            else {
                TestChooseFormShow.Enabled = false;
                TestChooseFormShow.BackgroundImage = Resources.TestChooseFormShow_disable;
            }
        }

        public void TestRun_Enable(bool enable) {
            if (enable) {
                TestRun.Enabled = true;
                TestRun.BackgroundImage = Resources.TestRun_Enable;
            }
            else {
                //TestRun.Enabled = false;
                TestRun.BackgroundImage = Resources.TestStop;
            }
        }

        public void Monitor_Enable(bool enable) {
            if (enable) {
                Monitor.Enabled = true;
                Monitor.BackgroundImage = Resources.Monitor_Enable;
            }
            else {
                Monitor.Enabled = false;
                Monitor.BackgroundImage = Resources.Monitor_Disable;
            }
        }

        public void CurrentTestResult_Enable(bool enable) {
            if (enable) {
                CurrentTestResult.Enabled = true;
                CurrentTestResult.BackgroundImage = Resources.CurrentDataProcess_Enable;
            }
            else {
                CurrentTestResult.Enabled = false;
                CurrentTestResult.BackgroundImage = Resources.CurrentDataProcess_Disable;
            }
        }

        public void HistoryTestResult_Enable(bool enable) {
            if (enable) {
                HistoryTestResult.Enabled = true;
                HistoryTestResult.BackgroundImage = Resources.HistoryDataProcess_Enable;
            }
            else {
                HistoryTestResult.Enabled = false;
                HistoryTestResult.BackgroundImage = Resources.HistoryDataProcess_Disable;
            }
        }

        public void SerialPort_Enable(bool enable) {
            if (enable) {
                SerialPort.Enabled = true;
                SerialPort.BackgroundImage = Resources.SerialPort_Enable;
            }
            else {
                SerialPort.Enabled = false;
                SerialPort.BackgroundImage = Resources.SerialPort_Disable;
            }
        }

        public void AdvancedSetting_Enable(bool enable) {
            if (enable) {
                AdvancedSetting.Enabled = true;
                AdvancedSetting.BackgroundImage = Resources.AdvancedSetting_Enable;
            }
            else {
                AdvancedSetting.Enabled = false;
                AdvancedSetting.BackgroundImage = Resources.AdvancedSetting_Disable;
            }
        }

        public void ModifyParameter_Enable(bool enable, bool modify) {
            if (enable) {
                ModifyParameter.Enabled = true;
                if (modify) ModifyParameter.BackgroundImage = Resources.ModifyParameter_Enable;
                else ModifyParameter.BackgroundImage = Resources.SaveParameter_Enable;
            }
            else {
                ModifyParameter.Enabled = false;
                ModifyParameter.BackgroundImage = Resources.ModifyParameter_Disable;
            }
        }

        public void ExportResult_Enable(bool enable) {
            if (enable) {
                ExportResult.Enabled = true;
                ExportResult.BackgroundImage = Resources.ExportResult_Enable;
            }
            else {
                ExportResult.Enabled = false;
                ExportResult.BackgroundImage = Resources.ExportResult_Disable;
            }
        }
    }
}