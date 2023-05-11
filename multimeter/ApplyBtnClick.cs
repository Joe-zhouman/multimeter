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
                var heatMeterPositionBoxes1 = new List<TextBox>
                    {LengthTextBox1_1, LengthTextBox1_2, LengthTextBox1_3, LengthTextBox1_4};
                var heatMeterChannelBoxes1 = new List<TextBox> {
                    ChannelTextBox1_1, ChannelTextBox1_2, ChannelTextBox1_3, ChannelTextBox1_4
                };
                if (!BoxToSpecimen(ref _device.HeatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1,
                    K1TextBox1_1, S1TextBox1_1)) return false;
                var heatMeterPositionBoxes2 = new List<TextBox>
                    {LengthTextBox1_9, LengthTextBox1_10, LengthTextBox1_11, LengthTextBox1_12};
                var heatMeterChannelBoxes2 = new List<TextBox> {
                    ChannelTextBox1_9, ChannelTextBox1_10, ChannelTextBox1_11, ChannelTextBox1_12
                };
                if (!BoxToSpecimen(ref _device.HeatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2,
                    K2TextBox1_2, S2TextBox1_2)) return false;
            }

            if (!ForceToDevice(ref _device, ForceTextBox1)) return false;
            var samplePositionBoxes = new List<TextBox>
                {LengthTextBox1_5, LengthTextBox1_6, LengthTextBox1_7, LengthTextBox1_8};
            var sampleChannelBoxes = new List<TextBox> {
                ChannelTextBox1_5, ChannelTextBox1_6, ChannelTextBox1_7, ChannelTextBox1_8
            };
            return BoxToSpecimen(ref _device.Sample1, samplePositionBoxes, sampleChannelBoxes, STextBox1_1);
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
                var heatMeterPositionBoxes1 = new List<TextBox>
                    {LengthTextBox2_1, LengthTextBox2_2, LengthTextBox2_3, LengthTextBox2_4};
                var heatMeterChannelBoxes1 = new List<TextBox> {
                    ChannelTextBox2_1, ChannelTextBox2_2, ChannelTextBox2_3, ChannelTextBox2_4
                };
                if (!BoxToSpecimen(ref _device.HeatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1,
                    K1TextBox2_1, S1TextBox2_1)) return false;
                var heatMeterPositionBoxes2 = new List<TextBox>
                    {LengthTextBox2_13, LengthTextBox2_14, LengthTextBox2_15, LengthTextBox2_16};
                var heatMeterChannelBoxes2 = new List<TextBox> {
                    ChannelTextBox2_13, ChannelTextBox2_14, ChannelTextBox2_15, ChannelTextBox2_16
                };
                if (!BoxToSpecimen(ref _device.HeatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2,
                    K2TextBox2_2,
                    S2TextBox2_2)) return false;
            }

            if (!ForceToDevice(ref _device, ForceTextBox2)) return false;
            var samplePositionBoxes1 = new List<TextBox>
                {LengthTextBox2_5, LengthTextBox2_6, LengthTextBox2_7, LengthTextBox2_8};
            var sampleChannelBoxes1 = new List<TextBox> {
                ChannelTextBox2_5, ChannelTextBox2_6, ChannelTextBox2_7, ChannelTextBox2_8
            };
            if (!BoxToSpecimen(ref _device.Sample1, samplePositionBoxes1, sampleChannelBoxes1, SuTextBox2_1))
                return false;
            var samplePositionBoxes2 = new List<TextBox>
                {LengthTextBox2_9, LengthTextBox2_10, LengthTextBox2_11, LengthTextBox2_12};
            var sampleChannelBoxes2 = new List<TextBox> {
                ChannelTextBox2_9, ChannelTextBox2_10, ChannelTextBox2_11, ChannelTextBox2_12
            };
            return BoxToSpecimen(ref _device.Sample2, samplePositionBoxes2, sampleChannelBoxes2, SlTextBox2_2);
        }

        private bool apply_btm_3_Click() {
            if (User == UserType.ADVANCE) {
                var heatMeterPositionBoxes1 = new List<TextBox>
                    {LengthTextBox3_1, LengthTextBox3_2, LengthTextBox3_3, LengthTextBox3_4};
                var heatMeterChannelBoxes1 = new List<TextBox> {
                    ChannelTextBox3_1, ChannelTextBox3_2, ChannelTextBox3_3, ChannelTextBox3_4
                };
                if (!BoxToSpecimen(ref _device.HeatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1,
                    K1TextBox3_1,
                    S1TextBox3_1)) return false;
                var heatMeterPositionBoxes2 = new List<TextBox>
                    {LengthTextBox3_5, LengthTextBox3_6, LengthTextBox3_7, LengthTextBox3_8};
                var heatMeterChannelBoxes2 = new List<TextBox> {
                    ChannelTextBox3_5, ChannelTextBox3_6, ChannelTextBox3_7, ChannelTextBox3_8
                };
                if (!BoxToSpecimen(ref _device.HeatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2,
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
                var heatMeterPositionBoxes1 = new List<TextBox>
                    {LengthTextBox4_1, LengthTextBox4_2, LengthTextBox4_3, LengthTextBox4_4};
                var heatMeterChannelBoxes1 = new List<TextBox> {
                    ChannelTextBox4_1, ChannelTextBox4_2, ChannelTextBox4_3, ChannelTextBox4_4
                };
                if (!BoxToSpecimen(ref _device.HeatMeter1, heatMeterPositionBoxes1, heatMeterChannelBoxes1,
                    K1TextBox4_1,
                    S1TextBox4_1)) return false;
                var heatMeterPositionBoxes2 = new List<TextBox>
                    {LengthTextBox4_13, LengthTextBox4_14, LengthTextBox4_15, LengthTextBox4_16};
                var heatMeterChannelBoxes2 = new List<TextBox> {
                    ChannelTextBox4_13, ChannelTextBox4_14, ChannelTextBox4_15, ChannelTextBox4_16
                };
                if (!BoxToSpecimen(ref _device.HeatMeter2, heatMeterPositionBoxes2, heatMeterChannelBoxes2,
                    K4TextBox4_2,
                    S2TextBox4_2)) return false;
            }

            if (!ForceToDevice(ref _device, ForceTextBox4)) return false;
            var samplePositionBoxes1 = new List<TextBox>
                {LengthTextBox4_5, LengthTextBox4_6, LengthTextBox4_7, LengthTextBox4_8};
            var sampleChannelBoxes1 = new List<TextBox> {
                ChannelTextBox4_5, ChannelTextBox4_6, ChannelTextBox4_7, ChannelTextBox4_8
            };
            if (!BoxToSpecimen(ref _device.Sample1, samplePositionBoxes1, sampleChannelBoxes1, SuTextBox4_1))
                return false;
            var samplePositionBoxes2 = new List<TextBox>
                {LengthTextBox4_9, LengthTextBox4_10, LengthTextBox4_11, LengthTextBox4_12};
            var sampleChannelBoxes2 = new List<TextBox> {
                ChannelTextBox4_9, ChannelTextBox4_10, ChannelTextBox4_11, ChannelTextBox4_12
            };
            if (!BoxToSpecimen(ref _device.Sample2, samplePositionBoxes2, sampleChannelBoxes2, SlTextBox4_2))
                return false;
            return BoxToItm(ref _device.Itm, FilmThickness2, TimAreaTextBox4, SuTextBox4_1);
        }


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
                        var textBoxes1 = new List<TextBox> {
                        LengthTextBox1_5, LengthTextBox1_6, LengthTextBox1_7, LengthTextBox1_8,
                        ChannelTextBox1_5, ChannelTextBox1_6, ChannelTextBox1_7, ChannelTextBox1_8,
                        STextBox1_1, ForceTextBox1
                    };
                        textBoxes.AddRange(textBoxes1);
                    }
                    break;
                case TestMethod.ITC: {
                        var textBoxes2 = new List<TextBox> {
                        LengthTextBox2_5, LengthTextBox2_6, LengthTextBox2_7, LengthTextBox2_8,
                        ChannelTextBox2_5, ChannelTextBox2_6, ChannelTextBox2_7, ChannelTextBox2_8,
                        LengthTextBox2_9, LengthTextBox2_10, LengthTextBox2_11, LengthTextBox2_12,
                        ChannelTextBox2_9, ChannelTextBox2_10, ChannelTextBox2_11, ChannelTextBox2_12,
                        SuTextBox2_1, SlTextBox2_2, ForceTextBox2
                    };
                        textBoxes.AddRange(textBoxes2);
                    }
                    break;
                case TestMethod.ITM: {
                        var textBoxes3 = new List<TextBox> {
                        ForceTextBox3, FilmThickness1,TimAreaTextBox3
                    };
                        textBoxes.AddRange(textBoxes3);
                    }
                    break;
                case TestMethod.ITMS: {
                        var textBoxes4 = new List<TextBox> {
                        LengthTextBox4_5, LengthTextBox4_6, LengthTextBox4_7, LengthTextBox4_8,
                        ChannelTextBox4_5, ChannelTextBox4_6, ChannelTextBox4_7, ChannelTextBox4_8,
                        LengthTextBox4_9, LengthTextBox4_10, LengthTextBox4_11, LengthTextBox4_12,
                        ChannelTextBox4_9, ChannelTextBox4_10, ChannelTextBox4_11, ChannelTextBox4_12,
                        SuTextBox4_1, SlTextBox4_2, ForceTextBox4, FilmThickness2,TimAreaTextBox4
                    };
                        textBoxes.AddRange(textBoxes4);
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