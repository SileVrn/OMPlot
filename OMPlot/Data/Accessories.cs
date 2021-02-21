using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    internal static class Marker
    {
        static PointF[] markerTriangle = { new PointF(0, -0.6f), new PointF(0.6f * (float)Math.Cos(7.0 / 6.0 * Math.PI), -0.6f * (float)Math.Sin(7.0 / 6.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(11.0 / 6.0 * Math.PI), -0.6f * (float)Math.Sin(11.0 / 6.0 * Math.PI)) };
        static PointF[] markerPentagon = { new PointF(0, -0.6f), new PointF(0.6f * (float)Math.Cos(9.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(9.0 / 10.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(13.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(13.0 / 10.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(17.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(17.0 / 10.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(21.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(21.0 / 10.0 * Math.PI)) };
        static PointF[] markerDiamond = { new PointF(0, -0.6f), new PointF(-0.6f, 0), new PointF(0, 0.6f), new PointF(0.6f, 0) };

        internal static void Draw(Graphics g, Color markerColor, MarkerStyle markerStryle, float markerSize, PointF[] pointList)
        {
            float halfMarkerSize = markerSize / 2.0f;
            Brush MarkerBrush = new SolidBrush(markerColor);
            Pen MarkerPen = new Pen(markerColor);
            if (markerStryle == MarkerStyle.SolidCircle)
                foreach (var point in pointList) g.FillEllipse(MarkerBrush, point.X - halfMarkerSize, point.Y - halfMarkerSize, markerSize, markerSize);
            else if (markerStryle == MarkerStyle.SolidSquare)
                foreach (var point in pointList) g.FillRectangle(MarkerBrush, point.X - halfMarkerSize, point.Y - halfMarkerSize, markerSize, markerSize);
            else if (markerStryle == MarkerStyle.SolidTriangle)
                foreach (var point in pointList)
                {
                    g.TranslateTransform(point.X, point.Y);
                    g.ScaleTransform(markerSize, markerSize);
                    g.FillPolygon(MarkerBrush, markerTriangle);
                    g.ResetTransform();
                }
            else if (markerStryle == MarkerStyle.SolidPentagon)
                foreach (var point in pointList)
                {
                    g.TranslateTransform(point.X, point.Y);
                    g.ScaleTransform(markerSize, markerSize);
                    g.FillPolygon(MarkerBrush, markerPentagon);
                    g.ResetTransform();
                }
            else if (markerStryle == MarkerStyle.SolidDiamond)
                foreach (var point in pointList)
                {
                    g.TranslateTransform(point.X, point.Y);
                    g.ScaleTransform(markerSize, markerSize);
                    g.FillPolygon(MarkerBrush, markerDiamond);
                    g.ResetTransform();
                }
            else if (markerStryle == MarkerStyle.EmptyCircle)
                foreach (var point in pointList) g.DrawEllipse(MarkerPen, point.X - halfMarkerSize, point.Y - halfMarkerSize, markerSize, markerSize);
            else if (markerStryle == MarkerStyle.EmptySquare)
                foreach (var point in pointList) g.DrawRectangle(MarkerPen, point.X - halfMarkerSize, point.Y - halfMarkerSize, markerSize, markerSize);
            else if (markerStryle == MarkerStyle.EmptyTriangle)
            {
                MarkerPen.Width /= markerSize;
                foreach (var point in pointList)
                {
                    g.TranslateTransform(point.X, point.Y);
                    g.ScaleTransform(markerSize, markerSize);
                    g.DrawPolygon(MarkerPen, markerTriangle);
                    g.ResetTransform();
                }
            }
            else if (markerStryle == MarkerStyle.EmptyPentagon)
            {
                MarkerPen.Width /= markerSize;
                foreach (var point in pointList)
                {
                    g.TranslateTransform(point.X, point.Y);
                    g.ScaleTransform(markerSize, markerSize);
                    g.DrawPolygon(MarkerPen, markerPentagon);
                    g.ResetTransform();
                }
            }
            else if (markerStryle == MarkerStyle.EmptyDiamond)
            {
                MarkerPen.Width /= markerSize;
                foreach (var point in pointList)
                {
                    g.TranslateTransform(point.X, point.Y);
                    g.ScaleTransform(markerSize, markerSize);
                    g.DrawPolygon(MarkerPen, markerDiamond);
                    g.ResetTransform();
                }
            }
            else if (markerStryle == MarkerStyle.Plus)
            {
                float sizeMarker = 0.6f * markerSize;
                foreach (var point in pointList)
                {
                    g.DrawLine(MarkerPen, point.X - sizeMarker, point.Y, point.X + sizeMarker, point.Y);
                    g.DrawLine(MarkerPen, point.X, point.Y - sizeMarker, point.X, point.Y + sizeMarker);
                }
            }
            else if (markerStryle == MarkerStyle.Cross)
            {
                float sizeMarker45Deg = 0.6f * markerSize * 0.70710678118654752440084436210485f;
                foreach (var point in pointList)
                {
                    g.DrawLine(MarkerPen, point.X - sizeMarker45Deg, point.Y - sizeMarker45Deg, point.X + sizeMarker45Deg, point.Y + sizeMarker45Deg);
                    g.DrawLine(MarkerPen, point.X + sizeMarker45Deg, point.Y - sizeMarker45Deg, point.X - sizeMarker45Deg, point.Y + sizeMarker45Deg);
                }
            }
            else if (markerStryle == MarkerStyle.Star)
            {
                float sizeMarker = 0.6f * markerSize;
                float sizeMarker30Deg = 0.6f * markerSize * 0.5f;
                float sizeMarker60Deg = 0.6f * markerSize * 0.86602540378443864676372317075294f;
                foreach (var point in pointList)
                {
                    g.DrawLine(MarkerPen, point.X, point.Y - sizeMarker, point.X, point.Y + sizeMarker);
                    g.DrawLine(MarkerPen, point.X - sizeMarker60Deg, point.Y - sizeMarker30Deg, point.X + sizeMarker60Deg, point.Y + sizeMarker30Deg);
                    g.DrawLine(MarkerPen, point.X + sizeMarker60Deg, point.Y - sizeMarker30Deg, point.X - sizeMarker60Deg, point.Y + sizeMarker30Deg);
                }
            }
            else if (markerStryle == MarkerStyle.Asterisk)
            {
                foreach (var point in pointList)
                {
                    g.DrawLine(MarkerPen, point.X, point.Y, point.X + markerSize * markerPentagon[0].X, point.Y + markerSize * markerPentagon[0].Y);
                    g.DrawLine(MarkerPen, point.X, point.Y, point.X + markerSize * markerPentagon[1].X, point.Y + markerSize * markerPentagon[1].Y);
                    g.DrawLine(MarkerPen, point.X, point.Y, point.X + markerSize * markerPentagon[2].X, point.Y + markerSize * markerPentagon[2].Y);
                    g.DrawLine(MarkerPen, point.X, point.Y, point.X + markerSize * markerPentagon[3].X, point.Y + markerSize * markerPentagon[3].Y);
                    g.DrawLine(MarkerPen, point.X, point.Y, point.X + markerSize * markerPentagon[4].X, point.Y + markerSize * markerPentagon[4].Y);
                }
            }

        }
    }
    internal class Line
    {
        static float[] dot = new float[] { 1.0F, 2.0F };
        static float[] longdot = new float[] { 1.0F, 4.0F };
        static float[] dash = new float[] { 10.0F, 5.0F };
        static float[] longdash = new float[] { 20.0F, 5.0F };
        static float[] doubledash = new float[] { 30.0F, 5.0F };
        static float[] dashdot = new float[] { 10.0F, 3.0F, 1.0F, 3.0F };
        static float[] longdashdot = new float[] { 20.0F, 3.0F, 1.0F, 3.0F };
        static float[] dashdotdot = new float[] { 10.0F, 3.0F, 1.0F, 3.0F, 1.0F, 3.0F };

        internal static Pen GetPen(Color c, LineStyle s, float w)
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

        internal static void DrawLine(Graphics g, Color c, LineStyle s, float w, float x1, float y1, float x2, float y2)
        {
            if (s != LineStyle.None)
            {
                Pen linePen = GetPen(c, s, w);
                g.DrawLine(linePen, x1, y1, x2, y2);
                linePen.Dispose();
            }
        }
        internal static void DrawLines(Graphics g, Color c, LineStyle s, float w, PointF[] points)
        {
            if (s != LineStyle.None)
            {
                Pen linePen = GetPen(c, s, w);
                g.DrawLines(linePen, points);
                linePen.Dispose();
            }
        }
        internal static void DrawPath(Graphics g, Color c, LineStyle s, float w, GraphicsPath path)
        {
            if (s != LineStyle.None)
            {
                Pen linePen = GetPen(c, s, w);
                g.DrawPath(linePen, path);
                linePen.Dispose();
            }
        }

    }

    /// <summary>
    /// Enumerates the available line styles.
    /// </summary>
    public enum LineStyle
    {
        Solid, Dash, LongDash, DoubleDash, Dot, LongDot, DashDot, LongDashDot, DashDotDot,
        /// <summary>
        /// Do not draw lines.
        /// </summary>
        None
    }
    /// <summary>
    /// Enumerates the available interpolation mode.
    /// </summary>
    public enum Interpolation
    {
        /// <summary>
        /// A straight line between two points.
        /// </summary>
        Line,
        /// <summary>
        /// Spline interpolation.
        /// </summary>
        Spline,
        /// <summary>
        /// A step between two points, located closer to the first point.
        /// </summary>
        StepNear,
        /// <summary>
        /// A step between two points, located closer to the last point.
        /// </summary>
        StepFar,
        /// <summary>
        /// A step between two points, located at the center.
        /// </summary>
        StepCenter,
        /// <summary>
        /// A vertical step between two points, located at the center.
        /// </summary>
        StepVertical
    }
    /// <summary>
    /// Enumerates the available filling styles.
    /// </summary>
    public enum FillStyle
    {
        /// <summary>
        /// Do not fill graph.
        /// </summary>
        None,
        ToValue,
        ToNInfitity,
        ToPInfinity,
        ToPlot
    }
    /// <summary>
    /// Enumerates the available marker styles.
    /// </summary>
    public enum MarkerStyle
    {
        /// <summary>
        /// Do not draw markers.
        /// </summary>
        None,
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
    /// <summary>
    /// Enumerates the available bar styles.
    /// </summary>
    public enum BarStyle
    {
        /// <summary>
        /// Do not draw bars.
        /// </summary>
        None,
        Vertical,
        Horisontal
    }
    /// <summary>
    /// Represents a structure containing a distance and a point.
    /// </summary>
    public struct PointDistance
    {
        /// <summary>
        /// Point.
        /// </summary>
        public PointF Point;
        /// <summary>
        /// Distance to <see cref="PointDistance.Point"./>.
        /// </summary>
        public double Distance;
    }
}
