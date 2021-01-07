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

            double[] dataX = new double[1000000];
            double[] dataY1 = new double[dataX.Length];
            double[] dataY2 = new double[dataX.Length];
            List<double> dataY3 = new List<double>();

            double f = 3000;
            double dt = 2.0 / dataX.Length;

            for(int i = 0; i < dataX.Length; i++)
            {
                double x = Math.PI * 10 * i / (dataX.Length - 1);
                dataX[i] = x * Math.Cos(x);
                dataY1[i] = x * Math.Sin(x); ;
                //dataX[i] = (dataX.Length - 1 - i) * dt;
                //dataY1[i] = Math.Sin(2 * Math.PI * dataX[i] * f) + 0.1;
                //dataY2[i] = Math.Sin(2 * Math.PI * dataX[i] * f) + 0.2;
                //dataY3.Add(Math.Sin(2 * Math.PI * dataX[i] * f) + 0.3);
            }

            var pl1 = plot1.Add(dataX, dataY1);
            //var pl2 = plot1.Add(dataX, dataY2);
            //var pl3 = plot1.Add(dataX, dataY3);

            pl1.LineStyle = Data.LineStyle.None;
            pl1.MarkStyle = Data.MarkerStyle.SolidCircle;

            //Axis xAxis = plot1.GetHorizontalAxis();
            //xAxis.Minimum = 0;
            //xAxis.Maximum = (float)(dt * dataX.Length);
            //xAxis.Title = "X";

            //xAxis.CustomTicks = new double[] { 0, 0.3 * dt * dataX.Length, 0.5111 * dt * dataX.Length, 0.7 * dt * dataX.Length };
            //xAxis.CustomTicksLabels = new string[] { "Zero", "Third", "Almost half", "O dot seven" };

            //Axis yAxis = plot1.GetVerticalAxis();
            //yAxis.Minimum = -2;
            //yAxis.Maximum = 2;
            //yAxis.Title = "Y";

            /*plot1.Title = "Hello, OMPlot!";

            double[] dataX = new double[10];
            double[] dataY1 = new double[dataX.Length];


            for (int i = 0; i < dataX.Length; i++)
            {
                dataX[i] = Math.Pow(10, i);
                dataY1[i] = i;
            }

            Data.XY plotXY1 = new Data.XY(dataX, dataY1);

            plot1.Data.Add(plotXY1);

            OMPlot.Axis xAxis = new Axis();
            xAxis.Logarithmic = true;
            xAxis.Minimum = (float)dataX.Min();
            xAxis.Maximum = (float)dataX.Max();
            xAxis.Title = "X";

            OMPlot.Axis yAxis = new Axis();
            yAxis.Minimum = (float)dataY1.Min(); ;
            yAxis.Maximum = (float)dataY1.Max(); ;
            yAxis.Title = "Y";

            plot1.AddVerticalAxis(yAxis);
            plot1.AddHorizontalAxis(xAxis);

            plot1.Refresh();*/



        }
                
    }
}
