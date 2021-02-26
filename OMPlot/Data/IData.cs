using System.Drawing;

namespace OMPlot.Data
{
    public interface IData
    {
        string Name { get; set; }
        PointDistance DistanceToPoint(double x, double y);
        void CalculateGraphics(RectangleExtended plotRectangle);
        void Draw(Graphics g, RectangleExtended plotRectangle);
        void DrawLegend(Graphics g, RectangleF rect);
    }
}
