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
using DataProcessor;

namespace multimeter {
    public partial class SetupTest {
        private void ExportResult_Click(object sender, EventArgs e) {
            Bitmap bitmap = new Bitmap(1250, 855);
            switch (_method) {
                case TestMethod.KAPPA:
                {
                    pictureTest1.BringToFront();
                    TextGroupbox1.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                }
                    break;
                case TestMethod.ITC: {
                    TextGroupbox2.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                }
                    break;
                case TestMethod.ITM: {
                    TextGroupbox3.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                }
                    break;
                case TestMethod.ITMS: {
                    TextGroupbox4.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                }
                    break;
                default: {
                    return;
                }
            }

            try {
                saveFileDialog1.Filter = "bmp 文件(.BMP) |*.BMP|All File(*.*)|*.*|bmp 文件 （.jpg) |*.jpg";
                saveFileDialog1.ShowDialog();
                string filepath = saveFileDialog1.FileName;
                bitmap.Save(filepath);
            }
            catch {
                return;
            }

        }
    }
}
