using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace multimeter
{
    public partial class TestResultTemp : Form
    {
        public TestResultTemp()
        {
            InitializeComponent();
        }

        private void TestResultTemp_Load(object sender, EventArgs e)
        {
            //ViewGroupBox1.Size = new Size(1531, 966);
            ViewGroupBox2.Size = new Size(607, 811);
            //ViewGroupBox3.Size = new Size(0, 0);
            //ViewGroupBox4.Size = new Size(0, 0);
        }
    }
}
