using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace multimeter
{
    static class DataResult
    {
        public static void Test1DataProcess(double [] T,double[] HMLocation, double A1, double A2, double As, double k1, double k2,
                                     double k_s)
        {
            #region //导热系数测量
            double Q_u, Q_s, Q_l;                                                                                     //定义“流量计”“试件”热流量
            //以下代码为读取热电偶位置值，单位mm,转换为m
            double L_u1 = 0.001 * HMLocation[0];
            double L_u2 = 0.001 * HMLocation[1];
            double L_u3 = 0.001 * HMLocation[2];
            double L_u4 = 0.001 * HMLocation[3];
            double L_s1 = 0.001 * HMLocation[4];
            double L_s2 = 0.001 * HMLocation[5];
            double L_s3 = 0.001 * HMLocation[6];
            double L_l1 = 0.001 * HMLocation[7];
            double L_l2 = 0.001 * HMLocation[8];
            double L_l3 = 0.001 * HMLocation[9];
            double L_l4 = 0.001 * HMLocation[10];

            //以下代码计算“上流量计”热流量Q_u ，热导率为 k1 ,温度为T[0], T[1], T[2], T[3]，热电偶位置为L_u1，L_u2，L_u3，L_u4 ,线性拟合方程为Y_u = b_u * X_u + a_u
            double[] x_u = new double[4] { 0, L_u1, L_u1 + L_u2, L_u1 + L_u2 + L_u3 };
            double[] y_u = new double[4] { T[0], T[1], T[2], T[3] };
            double a_u, b_u, maxErr_u;
            if (CalcRegress(x_u, y_u, 4, out a_u, out b_u, out maxErr_u) != 0)
            {
                MessageBox.Show("计算出错！");
                return ;
            }
            else
            {
                // MessageBox.Show("Y_u = " + b_u.ToString() + " X_u +" + a_u.ToString() + "   maxErr_u=" + maxErr_u.ToString());
                Q_u = 0.000001* A1* k1 * Math.Abs(b_u);

            }

            //以下代码计算“下流量计”热流量Q_l ，热导率为 k2 ,温度为T[7], T[8], T[9], T[10] ，热电偶位置为L_l1，L_l2，L_l3，L_l4 ,线性拟合方程为Y_l = b_l * X_l + a_l
            double[] x_l = new double[4] { 0, L_l2, L_l2 + L_l3, L_l2 + L_l3 + L_l4 };
            double[] y_l = new double[4] { T[7], T[8], T[9], T[10] };
            double a_l, b_l, maxErr_l;
            if (CalcRegress(x_l, y_l, 4, out a_l, out b_l, out maxErr_l) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                // MessageBox.Show("Y_l = " + b_l.ToString() + " X_l +" + a_l.ToString() + "   maxErr_l=" + maxErr_l.ToString());
                Q_l = 0.000001 * A2 * k2 * Math.Abs(b_l);
            }

            //以下代码计算“试件”导热系数 k_s ,其中热流量Q_s ，温度为 T[4], T[5], T[6]，热电偶位置为L_s1，L_s2，L_s3 ,线性拟合方程为Y_s = b_s * X_s + a_s
            Q_s = (Q_u + Q_l) * 0.5;
            double[] x_s = new double[3] { 0, L_s1, L_s1 + L_s2 };
            double[] y_s = new double[3] { T[4], T[5], T[6] };
            double a_s, b_s, maxErr_s;
            if (CalcRegress(x_s, y_s, 3, out a_s, out b_s, out maxErr_s) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_s = " + b_s.ToString() + " X_s +" + a_s.ToString() + "   maxErr_s=" + maxErr_s.ToString());
                k_s = Q_s /(0.000001 * As * Math.Abs(b_s));  //求得“试件”导热系数 k_s ，单位为w/(m*k)
                                            // MessageBox.Show("k_s = " + k_s.ToString() + "w/(m*k)");
            }
            #endregion
        }

        public static void Test2DataProcess(double[] T, double[] HMLocation, double k1, double k2, double A1, double A2, double Asu, double Asl, 
                                     double ks1, double ks2 , double contactR, Chart chart)
        {
            #region //固_固接触热阻测量
            double Q_u, Q_s, Q_l;                                                                                     //定义“流量计”“试件”热流量

            //以下代码为读取热电偶位置值，单位mm，转换为m
            double L_u1 = 0.001 * HMLocation[0];
            double L_u2 = 0.001 * HMLocation[1];
            double L_u3 = 0.001 * HMLocation[2];
            double L_u4 = 0.001 * HMLocation[3];
            double L_su1 = 0.001 * HMLocation[4];
            double L_su2 = 0.001 * HMLocation[5];
            double L_su3 = 0.001 * HMLocation[6];
            double L_sl1 = 0.001 * HMLocation[7];
            double L_sl2 = 0.001 * HMLocation[8];
            double L_sl3 = 0.001 * HMLocation[9];
            double L_l1 = 0.001 * HMLocation[10];
            double L_l2 = 0.001 * HMLocation[11];
            double L_l3 = 0.001 * HMLocation[12];
            double L_l4 = 0.001 * HMLocation[13];

            //以下代码计算“上流量计”热流量Q_u ，热导率为 k1 ,温度为T[0], T[1], T[2], T[3] ，热电偶位置为L_u1，L_u2，L_u3，L_u4 ,线性拟合方程为Y_u = b_u * X_u + a_u
            double[] x_u = new double[4] { 0, L_u1, L_u1 + L_u2, L_u1 + L_u2 + L_u3 };
            double[] y_u = new double[4] { T[0], T[1], T[2], T[3] };
            double a_u, b_u, maxErr_u;
            if (CalcRegress(x_u, y_u, 4, out a_u, out b_u, out maxErr_u) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_u = " + b_u.ToString() + " X_u +" + a_u.ToString() + "   maxErr_u=" + maxErr_u.ToString());
                Q_u = 0.000001* A1 * k1 * Math.Abs(b_u);

            }

            //以下代码计算“下流量计”热流量Q_l ，热导率为 k2 ,温度为T[10], T[11], T[12], T[13] ，热电偶位置为L_l1，L_l2，L_l3，L_l4 ,线性拟合方程为Y_l = b_l * X_l + a_l
            double[] x_l = new double[4] { 0, L_l2, L_l2 + L_l3, L_l2 + L_l3 + L_l4 };
            double[] y_l = new double[4] { T[10], T[11], T[12], T[13] };
            double a_l, b_l, maxErr_l;
            if (CalcRegress(x_l, y_l, 4, out a_l, out b_l, out maxErr_l) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_l = " + b_l.ToString() + " X_l +" + a_l.ToString() + "   maxErr_l=" + maxErr_l.ToString());
                Q_l = 0.000001* A2 * k2 * Math.Abs(b_l);
            }

            //以下代码计算“试件1”接触面温度contactT_u，导热系数 ks1 ,其中热流量Q_s ，温度为 T[4], T[5], T[6] ，热电偶位置为L_su1，L_su2，L_su3 ,线性拟合方程为Y_su = b_su * X_su + a_su
            Q_s = (Q_u + Q_l) * 0.5;
            double[] x_su = new double[3] { 0, L_su1, L_su1 + L_su2 };
            double[] y_su = new double[3] { T[4], T[5], T[6] };
            double a_su, b_su, maxErr_su, contactT_u;
            if (CalcRegress(x_su, y_su, 3, out a_su, out b_su, out maxErr_su) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_su = " + b_su.ToString() + " X_su +" + a_su.ToString() + "   maxErr_su=" + maxErr_su.ToString());
                contactT_u = b_su * (L_su1 + L_su2 + L_su3) + a_su;
                ks1 = Q_s /(0.000001 * Asu * Math.Abs(b_su));            //计算“试件1”热导率，单位w/((m*k)
            }

            //以下代码计算“试件2”接触面温度contactT_l，导热系数 ks2 ,其中热流量Q_s ，温度为T[7], T[8], T[9]  ，热电偶位置为L_sl1，L_sl2，L_sl3 ,线性拟合方程为Y_sl = b_sl * X_sl + a_sl
            double[] x_sl = new double[3] { L_sl1, L_sl1 + L_sl2, L_sl1 + L_sl2 + L_sl3 };
            double[] y_sl = new double[3] { T[7], T[8], T[9] };
            double a_sl, b_sl, maxErr_sl, contactT_l;
            if (CalcRegress(x_sl, y_sl, 3, out a_sl, out b_sl, out maxErr_sl) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_sl = " + b_sl.ToString() + " X_sl +" + a_sl.ToString() + "   maxErr_sl=" + maxErr_sl.ToString());
                contactT_l = a_sl;
                ks2 = Q_s /(0.000001 * Asl* Math.Abs(b_sl));             //计算“试件2”热导率，单位w/((m*k)
            }

            contactR =(0.5 * 0.000001 * (Asl+Asu)* Math.Abs(contactT_u - contactT_l)) / Q_s;                                                   //代码计算试件1、2的接触热阻，单位(m^2 * K)/W
            //MessageBox.Show(contactR.ToString() + "(m^2 * K)/W");


            double[] Tchart_x = new double[6] { 0, L_su1, L_su1 + L_su2,
                                                L_su1 + L_su2 + L_su3 + L_sl1,
                                                L_su1 + L_su2 + L_su3 + L_sl1 + L_sl2,
                                                L_su1 + L_su2 + L_su3 + L_sl1 + L_sl2 + L_sl3 };

            double[] Tchart_y = new double[6] { T[4], T[5], T[6], T[7], T[8], T[9] };
            Test1ViewPoint(Tchart_x, Tchart_y, 6, chart); //绘制温度点
            Test1ViewLine(0, L_su1 + L_su2 + L_su3, a_su, b_su,
                          L_su1 + L_su2 + L_su3, Tchart_x[5], a_sl, b_sl, chart); //绘制拟合曲线  Y_sl = b_sl * X_sl + a_sl
            #endregion
        }
        public static void Test3DataProcess(double[] T, double[] HMLocation, double Thickness, double k1, double k2, double A1, double A2,  
                                     double k_s, double contactR, Chart chart)
        {
            #region //热界面材料测量_热流计间
            double Q_u, Q_s, Q_l;                                                                                     //定义“流量计”“试件”热流量
            double Thickness_s = 0.000001 * Thickness;                                                                //定义热界面材料厚度，单位um，转换为m

            //以下代码为读取热电偶位置值，单位mm，转换为m
            double L_u1 = 0.001 * HMLocation[0];
            double L_u2 = 0.001 * HMLocation[1];
            double L_u3 = 0.001 * HMLocation[2];
            double L_u4 = 0.001 * HMLocation[3];
            double L_l1 = 0.001 * HMLocation[4];
            double L_l2 = 0.001 * HMLocation[5];
            double L_l3 = 0.001 * HMLocation[6];
            double L_l4 = 0.001 * HMLocation[7];

            //以下代码计算“上流量计”热流量Q_u ，“热界面材料”上部接触温度为contactT_u，热导率为 k1 ,温度为T[0], T[1], T[2], T[3] ，热电偶位置为L_u1，L_u2，L_u3，L_u4 ,线性拟合方程为Y_u = b_u * X_u + a_u
            double[] x_u = new double[4] { 0, L_u1, L_u1 + L_u2, L_u1 + L_u2 + L_u3 };
            double[] y_u = new double[4] { T[0], T[1], T[2], T[3] };
            double a_u, b_u, maxErr_u, contactT_u;
            if (CalcRegress(x_u, y_u, 4, out a_u, out b_u, out maxErr_u) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_u = " + b_u.ToString() + " X_u +" + a_u.ToString() + "   maxErr_u=" + maxErr_u.ToString());
                Q_u =0.000001*A1* k1 * Math.Abs(b_u);
                contactT_u = b_u * (L_u1 + L_u2 + L_u3 + L_u4) + a_u;

            }

            //以下代码计算“下流量计”热流量Q_l ，“热界面材料”下部接触温度为contactT_l，热导率为 k2 ,温度为 T[4], T[5], T[6], T[7] ，热电偶位置为L_l1，L_l2，L_l3，L_l4 ,线性拟合方程为Y_l = b_l * X_l + a_l
            double[] x_l = new double[4] { L_l1, L_l1 + L_l2, L_l1 + L_l2 + L_l3, L_l1 + L_l2 + L_l3 + L_l4 };
            double[] y_l = new double[4] { T[4], T[5], T[6], T[7] };
            double a_l, b_l, maxErr_l, contactT_l;
            if (CalcRegress(x_l, y_l, 4, out a_l, out b_l, out maxErr_l) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_l = " + b_l.ToString() + " X_l +" + a_l.ToString() + "   maxErr_l=" + maxErr_l.ToString());
                Q_l = 0.000001*A2*k2 * Math.Abs(b_l);
                contactT_l = a_l;
            }

            //以下代码计算“热界面材料”热阻contactR和导热系数 k_s ,其中热流量Q_s
            Q_s = (Q_u +Q_l) * 0.5;
            contactR = (0.000001*0.5*(A1+ A2) *Math.Abs(contactT_u - contactT_l) )/ Q_s;
            k_s = (Thickness_s * Q_s) / (0.000001 * 0.5 * (A1 + A2) * Math.Abs(contactT_u - contactT_l));
            //MessageBox.Show("接触热阻=" + contactR.ToString() + "(m^2)K/W  \n" + "导热系数=" + k_s.ToString() + "W/(m*K)");


            double[] Tchart_x = new double[8] { 0, L_u1, L_u1 + L_u2, L_u1 + L_u2 + L_u3,
                                               L_u1 + L_u2 + L_u3 + L_u4 + Thickness_s + L_l1,
                                               L_u1 + L_u2 + L_u3 + L_u4 + Thickness_s + L_l1 + L_l2,
                                               L_u1 + L_u2 + L_u3 + L_u4 + Thickness_s + L_l1 + L_l2 + L_l3,
                                               L_u1 + L_u2 + L_u3 + L_u4 + Thickness_s + L_l1 + L_l2 + L_l3 + L_l4 };

            double[] Tchart_y = new double[8] { T[0], T[1], T[2], T[3], T[4], T[5], T[6], T[7] };
            Test1ViewPoint(Tchart_x, Tchart_y, 8, chart); //绘制温度点
            Test1ViewLine(0, L_u1 + L_u2 + L_u3 + L_u4, a_u, b_u,
                                  L_u1 + L_u2 + L_u3 + L_u4 + Thickness_s, Tchart_x[7], a_l, b_l, chart); //绘制拟合曲线  Y_sl = b_sl * X_sl + a_sl

            #endregion
        }
        public static void Test4DataProcess(double[] T, double[] HMLocation, double Thickness, double k1, double k2, double A1, double A2, double Asu, double Asl,  
                                     double ks1, double ks2, double k_s, double contactR, Chart chart)
        {
            #region //热界面材料测量_试件间
            double  Q_u, Q_s, Q_l;                                                                                     //定义“流量计”“试件”热流量
            double Thickness_s = 0.000001 * Thickness;                                                               //定义热界面材料厚度，单位um，转换为m

            //以下代码为读取热电偶位置值，单位mm，转换为m
            double L_u1 = 0.001 * HMLocation[0];
            double L_u2 = 0.001 * HMLocation[1];
            double L_u3 = 0.001 * HMLocation[2];
            double L_u4 = 0.001 * HMLocation[3];
            double L_su1 = 0.001 * HMLocation[4];
            double L_su2 = 0.001 * HMLocation[5];
            double L_su3 = 0.001 * HMLocation[6];
            double L_sl1 = 0.001 * HMLocation[7];
            double L_sl2 = 0.001 * HMLocation[8];
            double L_sl3 = 0.001 * HMLocation[9];
            double L_l1 = 0.001 * HMLocation[10];
            double L_l2 = 0.001 * HMLocation[11];
            double L_l3 = 0.001 * HMLocation[12];
            double L_l4 = 0.001 * HMLocation[13];

            //以下代码计算“上流量计”热流量Q_u ，热导率为 k1 ,温度为 T[0], T[1], T[2], T[3] ，热电偶位置为L_u1，L_u2，L_u3，L_u4 ,线性拟合方程为Y_u = b_u * X_u + a_u
            double[] x_u = new double[4] { 0, L_u1, L_u1 + L_u2, L_u1 + L_u2 + L_u3 };
            double[] y_u = new double[4] { T[0], T[1], T[2], T[3] };
            double a_u, b_u, maxErr_u;
            if (CalcRegress(x_u, y_u, 4, out a_u, out b_u, out maxErr_u) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_u = " + b_u.ToString() + " X_u +" + a_u.ToString() + "   maxErr_u=" + maxErr_u.ToString());
                Q_u = 0.000001*A1*k1 * Math.Abs(b_u);

            }

            //以下代码计算“下流量计”热流量Q_l ，热导率为 k2 ,温度为T[10], T[11], T[12], T[13]  ，热电偶位置为L_l1，L_l2，L_l3，L_l4 ,线性拟合方程为Y_l = b_l * X_l + a_l
            double[] x_l = new double[4] { 0, L_l2, L_l2 + L_l3, L_l2 + L_l3 + L_l4 };
            double[] y_l = new double[4] { T[10], T[11], T[12], T[13] };
            double a_l, b_l, maxErr_l;
            if (CalcRegress(x_l, y_l, 4, out a_l, out b_l, out maxErr_l) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_l = " + b_l.ToString() + " X_l +" + a_l.ToString() + "   maxErr_l=" + maxErr_l.ToString());
                Q_l = 0.000001*A2*k2 * Math.Abs(b_l);
            }

            //以下代码计算“试件1”接触面温度contactT_u，导热系数 ks1 ,其中热流量Q_s ，温度为T[4], T[5], T[6]，热电偶位置为L_su1，L_su2，L_su3 ,线性拟合方程为Y_su = b_su * X_su + a_su
            Q_s = (Q_u + Q_l) * 0.5;
            double[] x_su = new double[3] { 0, L_su1, L_su1 + L_su2 };
            double[] y_su = new double[3] { T[4], T[5], T[6] };
            double a_su, b_su, maxErr_su, contactT_u;
            if (CalcRegress(x_su, y_su, 3, out a_su, out b_su, out maxErr_su) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_su = " + b_su.ToString() + " X_su +" + a_su.ToString() + "   maxErr_su=" + maxErr_su.ToString());
                contactT_u = b_su * (L_su1 + L_su2 + L_su3) + a_su;
                ks1 = Q_s / (0.000001*Asu*Math.Abs(b_su));                                  //计算“试件1”热导率，单位w/((m*k)
            }

            //以下代码计算“试件2”接触面温度contactT_l，导热系数 ks2 ,其中热流量Q_s ，温度为T[7], T[8], T[9] ，热电偶位置为L_sl1，L_sl2，L_sl3 ,线性拟合方程为Y_sl = b_sl * X_sl + a_sl
            double[] x_sl = new double[3] { L_sl1, L_sl1 + L_sl2, L_sl1 + L_sl2 + L_sl3 };
            double[] y_sl = new double[3] { T[7], T[8], T[9] };
            double a_sl, b_sl, maxErr_sl, contactT_l;
            if (CalcRegress(x_sl, y_sl, 3, out a_sl, out b_sl, out maxErr_sl) != 0)
            {
                MessageBox.Show("计算出错！");
                return;
            }
            else
            {
                //MessageBox.Show("Y_sl = " + b_sl.ToString() + " X_sl +" + a_sl.ToString() + "   maxErr_sl=" + maxErr_sl.ToString());
                contactT_l = a_sl;
                ks2 =Q_s /(0.000001*Asl* Math.Abs(b_sl));                                  //计算“试件2”热导率，单位w/((m*k)
            }

            //以下代码计算“热界面材料_试件间”热阻contactR和导热系数 k_s ,其中热流量Q_s 
            contactR = (0.000001*0.5*(Asl+Asu)*Math.Abs(contactT_u - contactT_l) )/ Q_s;                                                   //计算试件1、2的接触热阻，单位(m^2*K)/W 
            k_s = (Thickness_s * Q_s) / ((0.000001 * 0.5 * (Asl + Asu) * Math.Abs(contactT_u - contactT_l)));                     //计算热界面材料导热系数，单位W/(m*K)
            //MessageBox.Show("接触热阻=" + contactR.ToString() + "(m^2*K)/W  \n" + "导热系数=" + k_s.ToString() + "W/(m*K)");

            double[] Tchart_x = new double[6] { 0, L_su1, L_su1 + L_su2,
                                                L_su1 + L_su2 + L_su3 + Thickness_s + L_sl1,
                                                L_su1 + L_su2 + L_su3 + Thickness_s + L_sl1 + L_sl2,
                                                L_su1 + L_su2 + L_su3 + Thickness_s + L_sl1 + L_sl2 + L_sl3 };

            double[] Tchart_y = new double[6] { T[4], T[5], T[6], T[7], T[8], T[9] };
            Test1ViewPoint(Tchart_x, Tchart_y, 6, chart); //绘制温度点

            Test1ViewLine(0, L_su1 + L_su2 + L_su3, a_su, b_su,
                          L_su1 + L_su2 + L_su3 + Thickness_s, Tchart_x[5], a_sl, b_sl, chart); //绘制拟合曲线  Y_sl = b_sl * X_sl + a_sl
            #endregion
        }
        private static int CalcRegress(double[] x, double[] y, int dataCnt, out double a, out double b, out double maxErr) //最小二乘法线性回归
        {
            #region //拟合函数
            double sumX = 0;
            double sum_y = 0;
            double avgX;
            double avg_y;

            if (dataCnt < 2)
            {
                a = 0;
                b = 0;
                maxErr = 0;
                return -1;
            }

            for (int i = 0; i < dataCnt; i++)
            {
                sumX += x[i];
                sum_y += y[i];
            }

            avgX = sumX / dataCnt;
            avg_y = sum_y / dataCnt;

            double SPxy = 0;
            double SSx = 0;

            for (int i = 0; i < dataCnt; i++)
            {
                SPxy += (x[i] - avgX) * (y[i] - avg_y);
                SSx += (x[i] - avgX) * (x[i] - avgX);
            }

            if (SSx == 0)
            {
                a = 0;
                b = 0;
                maxErr = 0;
                return -1;
            }
            b = SPxy / SSx;
            a = avg_y - b * avgX;
            //下面代码计算最大偏差            
            maxErr = 0;
            for (int i = 0; i < dataCnt; i++)
            {
                double yi = a + b * x[i];
                double absErrYi = Math.Abs(yi - y[i]);

                if (absErrYi > maxErr)
                {
                    maxErr = absErrYi;
                }
            }
            return 0;

            /*拟合函数测试
              double[] x = new double[2]{0,1};
              double[] y = new double[2]{1,3};
              double a1, b1, maxErr;
              int dataCnt = 2;
              if (CalcRegress(x, y, dataCnt, out a1, out b1, out maxErr) != 0)
              {
                  MessageBox.Show("计算出错！");
                  return;
              }
              else
              {
                  MessageBox.Show("y=" + b1.ToString() + "x+" + a1.ToString() + "   maxErr=" + maxErr.ToString());
              }
              */
            #endregion
        }

        private static int Test1ViewPoint(double[] x, double[] y, int dataCnt, Chart chart) //绘制温度离散点
        {
            #region  //绘制温度离散点
            Series T_point = chart.Series[0];
            T_point.Points.Clear();
            for (int i = 0; i < dataCnt; i++)
            {
                T_point.Points.AddXY(x[i], y[i]);
            }
            return 0;
            #endregion
        }

        private static int Test1ViewLine(double x1_1, double x1_2, double a1, double b1,
                                  double x2_1, double x2_2, double a2, double b2, Chart chart) //绘制拟合曲线  Y_sl = b_sl * X_sl + a_sl
        {
            #region  //绘制拟合曲线  Y_sl = b_sl * X_sl + a_sl
            Series T_fittingLine = chart.Series[1];
            T_fittingLine.Points.Clear();
            for (double i = x1_1 - 0.01; i < x1_2;)
            {
                T_fittingLine.Points.AddXY(i, (b1 * i + a1));
                i += 0.002;
            }
            T_fittingLine.Points.AddXY(x1_2, (b1 * x1_2 + a1));
            for (double i = x2_1; i < x2_2 + 0.01;)
            {
                T_fittingLine.Points.AddXY(i, (b2 * (i - x2_1) + a2));
                i += 0.002;
            }
            return 0;
            #endregion
        }

    }
}
