using System;
using System.Drawing;

namespace OMPlot
{
    internal static class PointFExtension
    {
        public static double Distance(this PointF point1, PointF point2) { return Math.Sqrt((point1.Y - point2.Y) * (point1.Y - point2.Y) + (point1.X - point2.X) * (point1.X - point2.X)); }
        public static double Distance(this PointF point1, float x, float y) { return Math.Sqrt((point1.Y - y) * (point1.Y - y) + (point1.X - x) * (point1.X - x)); }
        public static double Distance(this PointF point1, double x, double y) {  return Math.Sqrt((point1.Y - y) * (point1.Y - y) + (point1.X - x) * (point1.X - x)); }
    }
    internal static class RectangleFExtension
    {
        public static float GetCenterX(this RectangleF rect) { return rect.X + rect.Width / 2.0f; }
        public static float GetCenterY(this RectangleF rect) { return rect.Y + rect.Height / 2.0f; }
        public static void SetCenterX(this ref RectangleF rect, float value) { rect.X = value - rect.Width / 2.0f; }
        public static void SetCenterY(this ref RectangleF rect, float value) { rect.Y = value - rect.Height / 2.0f; }
        public static void SetTop(this ref RectangleF rect, float value) { rect.Height -= value - rect.Y; rect.Y = value; }
        public static void SetBottom(this ref RectangleF rect, float value) { rect.Height = value - rect.Y; }
        public static void SetLeft(this ref RectangleF rect, float value) { rect.Width -= value - rect.X; rect.X = value; }
        public static void SetRight(this ref RectangleF rect, float value) { rect.Width = value - rect.X; }
    }
}
