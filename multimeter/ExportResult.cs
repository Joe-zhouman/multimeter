using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Model;

namespace multimeter {
    public partial class SetupTest {
        private void ExportResult_Click(object sender, EventArgs e) {
            Bitmap bitmap = new Bitmap(1250, 855);
            switch (_method) {
                case TestMethod.KAPPA:
                {
                    pictureTest1.BringToFront();
                    TextGroupbox1.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                    pictureTest1.SendToBack();
                    }
                    break;
                case TestMethod.ITC: {
                    pictureTest2.BringToFront();
                    TextGroupbox2.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                    pictureTest2.SendToBack();
                    }
                    break;
                case TestMethod.ITM: {
                    pictureTest3.BringToFront();
                    TextGroupbox3.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                    pictureTest3.SendToBack();
                    }
                    break;
                case TestMethod.ITMS: {
                    pictureTest4.BringToFront();
                    TextGroupbox4.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                    pictureTest4.SendToBack();
                    }
                    break;
                default: {
                    return;
                }
            }

            try {
                saveFileDialog1.Filter = @"bmp 文件(.BMP) |*.BMP|All File(*.*)|*.*|bmp 文件 （.jpg) |*.jpg";
                saveFileDialog1.ShowDialog();
                string filepath = saveFileDialog1.FileName;
                bitmap.Save(filepath);
                MessageBox.Show(@"导出结果成功", @"提示", MessageBoxButtons.OK);
            }
            catch {
                MessageBox.Show(@"请正确选择路径", @"提示", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }
    }
}
