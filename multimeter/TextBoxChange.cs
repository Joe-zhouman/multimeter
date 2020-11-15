using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DataProcessor;
using System.IO;
using CCWin;
using CCWin.SkinControl;
using System.Runtime.InteropServices;
using System.Threading;

namespace multimeter {
    public partial class SetupTest{
        private void HeatMeterToBox(HeatMeter heatMeter, List<TextBox> positionBoxes, List<TextBox> channelBoxes,TextBox diameterBox,
            TextBox kappaBox) {
            for (int i = 0; i < 4; i++) {
                positionBoxes[i].Text = heatMeter.Position[i];
                channelBoxes[i].Text = heatMeter.Channel[i];
            }
            diameterBox.Text = heatMeter.Diameter;
            kappaBox.Text = heatMeter.Kappa;
        }
        private void SampleToBox(Sample sample, List<TextBox> positionBoxes, List<TextBox> channelBoxes, TextBox diameterBox
            ) {
            for (int i = 0; i < 3; i++) {
                positionBoxes[i].Text = sample.Position[i];
                channelBoxes[i].Text = sample.Channel[i];
            }diameterBox.Text = sample.Diameter;
        }
        private void BoxToHeatMeter(ref HeatMeter heatMeter, List<TextBox> positionBoxes, List<TextBox> channelBoxes,
            TextBox kappaBox) {
            for (int i = 0; i < 4; i++) {
                heatMeter.Position[i] = positionBoxes[i].Text;
                heatMeter.Channel[i] = channelBoxes[i].Text;
            }

            heatMeter.Kappa = kappaBox.Text;
        }
        private void BoxToSample(ref Sample sample, List<TextBox> positionBoxes, List<TextBox> channelBoxes,TextBox diameterBox
        ) {
            for (int i = 0; i < 3; i++) {
                sample.Position[i] = positionBoxes[i].Text;
                sample.Channel[i] = channelBoxes[i].Text;
            }
            sample.Diameter = diameterBox.Text;
        }
    }
}