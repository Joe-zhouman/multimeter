// Joe
// 周漫
// 2021012512:01

using Model;

namespace BusinessLogic {
    public static class SerialPortOpt {
        public static string ResolveCmd(MultiMeterInfo multiMeter) {
            #region

            var st = @"*CLS 
*RST   
FORM:ELEM READ 
TRAC:CLE   
*ESE 0   
*SRE 1   
:STAT:MEAS:ENAB 512   
*s1*  
*s2*
*s3*  
:TEMP:TC:ODET OFF   
:TRAC:FEED SENS   
:TRAC:POIN *nchannel*  
INIT:CONT OFF 
TRIG:SOUR IMM 
TRIG:COUN 1   
TRIG:DEL 0  
SAMP:COUN *nchannel*  
ROUT:SCAN (@*allchannel*)
ROUT:SCAN:TSO IMM
ROUT:SCAN:LSEL INT";


            if (multiMeter.VoltageNum > 0) {
                var str = $@"SENS:FUNC 'VOLT:DC',(@{multiMeter.VoltageChn}
SENS:VOLT:DC:NPLC 1,(@{multiMeter.VoltageChn})
SNES:VOLT:DC:RANG:AUTO ON,(@{multiMeter.VoltageChn})";
                st = st.Replace("*s1*", str);
            }

//            if (multiMeter.ThermocoupleNum > 0) {
//                var str = $@"SENS:FUNC 'TEMP',(@{multiMeter.ThermocoupleChn})   
//SENS:TEMP:NPLC 1,(@{multiMeter.ThermocoupleChn})   
//:TEMP:TRAN TC,(@{multiMeter.ThermocoupleChn})   
//:TEMP:TC:TYPE K,(@{multiMeter.ThermocoupleChn})
//:TEMP:TC:ODET OFF";
//                st = st.Replace("*s2*", str);
//            }
            if (multiMeter.ThermocoupleNum > 0)
            {
                var str = $@"SENS: FUNC 'VOLT:DC',(@{multiMeter.ThermocoupleChn})   
SENS:VOLT:DC:NPLC 1,(@{multiMeter.ThermocoupleChn})   
SNES:VOLT:DC:RANG:AUTO ON,(@{multiMeter.ThermocoupleChn})";
                st = st.Replace("*s2*", str);
            }
            if (multiMeter.ResistanceNum > 0) {
                var str = $@"SENS:FUNC 'FRES',(@{multiMeter.ResistanceChn})   
SENS:RES:NPLC 1,(@{multiMeter.ResistanceChn})   
SENS:RES:RANG:AUTO ON,(@{multiMeter.ResistanceChn})";
                st = st.Replace("*s3*", str);
            }

            if (multiMeter.TotalNum > 0) st = st.Replace("*nchannel*", multiMeter.TotalNum.ToString());
            st = st.Replace("*allchannel*", multiMeter.TotalChn);
            return st;

            #endregion
        }
    }
}