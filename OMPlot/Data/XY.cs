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
        

        float[] X, Y;        
        Pen linePen;

        public PlotStyle Style { get; set; }
        public Color LineColor { get; set; }
        public System.Drawing.Drawing2D.DashStyle LineStyle { get; set; }
        public float LineWidth { get; set; }
        public Color MarkColor { get; set; }
        public MarkerStyle MarkStyle { get; set; }
        public float MarkSize { get; set; }

        public string Name { get; set; }
        public string AxisHorizontalName { get; set; }
        public string AxisVerticalName { get; set; }
        
        public XY(double[] x, double[] y)
        {
            MarkSize = 10;
            linePen = new Pen(Color.Empty);
            X = new float[x.Length];
            Y = new float[y.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X[i] = (float)x[i];
                Y[i] = (float)y[i];
            }                
        }

        public XY(double[] x, double[] y, string name)
        {
            MarkSize = 10;
            linePen = new Pen(Color.Empty);
            X = new float[x.Length];
            Y = new float[y.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X[i] = (float)x[i];
                Y[i] = (float)y[i];
            }
            Name = name;
        }

        public XY(double[] x, double[] y, string axisHorizontalName, string axisVerticalName)
        {
            MarkSize = 10;
            linePen = new Pen(Color.Empty);
            X = new float[x.Length];
            Y = new float[y.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X[i] = (float)x[i];
                Y[i] = (float)y[i];
            }
            AxisHorizontalName = axisHorizontalName;
            AxisVerticalName = axisVerticalName;
        }

        public XY(double[] x, double[] y, string name, string axisHorizontalName, string axisVerticalName)
        {
            MarkSize = 10;
            linePen = new Pen(Color.Empty);
            X = new float[x.Length];
            Y = new float[y.Length];
            for (int i = 0; i < X.Length; i++)
            {
                X[i] = (float)x[i];
                Y[i] = (float)y[i];
            }
            Name = name;
            AxisHorizontalName = axisHorizontalName;
            AxisVerticalName = axisVerticalName;
        }


        public void Draw(Graphics g, OMPlot.Axis vertical, OMPlot.Axis Horizontal, RectangleExtended plotRectangle, int plotIndex)
        {
            if (linePen.Color.R == 0 && linePen.Color.G == 0 && linePen.Color.B == 0 && linePen.Color.A == 0)
                linePen.Color = defaultPlotColors[plotIndex % 3];
            if (MarkColor.R == 0 && MarkColor.G == 0 && MarkColor.B == 0 && MarkColor.A == 0)
                MarkColor = defaultPlotColors[plotIndex % 3];

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

            var pointArray = pointList.ToArray();

            if (Style == PlotStyle.Line || Style == PlotStyle.Both)
            {
                if(pointList.Count > 1)
                    g.DrawLines(linePen, pointArray);
            }
            if (Style == PlotStyle.Marker || Style == PlotStyle.Both)
            {
                Marker.Draw(g, MarkColor, MarkStyle, MarkSize, pointArray);
            }
        }

        public enum PlotStyle
        {
            Line,
            Marker,
            Both
        }

        
    }
}
