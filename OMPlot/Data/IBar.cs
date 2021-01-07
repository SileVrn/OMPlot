using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    interface IBar
    {
        BarStyle BarStyle { get; set; }
        Color BarLineColor { get; set; }
        Color BarFillColor { get; set; }
        float BarDuty { get; set; }
        double BarValue { get; set; }
        bool BarStacking { get; set; }
        int BarIndex { get; set; }
        int BarCount { get; set; }
    }
    public enum BarStyle
    {
        None,
        Vertical,
        Horisontal
    }
}
