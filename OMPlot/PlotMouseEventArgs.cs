using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMPlot
{
    public class PlotMouseEventArgs : MouseEventArgs
    {
        public PointF PlotPoint { get; }
        public Data.IData Plot { get; }
        public bool ClickOnPlot { get; }
        
        public PlotMouseEventArgs(MouseEventArgs e, Data.IData plot, PointF plotPoint) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            ClickOnPlot = true;
            Plot = plot;
            PlotPoint = plotPoint;
        }

        public PlotMouseEventArgs(MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            ClickOnPlot = false;
        }
    }
}
