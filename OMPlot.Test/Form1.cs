using System;
using System.Collections.Generic;
using System.Drawing;
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


            double[] sinX0 = new double[20];
            double[] sinY0 = new double[sinX0.Length];

            double[] sinX = new double[200000];
            double[] sinY1 = new double[sinX.Length];
            double[] sinY2 = new double[sinX.Length];
            double[] sinY3 = new double[sinX.Length];
            double[] sinY4 = new double[sinX.Length];
            double[] sinY5 = new double[sinX.Length];
            double[] sinY6 = new double[sinX.Length];
            double[] sinY7 = new double[sinX.Length];
            double[] sinY8 = new double[sinX.Length];
            //double[] sinY9 = new double[sinX.Length];
            //double[] sinY11 = new double[sinX.Length];
            //double[] sinY12 = new double[sinX.Length];
            //double[] sinY13 = new double[sinX.Length];
            //double[] sinY14 = new double[sinX.Length];
            //double[] sinY15 = new double[sinX.Length];
            //double[] sinY16 = new double[sinX.Length];
            //double[] sinY17 = new double[sinX.Length];
            //double[] sinY18 = new double[sinX.Length];
            //double[] sinY19 = new double[sinX.Length];

            double f = 3;
            double dt = 2.0 / sinX.Length;

            for (int i = 0; i < sinX0.Length; i++)
            {
                sinX0[i] = 2* i * dt;
                sinY0[i] = Math.Sin(2 * Math.PI * sinX0[i] * f) + 1;
            }

            for (int i = 0; i < sinX.Length; i++)
            {
                //double x = Math.PI * 10 * i / (dataX.Length - 1);
                //dataX[i] = x * Math.Cos(x);
                //dataY1[i] = x * Math.Sin(x);
                sinX[i] = i * dt;
                sinY1[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.1;
                sinY2[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.2;
                sinY3[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.3;
                sinY4[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.4;
                sinY5[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.5;
                sinY6[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.6;
                sinY7[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.7;
                sinY8[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.8;
                //sinY9[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 0.9;
                //sinY11[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.1;
                //sinY12[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.2;
                //sinY13[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.3;
                //sinY14[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.4;
                //sinY15[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.5;
                //sinY16[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.6;
                //sinY17[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.7;
                //sinY18[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.8;
                //sinY19[i] = Math.Sin(2 * Math.PI * sinX[i] * f) + 1.9;
            }

            //Array.Reverse(sinX);
                       
            var pl0 = plot1.Add(sinY3, dt, ps: PlotStyle.Lines);
            var pl1 = plot1.Add(sinX, sinY1, ps: PlotStyle.Lines);
            var pl2 = plot1.Add(sinX, sinY2, ps: PlotStyle.Lines);
            //var pl3 = plot1.Add(sinY3, sinX, "Plot3");
            //var pl4 = plot1.Add(sinY4, sinX, "Plot4");
            //var pl5 = plot1.Add(sinY5, sinX, "Plot5");
            //var pl6 = plot1.Add(sinY6, sinX, "Plot6");
            //var pl7 = plot1.Add(sinY7, sinX, "Plot7");
            //var pl8 = plot1.Add(sinY8, sinX, "Plot8");
            //pl1.MarkStyle = Data.MarkerStyle.Asterisk;
            //pl1.MarkColor = Color.Red;


            Dictionary<string, double> dict1 = new Dictionary<string, double>();
            dict1.Add("None", 0.1);
            dict1.Add("First", 1.0);
            dict1.Add("Second", 2.0);
            dict1.Add("Third", 3.0);


            Dictionary<string, double> dict2 = new Dictionary<string, double>();
            dict2.Add("None", 1.0);
            dict2.Add("First", 2.0);
            dict2.Add("Second", 3.0);
            dict2.Add("Third", 0.0);
            dict2.Add("Fourth", 4.0);

            //var dp1 = plot1.Add(dict1, "Dict0");
            //plot1.Add(dict2, "Dict1");

            pl1.LineWidth = 2;
            pl2.LineWidth = 2;
            

            plot1.LegendStyle = LegendStyle.Inside;
            plot1.Autoscale();
            
            //pl0.MouseDoubleClick += Pl0_DoubleClick;
            //pl1.MouseDoubleClick += Pl0_DoubleClick;
            //pl2.MouseDoubleClick += Pl0_DoubleClick;

            //pl0.MouseMove += Pl0_MouseMove;
            //pl1.MouseMove += Pl0_MouseMove;
            //pl2.MouseMove += Pl0_MouseMove;

            //pl0.MouseDown += Plot1_PlotMouseDown;
            //plot1.MouseUp += Plot1_PlotMouseUp;
            //plot1.MouseMove += Plot1_MouseMove;
        }

        private void Pl0_MouseMove(object sender, PlotMouseEventArgs e)
        {
            var pl = (OMPlot.Data.ScatterSeries)sender;
            this.Text = pl.Name + " (" + pl.GetX(e.NearestIndex) + ";" + pl.GetY(e.NearestIndex) + ")";
        }

        private void Pl0_DoubleClick(object sender, PlotMouseEventArgs e)
        {
            MessageBox.Show(((OMPlot.Data.ScatterSeries)sender).Name);
        }

        OMPlot.Data.ScatterSeries pl;
        int pointIndex = -1;
        private void Plot1_MouseMove(object sender, MouseEventArgs e)
        {
            if(pointIndex >= 0)
            {
                pl.SetX(pointIndex, pl.HorizontalAxis.TransformBack(e.X));
                pl.SetY(pointIndex, pl.VerticalAxis.TransformBack(e.Y));
                plot1.Refresh();
            }
        }

        private void Plot1_PlotMouseUp(object sender, MouseEventArgs e)
        {
            pointIndex = -1;
        }

        private void Plot1_PlotMouseDown(object sender, PlotMouseEventArgs e)
        {
            if (e.DistanceToNearest < OMPlot.Plot.MouseEventDistance)
            {
                pl = (OMPlot.Data.ScatterSeries)sender;
                pointIndex = e.NearestIndex;
            }
        }
    }
}
