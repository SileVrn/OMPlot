using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot
{
    public class Axis
    {
        double minimum;
        double maximum;

        string title;
        bool logarithmic;
        bool vertical;
        bool reverse;
        bool zoomLocked;
        bool moveLocked;

        AxisPosition axisPosition;
        double crossValue;

        TickStyle majorTickStyle, minorTickStyle;

        LabelsPosition ticksLabelsPosition;
        Alignment ticksLabelsAlignment;
        Alignment ticksLabelsLineAlignment;
        TicksLabelsRotation ticksLabelsRotation;

        Alignment titleAlignment;
        LabelsPosition titlePosition;
        GridStyle gridStyle;

        Font font;
        Brush brush;
        Pen pen;
        Color color;

        float sizeNear;
        float sizeFar;
        float overflowNear;
        float overflowFar;

        RectangleExtended drawnRectangle;

        public int TickNumber;
        public int SubTickNumber;


        public float SizeNear { get { return sizeNear; } }
        public float SizeFar { get { return sizeFar; } }
        public float OverflowNear { get { return overflowNear; } }
        public float OverflowFar { get { return overflowFar; } }


        private SizeF titleSize;
        private double res;
        private double offset;
        private double[] tick;
        private string[] tickLabel;
        private SizeF[] tickLabelSize;
        private double[] subTick;
        private string tickLabelFormat;

        public Axis()
        {
            Color = Color.Black;
            TickNumber = 10;
        }

        public double FullScale
        {
            get { return maximum - minimum; }
            set
            {
                double center = Center;
                maximum = center + value / 2.0f;
                minimum = center - value / 2.0f;
            }
        }
        public double Center
        {
            get { return (maximum + minimum) / 2.0f; }
            set
            {
                double delta = value - Center;
                maximum += delta;
                minimum += delta;
            }
        }

        public double Minimum { get { return minimum; } set { minimum = value; } }
        public double Maximum { get { return maximum; } set { maximum = value; } }

        public string Title { get { return title; } set { title = value; } }
        public bool Logarithmic { get { return logarithmic; } set { logarithmic = value; } }
        public bool Vertical { get { return vertical; } set { vertical = value; } }
        public bool Reverse { get { return reverse; } set { reverse = value; } }
        public bool MoveLocked { get { return moveLocked; } set { moveLocked = value; } }
        public bool ZoomLocked { get { return zoomLocked; } set { zoomLocked = value; } }

        public AxisPosition Position { get { return axisPosition; } set { axisPosition = value; } }
        public double CrossValue { get { return crossValue; } set { crossValue = value; } }

        public TickStyle MajorTickStyle { get { return majorTickStyle; } set { majorTickStyle = value; } }
        public TickStyle MinorTickStyle { get { return minorTickStyle; } set { minorTickStyle = value; } }
        public LabelsPosition TicksLabelsPosition { get { return ticksLabelsPosition; } set { ticksLabelsPosition = value; } }
        public Alignment TicksLabelsAlignment { get { return ticksLabelsAlignment; } set { ticksLabelsAlignment = value; } }
        public Alignment TicksLabelsLineAlignment { get { return ticksLabelsLineAlignment; } set { ticksLabelsLineAlignment = value; } }
        public TicksLabelsRotation TicksLabelsRotation { get { return ticksLabelsRotation; } set { ticksLabelsRotation = value; } }

        public Alignment TitleAlignment { get { return titleAlignment; } set { titleAlignment = value; } }
        public LabelsPosition TitlePosition { get { return titlePosition; } set { titlePosition = value; } }
        public GridStyle GridStyle { get { return gridStyle; } set { gridStyle = value; } }

        public Font Font { get { return font; } set { font = value; } }
        public Color Color { get { return color; } set { color = value; brush = new SolidBrush(color); pen = new Pen(brush); } }

        public void CalculateTranformVertical(float minimum, float maximum)
        {
            if (!logarithmic)
            {
                if (!reverse)
                {
                    res = (maximum - minimum) / (this.minimum - this.maximum);
                    offset = maximum - this.minimum * res;
                }
                else
                {
                    res = (maximum - minimum) / (this.maximum - this.minimum);
                    offset = minimum - this.minimum * res;
                }
            }
            else
            {
                if (!reverse)
                {
                    res = (maximum - minimum) / (float)(Math.Log10(this.minimum) - Math.Log10(this.maximum));
                    offset = maximum - (float)Math.Log10(this.minimum) * res;
                }
                else
                {
                    res = (maximum - minimum) / (float)(Math.Log10(this.maximum) - Math.Log10(this.minimum));
                    offset = minimum - (float)Math.Log10(this.minimum) * res;
                }
            }
        }
        public void CalculateTranformHorizontal(float minimum, float maximum)
        {
            if (!logarithmic)
            {
                if (reverse)
                {
                    res = (maximum - minimum) / (this.minimum - this.maximum);
                    offset = maximum - this.minimum * res;
                }
                else
                {
                    res = (maximum - minimum) / (this.maximum - this.minimum);
                    offset = minimum - this.minimum * res;
                }
            }
            else
            {
                if (reverse)
                {
                    res = (maximum - minimum) / (float)(Math.Log10(this.minimum) - Math.Log10(this.maximum));
                    offset = maximum - (float)Math.Log10(this.minimum) * res;
                }
                else
                {
                    res = (maximum - minimum) / (float)(Math.Log10(this.maximum) - Math.Log10(this.minimum));
                    offset = minimum - (float)Math.Log10(this.minimum) * res;
                }
            }
        }
        double TransformBack(double value)
        {
            if (!logarithmic)
                return (value - offset) / res;
            return Math.Pow(10, (value - offset) / res);
        }
        public float Transform(double value)
        {
            return (float)(offset + (logarithmic ? Math.Log10(value) : value) * res);
            /*float transformed = ;
            if (transformed < -100)
                return -100;
            if (transformed > 10000)
                return 10000;
            return transformed;*/
        }

        public bool ActionOnAxis(float x, float y)
        {
            return drawnRectangle.InRectangle(x, y);
        }
        public void Move(float length)
        {
            if (!moveLocked)
            {
                if (!logarithmic)
                {
                    minimum += length / res;
                    maximum += length / res;
                }
                else
                {
                    minimum *= Math.Pow(10, length / res);
                    maximum *= Math.Pow(10, length / res);
                }
            }
        }
        public void Select(float min, float max)
        {
            if (!zoomLocked)
            {
                minimum = TransformBack(min);
                maximum = TransformBack(max);
            }
        }
        public void Zoom(double zoom, float length)
        {
            if (!zoomLocked)
            {
                if (!logarithmic)
                {
                    double halfscale = (maximum - minimum) / 2;
                    minimum += (1 - zoom) * (halfscale + length / res);
                    maximum -= (1 - zoom) * (halfscale - length / res);
                }
                else
                {
                    double halfscale = (Math.Log10(maximum) - Math.Log10(minimum)) / 2;
                    minimum = Math.Pow(10, Math.Log10(minimum) + (1 - zoom) * (halfscale + length / res));
                    maximum = Math.Pow(10, Math.Log10(maximum) - (1 - zoom) * (halfscale - length / res));
                }
            }
        }

        private void CalculateTicks(Graphics g)
        {
            if (!logarithmic)
            {
                double step = (maximum - minimum) / TickNumber;
                int roundStepSign = Accessories.FirstSignRound(step);
                step = Accessories.Round(step, roundStepSign);
                double mark = Math.Ceiling(minimum / step) * step;
                int maxDegree = 0;
                
                double nextmark = mark + step;
                if (mark - nextmark == 0) //step smaller than floating point resolution
                {
                    tick = new double[] { minimum, maximum };
                    subTick = new double[] { };
                    maxDegree = Accessories.Degree(minimum);
                    if (maxDegree < Accessories.Degree(maximum))
                        maxDegree = Accessories.Degree(maximum);
                }
                else
                {
                    List<double> tickList = new List<double>();
                    List<double> subTickList = new List<double>();

                    double subStep = step / SubTickNumber;
                    double subMark = mark - subStep;

                    while (subMark > Minimum) //MinorTick before first MajorTick
                    {
                        subTickList.Add(subMark);
                        subMark -= subStep;
                    }

                    while (mark <= maximum)
                    {
                        tickList.Add(mark);
                        //if (Math.Abs(maxDegree) < Math.Abs(Accessories.Degree(mark)))
                            if (maxDegree < Accessories.Degree(mark))
                                maxDegree = Accessories.Degree(mark);

                        subMark = mark + subStep;
                        for (int i = 0; i < SubTickNumber - 1 && subMark < Maximum; i++)
                        {
                            subTickList.Add(subMark);
                            subMark += subStep;
                        }

                        mark = Accessories.Round(mark + step, roundStepSign);
                    }
                    tick = tickList.ToArray();
                    subTick = subTickList.ToArray();
                }

                tickLabelFormat = Accessories.FloatFormat(Accessories.FirstSignRound(step / Accessories.Pow1000(maxDegree)));
            }
            else
            {
                tick = new double[(int)(Math.Log10(maximum) - Math.Log10(minimum) + 1)];
                subTick = new double[0];

                for (int i = 0; i < tick.Length; i++)
                    tick[i] = Accessories.Pow10(i) * minimum;

                tickLabelFormat = "###";
            }

            tickLabel = new string[tick.Length];
            tickLabelSize = new SizeF[tick.Length];
            for (int i = 0; i < tick.Length; i++)
            {
                tickLabel[i] = Accessories.ToSI(tick[i], tickLabelFormat);
                tickLabelSize[i] = g.MeasureString(tickLabel[i], this.Font);
            }
        }
        public void MeasureVertical(Graphics g, Size plotSize)
        {
            if (ticksLabelsLineAlignment == Alignment.Center && ticksLabelsRotation == TicksLabelsRotation.Parallel)
                throw new Exception("Central line alignment is not possible for parallel rotation of the tick`s labels.");

            bool decreaseTickNumber;
            SizeF tickLabelFormatSize;
            TickNumber = 10;
            do
            {
                CalculateTicks(g);
                tickLabelFormatSize = new SizeF();
                tickLabelFormatSize.Width = tickLabelSize.Max(e => e.Width);
                tickLabelFormatSize.Height = tickLabelSize.First().Height;
                float ticksLabelsLengthTotal = tickLabelSize.Sum(e => e.Width + e.Height);
                decreaseTickNumber = ticksLabelsPosition != LabelsPosition.None && ticksLabelsRotation == TicksLabelsRotation.Parallel && ticksLabelsLengthTotal > plotSize.Width;
                TickNumber = decreaseTickNumber ? (int)Math.Floor((double)(TickNumber) / 2.0) : TickNumber;
            }
            while (decreaseTickNumber);

            sizeNear = ticksLabelsPosition == LabelsPosition.Near ? 6 : 0;
            sizeFar = ticksLabelsPosition == LabelsPosition.Far ? 6 : 0;
            overflowFar = tickLabelFormatSize.Height;
            overflowNear = tickLabelFormatSize.Height;
            if (ticksLabelsPosition != LabelsPosition.None && tickLabel.Any())
            {
                if (ticksLabelsPosition == LabelsPosition.Near)
                {
                    if (ticksLabelsRotation == TicksLabelsRotation.Parallel)
                        sizeNear += tickLabelFormatSize.Height;
                    else if (ticksLabelsRotation == TicksLabelsRotation.Perpendicular)
                        sizeNear += tickLabelFormatSize.Width;
                    else
                        sizeNear += tickLabelFormatSize.Width / 1.4f;
                }
                else
                {
                    if (ticksLabelsRotation == TicksLabelsRotation.Parallel)
                        sizeFar += tickLabelFormatSize.Height;
                    else if (ticksLabelsRotation == TicksLabelsRotation.Perpendicular)
                        sizeFar += tickLabelFormatSize.Width;
                    else
                        sizeFar += tickLabelFormatSize.Width / 1.4f;
                }

                if (ticksLabelsRotation == TicksLabelsRotation.Parallel)
                {
                    if (ticksLabelsAlignment == Alignment.Near)
                        overflowFar = tickLabelFormatSize.Width;
                    else
                        overflowNear = tickLabelFormatSize.Width;
                }
                else if (ticksLabelsAlignment == Alignment.Center)
                {
                    overflowFar = tickLabelFormatSize.Width / 2.0f;
                    overflowNear = overflowFar;
                }
                else
                {
                    if (ticksLabelsAlignment == Alignment.Far)
                        overflowNear = tickLabelFormatSize.Width / 1.4f;
                    else
                        overflowFar = tickLabelFormatSize.Width / 1.4f;
                }
            }

            if (!string.IsNullOrEmpty(Title) && titlePosition != LabelsPosition.None)
            {
                titleSize = g.MeasureString(title, this.font);
                if (titlePosition == LabelsPosition.Near)
                    sizeNear += titleSize.Height;
                else if (titlePosition == LabelsPosition.Far)
                    sizeFar += titleSize.Height;

                if (titleAlignment == Alignment.Near)
                    overflowNear = (titleSize.Width / 2) > overflowNear ? (titleSize.Width / 2) : overflowNear;
                else if (titleAlignment == Alignment.Far)
                    overflowFar = (titleSize.Width / 2) > overflowFar ? (titleSize.Width / 2) : overflowFar;
            }
        }
        public void MeasureHorizontal(Graphics g, Size plotSize)
        {
            if (ticksLabelsAlignment == Alignment.Center && ticksLabelsRotation != TicksLabelsRotation.Parallel)
                    throw new Exception("Central alignment is only possible for parallel rotation of the tick`s labels.");

            bool decreaseTickNumber;
            SizeF tickLabelFormatSize;
            TickNumber = 10;
            do
            {
                CalculateTicks(g);
                tickLabelFormatSize = new SizeF();
                tickLabelFormatSize.Width = tickLabelSize.Max(e => e.Width);
                tickLabelFormatSize.Height = tickLabelSize.First().Height;
                float ticksLabelsLengthTotal = tickLabelSize.Sum(e => e.Width);
                decreaseTickNumber = ticksLabelsPosition != LabelsPosition.None && ticksLabelsRotation == TicksLabelsRotation.Parallel && ticksLabelsLengthTotal > plotSize.Width;
                TickNumber = decreaseTickNumber ? (int)Math.Floor((double)(TickNumber) / 2.0) : TickNumber;
            }
            while (decreaseTickNumber);

            sizeNear = ticksLabelsPosition == LabelsPosition.Near ? 6 : 0;
            sizeFar = ticksLabelsPosition == LabelsPosition.Far ? 6 : 0;
            overflowFar = tickLabelFormatSize.Height;
            overflowNear = tickLabelFormatSize.Height;

            if (ticksLabelsPosition != LabelsPosition.None && tickLabel.Any())
            {
                if (ticksLabelsPosition == LabelsPosition.Far)
                {
                    if (ticksLabelsRotation == TicksLabelsRotation.Parallel)
                        sizeFar += tickLabelFormatSize.Height;
                    else if (ticksLabelsRotation == TicksLabelsRotation.Perpendicular)
                        sizeFar += tickLabelFormatSize.Width;
                    else
                        sizeFar += tickLabelFormatSize.Width / 1.4f;
                }
                else
                {
                    if (ticksLabelsRotation == TicksLabelsRotation.Parallel)
                        sizeNear += tickLabelFormatSize.Height;
                    else if (ticksLabelsRotation == TicksLabelsRotation.Perpendicular)
                        sizeNear += tickLabelFormatSize.Width;
                    else
                        sizeNear += tickLabelFormatSize.Width / 1.4f;
                }

                if (ticksLabelsAlignment == Alignment.Center)
                {
                    overflowFar = tickLabelFormatSize.Width / 2.0f;
                    overflowNear = overflowFar;
                }
                else if (ticksLabelsRotation == TicksLabelsRotation.Parallel)
                {
                    if (ticksLabelsAlignment == Alignment.Near)
                        overflowFar = tickLabelFormatSize.Width;
                    else
                        overflowNear = tickLabelFormatSize.Width;
                }
                else
                {
                    if (ticksLabelsAlignment == Alignment.Near)
                        overflowFar = tickLabelFormatSize.Width / 1.4f;
                    else
                        overflowNear = tickLabelFormatSize.Width / 1.4f;
                }
            }

            if (!string.IsNullOrEmpty(Title) && titlePosition != LabelsPosition.None)
            {
                titleSize = g.MeasureString(title, this.font);
                if (titlePosition == LabelsPosition.Near)
                    sizeNear += titleSize.Height;
                else if (titlePosition == LabelsPosition.Far)
                    sizeFar += titleSize.Height;

                if (titleAlignment == Alignment.Near)
                    overflowNear = (titleSize.Width / 2) > overflowNear ? (titleSize.Width / 2) : overflowNear;
                else if (titleAlignment == Alignment.Far)
                    overflowFar = (titleSize.Width / 2) > overflowFar ? (titleSize.Width / 2) : overflowFar;
            }
        }
        
        public void DrawVertical(Graphics g, float x, float y, RectangleExtended rect)
        {
            drawnRectangle = new RectangleExtended(x, y, 0, rect.Height);

            if (ticksLabelsPosition == LabelsPosition.Near)
                drawnRectangle.Left -= sizeNear;
            else if (ticksLabelsPosition == LabelsPosition.Far)
                drawnRectangle.Right += sizeFar;
            else
                drawnRectangle.FullScaleX = 10;

            StringFormat stringFormat = new StringFormat();
            int rotationSign = 1;


            if (ticksLabelsRotation == TicksLabelsRotation.Parallel)
            {
                stringFormat.LineAlignment = Alignment2StringAlignment(ticksLabelsLineAlignment);
                stringFormat.Alignment = Alignment2StringAlignment(ticksLabelsAlignment);
                if ((ticksLabelsPosition == LabelsPosition.Near && ticksLabelsLineAlignment == Alignment.Near) || (ticksLabelsPosition == LabelsPosition.Far && ticksLabelsLineAlignment == Alignment.Far))
                    rotationSign = 1;
                else
                    rotationSign = -1;
            }
            else if(ticksLabelsRotation == TicksLabelsRotation.Perpendicular)
            {
                stringFormat.LineAlignment = Alignment2StringAlignment(ticksLabelsLineAlignment);
                if (ticksLabelsPosition == LabelsPosition.Near)
                    stringFormat.Alignment = StringAlignment.Far;
                else
                    stringFormat.Alignment = StringAlignment.Near;
            }
            else
            {
                stringFormat.LineAlignment = StringAlignment.Center;
                if (ticksLabelsPosition == LabelsPosition.Near)
                    stringFormat.Alignment = StringAlignment.Far;
                else
                    stringFormat.Alignment = StringAlignment.Near;
                if (ticksLabelsLineAlignment == Alignment.Near)
                    rotationSign = 1;
                else
                    rotationSign = -1;
            }

            float tickLabelPositionX = ticksLabelsPosition == LabelsPosition.Near ? x - 6 : x + 6;

            for (int i = 0; i < tickLabel.Length; i++)
            {
                float ticky = Transform(tick[i]);
                if (gridStyle == GridStyle.Both || gridStyle == GridStyle.Major)
                    g.DrawLine(Pens.Gray, rect.Left, ticky, rect.Right, ticky);

                switch (MajorTickStyle)
                {
                    case TickStyle.Far: g.DrawLine(pen, x, ticky, x + 5, ticky); break;
                    case TickStyle.Near: g.DrawLine(pen, x, ticky, x - 5, ticky); break;
                    case TickStyle.Cross: g.DrawLine(pen, x + 5, ticky, x - 5, ticky); break;
                    case TickStyle.None: break;
                }

                if (ticksLabelsPosition != LabelsPosition.None)
                {
                    if (ticksLabelsRotation == TicksLabelsRotation.Parallel)
                    {
                        g.TranslateTransform(tickLabelPositionX, ticky);
                        g.RotateTransform(rotationSign * 90);
                        g.TranslateTransform(-tickLabelPositionX, -ticky);
                        g.DrawString(tickLabel[i], this.font, brush, tickLabelPositionX, ticky, stringFormat);
                        g.ResetTransform();
                    }
                    else if (ticksLabelsRotation == TicksLabelsRotation.Tilted)
                    {
                        g.TranslateTransform(tickLabelPositionX, ticky);
                        g.RotateTransform(rotationSign * 45);
                        g.TranslateTransform(-tickLabelPositionX, -ticky);
                        g.DrawString(tickLabel[i], this.font, brush, tickLabelPositionX, ticky, stringFormat);
                        g.ResetTransform();
                    }
                    else
                        g.DrawString(tickLabel[i], this.font, brush, tickLabelPositionX, ticky, stringFormat);
                }
            }
            for (int i = 0; i < subTick.Length; i++)
            {
                float ticky = Transform(subTick[i]);
                if (gridStyle == GridStyle.Both || gridStyle == GridStyle.Minor)
                    g.DrawLine(Pens.Silver, rect.Left, ticky, rect.Right, ticky);

                switch (MinorTickStyle)
                {
                    case TickStyle.Far: g.DrawLine(pen, x, ticky, x + 2, ticky); break;
                    case TickStyle.Near: g.DrawLine(pen, x, ticky, x - 2, ticky); break;
                    case TickStyle.Cross: g.DrawLine(pen, x + 2, ticky, x - 2, ticky); break;
                    case TickStyle.None: break;
                }
            }

            //Axis line
            g.DrawLine(pen, x, Transform(minimum), x, Transform(maximum));

            //Title drawing
            if (!string.IsNullOrEmpty(title))
            {
                StringFormat titleFormat = new StringFormat();
                float titleX = 0;
                float titleY = 0;
                if (titlePosition == LabelsPosition.Near)
                {
                    titleFormat.LineAlignment = StringAlignment.Near;
                    if (titleAlignment == Alignment.Center)
                    {
                        titleFormat.Alignment = StringAlignment.Center;
                        titleX = x - SizeNear;
                        titleY = y + rect.Height / 2;
                    }
                    else if (titleAlignment == Alignment.Near)
                    {
                        titleFormat.Alignment = StringAlignment.Far;
                        titleX = x - SizeNear;
                        titleY = y;
                    }
                    else
                    {
                        titleFormat.Alignment = StringAlignment.Near;
                        titleX = x - SizeNear;
                        titleY = y + rect.Height;
                    }
                }
                else if (titlePosition == LabelsPosition.Far)
                {
                    titleFormat.LineAlignment = StringAlignment.Far;
                    if (titleAlignment == Alignment.Center)
                    {
                        titleX = x + SizeFar;
                        titleY = y + rect.Height / 2;
                    }
                    else if (titleAlignment == Alignment.Near)
                    {
                        titleFormat.Alignment = StringAlignment.Far;
                        titleX = x + SizeFar;
                        titleY = y;
                    }
                    else if (titleAlignment == Alignment.Far)
                    {
                        titleFormat.Alignment = StringAlignment.Near;
                        titleX = x + SizeFar;
                        titleY = y + rect.Height;
                    }
                }

                g.TranslateTransform(titleX, titleY);
                g.RotateTransform(-90);
                g.TranslateTransform(-titleX, -titleY);
                g.DrawString(title, this.font, brush, titleX, titleY, titleFormat);
                g.ResetTransform();
            }
            //end of Horizontal drawing
        }
        public void DrawHorizontal(Graphics g, float x, float y, RectangleExtended rect)
        {
            drawnRectangle = new RectangleExtended(x, y, rect.Width, 0);

            if (ticksLabelsPosition == LabelsPosition.Far)
                drawnRectangle.Bottom += sizeFar;
            else if (ticksLabelsPosition == LabelsPosition.Near)
                drawnRectangle.Top -= sizeNear;
            else
                drawnRectangle.FullScaleY = 10;

            StringFormat stringFormat = new StringFormat();

            if (ticksLabelsAlignment == Alignment.Near)
                stringFormat.Alignment = StringAlignment.Near;
            else if (ticksLabelsAlignment == Alignment.Center)
                stringFormat.Alignment = StringAlignment.Center;
            else
                stringFormat.Alignment = StringAlignment.Far;

            if (ticksLabelsRotation == TicksLabelsRotation.Parallel)
            {
                if (ticksLabelsPosition == LabelsPosition.Far)
                    stringFormat.LineAlignment = StringAlignment.Near;
                else
                    stringFormat.LineAlignment = StringAlignment.Far;
            }
            else
                stringFormat.LineAlignment = StringAlignment.Center;

            int rotationSign = -1;
            if (ticksLabelsPosition == LabelsPosition.Far && ticksLabelsAlignment == Alignment.Near ||
                ticksLabelsPosition == LabelsPosition.Near && ticksLabelsAlignment == Alignment.Far)
                rotationSign = 1;

            float tickLabelPositionY = ticksLabelsPosition == LabelsPosition.Far ? y + 6 : y - 6;

            for (int i = 0; i < tickLabel.Length; i++)
            {
                float tickx = Transform(tick[i]);
                if (gridStyle == GridStyle.Both || gridStyle == GridStyle.Major)
                    g.DrawLine(Pens.Gray, tickx, rect.Top, tickx, rect.Bottom);

                switch (MajorTickStyle)
                {
                    case TickStyle.Near: g.DrawLine(pen, tickx, y, tickx, y + 5); break;
                    case TickStyle.Far: g.DrawLine(pen, tickx, y, tickx, y - 5); break;
                    case TickStyle.Cross: g.DrawLine(pen, tickx, y + 5, tickx, y - 5); break;
                    case TickStyle.None: break;
                }
                if (ticksLabelsPosition != LabelsPosition.None)
                {

                    if (ticksLabelsRotation == TicksLabelsRotation.Perpendicular)
                    {
                        g.TranslateTransform(tickx, tickLabelPositionY);
                        g.RotateTransform(rotationSign * 90);
                        g.TranslateTransform(-tickx, -tickLabelPositionY);
                        g.DrawString(tickLabel[i], this.font, brush, tickx, tickLabelPositionY, stringFormat);
                        g.ResetTransform();
                    }
                    else if (ticksLabelsRotation == TicksLabelsRotation.Tilted)
                    {
                        g.TranslateTransform(tickx, tickLabelPositionY);
                        g.RotateTransform(rotationSign * 45);
                        g.TranslateTransform(-tickx, -tickLabelPositionY);
                        g.DrawString(tickLabel[i], this.font, brush, tickx, tickLabelPositionY, stringFormat);
                        g.ResetTransform();
                    }
                    else
                        g.DrawString(tickLabel[i], this.font, brush, tickx, tickLabelPositionY, stringFormat);
                }
            }
            for (int i = 0; i < subTick.Length; i++)
            {
                float tickx = Transform(subTick[i]);
                if (gridStyle == GridStyle.Both || gridStyle == GridStyle.Minor)
                    g.DrawLine(Pens.Silver, tickx, rect.Top, tickx, rect.Bottom);

                switch (MinorTickStyle)
                {
                    case TickStyle.Near: g.DrawLine(pen, tickx, y, tickx, y + 2); break;
                    case TickStyle.Far: g.DrawLine(pen, tickx, y, tickx, y - 2); break;
                    case TickStyle.Cross: g.DrawLine(pen, tickx, y + 2, tickx, y - 2); break;
                    case TickStyle.None: break;
                }
            }

            //Axis line
            g.DrawLine(pen, Transform(minimum), y, Transform(maximum), y);

            //Title drawing
            if (!string.IsNullOrEmpty(title))
            {
                StringFormat titleFormat = new StringFormat();
                if (titlePosition == LabelsPosition.Near)
                {
                    titleFormat.LineAlignment = StringAlignment.Near;
                    if (titleAlignment == Alignment.Center)
                    {
                        titleFormat.Alignment = StringAlignment.Center;
                        g.DrawString(title, this.font, brush, x + rect.Width / 2, y - SizeNear, titleFormat);
                    }
                    else if (titleAlignment == Alignment.Near)
                    {
                        titleFormat.Alignment = StringAlignment.Near;
                        g.DrawString(title, this.font, brush, x, y - SizeNear, titleFormat);
                    }
                    else
                    {
                        titleFormat.Alignment = StringAlignment.Far;
                        g.DrawString(title, this.font, brush, x + rect.Width, y - SizeNear, titleFormat);
                    }
                }
                else if (titlePosition == LabelsPosition.Far)
                {
                    titleFormat.LineAlignment = StringAlignment.Far;
                    if (titleAlignment == Alignment.Center)
                    {
                        g.DrawString(title, this.font, brush, x + rect.Width / 2, y + sizeFar, titleFormat);
                    }
                    else if (titleAlignment == Alignment.Near)
                    {
                        titleFormat.Alignment = StringAlignment.Near;
                        g.DrawString(title, this.font, brush, x, y + sizeFar, titleFormat);
                    }
                    else if (titleAlignment == Alignment.Far)
                    {
                        titleFormat.Alignment = StringAlignment.Far;
                        g.DrawString(title, this.font, brush, x + rect.Width, y + sizeFar, titleFormat);
                    }
                }
            }
            //end of Horizontal drawing
        }

        StringAlignment Alignment2StringAlignment(Alignment a)
        {
            if (a == Alignment.Near)
                return StringAlignment.Near;
            else if (a == Alignment.Center)
                return StringAlignment.Center;
            else
                return StringAlignment.Far;
        }
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
    public enum LabelsPosition
    {
        Near,
        Far,
        None
    }
    public enum Alignment
    {
        Center,
        Near,
        Far
    }
    public enum TicksLabelsRotation
    {
        Parallel,
        Tilted,
        Perpendicular
    }
    public enum GridStyle
    {
        None,
        Major,
        Minor,
        Both
    }

}

