using System;
using System.Windows.Forms;
using log4net;

namespace multimeter {
    internal static class Program {
        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
            //Application.Run(new SetupTest());
            Application.Run(new LogoLoad());
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args) {
            MessageBox.Show(@"很抱歉,程序由于未知错误崩溃,请您重启软件.为改善您之后的使用,请积极联系软件开发商以解决该问题.", @"致命", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Error);
            var e = (Exception) args.ExceptionObject;
            var logger = LogManager.GetLogger("Unexcepted Exception");
            logger.Fatal(e);
        }
    }
}