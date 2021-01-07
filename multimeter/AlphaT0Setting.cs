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
using DataProcessor;

namespace multimeter
{
    public partial class AlphaT0Setting : Form
    {
        public AlphaT0Setting()
        {
            InitializeComponent();
        }

        private void AlphaT0Setting_Load(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Add("channel", "Channel");
            dataGridView.Columns.Add("A0", $"A\x2080");
            dataGridView.Columns.Add("A1", $"A\x2081");
            dataGridView.Columns.Add("A3", $"A\x2083");

            for (int i = 0; i < 4; i++) {
                dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < 13; i++) {
                dataGridView.Rows.Add((201+i).ToString(), "", "");
                dataGridView["channel", i].ReadOnly = true;
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
                dataGridView[1,i].Value = INIHelper.Read(dataGridView[0, i].Value.ToString(), "A0", dataGridView[1, i].Value.ToString(),
                    filePath);
                dataGridView[1, i].ValueType = typeof(double);
                dataGridView[2, i].Value = INIHelper.Read(dataGridView[0, i].Value.ToString(), "A1", dataGridView[1, i].Value.ToString(),
                    filePath);
                dataGridView[2, i].ValueType = typeof(double);
                dataGridView[3, i].Value = INIHelper.Read(dataGridView[0, i].Value.ToString(), "A3", dataGridView[1, i].Value.ToString(),
                    filePath);
                dataGridView[3, i].ValueType = typeof(double);
            }
            
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sln.ini"); //在当前程序路径创建
            string bakFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bak", $"sln.ini.{DateTime.Now:yyyy-MM-dd-ss-ffff}.bak");
            File.Copy(filePath, bakFilePath);
            for (int i = 0; i < dataGridView.Rows.Count - 1; i++) {
                INIHelper.Write(dataGridView[0, i].Value.ToString(), "A0", dataGridView[1, i].Value.ToString(),
                    filePath);
                INIHelper.Write(dataGridView[0, i].Value.ToString(), "A1", dataGridView[2, i].Value.ToString(),
                    filePath);
                INIHelper.Write(dataGridView[0, i].Value.ToString(), "A3", dataGridView[3, i].Value.ToString(),
                    filePath);
                
            }
            MessageBox.Show($@"修改成功!
配置文件备份在{bakFilePath}.
如需找回配置,将其重命名为'sln.ini',并替换{filePath}.", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();

        }
        private void Cancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0&&e.RowIndex>0) {
                dataGridView[e.ColumnIndex, e.RowIndex].ReadOnly = true;
            }
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(@"请输入一个正确的数字,如: 
1.2345,1.2345e-3,1.2353E+04", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }
    }
}
