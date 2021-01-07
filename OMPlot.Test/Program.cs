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


            double[] dataX = new double[10];
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
            plot0.FillPlot = plot1;


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

            dataX = new double[11];
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
            List<double> dx = new List<double>();
            List<double> dy = new List<double>();
            dx.Add(0);
            dy.Add(0);
            dx.Add(20);
            dy.Add(0);
            dx.Add(45);
            dy.Add(-47);
            dx.Add(53);
            dy.Add(335);
            dx.Add(57);
            dy.Add(26);
            dx.Add(62);
            dy.Add(387);
            dx.Add(74);
            dy.Add(104);
            dx.Add(89);
            dy.Add(0);
            dx.Add(95);
            dy.Add(100);
            dx.Add(100);
            dy.Add(0);
            p.Add(dx, dy);            
            var ppp = p.Add(dx, dy);
            ppp.Interpolation = Data.PlotInterpolation.Spline;
            p.ToImage().Save("SplineTest.png");

            Application.Run(new Form1());            
        }

        static void PlotStyleTestBitmap(Plot p, Data.XY plot, Data.PlotInterpolation inter, Data.FillStyle fill)
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
