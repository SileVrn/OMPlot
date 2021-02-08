using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace OMPlot.Test
{    
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            /*double[] dataX = new double[10];
            double[] dataY1 = new double[dataX.Length];
            double[] dataY2 = new double[dataX.Length];
            for (int i = 0; i < dataX.Length; i++)
            {
                dataX[i] = i;
                dataY1[i] = Math.Sin(2 * Math.PI * dataX[i] * 0.1);
                dataY2[i] = Math.Sin(2 * Math.PI * dataX[i] * 0.1) - 0.2;
            }

            OMPlot.Plot p = new Plot();
            p.Height = 500;
            p.Width = 500;
            var plot0 = p.Add(dataX, dataY1);
            plot0.MarkStyle = Data.MarkerStyle.SolidCircle;

            var plot1 = p.Add(dataX, dataY2);
            plot1.LineStyle = Data.LineStyle.None;
            plot0.FillPlot = plot1;*/


            /*foreach (Data.PlotInterpolation inter in Enum.GetValues(typeof(Data.PlotInterpolation)))
                foreach (Data.FillStyle fill in Enum.GetValues(typeof(Data.FillStyle)))
                    PlotStyleTestBitmap(p, plot0, inter, fill);*/


            //AxisTestBitmap(p, "Title", AxisPosition.Center, LabelsPosition.Near, Alignment.Center, LabelsPosition.Far, Alignment.Near, Alignment.Center, TicksLabelsRotation.Tilted);

            /*foreach (AxisPosition ap in Enum.GetValues(typeof(AxisPosition)))
                foreach (LabelsPosition tp in Enum.GetValues(typeof(LabelsPosition)))
                    foreach (Alignment ta in Enum.GetValues(typeof(Alignment)))
                        foreach (LabelsPosition tlp in new LabelsPosition[] { LabelsPosition.Near, LabelsPosition.Far })
                            foreach (Alignment tla in Enum.GetValues(typeof(Alignment)))
                                foreach (Alignment tlla in Enum.GetValues(typeof(Alignment)))
                                    foreach (TicksLabelsRotation tlr in Enum.GetValues(typeof(TicksLabelsRotation)))
                                        try
                                        {
                                            AxisTestBitmap(p, "Title", ap, tp, ta, tlp, tla, tlla, tlr);
                                        }
                                        catch
                                        { }*/

            /*dataX = new double[11];
            dataY1 = new double[dataX.Length];
            dataY2 = new double[dataX.Length];
            for (int i = 0; i < dataX.Length; i++)
            {
                dataX[i] = i - 5;
                dataY1[i] = 25 - dataX[i] * dataX[i];
                dataY2[i] = 25 - (dataX[i] - 2) * (dataX[i] - 2);
            }

            p.Clear();
            plot0 = p.Add(dataX, dataY1);
            plot0.LineStyle = Data.LineStyle.None;
            plot0.BarStyle = Data.BarStyle.Vertical;
            plot0.BarDuty = 1.0f;
            plot0.BarFillColor = Color.Red;

            plot1 = p.Add(dataX, dataY2);
            plot1.LineStyle = Data.LineStyle.None;
            plot1.BarStyle = Data.BarStyle.Vertical;
            plot1.BarDuty = 1.0f;
            plot1.BarFillColor = Color.Blue;

            p.ToImage().Save("Bar_Vertical_Fill.png");

            plot0.BarStacking = true;
            plot1.BarStacking = true;
            p.ToImage().Save("Bar_Vertical_Stacking_Fill.png");

            plot0.BarDuty = 0.5f;
            plot1.BarDuty = 0.5f;
            p.ToImage().Save("Bar_Vertical_Stacking_Duty_Fill.png");


            plot0.BarFillColor = Color.FromArgb(0, 0, 0,0);
            plot1.BarFillColor = Color.FromArgb(0, 0, 0, 0);
            plot0.BarLineColor = Color.Red;
            plot1.BarLineColor = Color.Blue;
            p.ToImage().Save("Bar_Vertical_Stacking_Duty_Line.png");



            p.Clear();
            plot0 = p.Add(dataY1, dataX);
            plot0.LineStyle = Data.LineStyle.None;
            plot0.BarStyle = Data.BarStyle.Horisontal;
            plot0.BarDuty = 1.0f;
            plot0.BarFillColor = Color.Red;

            plot1 = p.Add(dataY2, dataX);
            plot1.LineStyle = Data.LineStyle.None;
            plot1.BarStyle = Data.BarStyle.Horisontal;
            plot1.BarDuty = 1.0f;
            plot1.BarFillColor = Color.Blue;

            p.ToImage().Save("Bar_Horisontal_Fill.png");

            plot0.BarStacking = true;
            plot1.BarStacking = true;
            p.ToImage().Save("Bar_Horisontal_Stacking_Fill.png");

            plot0.BarDuty = 0.5f;
            plot1.BarDuty = 0.5f;
            p.ToImage().Save("Bar_Horisontal_Stacking_Duty_Fill.png");


            plot0.BarFillColor = Color.FromArgb(0, 0, 0, 0);
            plot1.BarFillColor = Color.FromArgb(0, 0, 0, 0);
            plot0.BarLineColor = Color.Red;
            plot1.BarLineColor = Color.Blue;
            p.ToImage().Save("Bar_Horisontal_Stacking_Duty_Line.png");

            p.Clear();
            double[] sinX = new double[100];
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

            var pl1 = p.Add(sinX, sinY1, "Plot1");
            var pl2 = p.Add(sinX, sinY2, "Plot2");
            var pl3 = p.Add(sinX, sinY3, "Plot3");
            var pl4 = p.Add(sinX, sinY4, "Plot4");
            var pl5 = p.Add(sinX, sinY5, "Plot5Plot5Plot5");
            var pl6 = p.Add(sinX, sinY6, "Plot6");
            var pl7 = p.Add(sinX, sinY7, "Plot7");
            var pl8 = p.Add(sinX, sinY8, "Plot8");
            var pl9 = p.Add(sinX, sinY9, "Plot9");
            var pl11 = p.Add(sinX, sinY11, "Plot11");
            var pl12 = p.Add(sinX, sinY12, "Plot12");
            var pl13 = p.Add(sinX, sinY13, "Plot13");
            var pl14 = p.Add(sinX, sinY14, "Plot14");
            var pl15 = p.Add(sinX, sinY15, "Plot15");
            var pl16 = p.Add(sinX, sinY16, "Plot16");
            var pl17 = p.Add(sinX, sinY17, "Plot17");
            var pl18 = p.Add(sinX, sinY18, "Plot18");
            var pl19 = p.Add(sinX, sinY19, "Plot19");

            pl1.BarStyle = Data.BarStyle.Vertical;
            pl1.BarFillColor = Color.Red;
            pl2.MarkStyle = Data.MarkerStyle.SolidCircle;
            pl3.LineStyle = Data.LineStyle.Dash;
            pl4.LineStyle = Data.LineStyle.DashDot;
            pl5.LineStyle = Data.LineStyle.DashDotDot;
            pl6.LineStyle = Data.LineStyle.Dot;

            p.LegendStyle = LegendStyle.Outside;
            p.LegendPosition = LegendPosition.Top;     p.ToImage().Save("Legend_Outside_Top.png");
            p.LegendPosition = LegendPosition.Bottom;  p.ToImage().Save("Legend_Outside_Bottom.png");
            p.LegendPosition = LegendPosition.Left;    p.ToImage().Save("Legend_Outside_Left.png");
            p.LegendPosition = LegendPosition.Right;   p.ToImage().Save("Legend_Outside_Right.png");

            p.LegendStyle = LegendStyle.Inside;
            p.LegendAlign = LegendAlign.Near;
            p.LegendPosition = LegendPosition.Top;      p.ToImage().Save("Legend_Inside_Near_Top.png");
            p.LegendPosition = LegendPosition.Bottom;   p.ToImage().Save("Legend_Inside_Near_Bottom.png");
            p.LegendPosition = LegendPosition.Left;     p.ToImage().Save("Legend_Inside_Near_Left.png");
            p.LegendPosition = LegendPosition.Right;    p.ToImage().Save("Legend_Inside_Near_Right.png");

            p.LegendStyle = LegendStyle.Inside;
            p.LegendAlign = LegendAlign.Center;
            p.LegendPosition = LegendPosition.Top;      p.ToImage().Save("Legend_Inside_Center_Top.png");
            p.LegendPosition = LegendPosition.Bottom;   p.ToImage().Save("Legend_Inside_Center_Bottom.png");
            p.LegendPosition = LegendPosition.Left;     p.ToImage().Save("Legend_Inside_Center_Left.png");
            p.LegendPosition = LegendPosition.Right;    p.ToImage().Save("Legend_Inside_Center_Right.png");

            p.LegendStyle = LegendStyle.Inside;
            p.LegendAlign = LegendAlign.Far;
            p.LegendPosition = LegendPosition.Top;      p.ToImage().Save("Legend_Inside_Far_Top.png");
            p.LegendPosition = LegendPosition.Bottom;   p.ToImage().Save("Legend_Inside_Far_Bottom.png");
            p.LegendPosition = LegendPosition.Left;     p.ToImage().Save("Legend_Inside_Far_Left.png");
            p.LegendPosition = LegendPosition.Right;    p.ToImage().Save("Legend_Inside_Far_Right.png");*/

            Application.Run(new Form1());            
        }

        static void PlotStyleTestBitmap(Plot p, Data.XYSeries plot, Data.PlotInterpolation inter, Data.FillStyle fill)
        {
            plot.Interpolation = inter;
            plot.FillStyle = fill;

            p.ToImage().Save(
                Enum.GetName(typeof(Data.PlotInterpolation), inter) + "_" +
                Enum.GetName(typeof(Data.FillStyle), fill) + ".png");
        }

        static void AxisTestBitmap(Plot p, string title, AxisPosition ap, LabelsPosition tp, Alignment ta, LabelsPosition tlp, Alignment tla, Alignment tlla, TicksLabelsRotation tlr)
        {
            Axis xAxis = p.GetHorizontalAxis();
            xAxis.Minimum = 100000;
            xAxis.Maximum = 100000.001;
            xAxis.Position = ap;
            /*if (tlp == LabelsPosition.Near)
                xAxis.Position = AxisPosition.Near;
            else if (tlp == LabelsPosition.Far)
                xAxis.Position = AxisPosition.Far; */
            xAxis.Title = title;
            xAxis.TitlePosition = tp;
            xAxis.TitleAlignment = ta;
            xAxis.TicksLabelsPosition = tlp;
            xAxis.TicksLabelsAlignment = tla;
            xAxis.TicksLabelsLineAlignment = tlla;
            xAxis.TicksLabelsRotation = tlr;

            Axis yAxis = p.GetVerticalAxis();
            yAxis.Minimum = -1.5;
            yAxis.Maximum = 1.5;
            yAxis.Position = ap;
            /*if (tlp == LabelsPosition.Near)
                yAxis.Position = AxisPosition.Near;
            else if (tlp == LabelsPosition.Far)
                yAxis.Position = AxisPosition.Far;*/
            yAxis.Title = title;
            yAxis.TitlePosition = tp;
            yAxis.TitleAlignment = ta;
            yAxis.TicksLabelsPosition = tlp;
            yAxis.TicksLabelsAlignment = tla;
            yAxis.TicksLabelsLineAlignment = tlla;
            yAxis.TicksLabelsRotation = tlr;

            p.ToImage().Save(title + "_" +
                Enum.GetName(typeof(AxisPosition), ap) + "_" +
                Enum.GetName(typeof(LabelsPosition), tp) + "_" + 
                Enum.GetName(typeof(Alignment), ta) + "_" + 
                Enum.GetName(typeof(LabelsPosition), tlp) + "_" +
                Enum.GetName(typeof(Alignment), tla) + "_" +
                Enum.GetName(typeof(Alignment), tlla) + "_" +
                Enum.GetName(typeof(TicksLabelsRotation), tlr) + ".png");
        }
    }
}
