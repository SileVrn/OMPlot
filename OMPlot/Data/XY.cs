using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public class XY : IData
    {
        Color[] defaultPlotColors = new Color[] { Color.Red, Color.Blue, Color.Green };

        double[] X, Y;     

        public PlotStyle Style { get; set; }
        public Color LineColor { get; set; }
        public System.Drawing.Drawing2D.DashStyle LineStyle { get; set; }
        public float LineWidth { get; set; }
        public Color MarkColor { get; set; }
        public MarkerStyle MarkStyle { get; set; }
        public float MarkSize { get; set; }
        public Interpolation Interpolation { get; set; }

        public string Name { get; set; }
        public string AxisHorizontalName { get; set; }
        public string AxisVerticalName { get; set; }

        public double MinimumX { get; private set; }
        public double MinimumY { get; private set; }
        public double MaximumX { get; private set; }
        public double MaximumY { get; private set; }

        public XY(IEnumerable<double> x, IEnumerable<double> y)
        {
            MarkSize = 10;
            LineWidth = 1;
            X = x.ToArray();
            Y = y.ToArray();
            MinimumX = double.MaxValue;
            MinimumY = double.MaxValue;
            MaximumX = double.MinValue;
            MaximumY = double.MinValue;
            for (int i = 0; i < X.Length; i++)
            {
                MinimumX = MinimumX > X[i] ? X[i] : MinimumX;
                MinimumY = MinimumY > Y[i] ? Y[i] : MinimumY;
                MaximumX = MaximumX < X[i] ? X[i] : MaximumX;
                MaximumY = MaximumY < Y[i] ? Y[i] : MaximumY;
            }
        }
        public XY(IEnumerable<double> x, IEnumerable<double> y, string name) : this(x, y)
        {
            Name = name;
        }
        public XY(IEnumerable<double> x, IEnumerable<double> y, string axisHorizontalName, string axisVerticalName) : this(x, y)
        {
            AxisHorizontalName = axisHorizontalName;
            AxisVerticalName = axisVerticalName;
        }
        public XY(IEnumerable<double> x, IEnumerable<double> y, string name, string axisHorizontalName, string axisVerticalName) : this(x, y)
        {
            Name = name;
            AxisHorizontalName = axisHorizontalName;
            AxisVerticalName = axisVerticalName;
        }

        public void Draw(Graphics g, Axis vertical, Axis horizontal, RectangleExtended plotRectangle, int plotIndex)
        {
            if (LineColor.R == 0 && LineColor.G == 0 && LineColor.B == 0 && LineColor.A == 0)
                LineColor = defaultPlotColors[plotIndex % 3];
            if (MarkColor.R == 0 && MarkColor.G == 0 && MarkColor.B == 0 && MarkColor.A == 0)
                MarkColor = defaultPlotColors[plotIndex % 3];

            float leftLimit = plotRectangle.Left - 100;
            float rightLimit = plotRectangle.Right + 100;
            float topLimit = plotRectangle.Left - 100;
            float bottomLimit = plotRectangle.Right + 100;

            List<PointF> pointList = new List<PointF>();
            float prevX = horizontal.Transform(X[0]);
            float prevY = vertical.Transform(Y[0]);
            if (prevX > leftLimit && prevX < rightLimit && prevY > topLimit && prevY < bottomLimit)
                pointList.Add(new PointF(prevX, prevY));
            float curX, curY;
            for (int i = 1; i < X.Length; i++)
            {
                curX = horizontal.Transform(X[i]);
                if (curX > leftLimit && curX < rightLimit)
                {
                    curY = vertical.Transform(Y[i]);
                    if(curY > topLimit && curY < bottomLimit)
                        if (curX - prevX > 1 || curY - prevY > 1 || prevX - curX > 1 || prevY - curY > 1)
                        {
                            prevX = curX;
                            prevY = curY;
                            pointList.Add(new PointF(prevX, prevY));
                        }
                }
            }

            var pointArray = pointList.ToArray();

            if (Style == PlotStyle.Line || Style == PlotStyle.Both)
            {
                Pen linePen = new Pen(LineColor, LineWidth);
                linePen.DashStyle = LineStyle;
                if (pointList.Count > 1)
                {
                    if(Interpolation == Interpolation.Line)
                        g.DrawLines(linePen, pointArray);
                    else if (Interpolation == Interpolation.Spline)
                        g.DrawCurve(linePen, pointArray);
                    else if(Interpolation == Interpolation.StepNear)
                    {
                        for(int i = 0; i < pointArray.Length - 1; i++)
                        {
                            g.DrawLine(linePen, pointArray[i].X, pointArray[i].Y, pointArray[i].X, pointArray[i + 1].Y);
                            g.DrawLine(linePen, pointArray[i].X, pointArray[i + 1].Y, pointArray[i + 1].X, pointArray[i + 1].Y);
                        }
                    }
                    else if (Interpolation == Interpolation.StepFar)
                    {
                        for (int i = 0; i < pointArray.Length - 1; i++)
                        {
                            g.DrawLine(linePen, pointArray[i].X, pointArray[i].Y, pointArray[i + 1].X, pointArray[i].Y);
                            g.DrawLine(linePen, pointArray[i + 1].X, pointArray[i].Y, pointArray[i + 1].X, pointArray[i + 1].Y);
                        }
                    }
                    else if (Interpolation == Interpolation.StepCenter)
                    {
                        float center1 = (pointArray[1].X + pointArray[0].X) / 2;
                        g.DrawLine(linePen, pointArray[0].X, pointArray[0].Y, center1, pointArray[0].Y);
                        float center2;

                        for (int i = 1; i < pointArray.Length - 1; i++)
                        {
                            g.DrawLine(linePen, center1, pointArray[i - 1].Y, center1, pointArray[i].Y);
                            center2 = (pointArray[i + 1].X + pointArray[i].X) / 2;
                            g.DrawLine(linePen, center1, pointArray[i].Y, center2, pointArray[i].Y);
                            center1 = center2;
                        }
                        g.DrawLine(linePen, center1, pointArray[pointArray.Length - 2].Y, center1, pointArray[pointArray.Length - 1].Y);
                        g.DrawLine(linePen, center1, pointArray[pointArray.Length - 1].Y, pointArray[pointArray.Length - 1].X, pointArray[pointArray.Length - 1].Y);
                    }
                    else if (Interpolation == Interpolation.StepVertical)
                    {
                        float center1 = (pointArray[1].Y + pointArray[0].Y) / 2;
                        g.DrawLine(linePen, pointArray[0].X, pointArray[0].Y, pointArray[0].X, center1);
                        float center2;

                        for (int i = 1; i < pointArray.Length - 1; i++)
                        {
                            g.DrawLine(linePen, pointArray[i - 1].X, center1, pointArray[i].X, center1);
                            center2 = (pointArray[i + 1].Y + pointArray[i].Y) / 2;
                            g.DrawLine(linePen, pointArray[i].X, center1, pointArray[i].X, center2);
                            center1 = center2;
                        }
                        g.DrawLine(linePen, pointArray[pointArray.Length - 2].X, center1, pointArray[pointArray.Length - 1].X, center1);
                        g.DrawLine(linePen, pointArray[pointArray.Length - 1].X, center1, pointArray[pointArray.Length - 1].X, pointArray[pointArray.Length - 1].Y);
                    }
                }
            }
            if (Style == PlotStyle.Marker || Style == PlotStyle.Both)
                Marker.Draw(g, MarkColor, MarkStyle, MarkSize, pointArray);
        }

    }

    public enum Interpolation
    {
        Line,
        Spline,
        StepNear, 
        StepFar,
        StepCenter, 
        StepVertical
    }
}
