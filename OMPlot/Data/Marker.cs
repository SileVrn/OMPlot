using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public static class Marker
    {
        static PointF[] markerTriangle = { new PointF(0, -0.6f), new PointF(0.6f * (float)Math.Cos(7.0 / 6.0 * Math.PI), -0.6f * (float)Math.Sin(7.0 / 6.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(11.0 / 6.0 * Math.PI), -0.6f * (float)Math.Sin(11.0 / 6.0 * Math.PI)) };
        static PointF[] markerPentagon = { new PointF(0, -0.6f), new PointF(0.6f * (float)Math.Cos(9.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(9.0 / 10.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(13.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(13.0 / 10.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(17.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(17.0 / 10.0 * Math.PI)), new PointF(0.6f * (float)Math.Cos(21.0 / 10.0 * Math.PI), -0.6f * (float)Math.Sin(21.0 / 10.0 * Math.PI)) };
        static PointF[] markerDiamond = { new PointF(0, -0.6f), new PointF(-0.6f, 0), new PointF(0, 0.6f), new PointF(0.6f, 0) };

        public static void Draw(Graphics g, Color markerColor, MarkerStyle markerStryle, float markerSize, PointF[] pointList)
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
