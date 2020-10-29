using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace multimeter
{
    public static class TestFileRead
    {

        public static string[,] ReadtData(string FileName,int linNum, string TestChoose) {
            int lin = 0;                 //行
            int column = 0;              //列
            int i=0, j=0;
            string[,] d=new string[50,20];  //最大范围限制
            try {
                StreamReader read = new StreamReader(FileName, Encoding.Default);
                while (!read.EndOfStream){
                    string[] sArray = read.ReadLine().Split(',');
                    foreach(string k in sArray){
                        d[i,j] = k;
                        j++;
                    }
                    i++;
                    lin = Math.Max(lin,i); column =Math.Max(column,j);
                    j = 0;
                }

                TestChoose = d[0, 0];
                read.DiscardBufferedData();
                read.Close();      
            }
            catch (System.Exception ex){
                //MessageBox.Show(ex.ToString());
            }

        int linBegin;
        if (lin - 1 > linNum) linBegin = lin - 1 - linNum;
        else{
            linBegin = 0;
            linNum = lin - 1;
        }
        string[,] Out = new string[linNum, column-1];
        for (int m = 0; m < linNum; m++)
            for (int n = 0; n < column-1; n++)
                Out[m, n] = d[linBegin+m, n+1];

        return Out;
        }//读取.csv文件数据


    }
}
