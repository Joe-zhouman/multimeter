// Joe
// 周漫
// 2021012419:11

namespace Model {
    public class AppCfg {
        public SerialPortPara SerialPortPara;
        public SysPara SysPara;

        public AppCfg() {
            SerialPortPara = new SerialPortPara(); 
            SysPara = new SysPara();
        }
    }
}