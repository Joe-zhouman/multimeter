using BusinessLogic;
using DataAccess;
using Model;
using System.Collections.Generic;
using System.Windows.Forms;

namespace multimeter {
    public partial class SetupTest {
        public bool apply_btm() {
            var filePath = IniReadAndWrite.IniFilePath;
            switch (_method) {
                case TestMethod.KAPPA:
                    if (!apply_btm_1_Click()) return false;
                    break;
                case TestMethod.ITC:
                    if (!apply_btm_2_Click()) return false;

                    break;
                case TestMethod.ITM:
                    if (!apply_btm_3_Click()) return false;

                    break;
                case TestMethod.ITMS:
                    if (!apply_btm_4_Click()) return false;
                    break;
                default: {
                        MessageBox.Show(@"请选择测试方法!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
            }

            if (!CheckData.CheckChannelList(_device)) {
                MessageBox.Show(@"存在除相同的频道,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            IniReadAndWrite.DeviceToApp(_device.Channels, ref _appCfg.SerialPortPara);
            IniReadAndWrite.WriteChannelPara(_appCfg.SerialPortPara, IniReadAndWrite.IniFilePath);
            IniReadAndWrite.WriteDevicePara(_device, IniReadAndWrite.IniFilePath);
            return true;
        }

        private bool apply_btm_1_Click() {
            if (User == UserType.ADVANCE) {
                if (!BoxToSpecimen(ref _device.HeatMeter1, _KAPPA_HEATMETER_1_POSITION_BOXES, _KAPPA_HEATMETER_1_CHANNEL_BOXES,
                    K1TextBox1_1, S1TextBox1_1)) return false;
                if (!BoxToSpecimen(ref _device.HeatMeter2, _KAPPA_HEATMETER_2_POSITION_BOXES, _KAPPA_HEATMETER_2_CHANNEL_BOXES,
                    K2TextBox1_2, S2TextBox1_2)) return false;
            }
            if (!ForceToDevice(ref _device, ForceTextBox1)) return false;
            return BoxToSpecimen(ref _device.Sample1, _KAPPA_SAMPLE_POSITION_BOXES, _KAPPA_SAMPLE_CHANNEL_BOXES, STextBox1_1);
        }

        private bool ForceToDevice(ref TestDevice device, TextBox forceBox) {
            var force = forceBox.Text.Replace(" ", "");
            if (!CheckData.CheckDouble(force)) {
                MessageBox.Show(@"存在不合理的压力,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            device.Force = force;
            return true;
        }
        private bool apply_btm_2_Click() {
            if (User == UserType.ADVANCE) {
                if (!BoxToSpecimen(ref _device.HeatMeter1, _TCR_HEATMETER_1_POSITION_BOXES, _TCR_HEATMETER_1_CHANNEL_BOXES,
                    K1TextBox2_1, S1TextBox2_1)) return false;

                if (!BoxToSpecimen(ref _device.HeatMeter2, _TCR_HEATMETER_2_POSITION_BOXES, _TCR_HEATMETER_2_CHANNEL_BOXES,
                    K2TextBox2_2,
                    S2TextBox2_2)) return false;
            }
            if (!ForceToDevice(ref _device, ForceTextBox2)) return false;
            if (!BoxToSpecimen(ref _device.Sample1, _TCR_SAMPLE_1_POSITION_BOXES, _TCR_SAMPLE_1_CHANNEL_BOXES, SuTextBox2_1)) return false;
            return BoxToSpecimen(ref _device.Sample2, _TCR_SAMPLE_2_POSITION_BOXES, _TCR_SAMPLE_2_CHANNEL_BOXES, SlTextBox2_2);
        }

        private bool apply_btm_3_Click() {
            if (User == UserType.ADVANCE) {
                if (!BoxToSpecimen(ref _device.HeatMeter1, _TIM_HEATMETER_1_POSITION_BOXES, _TIM_HEATMETER_1_CHANNEL_BOXES,
                    K1TextBox3_1,
                    S1TextBox3_1)) return false;
                if (!BoxToSpecimen(ref _device.HeatMeter2, _TIM_HEATMETER_2_POSITION_BOXES, _TIM_HEATMETER_2_CHANNEL_BOXES,
                    K2TextBox3_2,
                    S2TextBox3_2)) return false;
            }
            if (!ForceToDevice(ref _device, ForceTextBox3)) return false;
            return BoxToItm(ref _device.Itm, FilmThickness1, TimAreaTextBox3, S1TextBox3_1);
        }

        private bool BoxToItm(ref Tim deviceItm, TextBox lengthBox, TextBox areaBox, TextBox compareBox) {
            if (!CheckData.CheckDouble(lengthBox.Text)) {
                MessageBox.Show(@"存在不合理的热界面材料厚度,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!CheckData.CheckDouble(areaBox.Text)) {
                MessageBox.Show(@"存在不合理的热界面材料面积,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!CheckData.Compare(areaBox.Text, compareBox.Text)) {
                if (DialogResult.No == MessageBox.Show(@"界面材料面积与试件/热流计面积相差过大，将影响实验结果的精度，是否继续？", @"警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) return false;
            }
            deviceItm.Thickness = lengthBox.Text;
            deviceItm.Area = areaBox.Text;
            return true;
        }

        private bool apply_btm_4_Click() {
            if (User == UserType.ADVANCE) {
                if (!BoxToSpecimen(ref _device.HeatMeter1, _TIM_S_HEATMETER_1_POSITION_BOXES, _TIM_S_HEATMETER_1_CHANNEL_BOXES,
                    K1TextBox4_1,
                    S1TextBox4_1)) return false;
                if (!BoxToSpecimen(ref _device.HeatMeter2, _TIM_S_HEATMETER_2_POSITION_BOXES, _TIM_S_HEATMETER_2_CHANNEL_BOXES,
                    K4TextBox4_2,
                    S2TextBox4_2)) return false;
            }
            if (!ForceToDevice(ref _device, ForceTextBox4)) return false;
            if (!BoxToSpecimen(ref _device.Sample1, _TIM_S_SAMPLE_1_POSITION_BOXES, _TIM_S_SAMPLE_1_CHANNEL_BOXES, SuTextBox4_1))
                return false;
            if (!BoxToSpecimen(ref _device.Sample2, _TIM_S_SAMPLE_2_POSITION_BOXES, _TIM_S_SAMPLE_2_CHANNEL_BOXES, SlTextBox4_2))
                return false;
            return BoxToItm(ref _device.Itm, FilmThickness2, TimAreaTextBox4, SuTextBox4_1);
        }
        /// <summary>
        /// 将试件的测试点位置,频道,面积信息写入文本框中
        /// </summary>
        /// <param name="specimen"></param>
        /// <param name="positionBoxes"></param>
        /// <param name="channelBoxes"></param>
        /// <param name="areaBox"></param>
        /// <returns></returns>
        private bool BoxToSpecimen(ref Specimen specimen, List<TextBox> positionBoxes, List<TextBox> channelBoxes,
            TextBox areaBox) {
            for (var i = 0; i < specimen.TestPoint; i++) {
                var channel = channelBoxes[i].Text.Replace(" ", "");
                if (!_appCfg.SysPara.AllowedChannels.Contains(channel)) {
                    MessageBox.Show($@"{specimen.Name}存在不可用频道,请重新设置!", @"错误", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
                specimen.Channel[i] = channel;
            }
            if (specimen.Channel.Length < 3) {
                MessageBox.Show($@"{specimen.Name}的测温点太少,请重新设置!", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            for (var i = 0; i < specimen.TestPoint; i++) {
                var position = positionBoxes[i].Text.Replace(" ", "");
                if (specimen.Channel[i] == "*" && position != "*") {
                    MessageBox.Show($@"{specimen.Name}未启用探测点的位置坐标应设为*，请重新设置！
注意，此时位置坐标应为实际测试点位之间的距离", @"错误", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
                if (specimen.Channel[i] != "*" && position == "*") {
                    MessageBox.Show($@"{specimen.Name}已启用探测点的位置坐标被设为*，请重新设置！", @"错误", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
                if (!CheckData.CheckDouble(position)) {
                    MessageBox.Show($@"{specimen.Name}存在不合理的位置坐标,请重新设置!", @"错误", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
                specimen.Position[i] = position;
            }
            var area = areaBox.Text.Replace(" ", "");
            if (!CheckData.CheckDouble(areaBox.Text)) {
                MessageBox.Show($@"{specimen.Name}存在不合理的面积,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            specimen.Area = area;
            return true;
        }

        private bool BoxToSpecimen(ref Specimen specimen, List<TextBox> positionBoxes, List<TextBox> channelBoxes,
            TextBox kappaBox, TextBox areaBox) {
            var kappa = kappaBox.Text.Replace(" ", "");
            if (!CheckData.CheckDouble(kappa)) {
                MessageBox.Show($@"{specimen.Name}存在不合理的热导率,请重新设置!", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            specimen.Kappa = kappaBox.Text;
            return BoxToSpecimen(ref specimen, positionBoxes, channelBoxes, areaBox);
        }

        public void AllTextBoxEnable(bool enable) {
            var groupBoxs = new List<GroupBox> { TextGroupbox1, TextGroupbox2, TextGroupbox3, TextGroupbox4 };
            groupBoxs.ForEach(c => {
                foreach (var control in c.Controls)
                    if (control.GetType().ToString() == "System.Windows.Forms.TextBox")
                        ((TextBox)control).Enabled = enable;
            });
        }

        public void NormalTextBoxEnable(bool enable) {
            var textBoxes = new List<TextBox>();
            switch (_method) {
                case TestMethod.KAPPA: {
                        textBoxes.AddRange(_KAPPA_SAMPLE_POSITION_BOXES);
                        textBoxes.AddRange(_KAPPA_SAMPLE_CHANNEL_BOXES);
                        textBoxes.Add(STextBox1_1);
                        textBoxes.Add(ForceTextBox1);
                    }
                    break;
                case TestMethod.ITC: {
                        textBoxes.AddRange(_TCR_SAMPLE_1_POSITION_BOXES);
                        textBoxes.AddRange(_TCR_SAMPLE_1_CHANNEL_BOXES);
                        textBoxes.AddRange(_TCR_SAMPLE_2_POSITION_BOXES);
                        textBoxes.AddRange(_TCR_SAMPLE_2_CHANNEL_BOXES);
                        textBoxes.Add(SuTextBox2_1);
                        textBoxes.Add(SlTextBox2_2);
                        textBoxes.Add(ForceTextBox2);
                    }
                    break;
                case TestMethod.ITM: {
                        textBoxes.Add(ForceTextBox3);
                        textBoxes.Add(FilmThickness1);
                        textBoxes.Add(TimAreaTextBox3);
                    }
                    break;
                case TestMethod.ITMS: {
                        textBoxes.AddRange(_TIM_S_SAMPLE_1_POSITION_BOXES);
                        textBoxes.AddRange(_TIM_S_SAMPLE_1_CHANNEL_BOXES);
                        textBoxes.AddRange(_TIM_S_SAMPLE_2_CHANNEL_BOXES);
                        textBoxes.AddRange(_TIM_S_SAMPLE_2_POSITION_BOXES);
                        textBoxes.Add(SuTextBox4_1);
                        textBoxes.Add(SlTextBox4_2);
                        textBoxes.Add(ForceTextBox4);
                        textBoxes.Add(FilmThickness2);
                        textBoxes.Add(TimAreaTextBox4);
                    }
                    break;
                default: {
                        return;
                    }
            }
            foreach (var textBox in textBoxes) textBox.Enabled = enable;
        }
    }
}