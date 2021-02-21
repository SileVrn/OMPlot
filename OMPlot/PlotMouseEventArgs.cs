using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMPlot
{
    /// <summary>
    /// 
    /// </summary>
    public class PlotMouseEventArgs : MouseEventArgs
    {
        /// <summary>
        /// Nearest point of the <see cref="PlotMouseEventArgs.Plot"/>. Can be interplolated point.
        /// </summary>
        public PointF PlotPoint { get; }
        /// <summary>
        /// Instance of the plot on which the event occurred.
        /// </summary>
        public Data.ScatterSeries Plot { get; }
        /// <summary>
        /// True if event occurred on a plot.
        /// </summary>
        public bool ClickOnPlot { get; }
        
        internal PlotMouseEventArgs(MouseEventArgs e, Data.ScatterSeries plot, PointF plotPoint) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            ClickOnPlot = true;
            Plot = plot;
            PlotPoint = plotPoint;
        }

        internal PlotMouseEventArgs(MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            ClickOnPlot = false;
        }
    }
}
