// Joe
// 周漫
// 2021012419:11

namespace Model {
    public class AppCfg {
        /// <summary>
        /// 串口配置
        /// </summary>
        public SerialPortPara SerialPortPara;
        /// <summary>
        /// 系统配置
        /// </summary>
        public SysPara SysPara;

        public AppCfg() {
            SerialPortPara = new SerialPortPara();
            SysPara = new SysPara();
        }
    }
}