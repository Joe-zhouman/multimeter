using DataAccess;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace multimeter {
    /// <summary>
    /// 温度探头参数设置界面
    /// <para name="_app">应用设置</para>
    /// <para name="_MAX_COL">列表的最大列数</para>
    /// <para name="_START_COL">列表每行的起始非参数单元格</para>
    /// </summary>
    public partial class ParaSetting : Form {

        private readonly AppCfg _app;
        private static readonly int _MAX_COL = 6;
        private static readonly int _START_COL = 2;
        public ParaSetting(AppCfg app) {
            InitializeComponent();
            _app = app;
        }

        private void ParaSetting_Load(object sender, EventArgs e) {
            ColInit();
            RowInit();
            Delay_Timer.Interval = 500; //延时*ms
            Delay_Timer.Enabled = true;
        }
        /// <summary>
        /// 初始化表单的各行
        /// </summary>
        private void RowInit() {
            var channels = _app.SysPara.AllowedChannels.GetRange(1, _app.SysPara.AllowedChannels.Count - 1);
            for (var i = 0; i < channels.Count; i++) {
                var card = FindChnIdx(channels[i], _app.SerialPortPara);

                var typeId = (int)card.Type;
                RisistGridView.Rows.Add(Constant.ProbeType[typeId], channels[i], "", "", "", "");
                RisistGridView["channel", i].ReadOnly = true;
                Probe voltage = ProbeFactory.Create(typeId);
                ShowProbePara(voltage, channels[i], i);
            } //默认读取双线热敏电阻
        }

        /// <summary>
        /// 初始化参数列表的各列
        /// </summary>
        private void ColInit() {
            var probeType = new DataGridViewComboBoxColumn {
                Name = "tempProbe",
                HeaderText = @"探头类型",
                DataSource = Constant.ProbeType,
            };
            RisistGridView.Rows.Clear();
            RisistGridView.Columns.Add(probeType);
            RisistGridView.Columns.Add("channel", "Channel");
            RisistGridView.Columns.Add("A0", @"A₀");
            RisistGridView.Columns.Add("A1", @"A₁");
            RisistGridView.Columns.Add("A2", "A\x2082");
            RisistGridView.Columns.Add("A3", "A\x2083");
            foreach (DataGridViewColumn col in RisistGridView.Columns) {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        /// <summary>
        /// 显示列表的一行
        /// </summary>
        /// <param name="probe">温度探头</param>
        /// <param name="channel">温度探头所在频道</param>
        /// <param name="rowIdx">显示的列的索引</param>
        private void ShowProbePara(Probe probe, string channel, int rowIdx) {
            var paraIdx = 0;
            if (!(probe is null)) {
                IniReadAndWrite.ReadTempPara(ref probe, channel, IniReadAndWrite.IniFilePath);
                for (; paraIdx < probe.Paras.Length; paraIdx++) {
                    RisistGridView[paraIdx + _START_COL, rowIdx].Value = probe.Paras[paraIdx];
                    RisistGridView[paraIdx + _START_COL, rowIdx].ValueType = typeof(double);
                }
            }
            ShowNoValue(rowIdx, paraIdx + _START_COL);
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
            var combo = (ComboBox)sender;
            if (combo != null) {
                var idx = RisistGridView.CurrentCell.RowIndex;
                Probe p = ProbeFactory.Create(combo.SelectedIndex);
                var numPara = p is null ? 0 : p.Paras.Length;
                ChangeGridCellStyle(idx, _START_COL + numPara);
            } //更新读取对应行数据  
        }
        private void ShowNoValue(int idx, int midPoint) {
            for (var j = midPoint; j < _MAX_COL; j++) {
                RisistGridView[j, idx].Value = '-';
                RisistGridView[j, idx].ReadOnly = true;
            }
        }
        private void ChangeGridCellStyle(int idx, int midPoint) {
            for (var i = _START_COL; i < midPoint; i++) {
                RisistGridView[i, idx].Value = "0.0";
                RisistGridView[i, idx].ReadOnly = false;
                RisistGridView[i, idx].ValueType = typeof(double);
            }
            ShowNoValue(idx, midPoint);
        }

        private void Confirm_Click(object sender, EventArgs e) {
            var bakFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bak",
                $"sys.ini.{DateTime.Now:yyyy-MM-dd-hh-mm-ss}.bak");
            try {
                File.Copy(IniReadAndWrite.IniFilePath, bakFilePath);
                var channels = new List<string> { "*" };
                for (var i = 0; i < RisistGridView.Rows.Count - 1; i++) {
                    var channel = RisistGridView[1, i].Value.ToString();
                    var card = FindChnIdx(channel, _app.SerialPortPara);

                    int probeTypeIdx = Array.IndexOf(Constant.ProbeType, RisistGridView[0, i].Value.ToString());
                    if (probeTypeIdx == -1) { return; }
                    card.Type = (ProbeType)probeTypeIdx;
                    Probe probe = ProbeFactory.Create(probeTypeIdx);
                    if (probe != null) {
                        for (var j = 0; j < probe.Paras.Length; j++) {
                            probe.Paras[j] = double.Parse(RisistGridView.Rows[i].Cells[j + _START_COL].Value.ToString());
                        }
                        IniReadAndWrite.WriteTempPara(probe, channel, IniReadAndWrite.IniFilePath);
                        channels.Add(channel);
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

            var setupTest = (SetupTest)Owner;
            switch (setupTest._method) {
                case TestMethod.KAPPA:
                    setupTest.TestChoosiest1_Click(sender, e);
                    break;
                case TestMethod.ITC:
                    setupTest.TestChoosiest2_Click(sender, e);
                    break;
                case TestMethod.ITM:
                    setupTest.TestChoosiest3_Click(sender, e);
                    break;
                case TestMethod.ITMS:
                    setupTest.TestChoose4_Click(sender, e);
                    break;
                default:
                    break;
            }
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == 1 && e.RowIndex > 0) RisistGridView[e.ColumnIndex, e.RowIndex].ReadOnly = true;
        }

        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            MessageBox.Show(@"请输入一个正确的数字,如: 
1.2345,1.2345e-3,1.2353E+04", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private Card FindChnIdx(string chn, SerialPortPara serialPort) {
            var chnNum = int.Parse(chn);
            return chnNum < 200 ? serialPort.CardList1[chnNum - 101] : serialPort.CardList2[chnNum - 201];
        }
    }
}