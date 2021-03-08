using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMPlot
{
    public delegate void PlotMouseEvent(object sender, PlotMouseEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class PlotMouseEventArgs : MouseEventArgs
    {
        public double ScaledX;
        public double ScaledY;

        public double InterpolatedX;
        public double InterpolatedY;

        public double DistanceToNearest;

        public int NearestIndex;
        
        internal PlotMouseEventArgs(MouseEventArgs e, PlotMouseEventStruct data) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            ScaledX = data.ScaledX;
            ScaledY = data.ScaledY;

            InterpolatedX = data.InterpolatedX;
            InterpolatedY = data.InterpolatedY;

            DistanceToNearest = data.DistanceToNearest;

            NearestIndex = data.NearestIndex;
            DistanceToNearest = data.DistanceToNearest;
        }
    }

    public struct PlotMouseEventStruct
    {
        public double ScreenDistance;

        public double ScaledX;
        public double ScaledY;

        public double InterpolatedX;
        public double InterpolatedY;

        public double DistanceToNearest;

        public int NearestIndex;
    }
}
