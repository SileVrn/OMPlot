﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public interface IData
    {
        string Name { get; set; }
        string AxisHorisontalName { get; set; }
        string AxisVerticalName { get; set; }

        void Draw(Graphics g, OMPlot.Axis vertical, OMPlot.Axis horisontal, RectangleExtended plotRectangle, int plotIndex);
    }
}
