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

namespace multimeter
{
    class conf_file
    {
        public int FileRead(int dataCnt, string FileName,out string[] d1)    //读取配置文件
        {
            string line = "";
            string[] d0 = new string[dataCnt];
            int i = 0;
            using (StreamReader sr = new StreamReader(FileName))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    d0[i] = line;
                    i++;              
                }
                d1 = d0;
            }
            MessageBox.Show("配置文件读取成功");
            return  0;
        }


        public int FileWrite(int dataCnt, string FileName,string[] d1)   //保存配置文件
        {
            using (StreamWriter sw = new StreamWriter(FileName))
            {
                foreach (string s in d1)
                {
                    sw.WriteLine(s);
                }
            }
            MessageBox.Show("配置文件保存成功");
            return 0;
        }

    }
}
