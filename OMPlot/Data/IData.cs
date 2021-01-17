using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public interface IData
    {
        string Name { get; set; }
        string AxisHorizontalName { get; set; }
        string AxisVerticalName { get; set; }
        GraphicsPath GraphicsPath { get; }

        void Calculate(Axis vertical, Axis horizontal, RectangleExtended plotRectangle);
        void Draw(Graphics g, OMPlot.Axis vertical, OMPlot.Axis Horizontal, RectangleExtended plotRectangle, int plotIndex);
        void DrawLegend(Graphics g, RectangleF rect);
    }
}
