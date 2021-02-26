using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public interface IAxisUsing
    {
        /// <summary>
        /// Relevant horizontal axis
        /// </summary>
        Axis HorizontalAxis { get; set; }
        /// <summary>
        /// Relevant vertical axis
        /// </summary>
        Axis VerticalAxis { get; set; }
        /// <summary>
        /// The minimum x value
        /// </summary>
        double MinimumX { get; }
        /// <summary>
        /// The minimum y value
        /// </summary>
        double MinimumY { get; }
        /// <summary>
        /// The maximum x value
        /// </summary>
        double MaximumX { get; }
        /// <summary>
        /// The maximum y value
        /// </summary>
        double MaximumY { get; }
    }
}
