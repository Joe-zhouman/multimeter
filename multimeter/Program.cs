using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
namespace multimeter
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SetupTest());
            Application.Run(new LogoLoad());
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args) {
            MessageBox.Show(@"很抱歉,程序由于未知错误崩溃,请您重启软件.为改善您之后的使用,请积极联系软件开发商以解决该问题.", @"致命", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            Exception e = (Exception)args.ExceptionObject;
            ILog logger = LogManager.GetLogger("Unexcepted Exception");
            logger.Fatal(e);
        }
    }
}
