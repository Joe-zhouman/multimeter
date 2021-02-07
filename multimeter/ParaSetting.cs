using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DataAccess;
using Model;

namespace multimeter {
    public partial class ParaSetting : Form {
        private static readonly string[] ProbeStrings = {"未启用", "电压探头", "K型热电偶", "四线热敏电阻"};
        private readonly AppCfg _app;

        public ParaSetting(AppCfg app) {
            InitializeComponent();
            _app = app;
        }

        private void ParaSetting_Load(object sender, EventArgs e) {
            var probeType = new DataGridViewComboBoxColumn {
                Name = "tempProbe", HeaderText = @"探头类型", DataSource = ProbeStrings
            };

            RisistGridView.Rows.Clear();
            RisistGridView.Columns.Add(probeType);
            RisistGridView.Columns.Add("channel", "Channel");
            RisistGridView.Columns.Add("A0", @"A₀");
            RisistGridView.Columns.Add("A1", @"A₁");
            RisistGridView.Columns.Add("A2", "A\x2082");
            RisistGridView.Columns.Add("A3", "A\x2083");

            for (var i = 0; i < 5; i++) RisistGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            var channels = _app.SysPara.AllowedChannels.GetRange(1, _app.SysPara.AllowedChannels.Count - 1);
            for (var i = 0; i < channels.Count; i++) {
                var card = FindChnIdx(channels[i], _app.SerialPortPara);

                var typeId = (int) card.Type;
                RisistGridView.Rows.Add(ProbeStrings[typeId], channels[i], "", "", "", "");
                RisistGridView["channel", i].ReadOnly = true;
                switch (typeId) {
                    case 0: {
                        for (var j = 2; j < 6; j++) {
                            RisistGridView[j, i].Value = '-';
                            RisistGridView[j, i].ReadOnly = true;
                        }
                    }
                        break;
                    case 1: {
                        Probe voltage = new Voltage();
                        ShowProbePara(voltage, channels[i], i);
                    }
                        break;
                    case 2: {
                        Probe voltage = new Thermocouple();
                        ShowProbePara(voltage, channels[i], i);
                        RisistGridView[5, i].Value = '-';
                        RisistGridView[5, i].ReadOnly = true;
                    }
                        break;
                    case 3: {
                        Probe voltage = new Thermistor();
                        ShowProbePara(voltage, channels[i], i);
                        RisistGridView[5, i].Value = '-';
                        RisistGridView[5, i].ReadOnly = true;
                    }
                        break;
                }
            } //默认读取双线热敏电阻

            Delay_Timer.Interval = 500; //延时*ms
            Delay_Timer.Enabled = true;
        }

        private void ShowProbePara(Probe probe, string channel, int i) {
            IniReadAndWrite.ReadTempPara(ref probe, channel, IniReadAndWrite.IniFilePath);
            for (var j = 0; j < probe.Paras.Length; j++) {
                RisistGridView[j + 2, i].Value = probe.Paras[j];
                RisistGridView[j + 2, i].ValueType = typeof(double);
            }
        }

        private void Delay_Timer_Tick(object sender, EventArgs e) {
            RisistGridView.Refresh();
            Delay_Timer.Enabled = false;
        }

        private void RisistGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
            if (e.Control is ComboBox combo) {
                combo.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                combo.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
                e.CellStyle.BackColor = RisistGridView.DefaultCellStyle.BackColor;
            }
        }


        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            var combo = (ComboBox) sender;
            if (combo != null) {
                var idx = RisistGridView.CurrentCell.RowIndex;
                switch (combo.SelectedIndex) {
                    case 0: {
                        ChangeGridCellStyle(idx, 2);
                    }
                        break;
                    case 1: {
                        ChangeGridCellStyle(idx, 6);
                    }
                        break;
                    case 2:
                    case 3: {
                        ChangeGridCellStyle(idx, 5);
                    }
                        break;
                }
            } //更新读取对应行数据  
        }

        private void ChangeGridCellStyle(int idx, int midPoint) {
            for (var i = 2; i < midPoint; i++) {
                RisistGridView[i, idx].Value = "0.0";
                RisistGridView[i, idx].ReadOnly = false;
                RisistGridView[i, idx].ValueType = typeof(double);
            }

            for (var i = midPoint; i < 6; i++) {
                RisistGridView[i, idx].Value = "-";
                RisistGridView[i, idx].ReadOnly = true;
            }
        }

        private void Confirm_Click(object sender, EventArgs e) {
            var bakFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bak",
                $"sys.ini.{DateTime.Now:yyyy-MM-dd-hh-mm-ss}.bak");
            try {
                File.Copy(IniReadAndWrite.IniFilePath, bakFilePath);
                var channels = new List<string> {"0"};
                for (var i = 0; i < RisistGridView.Rows.Count - 1; i++) {
                    var channel = RisistGridView[1, i].Value.ToString();
                    var card = FindChnIdx(channel, _app.SerialPortPara);
                    switch (RisistGridView[0, i].Value.ToString()) {
                        case "未启用": {
                            card.Type = ProbeType.NULL;
                        }
                            break;
                        case "电压探头": {
                            card.Type = ProbeType.VOLTAGE;
                            Probe probe = new Voltage();
                            for (var j = 0; j < 4; j++)
                                probe.Paras[j] = double.Parse(RisistGridView.Rows[i].Cells[j + 2].Value.ToString());
                            IniReadAndWrite.WriteTempPara(probe, channel, IniReadAndWrite.IniFilePath);
                            channels.Add(channel);
                        }
                            break;
                        case "K型热电偶": {
                            card.Type = ProbeType.THERMOCOUPLE;
                            Probe probe = new Thermocouple();
                            for (var j = 0; j < 3; j++)
                                probe.Paras[j] = double.Parse(RisistGridView.Rows[i].Cells[j + 2].Value.ToString());
                            IniReadAndWrite.WriteTempPara(probe, channel, IniReadAndWrite.IniFilePath);
                            channels.Add(channel);
                        }
                            break;
                        case "四线热敏电阻": {
                            card.Type = ProbeType.THERMISTOR;
                            Probe probe = new Thermistor();
                            for (var j = 0; j < 3; j++)
                                probe.Paras[j] = double.Parse(RisistGridView.Rows[i].Cells[j + 2].Value.ToString());
                            IniReadAndWrite.WriteTempPara(probe, channel, IniReadAndWrite.IniFilePath);
                            channels.Add(channel);
                        }
                            break;
                    }
                }

                _app.SysPara.AllowedChannels = channels;
            }
            catch (Exception ex) {
                MessageBox.Show($@"保存失败，请重试{ex}", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IniReadAndWrite.WritePara(_app.SysPara, IniReadAndWrite.IniFilePath);
            IniReadAndWrite.WriteChannelPara(_app.SerialPortPara, IniReadAndWrite.IniFilePath);
            MessageBox.Show($@"修改成功!
配置文件备份在{bakFilePath}.
如需找回配置,将其重命名为'sys.ini',并替换{IniReadAndWrite.IniFilePath}.", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == 1 && e.RowIndex > 0) RisistGridView[e.ColumnIndex, e.RowIndex].ReadOnly = true;
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            MessageBox.Show(@"请输入一个正确的数字,如: 
1.2345,1.2345e-3,1.2353E+04", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private Card FindChnIdx(string chn, SerialPortPara serialPort) {
            var chnNum = int.Parse(chn);
            return chnNum < 200 ? serialPort.CardList1[chnNum - 101] : serialPort.CardList2[chnNum - 201];
        }
    }
}