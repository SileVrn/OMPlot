using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public class XYSeries
    {        
        double[] X, Y;

        protected PointF[] points;
        protected PointF[] flatten;
        protected PointF[] flattenFill;
        protected RectangleF[] bars;

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
        public XYSeries FillPlot { get; set; }
        public BarStyle BarStyle { get; set; }
        public Color BarLineColor { get; set; }
        public Color BarFillColor { get; set; }
        public float BarDuty { get; set; }
        public double BarValue { get; set; }
        public bool BarStacking { get; set; }
        public int BarIndex { get; set; }
        public int BarCount { get; set; }

        public string Name { get; set; }
        public Axis HorizontalAxis { get; set; }
        public Axis VerticalAxis { get; set; }

        protected double minX, maxX, minY, maxY;
        protected double minXPre, maxXPre, minYPre, maxYPre;
        protected int minXIndex, maxXIndex, minYIndex, maxYIndex;
        public double MinimumX { get  {  return minX - (minXPre - minX) / 2; } }
        public double MinimumY { get { return minY - (minYPre - minY) / 2; } }
        public double MaximumX { get { return maxX + (maxX - maxXPre) / 2; } }
        public double MaximumY { get { return maxY + (maxY - maxYPre) / 2; } }

        public GraphicsPath GraphicsPath { get; protected set; }


        protected XYSeries()
        {
            BarDuty = 1;
            MarkSize = 10;
            LineWidth = 1;
            LineStyle = LineStyle.Solid;
            MarkStyle = MarkerStyle.None;
            BarStyle = BarStyle.None;
            FillStyle = FillStyle.None;
            LineColor = Color.Black;
            minX = double.MaxValue;
            minY = double.MaxValue;
            maxX = double.MinValue;
            maxY = double.MinValue;
            minXPre = double.MaxValue;
            minYPre = double.MaxValue;
            maxXPre = double.MinValue;
            maxYPre = double.MinValue;
        }
        public XYSeries(IEnumerable<double> x, IEnumerable<double> y) : this()
        {
            X = x.ToArray();
            Y = y.ToArray();
            for (int i = 0; i < X.Length; i++)
            {
                if (minX > X[i])
                {
                    minX = X[i];
                    minXIndex = i;
                }
                if (minY > Y[i])
                {
                    minY = Y[i];
                    minYIndex = i;
                }
                if (maxX < X[i])
                {
                    maxX = X[i];
                    maxXIndex = i;
                }
                if (maxY < Y[i])
                {
                    maxY = Y[i];
                    maxYIndex = i;
                }
            }
            for (int i = 0; i < X.Length; i++)
            {
                if (minXPre > X[i] && i != minXIndex)
                    minXPre = X[i];
                if (minYPre > Y[i] && i != minYIndex)
                    minYPre = Y[i];
                if (maxXPre < X[i] && i != maxXIndex)
                    maxXPre = X[i];
                if (maxYPre < Y[i] && i != maxYIndex)
                    maxYPre = Y[i];
            }
        }
        public XYSeries(IEnumerable<double> x, IEnumerable<double> y, string name) : this(x, y)
        {
            Name = name;
        }
        public XYSeries(IEnumerable<double> x, IEnumerable<double> y, Axis horizontalAxis, Axis verticalAxis) : this(x, y)
        {
            HorizontalAxis = horizontalAxis;
            VerticalAxis = verticalAxis;
        }
        public XYSeries(IEnumerable<double> x, IEnumerable<double> y, string name, Axis horizontalAxis, Axis verticalAxis) : this(x, y)
        {
            Name = name;
            HorizontalAxis = horizontalAxis;
            VerticalAxis = verticalAxis;
        }
        public virtual PointDistance DistanceToPoint(double x, double y)
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
            if(FillStyle != FillStyle.None)
            {
                double angleSum = 0;
                double aX = flattenFill[0].X - x;
                double aY = flattenFill[0].Y - y;
                double aLength = Math.Sqrt(aX * aX + aY * aY);
                double bX, bY, bLength;
                double dotProd, angle, crosProdZ;
                for (int i = 0; i < flattenFill.Length - 1; i++)
                {
                    bX = flattenFill[i + 1].X - x;
                    bY = flattenFill[i + 1].Y - y;
                    bLength = Math.Sqrt(bX * bX + bY * bY);
                    dotProd = aX * bX + aY * bY;
                    angle = Math.Acos(dotProd / aLength / bLength);
                    crosProdZ = aX * bY - aY * bX;

                    if (crosProdZ > 0)
                        angleSum += angle;
                    else
                        angleSum -= angle;
                    aX = bX;
                    aY = bY;
                    aLength = bLength;
                }
                bX = flattenFill[0].X - x;
                bY = flattenFill[0].Y - y;
                bLength = Math.Sqrt(bX * bX + bY * bY);
                dotProd = aX * bX + aY * bY;
                angle = Math.Acos(dotProd / aLength / bLength);
                crosProdZ = aX * bY - aY * bX;
                if (crosProdZ > 0)
                    angleSum += angle;
                else
                    angleSum -= angle;
                angleSum /= 2 * Math.PI;
                angleSum = Math.Round(angleSum);
                if(angleSum != 0)
                    return new PointDistance() { Point = new PointF((float)x, (float)y), Distance = 0 };

            }
            return new PointDistance() { Point = new PointF(float.NaN, float.NaN), Distance = double.MaxValue };
        }

        protected virtual PointF[] CalculatePoints()
        {
            List<PointF> pointList = new List<PointF>();
            float prevX = (float)HorizontalAxis.Transform(X[0]);
            float prevY = (float)VerticalAxis.Transform(Y[0]);
            pointList.Add(new PointF(prevX, prevY));
            float curX, curY;
            for (int i = 1; i < X.Length; i++)
            {
                curX = (float)HorizontalAxis.Transform(X[i]);
                curY = (float)VerticalAxis.Transform(Y[i]);
                if (curX - prevX > 1.5 || curY - prevY > 1.5 || prevX - curX > 1.5 || prevY - curY > 1.5)
                {
                    prevX = curX;
                    prevY = curY;
                    pointList.Add(new PointF(prevX, prevY));
                }
            }
            return pointList.ToArray();
        }
        public virtual void CalculateGraphics(RectangleExtended plotRectangle)
        {
            points = CalculatePoints();
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

                if (BarStyle != BarStyle.None)
                {
                    bars = new RectangleF[points.Length];
                    float barCount = BarStacking ? BarCount : 1.0f;
                    float W;

                    if (BarStyle == BarStyle.Vertical)
                    {
                        float refPositionY = (float)VerticalAxis.Transform(BarValue);

                        for (int i = 0; i < points.Length; i++)
                        {     
                            bars[i] = new RectangleF();                       
                            if (i == 0)
                                W = Math.Abs(points[1].X - points[0].X) / barCount;
                            else if (i == points.Length - 1)
                                W = Math.Abs(points[points.Length - 1].X - points[points.Length - 2].X) / barCount;
                            else
                                W = Math.Abs(points[i + 1].X - points[i - 1].X) / 2.0f / barCount;
                            bars[i].Width = W * BarDuty;
                            bars[i].X = points[i].X;
                            bars[i].X -= i == 0 ? (BarDuty * Math.Abs(points[1].X - points[0].X) / 2.0f) : (BarDuty * Math.Abs(points[i].X - points[i - 1].X) / 2.0f);
                            bars[i].X += BarIndex * bars[i].Width;
                            
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
                        float refPositionX = (float)HorizontalAxis.Transform(BarValue);
                        float H;
                        for (int i = 0; i < points.Length; i++)
                        {
                            bars[i] = new RectangleF();
                            if (i == 0)
                                H = Math.Abs(points[1].Y - points[0].Y) / barCount;
                            else if (i == points.Length - 1)
                                H = Math.Abs(points[points.Length - 1].Y - points[points.Length - 2].Y) / barCount;
                            else
                                H = Math.Abs(points[i + 1].Y - points[i - 1].Y) / 2.0f / barCount;
                            bars[i].Height = H * BarDuty;
                            bars[i].Y = points[i].Y;
                            bars[i].Y -= i == 0 ? (BarDuty * Math.Abs(points[1].Y - points[0].Y) / 2.0f) : (BarDuty * Math.Abs(points[i].Y - points[i - 1].Y) / 2.0f);
                            bars[i].Y += (barCount - BarIndex - 1) * bars[i].Height;

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
        public virtual void Draw(Graphics g, RectangleExtended plotRectangle, int plotIndex)
        {
            if (points.Length > 1)
            {
                if (BarStyle != BarStyle.None)
                {
                    if (BarFillColor.A > 0)
                        g.FillRectangles(new SolidBrush(BarFillColor), bars);
                    if (BarLineColor.A > 0)
                        g.DrawRectangles(new Pen(BarLineColor), bars);
                }
                if (FillStyle != FillStyle.None)
                {
                    GraphicsPath fillPath = new GraphicsPath();
                    if (FillStyle == FillStyle.ToNInfitity)
                    {
                        fillPath.AddPath(GraphicsPath, true);
                        fillPath.AddLine(points[0].X, points[0].Y, points[0].X, plotRectangle.Bottom);
                        fillPath.AddLine(points[0].X, plotRectangle.Bottom, points[points.Length - 1].X, plotRectangle.Bottom);
                        fillPath.AddLine(points[points.Length - 1].X, plotRectangle.Bottom, points[points.Length - 1].X, points[points.Length - 1].Y);
                    }
                    else if (FillStyle == FillStyle.ToPInfinity)
                    {
                        fillPath.AddPath(GraphicsPath, true);
                        fillPath.AddLine(points[0].X, points[0].Y, points[0].X, plotRectangle.Top);
                        fillPath.AddLine(points[0].X, plotRectangle.Top, points[points.Length - 1].X, plotRectangle.Top);
                        fillPath.AddLine(points[points.Length - 1].X, plotRectangle.Top, points[points.Length - 1].X, points[points.Length - 1].Y);
                    }
                    else if (FillStyle == FillStyle.ToValue)
                    {
                        float fillValue = (float)VerticalAxis.Transform(FillValue);
                        fillPath.AddPath(GraphicsPath, true);
                        fillPath.AddLine(points[0].X, points[0].Y, points[0].X, fillValue);
                        fillPath.AddLine(points[0].X, fillValue, points[points.Length - 1].X, fillValue);
                        fillPath.AddLine(points[points.Length - 1].X, fillValue, points[points.Length - 1].X, points[points.Length - 1].Y);
                    }
                    else if (FillStyle == FillStyle.ToPlot && FillPlot != null)
                    {
                        fillPath.AddPath(GraphicsPath, true);
                        fillPath.Reverse();
                        fillPath.AddPath(FillPlot.GraphicsPath, true);
                    }
                    g.FillPath(new SolidBrush(FillColor), fillPath);
                    fillPath.Flatten();
                    flattenFill = fillPath.PathPoints;
                }
                Line.DrawPath(g, LineColor, LineStyle, LineWidth, GraphicsPath);
                Marker.Draw(g, MarkColor, MarkStyle, MarkSize, points);
            }
        }
        public virtual void DrawLegend(Graphics g, RectangleF rect)
        {
            Marker.Draw(g, MarkColor, MarkStyle, MarkSize, new PointF[] { new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f) });

            if (points.Length > 1)
            {
                if (FillStyle != FillStyle.None)
                {
                    Brush fillBrush = new SolidBrush(FillColor);
                    g.FillPolygon(fillBrush, new PointF[] { new PointF(rect.X, rect.Y + rect.Height), new PointF(rect.X, rect.Y + 0.7f * rect.Height), new PointF(rect.X + 0.5f * rect.Width, rect.Y + 0.5f * rect.Height), new PointF(rect.X + rect.Width, rect.Y + 0.7f * rect.Height), new PointF(rect.X + rect.Width, rect.Y + rect.Height) }); 
                }
                if (LineStyle != LineStyle.None)
                {
                    Line.DrawLine(g, LineColor, LineStyle, LineWidth, rect.X, rect.Y + 0.7f * rect.Height, rect.X + 0.5f * rect.Width, rect.Y + 0.5f * rect.Height);
                    Line.DrawLine(g, LineColor, LineStyle, LineWidth, rect.X + 0.5f * rect.Width, rect.Y + 0.5f * rect.Height, rect.X + rect.Width, rect.Y + 0.7f * rect.Height);
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
