using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess;

namespace multimeter {
    public partial class ParaSetting : Form {
        public ParaSetting() {
            InitializeComponent();
        }

        private void ParaSetting_Load(object sender, EventArgs e) {

            DataGridViewComboBoxColumn tempProbeType = new DataGridViewComboBoxColumn();
            tempProbeType.Name = "tempProbe";
            tempProbeType.DataPropertyName = "tempProbe";
            tempProbeType.HeaderText = "TempProbe";
            List<string> tempProbeName = new List<string> { "双线热敏电阻", "K型热电偶" };
            tempProbeType.DataSource = tempProbeName;

            RisistGridView.Rows.Clear();
            RisistGridView.Columns.Add(tempProbeType);
            RisistGridView.Columns.Add("channel", "Channel");
            RisistGridView.Columns.Add("A0", $"A\x2080");
            RisistGridView.Columns.Add("A1", $"A\x2081");
            RisistGridView.Columns.Add("A3", $"A\x2083");

            for (int i = 0; i < 4; i++) { 
                RisistGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < 13; i++) {
                RisistGridView.Rows.Add("双线热敏电阻", (201 + i).ToString(), "", "");
                RisistGridView["channel", i].ReadOnly = true;
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
                RisistGridView[2,i].Value = IniHelper.Read(RisistGridView[1, i].Value.ToString(), "A0", RisistGridView[1, i].Value.ToString(),
                     filePath);           
                RisistGridView[2, i].ValueType = typeof(double);
                RisistGridView[3, i].Value = IniHelper.Read(RisistGridView[1, i].Value.ToString(), "A1", RisistGridView[1, i].Value.ToString(),
                     filePath);
                RisistGridView[3, i].ValueType = typeof(double);
                RisistGridView[4, i].Value = IniHelper.Read(RisistGridView[1, i].Value.ToString(), "A3", RisistGridView[1, i].Value.ToString(),
                     filePath);
                RisistGridView[4, i].ValueType = typeof(double);
            }//默认读取双线热敏电阻

            Delay_Timer.Interval = 500;      //延时*ms
            Delay_Timer.Enabled = true;
        }
       
        private void Delay_Timer_Tick(object sender, EventArgs e) {
             RisistGridView.Refresh();
             Delay_Timer.Enabled = false;
        }

        private void RisistGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
            ComboBox combo = e.Control as ComboBox;
            if (combo != null) {
                combo.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            }
          
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            ComboBox combo = sender as ComboBox;
            switch (combo.SelectedIndex) {
                case 0:

                    break;
                case 1:

                    break;
                default:
                    break;
            } //更新读取对应行数据

        } //"双线热敏电阻" "K型热电偶"

        private void Confirm_Click(object sender, EventArgs e) {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini"); //在当前程序路径创建
            string bakFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bak", $"sys.ini.{DateTime.Now:yyyy-MM-dd-ss-ffff}.bak");
            File.Copy(filePath, bakFilePath);
            for (int i = 0; i < RisistGridView.Rows.Count - 1; i++) {
                IniHelper.Write(RisistGridView[1, i].Value.ToString(), "A0", RisistGridView[1, i].Value.ToString(),
                    filePath);
                IniHelper.Write(RisistGridView[1, i].Value.ToString(), "A1", RisistGridView[2, i].Value.ToString(),
                    filePath);
                IniHelper.Write(RisistGridView[1, i].Value.ToString(), "A3", RisistGridView[3, i].Value.ToString(),
                    filePath);
                
            }
            MessageBox.Show($@"修改成功!
配置文件备份在{bakFilePath}.
如需找回配置,将其重命名为'sys.ini',并替换{filePath}.", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();

        }
        private void Cancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == 1 && e.RowIndex>0) {
                RisistGridView[e.ColumnIndex, e.RowIndex].ReadOnly = true;
            }
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            MessageBox.Show(@"请输入一个正确的数字,如: 
1.2345,1.2345e-3,1.2353E+04", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }

       
    }
}
