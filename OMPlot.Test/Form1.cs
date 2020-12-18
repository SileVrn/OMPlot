﻿using System;
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
            double[] dataY3 = new double[dataX.Length];

            double f = 10;
            double dt = 1e-5;

            for(int i = 0; i < dataX.Length; i++)
            {
                dataX[i] = i * dt;
                dataY1[i] = Math.Sin(2 * Math.PI * dataX[i] * f);
                dataY2[i] = Math.Cos(2 * Math.PI * dataX[i] * f);
                dataY3[i] = Math.Tan(2 * Math.PI * dataX[i] * f);
            }

            Data.XY plotXY1 = new Data.XY(dataX, dataY1);
            Data.XY plotXY2 = new Data.XY(dataX, dataY2);
            Data.XY plotXY3 = new Data.XY(dataX, dataY3);

            plot1.Data.Add(plotXY1);
            plot1.Data.Add(plotXY2);
            plot1.Data.Add(plotXY3);

            OMPlot.Axis xAxis = new Axis();
            //xAxis.Minimum = 0;
            //xAxis.Maximum = (float)(dt * dataX.Length);
            xAxis.Minimum = 0f;
            xAxis.Maximum = 0.001f;
            xAxis.Position = AxisPosition.Far;
            xAxis.TitlePosition = LabelsPosition.Far;
            xAxis.TicksLabelsPosition = LabelsPosition.Far;
            xAxis.TicksLabelsRotation = TicksLabelsRotation.Tilted;
            xAxis.TicksLabelsAlignment = Alignment.Far;
            xAxis.Title = "X";
            
            OMPlot.Axis yAxis = new Axis();
            yAxis.Minimum = -2;
            yAxis.Maximum = 2;
            yAxis.TicksLabelsRotation = TicksLabelsRotation.Tilted;
            yAxis.TicksLabelsAlignment = Alignment.Far;
            yAxis.Title = "Y";
            
            plot1.AddVerticalAxis(yAxis);
            plot1.AddHorizontalAxis(xAxis);


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
