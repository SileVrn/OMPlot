using System.Drawing;

namespace OMPlot.Data
{
    internal interface IData
    {
        event PlotMouseEvent MouseClick;
        event PlotMouseEvent MouseDoubleClick;
        event PlotMouseEvent MouseUp;
        event PlotMouseEvent MouseDown;
        event PlotMouseEvent MouseMove;

        bool OnMouseClick(System.Windows.Forms.MouseEventArgs e);
        bool OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e);
        bool OnMouseUp(System.Windows.Forms.MouseEventArgs e);
        bool OnMouseDown(System.Windows.Forms.MouseEventArgs e);
        bool OnMouseMove(System.Windows.Forms.MouseEventArgs e);

        string Name { get; set; }
        PlotMouseEventStruct DistanceToPoint(double ScreenX, double ScreenY);
        void CalculateGraphics(RectangleExtended plotRectangle);
        void Draw(Graphics g, RectangleExtended plotRectangle);
        void DrawLegend(Graphics g, RectangleF rect);
    }
}
