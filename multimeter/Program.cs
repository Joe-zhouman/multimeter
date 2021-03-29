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
        //Program TodoList
        
        //TODO fix solution bugs
        //TODO 自定义自动保存文件夹
        //TODO 多类型探头测试可能有bug
        //TODO 收敛判断
        //TODO 查询更新情况，若有要求，跳转至更新页面
        //TODO 用户注册功能（数据库实现）
        //TODO 输出数据到数据库
        //TODO 帮助文档跳转对应位置

    }
}