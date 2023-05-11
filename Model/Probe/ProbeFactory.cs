// Administrator
// multimeter
// Model
// 2021-03-23-10:47
// *Author*    : Joe, Zhou Man
// *Email*     : man.man.man.man.a@gmail.com
// *Email*     : joe_zhouman@foxmail.com
// *QQ*        : 1592020915
// *Weibo*     : @zhouman_LFC
// *Twitter*   : @zhouman_LFC
// *Website*   : www.joezhouman.com
// *Github*    : https://github.com/Joe-zhouman
// *LeetCode*  : https://leetcode-cn.com/u/joe_zm/

namespace Model.Probe {
    /// <summary>
    /// simple factory to create probe instance
    /// </summary>
    public static class ProbeFactory {
        public static ProbeBase Create(int i) {
            switch (i) {
                case 0:
                    return null;
                case 1:
                    return new CubicPolyVoltageProbe();
                case 2:
                    return new Thermocouple();
                case 3:
                    return new Thermistor();
                case 4:
                    return new CubicPolyResistanceProbe();
                default:
                    return null;
            }
        }
        public static ProbeBase Create(ProbeType type) {
            return Create((int)type);
        }
    }
}