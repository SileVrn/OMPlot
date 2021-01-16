using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public class XY : IData, IBar
    {
        Color[] defaultPlotColors = new Color[] { Color.Red, Color.Blue, Color.Green };

        double[] X, Y;
        PointF[] points;

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

        public void Calculate(Axis vertical, Axis horizontal, RectangleExtended plotRectangle)
        {
            //float leftLimit = plotRectangle.Left - 100;
            //float rightLimit = plotRectangle.Right + 100;
            //float topLimit = plotRectangle.Top - 100;
            //float bottomLimit = plotRectangle.Bottom + 100;

            List<PointF> pointList = new List<PointF>();
            float prevX = horizontal.Transform(X[0]);
            float prevY = vertical.Transform(Y[0]);
            //if (prevX > leftLimit && prevX < rightLimit && prevY > topLimit && prevY < bottomLimit)
                pointList.Add(new PointF(prevX, prevY));
            float curX, curY;
            for (int i = 1; i < X.Length; i++)
            {
                curX = horizontal.Transform(X[i]);
                //if (curX > leftLimit && curX < rightLimit)
                //{
                    curY = vertical.Transform(Y[i]);
                    //if (curY > topLimit && curY < bottomLimit)
                        if (curX - prevX > 1.5 || curY - prevY > 1.5 || prevX - curX > 1.5 || prevY - curY > 1.5)
                        {
                            prevX = curX;
                            prevY = curY;
                            pointList.Add(new PointF(prevX, prevY));
                        }
                //}
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

            Marker.Draw(g, MarkColor, MarkStyle, MarkSize, points);

            if (points.Length > 1)
            {
                Line.DrawPath(g, LineColor, LineStyle, LineWidth, GraphicsPath);

                if (FillStyle == FillStyle.ToNInfitity)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddPath(GraphicsPath, true);
                    path.AddLine(points[0].X, points[0].Y, points[0].X, plotRectangle.Bottom);
                    path.AddLine(points[0].X, plotRectangle.Bottom, points[points.Length - 1].X, plotRectangle.Bottom);
                    path.AddLine(points[points.Length - 1].X, plotRectangle.Bottom, points[points.Length - 1].X, points[points.Length - 1].Y);
                    Brush fillBrush = new SolidBrush(FillColor);
                    g.FillPath(fillBrush, path);
                }
                else if (FillStyle == FillStyle.ToPInfinity)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddPath(GraphicsPath, true);
                    path.AddLine(points[0].X, points[0].Y, points[0].X, plotRectangle.Top);
                    path.AddLine(points[0].X, plotRectangle.Top, points[points.Length - 1].X, plotRectangle.Top);
                    path.AddLine(points[points.Length - 1].X, plotRectangle.Top, points[points.Length - 1].X, points[points.Length - 1].Y);
                    Brush fillBrush = new SolidBrush(FillColor);
                    g.FillPath(fillBrush, path);
                }
                else if (FillStyle == FillStyle.ToValue)
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
                else if (FillStyle == FillStyle.ToPlot && FillPlot != null)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddPath(GraphicsPath, true);
                    path.Reverse();
                    path.AddPath(FillPlot.GraphicsPath, true);
                    Brush fillBrush = new SolidBrush(FillColor);
                    g.FillPath(fillBrush, path);
                }

                if(BarStyle != BarStyle.None)
                {
                    Brush barBrush = new SolidBrush(BarFillColor);
                    Pen barPen = new Pen(BarLineColor);

                    if (BarStyle == BarStyle.Vertical)
                    {
                        float refPositionY = vertical.Transform(BarValue);
                        for (int i = 0; i < points.Length; i++)                        
                            DrawVerticalBar(g, barBrush, barPen, refPositionY, i);
                    }
                    else if (BarStyle == BarStyle.Horisontal)
                    {
                        float refPositionX = horizontal.Transform(BarValue);
                        for (int i = 0; i < points.Length; i++)
                            DrawHorisontalBar(g, barBrush, barPen, refPositionX, i);
                    }
                }
            }
        }

        private void DrawVerticalBar(Graphics g, Brush barBrush, Pen barPen, float refPositionY, int i)
        {
            float barCount = BarStacking ? BarCount : 1.0f;
            float binWidth, barWidth, barX;
            if (i == 0)
                binWidth = Math.Abs(points[1].X - points[0].X) / barCount;
            else if (i == points.Length - 1)
                binWidth = Math.Abs(points[points.Length - 1].X - points[points.Length - 2].X) / barCount;
            else
                binWidth = Math.Abs(points[i + 1].X - points[i - 1].X) / 2.0f / barCount;
            barWidth = binWidth * BarDuty;
            barX = i == 0 ? points[0].X - binWidth * barCount / 2.0f : (points[i].X + points[i - 1].X) / 2.0f;
            barX += BarIndex * binWidth + (binWidth - barWidth) / 2.0f;
            barX -= (i != 0 && points[i].X < points[i - 1].X) ? barWidth * barCount : 0;

            float barHeight, barY;
            if (refPositionY > points[i].Y)
            {
                barY = points[i].Y;
                barHeight = refPositionY - points[i].Y;
            }
            else
            {
                barY = refPositionY;
                barHeight = points[i].Y - refPositionY;
            }

            if (BarFillColor.A > 0)
                g.FillRectangle(barBrush, barX, barY, barWidth, barHeight);
            if (BarLineColor.A > 0)
                g.DrawRectangle(barPen, barX, barY, barWidth, barHeight);
        }

        private void DrawHorisontalBar(Graphics g, Brush barBrush, Pen barPen, float refPositionX, int i)
        {
            float barCount = BarStacking ? BarCount : 1.0f;
            float binHeight, barHeight, barY;
            if (i == 0)
                binHeight = Math.Abs(points[1].Y - points[0].Y) / barCount;
            else if (i == points.Length - 1)
                binHeight = Math.Abs(points[points.Length - 1].Y - points[points.Length - 2].Y) / barCount;
            else
                binHeight = Math.Abs(points[i + 1].Y - points[i - 1].Y) / 2.0f / barCount;
            barHeight = binHeight * BarDuty;
            barY = i == 0 ? points[0].Y - binHeight * barCount / 2.0f : (points[i].Y + points[i - 1].Y) / 2.0f;
            barY += BarIndex * binHeight + (binHeight - barHeight) / 2.0f;
            barY -= (i != 0 && points[i].Y < points[i - 1].Y) ? barHeight * barCount : 0;

            float barWidth, barX;
            if (refPositionX > points[i].X)
            {
                barX = points[i].X;
                barWidth = refPositionX - points[i].X;
            }
            else
            {
                barX = refPositionX;
                barWidth = points[i].X - refPositionX;
            }

            if (BarFillColor.A > 0)
                g.FillRectangle(barBrush, barX, barY, barWidth, barHeight);
            if (BarLineColor.A > 0)
                g.DrawRectangle(barPen, barX, barY, barWidth, barHeight);
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

    public enum PlotInterpolation
    {
        Line,
        Spline,
        StepNear, 
        StepFar,
        StepCenter, 
        StepVertical
    }
    public enum FillStyle
    {
        None,
        ToValue,
        ToNInfitity,
        ToPInfinity,
        ToPlot
    }
}
