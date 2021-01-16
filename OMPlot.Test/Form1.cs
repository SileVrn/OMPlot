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


            double[] sinX = new double[1000];
            double[] sinY1 = new double[sinX.Length];
            double[] sinY2 = new double[sinX.Length];
            double[] sinY3 = new double[sinX.Length];
            double[] sinY4 = new double[sinX.Length];
            double[] sinY5 = new double[sinX.Length];
            double[] sinY6 = new double[sinX.Length];
            double[] sinY7 = new double[sinX.Length];
            double[] sinY8 = new double[sinX.Length];
            double[] sinY9 = new double[sinX.Length];

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
            }

            var pl1 = plot1.Add(sinX, sinY1, "Plot1");
            var pl2 = plot1.Add(sinX, sinY2, "Plot2");
            var pl3 = plot1.Add(sinX, sinY3, "Plot3");
            var pl4 = plot1.Add(sinX, sinY4, "Plot4");


            pl1.LineStyle = Data.LineStyle.Dash;
            pl2.LineStyle = Data.LineStyle.DashDot;
            pl3.LineStyle = Data.LineStyle.DashDotDot;
            pl4.LineStyle = Data.LineStyle.Dot;

            pl1.LineWidth = 1;
            pl2.LineWidth = 1;
            pl3.LineWidth = 1;
            pl4.LineWidth = 1;
        }
                
    }
}
