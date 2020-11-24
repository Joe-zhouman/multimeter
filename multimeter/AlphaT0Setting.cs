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
            dataGridView.Columns.Add("alpha", "Alpha");
            dataGridView.Columns.Add("t0", "T0");
            for (int i = 201; i < 214; i++) {
                dataGridView.Rows.Add(i.ToString(), "", "");
            }
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sln.ini"); //在当前程序路径创建
            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                INIHelper.Write(dataGridView[0, i].Value.ToString(), "alpha", dataGridView[1, i].Value.ToString(),
                    filePath);
                INIHelper.Write(dataGridView[0, i].Value.ToString(), "T0", dataGridView[2, i].Value.ToString(),
                    filePath);
            }

            Close();

        }
        private void Cancel_Click(object sender, EventArgs e) {
            Close();
        }



    }
}
