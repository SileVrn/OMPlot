using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public class XY : IData, IBar, IMarker, ILine, IFill
    {        
        double[] X, Y;
        PointF[] points;
        PointF[] flatten;
        RectangleF[] bars;
        GraphicsPath fillPath;

        public PlotInterpolation Interpolation { get; set; }
        public LineStyle LineStyle { get; set; }
        public float LineWidth { get; set; }
        public Color LineColor { get; set; }
        public MarkerStyle MarkStyle { get; set; }
        public float MarkSize { get; set; }
        public Color MarkColor { get; set; }
        public FillStyle FillStyle { get; set; }
        public Color FillColor { get; set; }
        public double FillValue { get; set; }
        public IData FillPlot { get; set; }
        public BarStyle BarStyle { get; set; }
        public Color BarLineColor { get; set; }
        public Color BarFillColor { get; set; }
        public float BarDuty { get; set; }
        public double BarValue { get; set; }
        public bool BarStacking { get; set; }
        public int BarIndex { get; set; }
        public int BarCount { get; set; }

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
            BarDuty = 1;
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
        public PointDistance DistanceToPoint(double x, double y)
        {
            if (LineStyle != LineStyle.None)
            {
                double minDistance = double.MaxValue;
                PointF interpolatedPoint = new PointF(float.NaN, float.NaN);

                for (int i = 1; i < flatten.Length; i++)
                {
                    double prod1 = (x - flatten[i].X) * (flatten[i - 1].X - flatten[i].X) + (y - flatten[i].Y) * (flatten[i - 1].Y - flatten[i].Y);
                    double prod2 = (x - flatten[i - 1].X) * (flatten[i].X - flatten[i - 1].X) + (y - flatten[i - 1].Y) * (flatten[i].Y - flatten[i - 1].Y);

                    if (prod1 >= 0 && prod2 >= 0)
                    {
                        double A = flatten[i].Y - flatten[i - 1].Y;
                        double B = flatten[i - 1].X - flatten[i].X;
                        double C = -A * flatten[i].X - B * flatten[i].Y;
                        double d = A * A + B * B;

                        if (Math.Abs(B) < float.Epsilon)
                        {
                            double distance = Math.Abs(A * (flatten[i].X - x)) / Math.Sqrt(d);
                            if (minDistance > distance)
                            {
                                minDistance = distance;
                                interpolatedPoint = new PointF(flatten[i].X, (float)y);
                            }
                        }
                        else
                        {
                            double distance = Math.Abs(-B * (flatten[i - 1].Y - y) - A * (flatten[i - 1].X - x)) / Math.Sqrt(d);
                            if (minDistance > distance)
                            {
                                minDistance = distance;
                                var pointX = (B * B * x - A * B * y - A * C) / d;
                                interpolatedPoint = new PointF((float)pointX, (float)((-A * pointX - C) / B));
                            }
                        }
                    }
                    else
                    {
                        double distance1 = flatten[i].Distance(x, y);
                        double distance2 = flatten[i - 1].Distance(x, y);
                        if (distance1 > distance2 && minDistance > distance2)
                        {
                            minDistance = distance2;
                            interpolatedPoint = flatten[i - 1];
                        }
                        else if (minDistance > distance1)
                        {
                            minDistance = distance1;
                            interpolatedPoint = flatten[i];
                        }
                    }
                }
                if(minDistance < Plot.MouseEventDistance)
                    return new PointDistance() { Point = interpolatedPoint, Distance = minDistance };
            }
            if(BarStyle != BarStyle.None)
            {
                for(int i = 0; i < bars.Length; i++)
                {
                    if(bars[i].Contains((float)x, (float)y))
                        return new PointDistance() { Point = points[i], Distance = 0 };
                }
            }
            if (MarkStyle != MarkerStyle.None)
            {
                double minDistance = double.MaxValue;
                PointF point = new PointF(float.NaN, float.NaN);
                double distance;

                for (int i = 0; i < points.Length; i++)
                {
                    distance = points[i].Distance(x, y);
                    if((distance < MarkSize / 2 + Plot.MouseEventDistance) && distance < minDistance)
                    {
                        point = points[i];
                        minDistance = distance;
                    }
                }
                return new PointDistance() { Point = point, Distance = minDistance };
            }
            return new PointDistance() { Point = new PointF(float.NaN, float.NaN), Distance = double.MaxValue };
        }

        private PointF[] CalculatePoints(Axis vertical, Axis horizontal)
        {
            List<PointF> pointList = new List<PointF>();
            float prevX = horizontal.Transform(X[0]);
            float prevY = vertical.Transform(Y[0]);
            pointList.Add(new PointF(prevX, prevY));
            float curX, curY;
            for (int i = 1; i < X.Length; i++)
            {
                curX = horizontal.Transform(X[i]);
                curY = vertical.Transform(Y[i]);
                if (curX - prevX > 1.5 || curY - prevY > 1.5 || prevX - curX > 1.5 || prevY - curY > 1.5)
                {
                    prevX = curX;
                    prevY = curY;
                    pointList.Add(new PointF(prevX, prevY));
                }
            }
            return pointList.ToArray();
        }

        public void CalculateGraphics(Axis vertical, Axis horizontal, RectangleExtended plotRectangle)
        {
            points = CalculatePoints(vertical, horizontal);
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

                GraphicsPath flattenPath = new GraphicsPath();
                flattenPath.AddPath(GraphicsPath, false);
                flattenPath.Flatten();
                flatten = flattenPath.PathPoints;

                if (FillStyle == FillStyle.ToNInfitity)
                {
                    fillPath = new GraphicsPath();
                    fillPath.AddPath(GraphicsPath, true);
                    fillPath.AddLine(points[0].X, points[0].Y, points[0].X, plotRectangle.Bottom);
                    fillPath.AddLine(points[0].X, plotRectangle.Bottom, points[points.Length - 1].X, plotRectangle.Bottom);
                    fillPath.AddLine(points[points.Length - 1].X, plotRectangle.Bottom, points[points.Length - 1].X, points[points.Length - 1].Y);
                }
                else if (FillStyle == FillStyle.ToPInfinity)
                {
                    fillPath = new GraphicsPath();
                    fillPath.AddPath(GraphicsPath, true);
                    fillPath.AddLine(points[0].X, points[0].Y, points[0].X, plotRectangle.Top);
                    fillPath.AddLine(points[0].X, plotRectangle.Top, points[points.Length - 1].X, plotRectangle.Top);
                    fillPath.AddLine(points[points.Length - 1].X, plotRectangle.Top, points[points.Length - 1].X, points[points.Length - 1].Y);
                }
                else if (FillStyle == FillStyle.ToValue)
                {
                    float fillValue = vertical.Transform(FillValue);
                    fillPath = new GraphicsPath();
                    fillPath.AddPath(GraphicsPath, true);
                    fillPath.AddLine(points[0].X, points[0].Y, points[0].X, fillValue);
                    fillPath.AddLine(points[0].X, fillValue, points[points.Length - 1].X, fillValue);
                    fillPath.AddLine(points[points.Length - 1].X, fillValue, points[points.Length - 1].X, points[points.Length - 1].Y);
                }
                else if (FillStyle == FillStyle.ToPlot && FillPlot != null)
                {
                    GraphicsPath path = new GraphicsPath();
                    fillPath.AddPath(GraphicsPath, true);
                    fillPath.Reverse();
                    fillPath.AddPath(FillPlot.GraphicsPath, true);
                }

                if (BarStyle != BarStyle.None)
                {
                    bars = new RectangleF[points.Length];
                    float barCount = BarStacking ? BarCount : 1.0f;

                    if (BarStyle == BarStyle.Vertical)
                    {
                        float refPositionY = vertical.Transform(BarValue);

                        for (int i = 0; i < points.Length; i++)
                        {     
                            bars[i] = new RectangleF();                       
                            if (i == 0)
                                bars[i].Width = Math.Abs(points[1].X - points[0].X) / barCount;
                            else if (i == points.Length - 1)
                                bars[i].Width = Math.Abs(points[points.Length - 1].X - points[points.Length - 2].X) / barCount;
                            else
                                bars[i].Width = Math.Abs(points[i + 1].X - points[i - 1].X) / 2.0f / barCount;
                            bars[i].Width *= BarDuty;
                            bars[i].X = i == 0 ? points[0].X - bars[i].Width * barCount / 2.0f : (points[i].X + points[i - 1].X) / 2.0f;
                            bars[i].X += BarIndex * bars[i].Width + (bars[i].Width - bars[i].Width) / 2.0f;
                            bars[i].X -= (i != 0 && points[i].X < points[i - 1].X) ? bars[i].Width * barCount : 0;

                            if (refPositionY > points[i].Y)
                            {
                                bars[i].Y = points[i].Y;
                                bars[i].Height = refPositionY - points[i].Y;
                            }
                            else
                            {
                                bars[i].Y = refPositionY;
                                bars[i].Height = points[i].Y - refPositionY;
                            }                            
                        }
                    }
                    else if (BarStyle == BarStyle.Horisontal)
                    {
                        float refPositionX = horizontal.Transform(BarValue);
                        for (int i = 0; i < points.Length; i++)
                        {
                            bars[i] = new RectangleF();
                            if (i == 0)
                                bars[i].Height = Math.Abs(points[1].Y - points[0].Y) / barCount;
                            else if (i == points.Length - 1)
                                bars[i].Height = Math.Abs(points[points.Length - 1].Y - points[points.Length - 2].Y) / barCount;
                            else
                                bars[i].Height = Math.Abs(points[i + 1].Y - points[i - 1].Y) / 2.0f / barCount;
                            bars[i].Height *= BarDuty;
                            bars[i].Y = i == 0 ? points[0].Y - bars[i].Height * barCount / 2.0f : (points[i].Y + points[i - 1].Y) / 2.0f;
                            bars[i].Y += BarIndex * bars[i].Height + (bars[i].Height - bars[i].Height) / 2.0f;
                            bars[i].Y -= (i != 0 && points[i].Y < points[i - 1].Y) ? bars[i].Height * barCount : 0;

                            if (refPositionX > points[i].X)
                            {
                                bars[i].X = points[i].X;
                                bars[i].Width = refPositionX - points[i].X;
                            }
                            else
                            {
                                bars[i].X = refPositionX;
                                bars[i].Width = points[i].X - refPositionX;
                            }
                        }
                    }
                }
            }
        }
        public void Draw(Graphics g, Axis vertical, Axis horizontal, RectangleExtended plotRectangle, int plotIndex)
        {
            Marker.Draw(g, MarkColor, MarkStyle, MarkSize, points);

            if (points.Length > 1)
            {
                Line.DrawPath(g, LineColor, LineStyle, LineWidth, GraphicsPath);

                if(FillStyle != FillStyle.None)
                    g.FillPath(new SolidBrush(FillColor), fillPath);

                if (BarStyle != BarStyle.None)
                {
                    if (BarFillColor.A > 0)
                        g.FillRectangles(new SolidBrush(BarFillColor), bars);
                    if (BarLineColor.A > 0)
                        g.DrawRectangles(new Pen(BarLineColor), bars);
                }
            }
        }
        public void DrawLegend(Graphics g, RectangleF rect)
        {
            Marker.Draw(g, MarkColor, MarkStyle, MarkSize, new PointF[] { new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f) });

            if (points.Length > 1)
            {
                if (LineStyle != LineStyle.None)
                {
                    Line.DrawLine(g, LineColor, LineStyle, LineWidth, rect.X, rect.Y + 0.7f * rect.Height, rect.X + 0.5f * rect.Width, rect.Y + 0.5f * rect.Height);
                    Line.DrawLine(g, LineColor, LineStyle, LineWidth, rect.X + 0.5f * rect.Width, rect.Y + 0.5f * rect.Height, rect.X + rect.Width, rect.Y + 0.7f * rect.Height);
                }
                if (FillStyle != FillStyle.None)
                {
                    Brush fillBrush = new SolidBrush(FillColor);
                    g.FillRectangle(fillBrush, rect.X, rect.Y + rect.Height / 2f, rect.Width, rect.Height / 2f);
                }

                if (BarStyle != BarStyle.None)
                {
                    Brush barBrush = new SolidBrush(BarFillColor);
                    Pen barPen = new Pen(BarLineColor);

                    if (BarFillColor.A > 0)
                    {
                        g.FillRectangle(barBrush, rect.X, rect.Y + 0.7f * rect.Height, rect.Width * 0.25f, 0.3f * rect.Height);
                        g.FillRectangle(barBrush, rect.X + 0.33f * rect.Width, rect.Y + 0.3f * rect.Height, rect.Width * 0.25f, 0.7f * rect.Height);
                        g.FillRectangle(barBrush, rect.X + 0.66f * rect.Width, rect.Y + 0.5f * rect.Height, rect.Width * 0.25f, 0.5f * rect.Height);
                    }
                    if (BarLineColor.A > 0)
                    {
                        g.DrawRectangle(barPen, rect.X, rect.Y + 0.7f * rect.Height, rect.Width * 0.25f, 0.3f * rect.Height);
                        g.DrawRectangle(barPen, rect.X + 0.33f * rect.Width, rect.Y + 0.3f * rect.Height, rect.Width * 0.25f, 0.7f * rect.Height);
                        g.DrawRectangle(barPen, rect.X + 0.66f * rect.Width, rect.Y + 0.5f * rect.Height, rect.Width * 0.25f, 0.5f * rect.Height);
                    }
                }
            }
        }
    }
}
