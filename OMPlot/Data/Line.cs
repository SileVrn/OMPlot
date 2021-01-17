using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    class Line
    {
        static float[] dot = new float[] { 1.0F, 2.0F };
        static float[] longdot = new float[] { 1.0F, 4.0F };
        static float[] dash = new float[] { 10.0F, 5.0F };
        static float[] longdash = new float[] { 20.0F, 5.0F };
        static float[] doubledash = new float[] { 30.0F, 5.0F };
        static float[] dashdot = new float[] { 10.0F, 3.0F, 1.0F, 3.0F };
        static float[] longdashdot = new float[] { 20.0F, 3.0F, 1.0F, 3.0F };
        static float[] dashdotdot = new float[] { 10.0F, 3.0F, 1.0F, 3.0F, 1.0F, 3.0F };

        public static Pen GetPen(Color c, LineStyle s, float w)
        {
            Pen linePen = new Pen(c, w);
            if (s == LineStyle.Dot)
                linePen.DashPattern = dot;
            else if (s == LineStyle.LongDot)
                linePen.DashPattern = longdot;
            else if (s == LineStyle.Dash)
                linePen.DashPattern = dash;
            else if (s == LineStyle.LongDash)
                linePen.DashPattern = longdash;
            else if (s == LineStyle.DoubleDash)
                linePen.DashPattern = doubledash;
            else if (s == LineStyle.DashDot)
                linePen.DashPattern = dashdot;
            else if (s == LineStyle.LongDashDot)
                linePen.DashPattern = longdashdot;
            else if (s == LineStyle.DashDotDot)
                linePen.DashPattern = dashdotdot;
            return linePen;
        }

        public static void DrawLine(Graphics g, Color c, LineStyle s, float w, float x1, float y1, float x2, float y2)
        {
            if (s != LineStyle.None)
            {
                Pen linePen = GetPen(c, s, w);
                g.DrawLine(linePen, x1, y1, x2, y2);
                linePen.Dispose();
            }
        }
        public static void DrawLines(Graphics g, Color c, LineStyle s, float w, PointF[] points)
        {
            if (s != LineStyle.None)
            {
                Pen linePen = GetPen(c, s, w);
                g.DrawLines(linePen, points);
                linePen.Dispose();
            }
        }
        public static void DrawPath(Graphics g, Color c, LineStyle s, float w, GraphicsPath path)
        {
            if (s != LineStyle.None)
            {
                Pen linePen = GetPen(c, s, w);
                g.DrawPath(linePen, path);
                linePen.Dispose();
            }
        }

    }
    interface ILine
    {
        PlotInterpolation Interpolation { get; set; }
        LineStyle LineStyle { get; set; }
        float LineWidth { get; set; }
        Color LineColor { get; set; }
    }
    interface IFill
    {
        FillStyle FillStyle { get; set; }
        Color FillColor { get; set; }
        double FillValue { get; set; }
        IData FillPlot { get; set; }
    }
    public enum LineStyle { Solid, Dash, LongDash, DoubleDash, Dot, LongDot, DashDot, LongDashDot, DashDotDot, None }
    public enum PlotInterpolation { Line, Spline, StepNear, StepFar, StepCenter, StepVertical }
    public enum FillStyle { None, ToValue, ToNInfitity, ToPInfinity, ToPlot }
}
