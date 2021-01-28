﻿// Joe
// 周漫
// 2021012419:35

using System;
using System.Linq;

namespace Model {
    public class Thermistor : Probe
    {

        public Thermistor() {
            Paras = new[] {0.0, 0.0, 0.0};
            Temp = 0;
        }

        public override void SetTemp(double resistance) {
            double tempVar = Math.Log(resistance);
            Temp = Math.Round(
                -273.15 + 1 / (Paras[0] + Paras[1] * tempVar +
                               Paras[2] * Math.Pow(tempVar, 3)), 4);
        }
    }
}