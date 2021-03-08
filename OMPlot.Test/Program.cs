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
            //Random r = new Random();
            //double[] array = new double[10000000];
            //for (int i = 0; i < array.Length; i++)
            //    array[i] = r.NextDouble() - 0.5;

            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            ////var min = array.Where(e => e > 0).Min();
            ////var max = array.Where(e => e > 0).Max();
            //double min = double.MaxValue;
            //double max = double.MinValue;

            //for (int i = 0; i < array.Length; i++)
            //{
            //    if(array[i] > 0)
            //    {
            //        if (min > array[i]) min = array[i];
            //        if (max < array[i]) max = array[i];
            //    }
            //}

            //sw.Stop();






            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            double[] sin0 = new double[100];
            double[] sin1 = new double[sin0.Length];
            double[] sin2 = new double[sin0.Length];
            for (int i = 0; i < sin0.Length; i++)
            {
                sin0[i] = Math.Sin(2 * Math.PI * i * 0.01);
                sin1[i] = Math.Sin(2 * Math.PI * (i + 5) * 0.01) + 0.1;
                sin2[i] = Math.Sin(2 * Math.PI * (i + 10) * 0.01) + 0.2;
            }

            double[] norm1 = new double[10];
            double[] norm2 = new double[norm1.Length];
            double sigma1 = 2;
            double sigma2 = 3;
            double avg1 = 5;
            double avg2 = 3;
            for (int i = 0; i < norm1.Length; i++)
            {
                norm1[i] = 1 / (sigma1 * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * (i - avg1) * (i - avg1) / 2 / sigma1 / sigma1);
                norm2[i] = 1 / (sigma2 * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 * (i - avg2) * (i - avg2) / 2 / sigma2 / sigma2);
            }

            double[] freq = new double[] { 10000, 10232, 10471, 10715, 10964, 11220, 11481, 11748, 12022, 12302, 12589, 12882,
            13182, 13489, 13803, 14125, 14454, 14791, 15135, 15488, 15848, 16218, 16595, 16982, 17378, 17782, 18197, 18620,
            19054, 19498, 19952, 20417, 20892, 21379, 21877, 22387, 22908, 23442, 23988, 24547, 25118, 25703, 26302, 26915,
            27542, 28183, 28840, 29512, 30199, 30902, 31622, 32359, 33113, 33884, 34673, 35481, 36307, 37153, 38018, 38904,
            39810, 40738, 41686, 42657, 43651, 44668, 45708, 46773, 47863, 48977, 50118, 51286, 52480, 53703, 54954, 56234,
            57543, 58884, 60255, 61659, 63095, 64565, 66069, 67608, 69183, 70794, 72443, 74131, 75857, 77624, 79432, 81283,
            83176, 85113, 87096, 89125, 91201, 93325, 95499, 97723, 100000, 102329, 104712, 107151, 109647, 112201, 114815,
            117489, 120226, 123026, 125892, 128824, 131825, 134896, 138038, 141253, 144543, 147910, 151356, 154881, 158489,
            162181, 165958, 169824, 173780, 177827, 181970, 186208, 190546, 194984, 199526, 204173, 208929, 213796, 218776,
            223872, 229086, 234422, 239883, 245470, 251188, 257039, 263026, 269153, 275422, 281838, 288403, 295120, 301995,
            309029, 316227, 323593, 331131, 338844, 346736, 354813, 363078, 371535, 380189, 389045, 398107, 407380, 416869,
            426579, 436515, 446683, 457088, 467735, 478630, 489778, 501187, 512861, 524807, 537031, 549540, 562341, 575439,
            588843, 602559, 616595, 630957, 645654, 660693, 676082, 691830, 707945, 724435, 741310, 758577, 776247, 794328,
            812830, 831763, 851138, 870963, 891250, 912010, 933254, 954992, 977237, 1000000 };
            double[] fresp = new double[] { 0.034, 0.035, 0.037, 0.039, 0.041, 0.043, 0.045, 0.047, 0.049, 0.052, 0.054, 0.057,
                0.059, 0.062, 0.065, 0.068, 0.071, 0.075, 0.078, 0.082, 0.086, 0.090, 0.094, 0.099, 0.104, 0.109, 0.114, 0.119,
                0.125, 0.131, 0.137, 0.144, 0.150, 0.158, 0.165, 0.173, 0.181, 0.190, 0.199, 0.209, 0.219, 0.229, 0.240, 0.251,
                0.264, 0.276, 0.289, 0.303, 0.318, 0.333, 0.349, 0.366, 0.384, 0.402, 0.422, 0.442, 0.464, 0.486, 0.510, 0.535,
                0.561, 0.588, 0.617, 0.647, 0.679, 0.712, 0.747, 0.784, 0.823, 0.863, 0.906, 0.952, 0.999, 1.049, 1.102, 1.158,
                1.216, 1.278, 1.343, 1.412, 1.484, 1.561, 1.642, 1.728, 1.818, 1.914, 2.016, 2.123, 2.237, 2.358, 2.487, 2.624,
                2.770, 2.925, 3.090, 3.267, 3.456, 3.658, 3.876, 4.109, 4.360, 4.631, 4.924, 5.243, 5.589, 5.966, 6.380, 6.836,
                7.340, 7.902, 8.531, 9.243, 10.057, 11.000, 12.111, 13.449, 15.114, 17.283, 20.339, 25.358, 37.682, 28.041, 21.125,
                17.147, 14.314, 12.091, 10.247, 8.660, 7.261, 6.003, 4.856, 3.798, 2.813, 1.889, 1.016, 0.187, -0.602, -1.360,
                -2.088, -2.790, -3.469, -4.127, -4.767, -5.390, -5.998, -6.591, -7.173, -7.742, -8.301, -8.850, -9.390, -9.922,
                -10.446, -10.963, -11.473, -11.977, -12.475, -12.967, -13.455, -13.938, -14.416, -14.890, -15.360, -15.827, -16.290,
                -16.750, -17.207, -17.661, -18.112, -18.561, -19.007, -19.451, -19.893, -20.332, -20.770, -21.206, -21.640, -22.073,
                -22.504, -22.933, -23.362, -23.788, -24.214, -24.638, -25.062, -25.484, -25.905, -26.325, -26.745, -27.163, -27.581,
                -27.998, -28.414, -28.829, -29.244, -29.658, -30.071, -30.484, -30.897, -31.309, -31.720 };

            Random r = new Random();
            double[] randoms = new double[100];
            double[] quantiles = new double[randoms.Length];
            for (int i = 0; i < randoms.Length; i++)
            {
                randoms[i] = r.NextDouble();
                quantiles[i] = QNorm(((double)i + 1.0 - 0.375) / ((double)randoms.Length + 0.25));
            }
            Array.Sort(randoms);

            Plot plot0 = new Plot { Height = 300, Width = 500 };
            plot0.Title = "Sin example";
            plot0.Add(sin0);
            plot0.Add(sin1);
            plot0.Add(sin2);
            plot0.LegendStyle = LegendStyle.Inside;
            var im0 = plot0.ToImage();

            Plot plot1 = new Plot { Height = 300, Width = 500 };
            plot1.Title = "Histogram example";
            plot1.Add(norm1, ps: PlotStyle.VerticalBars);
            plot1.Add(norm2, ps: PlotStyle.VerticalBars);
            plot1.LegendStyle = LegendStyle.Inside;
            var im1 = plot1.ToImage();


            Plot plot2 = new Plot { Height = 300, Width = 500 };
            plot2.Title = "Frequency response example";
            plot2.Add(freq, fresp);
            plot2.GetHorizontalAxis().Title = "F, Hz";
            plot2.GetHorizontalAxis().Logarithmic = true;
            plot2.GetHorizontalAxis().MinorTickNumber = 10;
            plot2.GetHorizontalAxis().GridStyle = GridStyle.Both;
            plot2.GetVerticalAxis().Title = "Vout, dB";
            plot2.GetVerticalAxis().GridStyle = GridStyle.Both;
            var im2 = plot2.ToImage();

            Plot plot3 = new Plot { Height = 300, Width = 500 };
            plot3.Title = "Probability plot example";
            plot3.GetHorizontalAxis().GridStyle = GridStyle.Both;
            plot3.GetVerticalAxis().GridStyle = GridStyle.Both;
            plot3.GetVerticalAxis().CustomTicks = new double[] { -2.326347874, -1.644853627, -1.281551566, -0.841621234,
                -0.524400513, -0.253347103, 0, 0.253347103, 0.524400513, 0.841621234, 1.281551566, 1.644853627, 2.326347874 };
            plot3.GetVerticalAxis().CustomTicksLabels = new string[] { "1", "5", "10", "20", "30", "40", "50", "60", "70", "80", "90", "95", "99" };
            plot3.Add(randoms, quantiles, ps: PlotStyle.Markers);
            plot3.Add(randoms, LinearRegression(randoms, quantiles, false)).LineWidth = 2;
            var im3 = plot3.ToImage();


            Image im = new Bitmap(1000, 600);
            Graphics g = Graphics.FromImage(im);
            g.DrawImage(im0, 0, 0);
            g.DrawImage(im1, 500, 0);
            g.DrawImage(im2, 0, 300);
            g.DrawImage(im3, 500, 300);
            im.Save("plot.png");


            /*OMPlot.Plot p = new Plot();
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

        static void PlotStyleTestBitmap(Plot p, Data.ScatterSeries plot, Data.Interpolation inter, Data.FillStyle fill)
        {
            plot.Interpolation = inter;
            plot.FillStyle = fill;

            p.ToImage().Save(
                Enum.GetName(typeof(Data.Interpolation), inter) + "_" +
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

        //https://stackoverflow.com/a/31474739		
        public static double[] LinearRegression(double[] xVals, double[] yVals, bool logScale)
        {
            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double sumCodeviates = 0;
            double count = xVals.Length;

            for (int ctr = 0; ctr < xVals.Length; ctr++)
            {
                double x = xVals[ctr];
                double y = logScale ? Math.Log10(yVals[ctr]) : yVals[ctr];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }
            double ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            //float ssY = sumOfYSq - ((sumOfY * sumOfY) / count);
            //float RNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            //float RDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
            double sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            double meanX = sumOfX / count;
            double meanY = sumOfY / count;
            //float dblR = RNumerator / (float)Math.Sqrt(RDenom);

            //float rsquared = dblR * dblR;
            double yintercept = meanY - ((sCo / ssX) * meanX);
            double slope = sCo / ssX;

            double[] fittedY = new double[xVals.Length];
            for (int ctr = 0; ctr < count; ctr++)
            {
                fittedY[ctr] = xVals[ctr] * slope + yintercept;
                if (logScale)
                {
                    fittedY[ctr] = Math.Pow(10.0, fittedY[ctr]);
                }
            }

            return fittedY;
        }

        /// <summary>
        /// Quantile function (Inverse CDF) for the normal distribution.
        /// </summary>
        /// <param name="p">Probability.</param>
        /// <param name="mu">Mean of normal distribution.</param>
        /// <param name="sigma">Standard deviation of normal distribution.</param>
        /// <param name="lower_tail">If true, probability is P[X <= x], otherwise P[X > x].</param>
        /// <param name="log_p">If true, probabilities are given as log(p).</param>
        /// <returns>P[X <= x] where x ~ N(mu,sigma^2)</returns>
        /// <remarks>See https://svn.r-project.org/R/trunk/src/nmath/qnorm.c</remarks>
        public static double QNorm(double p, double mu = 0, double sigma = 1, bool lower_tail = true, bool log_p = false)
        {
            if (double.IsNaN(p) || double.IsNaN(mu) || double.IsNaN(sigma)) return (p + mu + sigma);
            double ans;
            bool isBoundaryCase = R_Q_P01_boundaries(p, double.NegativeInfinity, double.PositiveInfinity, lower_tail, log_p, out ans);
            if (isBoundaryCase) return (ans);
            if (sigma < 0) return (double.NaN);
            if (sigma == 0) return (mu);

            double p_ = R_DT_qIv(p, lower_tail, log_p);
            double q = p_ - 0.5;
            double r, val;

            if (Math.Abs(q) <= 0.425)  // 0.075 <= p <= 0.925
            {
                r = .180625 - q * q;
                val = q * (((((((r * 2509.0809287301226727 +
                           33430.575583588128105) * r + 67265.770927008700853) * r +
                         45921.953931549871457) * r + 13731.693765509461125) * r +
                       1971.5909503065514427) * r + 133.14166789178437745) * r +
                     3.387132872796366608)
                / (((((((r * 5226.495278852854561 +
                         28729.085735721942674) * r + 39307.89580009271061) * r +
                       21213.794301586595867) * r + 5394.1960214247511077) * r +
                     687.1870074920579083) * r + 42.313330701600911252) * r + 1.0);
            }
            else
            {
                r = q > 0 ? R_DT_CIv(p, lower_tail, log_p) : p_;
                r = Math.Sqrt(-((log_p && ((lower_tail && q <= 0) || (!lower_tail && q > 0))) ? p : Math.Log(r)));

                if (r <= 5)              // <==> min(p,1-p) >= exp(-25) ~= 1.3888e-11
                {
                    r -= 1.6;
                    val = (((((((r * 7.7454501427834140764e-4 +
                            .0227238449892691845833) * r + .24178072517745061177) *
                          r + 1.27045825245236838258) * r +
                         3.64784832476320460504) * r + 5.7694972214606914055) *
                       r + 4.6303378461565452959) * r +
                      1.42343711074968357734)
                     / (((((((r *
                              1.05075007164441684324e-9 + 5.475938084995344946e-4) *
                             r + .0151986665636164571966) * r +
                            .14810397642748007459) * r + .68976733498510000455) *
                          r + 1.6763848301838038494) * r +
                         2.05319162663775882187) * r + 1.0);
                }
                else                     // very close to  0 or 1 
                {
                    r -= 5.0;
                    val = (((((((r * 2.01033439929228813265e-7 +
                            2.71155556874348757815e-5) * r +
                           .0012426609473880784386) * r + .026532189526576123093) *
                         r + .29656057182850489123) * r +
                        1.7848265399172913358) * r + 5.4637849111641143699) *
                      r + 6.6579046435011037772)
                     / (((((((r *
                              2.04426310338993978564e-15 + 1.4215117583164458887e-7) *
                             r + 1.8463183175100546818e-5) * r +
                            7.868691311456132591e-4) * r + .0148753612908506148525)
                          * r + .13692988092273580531) * r +
                         .59983220655588793769) * r + 1.0);
                }
                if (q < 0.0) val = -val;
            }

            return (mu + sigma * val);
        }

        static bool R_Q_P01_boundaries(double p, double _LEFT_, double _RIGHT_, bool lower_tail, bool log_p, out double ans)
        {
            if (log_p)
            {
                if (p > 0.0)
                {
                    ans = double.NaN;
                    return (true);
                }
                if (p == 0.0)
                {
                    ans = lower_tail ? _RIGHT_ : _LEFT_;
                    return (true);
                }
                if (p == double.NegativeInfinity)
                {
                    ans = lower_tail ? _LEFT_ : _RIGHT_;
                    return (true);
                }
            }
            else
            {
                if (p < 0.0 || p > 1.0)
                {
                    ans = double.NaN;
                    return (true);
                }
                if (p == 0.0)
                {
                    ans = lower_tail ? _LEFT_ : _RIGHT_;
                    return (true);
                }
                if (p == 1.0)
                {
                    ans = lower_tail ? _RIGHT_ : _LEFT_;
                    return (true);
                }
            }
            ans = double.NaN;
            return (false);
        }

        static double R_DT_qIv(double p, bool lower_tail, bool log_p)
        {
            return (log_p ? (lower_tail ? Math.Exp(p) : -ExpM1(p)) : R_D_Lval(p, lower_tail));
        }

        static double R_DT_CIv(double p, bool lower_tail, bool log_p)
        {
            return (log_p ? (lower_tail ? -ExpM1(p) : Math.Exp(p)) : R_D_Cval(p, lower_tail));
        }

        static double R_D_Lval(double p, bool lower_tail)
        {
            return lower_tail ? p : 0.5 - p + 0.5;
        }

        static double R_D_Cval(double p, bool lower_tail)
        {
            return lower_tail ? 0.5 - p + 0.5 : p;
        }

        private static double ExpM1(double x)
        {
            if (Math.Abs(x) < 1e-5)
                return x + 0.5 * x * x;
            else
                return Math.Exp(x) - 1.0;
        }
    }
}
