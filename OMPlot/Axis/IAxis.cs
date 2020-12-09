using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Axis
{
    public interface IAxis
    {
        float Minimum { get; set; }
        float Maximum { get; set; }
        float Center { get; set; }
        float FullScale { get; set; }

        float Resolution { get; }

        string Title { get; set; }
        bool Vertical { get; set; }
        bool Reverse { get; set; }
        bool MoveLocked { get; set; }
        bool ZoomLocked { get; set; }
        float CrossValue { get; set; }
        AxisPosition Position { get; set; }
        TickStyle TickStyle { get; set; }
        TicksLabelsLineAlignment TicksLabelsLineAlignment { get; set; }
        LabelAlignment LabelAlignment { get; set; }
        GridStyle GridStyle { get; set; }

        float OverflowNear { get; }
        float OverflowFar { get; }
        float HeightNear { get; }
        float HeightFar { get; }

        Font Font { get; set; }
        Color Color { get; set; }

        bool ActionOnAxis(float x, float y);
        void Move(float length);
        void Select(float min, float max);
        void Zoom(float zoom, float placelength);

        void Calculate(Graphics g);
        void CalculateTranform(float minimum, float maximum);
        void Draw(Graphics g, float x, float y, RectangleExtended rect);
        float Transform(float value);
    }

    public enum AxisPosition
    {
        Near,
        Center,
        Far,
        CrossValue
    }

    public enum TickStyle
    {
        Near,
        Far,
        Cross,
        None
    }
    
    public enum TicksLabelsLineAlignment
    {
        Near,
        Far,
        None
    }

    public enum LabelAlignment
    {
        Center,
        Near,
        Far
    }

    public enum GridStyle
    {
        None,
        Major,
        Minor,
        Both
    }

}
