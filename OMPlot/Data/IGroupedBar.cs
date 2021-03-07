using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public interface IGroupedBar
    {
        /// <summary>
        /// Visual style of bars.
        /// </summary>
        BarStyle BarStyle { get; set; }
        /// <summary>
        /// Enable bars grouping.
        /// </summary>
        bool BarGrouping { get; set; }
        /// <summary>
        /// Index of current graph`s bars in group.
        /// </summary>
        int BarIndex { get; set; }
        /// <summary>
        /// Number of graph in group.
        /// </summary>
        int BarCount { get; set; }
    }
}
