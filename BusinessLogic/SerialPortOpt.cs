// Joe
// 周漫
// 2021012512:01

using Model;

namespace BusinessLogic {
    public static class SerialPortOpt {
        /// <summary>
        /// 根据测试设置生成下位机控制命令
        /// </summary>
        /// <remarks>热电偶功能未实装</remarks>
        /// <param name="multiMeter">测试设置</param>
        /// <returns>控制命令字符串</returns>
        public static string ResolveCmd(MultiMeterInfo multiMeter) {
            #region
            var voltageTestCmd = multiMeter.VoltageNum <= 0 ? "" : $@"
SENS:FUNC 'VOLT:DC',(@{multiMeter.VoltageChn}
SENS:VOLT:DC:NPLC 1,(@{multiMeter.VoltageChn})
SNES:VOLT:DC:RANG:AUTO ON,(@{multiMeter.VoltageChn})";

            //按要求,热电偶功能未实装.实际功能如下 
            /*
                        var thermocoupleTestCmd = multiMeter.ThermocoupleNum <= 0 ? "" : $@"
            SENS:FUNC 'TEMP',(@{multiMeter.ThermocoupleChn})   
            SENS:TEMP:NPLC 1,(@{multiMeter.ThermocoupleChn})   
            :TEMP:TRAN TC,(@{multiMeter.ThermocoupleChn})   
            :TEMP:TC:TYPE K,(@{multiMeter.ThermocoupleChn})
            :TEMP:TC:ODET OFF";
            */
            var thermocoupleTestCmd = multiMeter.ThermocoupleNum <= 0 ? "" : $@"
SENS: FUNC 'VOLT:DC',(@{multiMeter.ThermocoupleChn})   
SENS:VOLT:DC:NPLC 1,(@{multiMeter.ThermocoupleChn})   
SNES:VOLT:DC:RANG:AUTO ON,(@{multiMeter.ThermocoupleChn})";

            var fourResisTestCmd = multiMeter.ResistanceNum <= 0 ? "" : $@"
SENS:FUNC 'FRES',(@{multiMeter.ResistanceChn})   
SENS:RES:NPLC 1,(@{multiMeter.ResistanceChn})   
SENS:RES:RANG:AUTO ON,(@{multiMeter.ResistanceChn})";

            var numChannel = multiMeter.TotalNum <= 0 ? "" : multiMeter.TotalNum.ToString();
            var channelList = multiMeter.TotalNum <= 0 ? "" : multiMeter.TotalChn;
            return $@"*CLS
*RST
FORM:ELEM READ
TRAC:CLE
*ESE 0
*SRE 1
:STAT:MEAS:ENAB 512{voltageTestCmd}{thermocoupleTestCmd}{fourResisTestCmd}  
:TEMP:TC:ODET OFF
:TRAC:FEED SENS
:TRAC:POIN *nchannel*
INIT:CONT OFF
TRIG:SOUR IMM
TRIG:COUN 1
TRIG:DEL 0
SAMP:COUN {numChannel}
ROUT:SCAN (@{channelList})
ROUT:SCAN:TSO IMM
ROUT:SCAN:LSEL INT";

            //            if (multiMeter.ThermocoupleNum > 0) {
            //                var str = $@"SENS:FUNC 'TEMP',(@{multiMeter.ThermocoupleChn})   
            //SENS:TEMP:NPLC 1,(@{multiMeter.ThermocoupleChn})   
            //:TEMP:TRAN TC,(@{multiMeter.ThermocoupleChn})   
            //:TEMP:TC:TYPE K,(@{multiMeter.ThermocoupleChn})
            //:TEMP:TC:ODET OFF";
            //                st = st.Replace("*s2*", str);
            //            }

            #endregion
        }
    }
}