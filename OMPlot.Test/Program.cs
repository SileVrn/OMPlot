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


            double[] dataX = new double[100];
            double[] dataY = new double[dataX.Length];
            for (int i = 0; i < dataX.Length; i++)
            {
                dataX[i] = 1000f + i * 1e-6;
                dataY[i] = Math.Sin(2 * Math.PI * dataX[i] * 100);
            }

            Image img = new Bitmap(500, 500);
            Graphics g = Graphics.FromImage(img);

            OMPlot.Plot p = new Plot();
            p.Height = 500;
            p.Width = 500;
            p.Add(dataX, dataY);

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

            Application.Run(new Form1());            
        }

        static void AxisTestBitmap(Plot p, string title, AxisPosition ap, LabelsPosition tp, Alignment ta, LabelsPosition tlp, Alignment tla, Alignment tlla, TicksLabelsRotation tlr)
        {
            Image img = new Bitmap(p.Width, p.Height);
            Graphics g = Graphics.FromImage(img);

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

            p.ControlPaint(g);
            img.Save(title + "_" +
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
