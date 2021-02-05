using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMPlot.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            plot1.Title = "Hello, OMPlot!";

            //List<double> dx = new List<double>();
            //List<double> dy = new List<double>();
            //dx.Add(10); dy.Add(-100);
            //dx.Add(20); dy.Add(0);
            //dx.Add(45); dy.Add(-47);
            //dx.Add(53); dy.Add(335);
            //dx.Add(57); dy.Add(26);
            //dx.Add(62); dy.Add(387);
            //dx.Add(74); dy.Add(104);
            //dx.Add(89); dy.Add(0);
            //dx.Add(95); dy.Add(100);
            //dx.Add(100); dy.Add(0);
            //dx.Add(110); dy.Add(10);
            //dx.Add(100); dy.Add(200);
            //dx.Add(130); dy.Add(10);
            //dx.Add(120); dy.Add(30);
            //dx.Add(110); dy.Add(40);
            //dx.Add(100); dy.Add(40);
            //dx.Add(90); dy.Add(30);
            //dx.Add(100); dy.Add(100);

            //var p = plot1.Add(dx, dy);
            //p.MarkStyle = Data.MarkerStyle.SolidCircle;
            //p.LineStyle = Data.LineStyle.None;
            //var ppp = plot1.Add(dx, dy);
            //ppp.Interpolation = Data.PlotInterpolation.Spline;
            //var pppp = plot1.Add(dx, dy);
            //pppp.Interpolation = Data.PlotInterpolation.NewSpline;


            double[] sinX = new double[30];
            double[] sinY1 = new double[sinX.Length];
            double[] sinY2 = new double[sinX.Length];
            double[] sinY3 = new double[sinX.Length];
            double[] sinY4 = new double[sinX.Length];
            double[] sinY5 = new double[sinX.Length];
            double[] sinY6 = new double[sinX.Length];
            double[] sinY7 = new double[sinX.Length];
            double[] sinY8 = new double[sinX.Length];
            double[] sinY9 = new double[sinX.Length];
            double[] sinY11 = new double[sinX.Length];
            double[] sinY12 = new double[sinX.Length];
            double[] sinY13 = new double[sinX.Length];
            double[] sinY14 = new double[sinX.Length];
            double[] sinY15 = new double[sinX.Length];
            double[] sinY16 = new double[sinX.Length];
            double[] sinY17 = new double[sinX.Length];
            double[] sinY18 = new double[sinX.Length];
            double[] sinY19 = new double[sinX.Length];

            double f = 3;
            double dt = 2.0 / sinX.Length;

            for (int i = 0; i < sinX.Length; i++)
            {
                //double x = Math.PI * 10 * i / (dataX.Length - 1);
                //dataX[i] = x * Math.Cos(x);
                //dataY1[i] = x * Math.Sin(x);
                sinX[i] = (sinX.Length - 1 - i) * dt;
                sinY1[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.1;
                sinY2[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.2;
                sinY3[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.3;
                sinY4[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.4;
                sinY5[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.5;
                sinY6[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.6;
                sinY7[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.7;
                sinY8[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.8;
                sinY9[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.9;
                sinY11[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.1;
                sinY12[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.2;
                sinY13[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.3;
                sinY14[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.4;
                sinY15[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.5;
                sinY16[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.6;
                sinY17[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.7;
                sinY18[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.8;
                sinY19[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.9;
            }

            plot1.PlotStyle = PlotStyle.Markers;
            var pl1 = plot1.Add(sinX, sinY1, "Plot1");
            var pl2 = plot1.Add(sinX, sinY2, "Plot2");
            //var pl3 = plot1.Add(sinX, sinY3, "Plot3");
            //var pl4 = plot1.Add(sinX, sinY4, "Plot4");
            //var pl5 = plot1.Add(sinX, sinY5, "Plot5");
            //var pl6 = plot1.Add(sinX, sinY6, "Plot6");
            //var pl7 = plot1.Add(sinX, sinY7, "Plot7");
            //var pl8 = plot1.Add(sinX, sinY8, "Plot8");

            plot1.LegendStyle = LegendStyle.Inside;

            plot1.PlotDoubleClick += Plot1_PlotDoubleClick;
        }

        private void Plot1_PlotDoubleClick(object sender, PlotMouseEventArgs e)
        {
            if(e.ClickOnPlot)
                MessageBox.Show(e.Plot.Name);
        }
    }
}
