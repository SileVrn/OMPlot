using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public class XY:IData
    {
        Color[] defaultPlotColors = new Color[] { Color.Red, Color.Blue, Color.Green };
        PointF[] markerTriangle = { new PointF(0, -0.6f), new PointF(0.6f * (float)Math.Cos(7.0 / 6.0 * Math.PI), -0.6f * (float)Math.Sin(7.0 / 6.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(11.0 / 6.0 * Math.PI), -0.6f * (float)Math.Sin(11.0 / 6.0 * Math.PI)) };
        PointF[] markerPentagon = { new PointF(0, -0.6f), new PointF(0.6f * (float)Math.Cos(9.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(9.0 / 10.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(13.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(13.0 / 10.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(17.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(17.0 / 10.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(21.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(21.0 / 10.0 * Math.PI)) };
        PointF[] markerDiamond = { new PointF(0, -0.6f), new PointF(-0.6f, 0), new PointF(0, 0.6f), new PointF(0.6f, 0) };


        float[] X, Y;
        string axisHorizontalName, axisVerticalName;
        string name;

        Pen linePen;
        Color markerColor;
        MarkerStyle markStyle;
        float markSize = 1;

        public PlotStyle Style;

        public Color LineColor
        {
            get { return linePen.Color; }
            set { linePen.Color = value; }
        }
        public System.Drawing.Drawing2D.DashStyle LineStyle 
        { 
            get { return linePen.DashStyle; }  
            set { linePen.DashStyle = value; }
        }
        public float LineWidth
        {
            get { return linePen.Width; }
            set { linePen.Width = value; }
        }
        public Color MarkerColor
        {
            get { return markerColor; }
            set { markerColor = value; }
        }
        public MarkerStyle MarkStyle
        {
            get { return markStyle; }
            set { markStyle = value; }
        }
        public float MarkSize
        {
            get { return markSize; }
            set { markSize = value; }
        }


        public string Name { get { return name; } set { name = value; } }
        public string AxisHorizontalName { get { return axisHorizontalName; } set { axisHorizontalName = value; } }
        public string AxisVerticalName { get { return axisVerticalName; } set { axisVerticalName = value; } }
        
        public XY(double[] x, double[] y)
        {
            linePen = new Pen(Color.Empty);
            X = new float[x.Length];
            Y = new float[y.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X[i] = (float)x[i];
                Y[i] = (float)y[i];
            }                
        }

        public XY(double[] x, double[] y, string Name)
        {
            linePen = new Pen(Color.Empty);
            X = new float[x.Length];
            Y = new float[y.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X[i] = (float)x[i];
                Y[i] = (float)y[i];
            }
            name = Name;
        }

        public XY(double[] x, double[] y, string AxisHorizontalName, string AxisVerticalName)
        {
            linePen = new Pen(Color.Empty);
            X = new float[x.Length];
            Y = new float[y.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X[i] = (float)x[i];
                Y[i] = (float)y[i];
            }
            axisHorizontalName = AxisHorizontalName;
            axisVerticalName = AxisVerticalName;
        }

        public XY(double[] x, double[] y, string Name, string AxisHorizontalName, string AxisVerticalName)
        {
            linePen = new Pen(Color.Empty);
            X = new float[x.Length];
            Y = new float[y.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X[i] = (float)x[i];
                Y[i] = (float)y[i];
            }
            name = Name;
            axisHorizontalName = AxisHorizontalName;
            axisVerticalName = AxisVerticalName;
        }


        public void Draw(Graphics g, OMPlot.Axis vertical, OMPlot.Axis Horizontal, RectangleExtended plotRectangle, int plotIndex)
        {
            //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            /*Point[] dataPoint = new Point[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                dataPoint[i] = new Point(Horizontal.Transform(X[i]), vertical.Transform(Y[i]));
            }
            g.DrawLines(pen, dataPoint);*/
            if (linePen.Color.R == 0 && linePen.Color.G == 0 && linePen.Color.B == 0 && linePen.Color.A == 0)
                linePen.Color = defaultPlotColors[plotIndex % 3];
            if (markerColor.R == 0 && markerColor.G == 0 && markerColor.B == 0 && markerColor.A == 0)
                markerColor = defaultPlotColors[plotIndex % 3];

            float leftLimit = plotRectangle.Left - 100;
            float rightLimit = plotRectangle.Right + 100;
            float topLimit = plotRectangle.Left - 100;
            float bottomLimit = plotRectangle.Right + 100;


            List<PointF> pointList = new List<PointF>();
            float prevX = Horizontal.Transform(X[0]);
            float prevY = vertical.Transform(Y[0]);
            pointList.Add(new PointF(prevX, prevY));
            float curX, curY;
            for (int i = 1; i < X.Length; i++)
            {
                curX = Horizontal.Transform(X[i]);
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

            if (Style == PlotStyle.Line || Style == PlotStyle.Both)
            {
                if(pointList.Count > 1)
                    g.DrawLines(linePen, pointList.ToArray());
            }
            if (Style == PlotStyle.Marker || Style == PlotStyle.Both)
            {
                float halfMarkerSize = markSize / 2.0f;
                Brush MarkerBrush = new SolidBrush(markerColor);
                Pen MarkerPen = new Pen(markerColor);
                if (markStyle == MarkerStyle.SolidCircle)
                    foreach (var point in pointList) g.FillEllipse(MarkerBrush, point.X - halfMarkerSize, point.Y - halfMarkerSize, markSize, markSize);
                else if (markStyle == MarkerStyle.SolidSquare)
                    foreach (var point in pointList) g.FillRectangle(MarkerBrush, point.X - halfMarkerSize, point.Y - halfMarkerSize, markSize, markSize);
                else if (markStyle == MarkerStyle.SolidTriangle)
                    foreach (var point in pointList)
                    {
                        g.TranslateTransform(point.X, point.Y);
                        g.ScaleTransform(markSize, markSize);
                        g.FillPolygon(MarkerBrush, markerTriangle);
                        g.ResetTransform();
                    }
                else if (markStyle == MarkerStyle.SolidPentagon)
                    foreach (var point in pointList)
                    {
                        g.TranslateTransform(point.X, point.Y);
                        g.ScaleTransform(markSize, markSize);
                        g.FillPolygon(MarkerBrush, markerPentagon);
                        g.ResetTransform();
                    }
                else if (markStyle == MarkerStyle.SolidDiamond)
                    foreach (var point in pointList)
                    {
                        g.TranslateTransform(point.X, point.Y);
                        g.ScaleTransform(markSize, markSize);
                        g.FillPolygon(MarkerBrush, markerDiamond);
                        g.ResetTransform();
                    }
                else if (markStyle == MarkerStyle.EmptyCircle)
                    foreach (var point in pointList) g.DrawEllipse(MarkerPen, point.X - halfMarkerSize, point.Y - halfMarkerSize, markSize, markSize);
                else if (markStyle == MarkerStyle.EmptySquare)
                    foreach (var point in pointList) g.DrawRectangle(MarkerPen, point.X - halfMarkerSize, point.Y - halfMarkerSize, markSize, markSize);
                else if (markStyle == MarkerStyle.EmptyTriangle)
                {
                    MarkerPen.Width /= markSize;
                    foreach (var point in pointList)
                    {
                        g.TranslateTransform(point.X, point.Y);
                        g.ScaleTransform(markSize, markSize);
                        g.DrawPolygon(MarkerPen, markerTriangle);
                        g.ResetTransform();
                    }
                }
                else if (markStyle == MarkerStyle.EmptyPentagon)
                {
                    MarkerPen.Width /= markSize;
                    foreach (var point in pointList)
                    {
                        g.TranslateTransform(point.X, point.Y);
                        g.ScaleTransform(markSize, markSize);
                        g.DrawPolygon(MarkerPen, markerPentagon);
                        g.ResetTransform();
                    }
                }
                else if (markStyle == MarkerStyle.EmptyDiamond)
                {
                    MarkerPen.Width /= markSize;
                    foreach (var point in pointList)
                    {
                        g.TranslateTransform(point.X, point.Y);
                        g.ScaleTransform(markSize, markSize);
                        g.DrawPolygon(MarkerPen, markerDiamond);
                        g.ResetTransform();
                    }
                }
                else if (markStyle == MarkerStyle.Plus)
                {
                    float sizeMarker = 0.6f * markSize;
                    foreach (var point in pointList)
                    {
                        g.DrawLine(MarkerPen, point.X - sizeMarker, point.Y, point.X + sizeMarker, point.Y);
                        g.DrawLine(MarkerPen, point.X, point.Y - sizeMarker, point.X, point.Y + sizeMarker);
                    }
                }
                else if (markStyle == MarkerStyle.Cross)
                {
                    float sizeMarker45Deg = 0.6f * markSize * 0.70710678118654752440084436210485f;
                    foreach (var point in pointList)
                    {
                        g.DrawLine(MarkerPen, point.X - sizeMarker45Deg, point.Y - sizeMarker45Deg, point.X + sizeMarker45Deg, point.Y + sizeMarker45Deg);
                        g.DrawLine(MarkerPen, point.X + sizeMarker45Deg, point.Y - sizeMarker45Deg, point.X - sizeMarker45Deg, point.Y + sizeMarker45Deg);
                    }
                }
                else if (markStyle == MarkerStyle.Star)
                {
                    float sizeMarker = 0.6f * markSize;
                    float sizeMarker30Deg = 0.6f * markSize * 0.5f;
                    float sizeMarker60Deg = 0.6f * markSize * 0.86602540378443864676372317075294f;
                    foreach (var point in pointList)
                    {
                        g.DrawLine(MarkerPen, point.X, point.Y - sizeMarker, point.X, point.Y + sizeMarker);
                        g.DrawLine(MarkerPen, point.X - sizeMarker60Deg, point.Y - sizeMarker30Deg, point.X + sizeMarker60Deg, point.Y + sizeMarker30Deg);
                        g.DrawLine(MarkerPen, point.X + sizeMarker60Deg, point.Y - sizeMarker30Deg, point.X - sizeMarker60Deg, point.Y + sizeMarker30Deg);
                    }
                }
                else if (markStyle == MarkerStyle.Asterisk)
                {
                    foreach (var point in pointList)
                    {
                        g.DrawLine(MarkerPen, point.X, point.Y, point.X + markSize * markerPentagon[0].X, point.Y + markSize * markerPentagon[0].Y);
                        g.DrawLine(MarkerPen, point.X, point.Y, point.X + markSize * markerPentagon[1].X, point.Y + markSize * markerPentagon[1].Y);
                        g.DrawLine(MarkerPen, point.X, point.Y, point.X + markSize * markerPentagon[2].X, point.Y + markSize * markerPentagon[2].Y);
                        g.DrawLine(MarkerPen, point.X, point.Y, point.X + markSize * markerPentagon[3].X, point.Y + markSize * markerPentagon[3].Y);
                        g.DrawLine(MarkerPen, point.X, point.Y, point.X + markSize * markerPentagon[4].X, point.Y + markSize * markerPentagon[4].Y);
                    }
                }

            }
        }

        public enum PlotStyle
        {
            Line,
            Marker,
            Both
        }

        public enum MarkerStyle
        {
            SolidCircle,
            SolidSquare,
            SolidTriangle,
            SolidDiamond,
            SolidPentagon,
            EmptyCircle,
            EmptySquare,
            EmptyTriangle,
            EmptyDiamond,
            EmptyPentagon,
            Cross,
            Plus,
            Asterisk,
            Star
        }
    }
}
