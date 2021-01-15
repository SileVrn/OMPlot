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


            double[] dataX = new double[100];
            double[] dataY1 = new double[dataX.Length];
            double[] dataY2 = new double[dataX.Length];
            double[] dataY3 = new double[dataX.Length];
            double[] dataY4 = new double[dataX.Length];
            double[] dataY5 = new double[dataX.Length];
            double[] dataY6 = new double[dataX.Length];
            double[] dataY7 = new double[dataX.Length];
            double[] dataY8 = new double[dataX.Length];
            double[] dataY9 = new double[dataX.Length];
            double[] dataY11 = new double[dataX.Length];
            double[] dataY12 = new double[dataX.Length];
            double[] dataY13 = new double[dataX.Length];
            double[] dataY14 = new double[dataX.Length];
            double[] dataY15 = new double[dataX.Length];
            double[] dataY16 = new double[dataX.Length];
            double[] dataY17 = new double[dataX.Length];
            double[] dataY18 = new double[dataX.Length];
            double[] dataY19 = new double[dataX.Length];

            double f = 3;
            double dt = 2.0 / dataX.Length;

            for (int i = 0; i < dataX.Length; i++)
            {
                //double x = Math.PI * 10 * i / (dataX.Length - 1);
                //dataX[i] = x * Math.Cos(x);
                //dataY1[i] = x * Math.Sin(x);
                dataX[i] = (dataX.Length - 1 - i) * dt;
                dataY1[i] = Math.Sin(2 * Math.PI * dataX[i] * f) + 0.1;
                dataY2[i] = Math.Sin(2 * Math.PI * dataX[i] * f) + 0.2;
                dataY3[i] = Math.Sin(2 * Math.PI * dataX[i] * f) + 0.3;
            }

            var pl1 = plot1.Add(dataX, dataY1, "Plot1");
            var pl2 = plot1.Add(dataX, dataY2, "Plot2");
            var pl3 = plot1.Add(dataX, dataY3, "Plot3");

            pl1.BarStyle = Data.BarStyle.Vertical;
            pl1.BarFillColor = Color.Red;

            pl2.MarkStyle = Data.MarkerStyle.SolidCircle;

            plot1.LegendStyle = LegendStyle.Inside;
            plot1.LegendPosition = LegendPosition.Left;
            plot1.LegendAlign = LegendAlign.Center;
        }
                
    }
}
