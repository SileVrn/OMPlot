using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public class XY : IData
    {
        Color[] defaultPlotColors = new Color[] { Color.Red, Color.Blue, Color.Green };

        double[] X, Y;
        PointF[] points;

        public PlotStyle Style { get; set; }
        public Color LineColor { get; set; }
        public System.Drawing.Drawing2D.DashStyle LineStyle { get; set; }
        public float LineWidth { get; set; }
        public Color MarkColor { get; set; }
        public MarkerStyle MarkStyle { get; set; }
        public float MarkSize { get; set; }
        public PlotInterpolation Interpolation { get; set; }
        public PlotFill Fill { get; set; }
        public Color FillColor { get; set; }
        public double FillValue { get; set; }
        public IData FillPlot { get; set; }

        public string Name { get; set; }
        public string AxisHorizontalName { get; set; }
        public string AxisVerticalName { get; set; }

        public double MinimumX { get; private set; }
        public double MinimumY { get; private set; }
        public double MaximumX { get; private set; }
        public double MaximumY { get; private set; }

        public GraphicsPath GraphicsPath { get; private set; }

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

        public void Calculate(Axis vertical, Axis horizontal, RectangleExtended plotRectangle)
        {
            float leftLimit = plotRectangle.Left - 100;
            float rightLimit = plotRectangle.Right + 100;
            float topLimit = plotRectangle.Top - 100;
            float bottomLimit = plotRectangle.Bottom + 100;

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
                    if (curY > topLimit && curY < bottomLimit)
                        if (curX - prevX > 1 || curY - prevY > 1 || prevX - curX > 1 || prevY - curY > 1)
                        {
                            prevX = curX;
                            prevY = curY;
                            pointList.Add(new PointF(prevX, prevY));
                        }
                }
            }

            points = pointList.ToArray();
            GraphicsPath = new GraphicsPath();

            if (points.Length > 1)
            {
                if (Interpolation == PlotInterpolation.Line)
                    GraphicsPath.AddLines(points);
                else if (Interpolation == PlotInterpolation.Spline)
                    GraphicsPath.AddCurve(points);
                else if (Interpolation == PlotInterpolation.StepNear)
                {
                    for (int i = 0; i < points.Length - 1; i++)
                    {
                        GraphicsPath.AddLine(points[i].X, points[i].Y, points[i].X, points[i + 1].Y);
                        GraphicsPath.AddLine(points[i].X, points[i + 1].Y, points[i + 1].X, points[i + 1].Y);
                    }
                }
                else if (Interpolation == PlotInterpolation.StepFar)
                {
                    for (int i = 0; i < points.Length - 1; i++)
                    {
                        GraphicsPath.AddLine(points[i].X, points[i].Y, points[i + 1].X, points[i].Y);
                        GraphicsPath.AddLine(points[i + 1].X, points[i].Y, points[i + 1].X, points[i + 1].Y);
                    }
                }
                else if (Interpolation == PlotInterpolation.StepCenter)
                {
                    float center1 = (points[1].X + points[0].X) / 2;
                    GraphicsPath.AddLine(points[0].X, points[0].Y, center1, points[0].Y);
                    float center2;

                    for (int i = 1; i < points.Length - 1; i++)
                    {
                        GraphicsPath.AddLine(center1, points[i - 1].Y, center1, points[i].Y);
                        center2 = (points[i + 1].X + points[i].X) / 2;
                        GraphicsPath.AddLine(center1, points[i].Y, center2, points[i].Y);
                        center1 = center2;
                    }
                    GraphicsPath.AddLine(center1, points[points.Length - 2].Y, center1, points[points.Length - 1].Y);
                    GraphicsPath.AddLine(center1, points[points.Length - 1].Y, points[points.Length - 1].X, points[points.Length - 1].Y);
                }
                else if (Interpolation == PlotInterpolation.StepVertical)
                {
                    float center1 = (points[1].Y + points[0].Y) / 2;
                    GraphicsPath.AddLine(points[0].X, points[0].Y, points[0].X, center1);
                    float center2;

                    for (int i = 1; i < points.Length - 1; i++)
                    {
                        GraphicsPath.AddLine(points[i - 1].X, center1, points[i].X, center1);
                        center2 = (points[i + 1].Y + points[i].Y) / 2;
                        GraphicsPath.AddLine(points[i].X, center1, points[i].X, center2);
                        center1 = center2;
                    }
                    GraphicsPath.AddLine(points[points.Length - 2].X, center1, points[points.Length - 1].X, center1);
                    GraphicsPath.AddLine(points[points.Length - 1].X, center1, points[points.Length - 1].X, points[points.Length - 1].Y);
                }
            }
        }
        public void Draw(Graphics g, Axis vertical, Axis horizontal, RectangleExtended plotRectangle, int plotIndex)
        {
            if (LineColor.R == 0 && LineColor.G == 0 && LineColor.B == 0 && LineColor.A == 0)
                LineColor = defaultPlotColors[plotIndex % 3];
            if (MarkColor.R == 0 && MarkColor.G == 0 && MarkColor.B == 0 && MarkColor.A == 0)
                MarkColor = defaultPlotColors[plotIndex % 3];
            if (FillColor.R == 0 && FillColor.G == 0 && FillColor.B == 0 && FillColor.A == 0)
                FillColor = defaultPlotColors[plotIndex % 3];

            if (Style == PlotStyle.Marker || Style == PlotStyle.Both)
                Marker.Draw(g, MarkColor, MarkStyle, MarkSize, points);

            if (points.Length > 1)
            {
                if (Style == PlotStyle.Line || Style == PlotStyle.Both)
                {
                    Pen linePen = new Pen(LineColor, LineWidth) { DashStyle = LineStyle };
                    g.DrawPath(linePen, GraphicsPath);
                }

                if (Fill == PlotFill.ToNInfitity)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddPath(GraphicsPath, true);
                    path.AddLine(points[0].X, points[0].Y, points[0].X, plotRectangle.Bottom);
                    path.AddLine(points[0].X, plotRectangle.Bottom, points[points.Length - 1].X, plotRectangle.Bottom);
                    path.AddLine(points[points.Length - 1].X, plotRectangle.Bottom, points[points.Length - 1].X, points[points.Length - 1].Y);
                    Brush fillBrush = new SolidBrush(FillColor);
                    g.FillPath(fillBrush, path);
                }
                else if (Fill == PlotFill.ToPInfinity)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddPath(GraphicsPath, true);
                    path.AddLine(points[0].X, points[0].Y, points[0].X, plotRectangle.Top);
                    path.AddLine(points[0].X, plotRectangle.Top, points[points.Length - 1].X, plotRectangle.Top);
                    path.AddLine(points[points.Length - 1].X, plotRectangle.Top, points[points.Length - 1].X, points[points.Length - 1].Y);
                    Brush fillBrush = new SolidBrush(FillColor);
                    g.FillPath(fillBrush, path);
                }
                else if (Fill == PlotFill.ToValue)
                {
                    float fillValue = vertical.Transform(FillValue);
                    GraphicsPath path = new GraphicsPath();
                    path.AddPath(GraphicsPath, true);
                    path.AddLine(points[0].X, points[0].Y, points[0].X, fillValue);
                    path.AddLine(points[0].X, fillValue, points[points.Length - 1].X, fillValue);
                    path.AddLine(points[points.Length - 1].X, fillValue, points[points.Length - 1].X, points[points.Length - 1].Y);
                    Brush fillBrush = new SolidBrush(FillColor);

                    g.FillPath(fillBrush, path);
                }
                else if (Fill == PlotFill.ToPlot)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddPath(GraphicsPath, true);
                    path.Reverse();
                    path.AddPath(FillPlot.GraphicsPath, true);
                    Brush fillBrush = new SolidBrush(FillColor);
                    g.FillPath(fillBrush, path);
                }
            }
        }

    }

    public enum PlotInterpolation
    {
        Line,
        Spline,
        StepNear, 
        StepFar,
        StepCenter, 
        StepVertical
    }

    public enum PlotFill
    {
        None,
        ToValue,
        ToNInfitity,
        ToPInfinity,
        ToPlot
    }
}
