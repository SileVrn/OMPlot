﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot
{

    internal delegate void AxisAutoscaleEvent(object sender);

    /// <summary>
    /// Represents an axis
    /// </summary>
    public class Axis
    {
        Brush brush;
        Pen pen;
        Color color;

        RectangleF drawnRectangle;

        private SizeF titleSize;
        private double res;
        private double offset;
        private double[] tick;
        private string[] tickLabel;
        private float[] tickLabelLocation;
        private SizeF[] tickLabelSize;
        private double[] subTicks;
        private string tickLabelFormat;

        private bool logarithmic;

        internal event AxisAutoscaleEvent AutoscaleEvent;
        public void Autoscale()
        {
            Minimum = double.MaxValue;
            Maximum = Logarithmic ? double.Epsilon : double.MinValue;
            AutoscaleEvent?.Invoke(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "Axis" /> class.
        /// </summary>
        public Axis()
        {
            Color = Color.Black;
            TickNumber = 10;
            TitleAlignment = Alignment.Center;
        }

        /// <summary>
        /// The minimum value of the axis.
        /// </summary>
        public double Minimum { get; set; }
        /// <summary>
        /// The maximum value of the axis.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// A title of the axis.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// True if the axis is logarithmic.
        /// </summary>
        public bool Logarithmic
        { 
            get { return logarithmic; }
            set { logarithmic = value; Autoscale(); }
        }
        internal bool Vertical { get; set; }
        /// <summary>
        /// True if the axis reversed.
        /// </summary>
        public bool Reverse { get; set; }
        /// <summary>
        /// Locking the axis movement.
        /// </summary>
        public bool MoveLocked { get; set; }
        /// <summary>
        /// Locking the axis zoom.
        /// </summary>
        public bool ZoomLocked { get; set; }
        /// <summary>
        /// Position of the axis on a plot.
        /// </summary>
        public AxisPosition Position { get; set; }
        /// <summary>
        /// Position of the axes crossing, valid only for Axis.Position == AxisPosition.CrossValue.
        /// </summary>
        public double CrossValue { get; set; }

        /// <summary>
        /// Style of the major ticks.
        /// </summary>
        public TickStyle MajorTickStyle { get; set; }
        /// <summary>
        /// Style of the minor ticks.
        /// </summary>
        public TickStyle MinorTickStyle { get; set; }
        /// <summary>
        /// Position of the tick labels relative to the axis.
        /// </summary>
        public LabelsPosition TicksLabelsPosition { get; set; }
        /// <summary>
        /// Alignment of the tick labels along the axis.
        /// </summary>
        public Alignment TicksLabelsAlignment { get; set; }
        /// <summary>
        /// Alignment of the tick labels perpendicular to the axis.
        /// </summary>
        public Alignment TicksLabelsLineAlignment { get; set; }
        /// <summary>
        /// Rotation of the tick labels.
        /// </summary>
        public TicksLabelsRotation TicksLabelsRotation { get; set; }
        /// <summary>
        /// Number of major ticks
        /// </summary>
        public int TickNumber { get; set; }
        /// <summary>
        /// Number of minor ticks
        /// </summary>
        public int MinorTickNumber { get; set; }
        /// <summary>
        /// Values of custom ticks.
        /// </summary>
        public double[] CustomTicks { get; set; }
        /// <summary>
        /// The custom labels for the CustomTicks.
        /// </summary>
        public string[] CustomTicksLabels { get; set; }

        /// <summary>
        /// Alignment of the title along the axis.
        /// </summary>
        public Alignment TitleAlignment { get; set; }
        /// <summary>
        /// Position of the title along the axis.
        /// </summary>
        public LabelsPosition TitlePosition { get; set; }
        /// <summary>
        /// Style of the grid.
        /// </summary>
        public GridStyle GridStyle { get; set; }

        /// <summary>
        /// Font of the title and the tick labels
        /// </summary>
        public Font Font { get; set; }
        /// <summary>
        /// Color of axis, ticks, labels.
        /// </summary>
        public Color Color { get { return color; } set { color = value; brush = new SolidBrush(color); pen = new Pen(brush); } }
        
        internal float SizeNear { get; private set; }
        internal float SizeFar { get; private set; }
        internal float OverflowNear { get; private set; }
        internal float OverflowFar { get; private set; }

        internal void CalculateTranformVertical(float minimum, float maximum)
        {
            if (!Logarithmic)
            {
                if (!Reverse)
                {
                    res = (maximum - minimum) / (this.Minimum - this.Maximum);
                    offset = maximum - this.Minimum * res;
                }
                else
                {
                    res = (maximum - minimum) / (this.Maximum - this.Minimum);
                    offset = minimum - this.Minimum * res;
                }
            }
            else
            {
                if (!Reverse)
                {
                    res = (maximum - minimum) / (float)(Math.Log10(this.Minimum) - Math.Log10(this.Maximum));
                    offset = maximum - (float)Math.Log10(this.Minimum) * res;
                }
                else
                {
                    res = (maximum - minimum) / (float)(Math.Log10(this.Maximum) - Math.Log10(this.Minimum));
                    offset = minimum - (float)Math.Log10(this.Minimum) * res;
                }
            }
        }
        internal void CalculateTranformHorizontal(float minimum, float maximum)
        {
            if (!Logarithmic)
            {
                if (Reverse)
                {
                    res = (maximum - minimum) / (this.Minimum - this.Maximum);
                    offset = maximum - this.Minimum * res;
                }
                else
                {
                    res = (maximum - minimum) / (this.Maximum - this.Minimum);
                    offset = minimum - this.Minimum * res;
                }
            }
            else
            {
                if (Reverse)
                {
                    res = (maximum - minimum) / (float)(Math.Log10(this.Minimum) - Math.Log10(this.Maximum));
                    offset = maximum - (float)Math.Log10(this.Minimum) * res;
                }
                else
                {
                    res = (maximum - minimum) / (float)(Math.Log10(this.Maximum) - Math.Log10(this.Minimum));
                    offset = minimum - (float)Math.Log10(this.Minimum) * res;
                }
            }
        }
        /// <summary>
        /// Convert a screen value to an axis value.
        /// </summary>
        /// <param name="value">A screen value</param>
        /// <returns>An axis value</returns>
        public double TransformBack(double value) { return Logarithmic ? Math.Pow(10, (value - offset) / res) : (value - offset) / res; }
        /// <summary>
        /// Convert an axis value to a screen value
        /// </summary>
        /// <param name="value">An axis value</param>
        /// <returns>A screen value</returns>
        public double Transform(double value)
        {
            double ret = offset + (Logarithmic ? Math.Log10(value) : value) * res;
            if (ret < -1000)
                return -1000;
            if (ret > 10000)
                return 10000;
            return ret;
        }

        public bool ActionOnAxis(float x, float y) { return drawnRectangle.Contains(x, y); }
        public void Move(float length)
        {
            if (!MoveLocked)
            {
                if (!Logarithmic)
                {
                    Minimum += length / res;
                    Maximum += length / res;
                }
                else
                {
                    Minimum *= Math.Pow(10, length / res);
                    Maximum *= Math.Pow(10, length / res);
                }
            }
        }
        public void Select(float min, float max)
        {
            if (!ZoomLocked)
            {
                Minimum = TransformBack(min);
                Maximum = TransformBack(max);
            }
        }
        public void Zoom(double zoom, float length)
        {
            if (!ZoomLocked)
            {
                if (!Logarithmic)
                {
                    double halfscale = (Maximum - Minimum) / 2;
                    Minimum += (1 - zoom) * (halfscale + length / res);
                    Maximum -= (1 - zoom) * (halfscale - length / res);
                }
                else
                {
                    double halfscale = (Math.Log10(Maximum) - Math.Log10(Minimum)) / 2;
                    Minimum = Math.Pow(10, Math.Log10(Minimum) + (1 - zoom) * (halfscale + length / res));
                    Maximum = Math.Pow(10, Math.Log10(Maximum) - (1 - zoom) * (halfscale - length / res));
                }
            }
        }

        private void CalculateTicks(Graphics g)
        {
            if(CustomTicks != null && CustomTicks.Length > 0)
            {
                tick = CustomTicks.Where(ct => ct >= Minimum && ct <= Maximum).ToArray();
                subTicks = new double[0];

                if(CustomTicksLabels != null && CustomTicksLabels.Length > 0)
                {
                    if (CustomTicksLabels.Length != CustomTicks.Length)
                        throw new Exception("Number of CustomTicks not equal to number of CustomTicksLabels.");

                    tickLabel = CustomTicksLabels.Where((ctl, i) => CustomTicks[i] >= Minimum && CustomTicks[i] <= Maximum).ToArray();
                    tickLabelSize = tickLabel.Select(tl => g.MeasureString(tl, this.Font)).ToArray();
                    tickLabelLocation = CustomTicks.Where(ct => ct >= Minimum && ct <= Maximum).Select(ct => (float)this.Transform(ct)).ToArray();

                    List<double> subTicksList = new List<double>();
                    double subTick;
                    for (int i = 0; i < CustomTicks.Length - 1; i++)
                    {
                        for (int j = 0; j < MinorTickNumber; j++)
                        {
                            subTick = CustomTicks[i] + (j + 1) * (CustomTicks[i + 1] - CustomTicks[i]) / (MinorTickNumber + 1);
                            if (subTick >= Minimum && subTick <= Maximum)
                                subTicksList.Add(subTick);
                        }
                    }
                    subTicks = subTicksList.ToArray();
                    return;
                }
            }
            else if (!Logarithmic || (Maximum / Minimum) < 10)
            {
                double step = (Maximum - Minimum) / TickNumber;
                int roundStepSign = Accessories.FirstSignRound(step);
                step = Accessories.Round(step, roundStepSign);
                double mark = Math.Ceiling(Minimum / step) * step;
                int maxDegree = 0;
                
                double nextmark = mark + step;
                if (mark - nextmark == 0) //step smaller than floating point resolution
                {
                    tick = new double[] { Minimum, Maximum };
                    subTicks = new double[] { };
                    maxDegree = Accessories.Degree(Minimum);
                    if (maxDegree < Accessories.Degree(Maximum))
                        maxDegree = Accessories.Degree(Maximum);
                }
                else
                {
                    List<double> tickList = new List<double>();
                    List<double> subTickList = new List<double>();

                    double subStep = step / MinorTickNumber;
                    double subMark = mark - subStep;

                    while (subMark > Minimum) //MinorTick before first MajorTick
                    {
                        subTickList.Add(subMark);
                        subMark -= subStep;
                    }

                    while (mark <= Maximum)
                    {
                        tickList.Add(mark);
                        //if (Math.Abs(maxDegree) < Math.Abs(Accessories.Degree(mark)))
                            if (maxDegree < Accessories.Degree(mark))
                                maxDegree = Accessories.Degree(mark);

                        subMark = mark + subStep;
                        for (int i = 0; i < MinorTickNumber - 1 && subMark < Maximum; i++)
                        {
                            subTickList.Add(subMark);
                            subMark += subStep;
                        }

                        mark = Accessories.Round(mark + step, roundStepSign);
                    }
                    tick = tickList.ToArray();
                    subTicks = subTickList.ToArray();
                }

                tickLabelFormat = Accessories.FloatFormat(Accessories.FirstSignRound(step / Accessories.Pow1000(maxDegree)));
            }
            else
            {
                List<double> tickList = new List<double>();
                List<double> subTickList = new List<double>();

                double mark = Accessories.Pow10(Accessories.FirstSignRound(Minimum));
                mark = mark < Minimum ? mark * 10 : mark;
                double subMark = 0.9 * mark;


                for (int i = 0; i < MinorTickNumber - 1 && subMark >= Minimum; i++)
                {
                    subTickList.Add(subMark);
                    subMark -= 0.1 * mark;
                }
                while (mark <= Maximum)
                {
                    mark = Accessories.Pow10(Accessories.FirstSignRound(mark));
                    tickList.Add(mark);
                    subMark = mark + mark;
                    for (int i = 0; i < MinorTickNumber - 1 && subMark < Maximum; i++)
                    {
                        subTickList.Add(subMark);
                        subMark += mark;
                    }
                    mark *= 10;                    
                }
                tick = tickList.ToArray();
                subTicks = subTickList.ToArray();
                var aaa = tick.Select(t => Accessories.Degree(t)).ToArray();

                tickLabelFormat = "###";
            }
                        
            tickLabel = tick.Select(t => Accessories.ToSI(t, tickLabelFormat)).ToArray();
            tickLabelSize = tickLabel.Select(tl => g.MeasureString(tl, this.Font)).ToArray();
            tickLabelLocation = tick.Select(t => (float)this.Transform(t)).ToArray();
        }
        private SizeF AutomaticTicksCalculation(Graphics g)
        {
            bool decreaseTickNumber;
            SizeF tickLabelFormatSize = new SizeF();
            TickNumber = 20;
            do
            {
                CalculateTicks(g);
                if (!tickLabelSize.Any())
                    break;
                tickLabelFormatSize = new SizeF(tickLabelSize.Max(e => e.Width), tickLabelSize.First().Height);
                decreaseTickNumber = false;
                if (TicksLabelsPosition != LabelsPosition.None && (CustomTicks == null || CustomTicks.Length < 1) && TickNumber > 1)
                {
                    if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
                    {
                        for (int i = 0; i < tick.Length - 1; i++)
                        {
                            if (Math.Abs(tickLabelLocation[i + 1] - tickLabelLocation[i]) < Math.Max(tickLabelSize[i + 1].Width + tickLabelSize[i + 1].Height, tickLabelSize[i].Width + tickLabelSize[i].Height))
                            {
                                decreaseTickNumber = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < tick.Length - 1; i++)
                        {
                            if (Math.Abs(tickLabelLocation[i + 1] - tickLabelLocation[i]) < Math.Max(tickLabelSize[i + 1].Height + tickLabelSize[i + 1].Height, tickLabelSize[i].Height + tickLabelSize[i].Height))
                            {
                                decreaseTickNumber = true;
                                break;
                            }
                        }
                    }
                    TickNumber = decreaseTickNumber ? (int)Math.Floor((double)(TickNumber) / 2.0) : TickNumber;
                }
            }
            while (decreaseTickNumber);

            return tickLabelFormatSize;
        }
        private void TitleSizeCalculation(Graphics g)
        {
            if (!string.IsNullOrWhiteSpace(Title) && TitlePosition != LabelsPosition.None)
            {
                titleSize = g.MeasureString(Title, Font);
                if (TitlePosition == LabelsPosition.Near)
                    SizeNear += titleSize.Height;
                else if (TitlePosition == LabelsPosition.Far)
                    SizeFar += titleSize.Height;

                if (TitleAlignment == Alignment.Near)
                    OverflowNear = (titleSize.Width / 2) > OverflowNear ? (titleSize.Width / 2) : OverflowNear;
                else if (TitleAlignment == Alignment.Far)
                    OverflowFar = (titleSize.Width / 2) > OverflowFar ? (titleSize.Width / 2) : OverflowFar;
            }
        }
        internal void MeasureVertical(Graphics g)
        {
            if (TicksLabelsLineAlignment == Alignment.Center && (TicksLabelsRotation == TicksLabelsRotation.Parallel || TicksLabelsRotation == TicksLabelsRotation.Tilted))
                throw new Exception("Central line alignment is not possible for parallel rotation of the tick`s labels.");

            var tickLabelFormatSize = AutomaticTicksCalculation(g);

            SizeNear = TicksLabelsPosition == LabelsPosition.Near ? 6 : 0;
            SizeFar = TicksLabelsPosition == LabelsPosition.Far ? 6 : 0;
            OverflowFar = tickLabelFormatSize.Height;
            OverflowNear = tickLabelFormatSize.Height;
            if (TicksLabelsPosition != LabelsPosition.None && tickLabel.Any())
            {
                if (TicksLabelsPosition == LabelsPosition.Near)
                {
                    if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
                        SizeNear += tickLabelFormatSize.Height;
                    else if (TicksLabelsRotation == TicksLabelsRotation.Perpendicular)
                        SizeNear += tickLabelFormatSize.Width;
                    else
                        SizeNear += tickLabelFormatSize.Width / 1.4f;
                }
                else
                {
                    if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
                        SizeFar += tickLabelFormatSize.Height;
                    else if (TicksLabelsRotation == TicksLabelsRotation.Perpendicular)
                        SizeFar += tickLabelFormatSize.Width;
                    else
                        SizeFar += tickLabelFormatSize.Width / 1.4f;
                }

                if (TicksLabelsAlignment == Alignment.Center)
                {
                    OverflowFar = tickLabelFormatSize.Width / 2.0f;
                    OverflowNear = OverflowFar;
                }
                else if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
                {
                    if ((TicksLabelsPosition == LabelsPosition.Near && TicksLabelsAlignment == TicksLabelsLineAlignment) ||
                        (TicksLabelsPosition == LabelsPosition.Far && TicksLabelsAlignment != TicksLabelsLineAlignment))
                        OverflowFar = tickLabelFormatSize.Width;
                    else
                        OverflowNear = tickLabelFormatSize.Width;
                }
                else if (TicksLabelsRotation == TicksLabelsRotation.Tilted)
                {
                    if ((TicksLabelsPosition == LabelsPosition.Far && TicksLabelsLineAlignment == Alignment.Far) ||
                        (TicksLabelsPosition == LabelsPosition.Near && TicksLabelsLineAlignment == Alignment.Near))
                        OverflowNear = tickLabelFormatSize.Width / 1.4f;
                    else
                        OverflowFar = tickLabelFormatSize.Width / 1.4f;
                }
            }

            TitleSizeCalculation(g);
        }
        internal void MeasureHorizontal(Graphics g)
        {
            if (TicksLabelsAlignment == Alignment.Center && TicksLabelsRotation != TicksLabelsRotation.Parallel)
                    throw new Exception("Central alignment is only possible for parallel rotation of the tick`s labels.");

            var tickLabelFormatSize = AutomaticTicksCalculation(g);

            SizeNear = TicksLabelsPosition == LabelsPosition.Near ? 6 : 0;
            SizeFar = TicksLabelsPosition == LabelsPosition.Far ? 6 : 0;
            OverflowFar = tickLabelFormatSize.Height;
            OverflowNear = tickLabelFormatSize.Height;

            if (TicksLabelsPosition != LabelsPosition.None && tickLabel.Any())
            {
                if (TicksLabelsPosition == LabelsPosition.Far)
                {
                    if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
                        SizeFar += tickLabelFormatSize.Height;
                    else if (TicksLabelsRotation == TicksLabelsRotation.Perpendicular)
                        SizeFar += tickLabelFormatSize.Width;
                    else
                        SizeFar += tickLabelFormatSize.Width / 1.4f;
                }
                else
                {
                    if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
                        SizeNear += tickLabelFormatSize.Height;
                    else if (TicksLabelsRotation == TicksLabelsRotation.Perpendicular)
                        SizeNear += tickLabelFormatSize.Width;
                    else
                        SizeNear += tickLabelFormatSize.Width / 1.4f;
                }

                if (TicksLabelsAlignment == Alignment.Center)
                {
                    OverflowFar = tickLabelFormatSize.Width / 2.0f;
                    OverflowNear = OverflowFar;
                }
                else if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
                {
                    if (TicksLabelsAlignment == Alignment.Near)
                        OverflowFar = tickLabelFormatSize.Width;
                    else
                        OverflowNear = tickLabelFormatSize.Width;
                }
                else if(TicksLabelsRotation == TicksLabelsRotation.Tilted)
                {
                    if (TicksLabelsAlignment == Alignment.Near)
                        OverflowFar = tickLabelFormatSize.Width / 1.4f;
                    else
                        OverflowNear = tickLabelFormatSize.Width / 1.4f;
                }
            }

            TitleSizeCalculation(g);
        }
        internal void DrawVertical(Graphics g, float x, float y, RectangleF rect)
        {
            drawnRectangle = new RectangleF(x, y, 0, rect.Height);

            if (TicksLabelsPosition == LabelsPosition.Near)
                drawnRectangle.SetLeft(drawnRectangle.Left - SizeNear);
            else if (TicksLabelsPosition == LabelsPosition.Far)
                drawnRectangle.SetRight(drawnRectangle.Right + SizeFar);
            else
                drawnRectangle.Width = 10;

            StringFormat stringFormat = new StringFormat();
            int rotationSign = 1;


            if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
            {
                stringFormat.LineAlignment = (StringAlignment)TicksLabelsLineAlignment;
                stringFormat.Alignment = (StringAlignment)TicksLabelsAlignment;
                if ((TicksLabelsPosition == LabelsPosition.Near && TicksLabelsLineAlignment == Alignment.Near) ||
                    (TicksLabelsPosition == LabelsPosition.Far && TicksLabelsLineAlignment == Alignment.Far))
                    rotationSign = 1;
                else
                    rotationSign = -1;
            }
            else if(TicksLabelsRotation == TicksLabelsRotation.Perpendicular)
            {
                stringFormat.LineAlignment = (StringAlignment)TicksLabelsLineAlignment;
                if (TicksLabelsPosition == LabelsPosition.Near)
                    stringFormat.Alignment = StringAlignment.Far;
                else
                    stringFormat.Alignment = StringAlignment.Near;
            }
            else
            {
                stringFormat.LineAlignment = StringAlignment.Center;
                if (TicksLabelsPosition == LabelsPosition.Near)
                    stringFormat.Alignment = StringAlignment.Far;
                else
                    stringFormat.Alignment = StringAlignment.Near;
                if (TicksLabelsLineAlignment == Alignment.Near)
                    rotationSign = 1;
                else
                    rotationSign = -1;
            }

            float tickLabelPositionX = TicksLabelsPosition == LabelsPosition.Near ? x - 6 : x + 6;

            for (int i = 0; i < tickLabel.Length; i++)
            {
                float ticky = (float)Transform(tick[i]);
                if (GridStyle == GridStyle.Both || GridStyle == GridStyle.Major)
                    g.DrawLine(Pens.Gray, rect.Left, ticky, rect.Right, ticky);

                switch (MajorTickStyle)
                {
                    case TickStyle.Far: g.DrawLine(pen, x, ticky, x + 5, ticky); break;
                    case TickStyle.Near: g.DrawLine(pen, x, ticky, x - 5, ticky); break;
                    case TickStyle.Cross: g.DrawLine(pen, x + 5, ticky, x - 5, ticky); break;
                    case TickStyle.None: break;
                }

                if (TicksLabelsPosition != LabelsPosition.None)
                {
                    if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
                    {
                        g.TranslateTransform(tickLabelPositionX, ticky);
                        g.RotateTransform(rotationSign * 90);
                        g.TranslateTransform(-tickLabelPositionX, -ticky);
                        g.DrawString(tickLabel[i], Font, brush, tickLabelPositionX, ticky, stringFormat);
                        g.ResetTransform();
                    }
                    else if (TicksLabelsRotation == TicksLabelsRotation.Tilted)
                    {
                        g.TranslateTransform(tickLabelPositionX, ticky);
                        g.RotateTransform(rotationSign * 45);
                        g.TranslateTransform(-tickLabelPositionX, -ticky);
                        g.DrawString(tickLabel[i], Font, brush, tickLabelPositionX, ticky, stringFormat);
                        g.ResetTransform();
                    }
                    else
                        g.DrawString(tickLabel[i], Font, brush, tickLabelPositionX, ticky, stringFormat);
                }
            }
            for (int i = 0; i < subTicks.Length; i++)
            {
                float ticky = (float)Transform(subTicks[i]);
                if (GridStyle == GridStyle.Both || GridStyle == GridStyle.Minor)
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
            g.DrawLine(pen, x, (float)Transform(Minimum), x, (float)Transform(Maximum));

            //Title drawing
            if (!string.IsNullOrWhiteSpace(Title) && TitlePosition != LabelsPosition.None)
            {
                StringFormat titleFormat = new StringFormat();
                float titleX = 0;
                float titleY = 0;
                if (TitlePosition == LabelsPosition.Near)
                {
                    titleFormat.LineAlignment = StringAlignment.Near;
                    if (TitleAlignment == Alignment.Center)
                    {
                        titleFormat.Alignment = StringAlignment.Center;
                        titleX = x - SizeNear;
                        titleY = y + rect.Height / 2;
                    }
                    else if (TitleAlignment == Alignment.Near)
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
                else if (TitlePosition == LabelsPosition.Far)
                {
                    titleFormat.LineAlignment = StringAlignment.Far;
                    if (TitleAlignment == Alignment.Center)
                    {
                        titleX = x + SizeFar;
                        titleY = y + rect.Height / 2;
                    }
                    else if (TitleAlignment == Alignment.Near)
                    {
                        titleFormat.Alignment = StringAlignment.Far;
                        titleX = x + SizeFar;
                        titleY = y;
                    }
                    else if (TitleAlignment == Alignment.Far)
                    {
                        titleFormat.Alignment = StringAlignment.Near;
                        titleX = x + SizeFar;
                        titleY = y + rect.Height;
                    }
                }

                if(rotationSign > 0)
                {
                    if (titleFormat.Alignment == StringAlignment.Far)
                        titleFormat.Alignment = StringAlignment.Near;
                    else if(titleFormat.Alignment == StringAlignment.Near)
                        titleFormat.Alignment = StringAlignment.Far;

                    if (titleFormat.LineAlignment == StringAlignment.Far)
                        titleFormat.LineAlignment = StringAlignment.Near;
                    else if (titleFormat.LineAlignment == StringAlignment.Near)
                        titleFormat.LineAlignment = StringAlignment.Far;
                }

                g.TranslateTransform(titleX, titleY);
                g.RotateTransform(rotationSign * 90);
                g.TranslateTransform(-titleX, -titleY);
                g.DrawString(Title, Font, brush, titleX, titleY, titleFormat);
                g.ResetTransform();
            }
            //end of Vertical drawing
        }
        internal void DrawHorizontal(Graphics g, float x, float y, RectangleF rect)
        {
            drawnRectangle = new RectangleF(x, y, rect.Width, 0);

            if (TicksLabelsPosition == LabelsPosition.Far)
                drawnRectangle.SetBottom(drawnRectangle.Bottom + SizeFar);
            else if (TicksLabelsPosition == LabelsPosition.Near)
                drawnRectangle.SetTop(drawnRectangle.Top - SizeNear);
            else
                drawnRectangle.Height = 10;

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = (StringAlignment)TicksLabelsAlignment;

            if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
            {
                if (TicksLabelsPosition == LabelsPosition.Far)
                    stringFormat.LineAlignment = StringAlignment.Near;
                else
                    stringFormat.LineAlignment = StringAlignment.Far;
            }
            else if(TicksLabelsRotation == TicksLabelsRotation.Perpendicular)
                stringFormat.LineAlignment = (StringAlignment)TicksLabelsLineAlignment;
            else 
                stringFormat.LineAlignment = StringAlignment.Center;

            int rotationSign = -1;
            if (TicksLabelsPosition == LabelsPosition.Far && TicksLabelsAlignment == Alignment.Near ||
                TicksLabelsPosition == LabelsPosition.Near && TicksLabelsAlignment == Alignment.Far)
                rotationSign = 1;

            float tickLabelPositionY = TicksLabelsPosition == LabelsPosition.Far ? y + 6 : y - 6;

            for (int i = 0; i < tickLabel.Length; i++)
            {
                float tickx = (float)Transform(tick[i]);
                if (GridStyle == GridStyle.Both || GridStyle == GridStyle.Major)
                    g.DrawLine(Pens.Gray, tickx, rect.Top, tickx, rect.Bottom);

                switch (MajorTickStyle)
                {
                    case TickStyle.Near: g.DrawLine(pen, tickx, y, tickx, y - 5); break;
                    case TickStyle.Far: g.DrawLine(pen, tickx, y, tickx, y + 5); break;
                    case TickStyle.Cross: g.DrawLine(pen, tickx, y + 5, tickx, y - 5); break;
                    case TickStyle.None: break;
                }
                if (TicksLabelsPosition != LabelsPosition.None)
                {

                    if (TicksLabelsRotation == TicksLabelsRotation.Perpendicular)
                    {
                        g.TranslateTransform(tickx, tickLabelPositionY);
                        g.RotateTransform(rotationSign * 90);
                        g.TranslateTransform(-tickx, -tickLabelPositionY);
                        g.DrawString(tickLabel[i], Font, brush, tickx, tickLabelPositionY, stringFormat);
                        g.ResetTransform();
                    }
                    else if (TicksLabelsRotation == TicksLabelsRotation.Tilted)
                    {
                        g.TranslateTransform(tickx, tickLabelPositionY);
                        g.RotateTransform(rotationSign * 45);
                        g.TranslateTransform(-tickx, -tickLabelPositionY);
                        g.DrawString(tickLabel[i], Font, brush, tickx, tickLabelPositionY, stringFormat);
                        g.ResetTransform();
                    }
                    else
                        g.DrawString(tickLabel[i], Font, brush, tickx, tickLabelPositionY, stringFormat);
                }
            }
            for (int i = 0; i < subTicks.Length; i++)
            {
                float tickx = (float)Transform(subTicks[i]);
                if (GridStyle == GridStyle.Both || GridStyle == GridStyle.Minor)
                    g.DrawLine(Pens.Silver, tickx, rect.Top, tickx, rect.Bottom);

                switch (MinorTickStyle)
                {
                    case TickStyle.Near: g.DrawLine(pen, tickx, y, tickx, y - 2); break;
                    case TickStyle.Far: g.DrawLine(pen, tickx, y, tickx, y + 2); break;
                    case TickStyle.Cross: g.DrawLine(pen, tickx, y + 2, tickx, y - 2); break;
                    case TickStyle.None: break;
                }
            }

            //Axis line
            g.DrawLine(pen, (float)Transform(Minimum), y, (float)Transform(Maximum), y);

            //Title drawing
            if (!string.IsNullOrWhiteSpace(Title) && TitlePosition != LabelsPosition.None)
            {
                StringFormat titleFormat = new StringFormat();
                if (TitlePosition == LabelsPosition.Near)
                {
                    titleFormat.LineAlignment = StringAlignment.Near;
                    if (TitleAlignment == Alignment.Center)
                    {
                        titleFormat.Alignment = StringAlignment.Center;
                        g.DrawString(Title, Font, brush, x + rect.Width / 2, y - SizeNear, titleFormat);
                    }
                    else if (TitleAlignment == Alignment.Near)
                    {
                        titleFormat.Alignment = StringAlignment.Near;
                        g.DrawString(Title, Font, brush, x, y - SizeNear, titleFormat);
                    }
                    else
                    {
                        titleFormat.Alignment = StringAlignment.Far;
                        g.DrawString(Title, Font, brush, x + rect.Width, y - SizeNear, titleFormat);
                    }
                }
                else if (TitlePosition == LabelsPosition.Far)
                {
                    titleFormat.LineAlignment = StringAlignment.Far;
                    if (TitleAlignment == Alignment.Center)
                    {
                        g.DrawString(Title, Font, brush, x + rect.Width / 2, y + SizeFar, titleFormat);
                    }
                    else if (TitleAlignment == Alignment.Near)
                    {
                        titleFormat.Alignment = StringAlignment.Near;
                        g.DrawString(Title, Font, brush, x, y + SizeFar, titleFormat);
                    }
                    else if (TitleAlignment == Alignment.Far)
                    {
                        titleFormat.Alignment = StringAlignment.Far;
                        g.DrawString(Title, Font, brush, x + rect.Width, y + SizeFar, titleFormat);
                    }
                }
            }
            //end of Horizontal drawing
        }

    }

    /// <summary>
    /// Enumerates the available axis positions on a plot.
    /// </summary>
    public enum AxisPosition
    {
        /// <summary>
        /// Closer to the lower-right corner.
        /// </summary>
        Near,
        /// <summary>
        /// Located in the center of the plot.
        /// </summary>
        Center,
        /// <summary>
        /// Closer to the upper-left corner.
        /// </summary>
        Far,
        /// <summary>
        /// Cross the perpendicular axis at the <see cref="Axis.CrossValue"/> point.
        /// </summary>
        CrossValue
    }
    /// <summary>
    /// Enumerates the available ticks style.
    /// </summary>
    public enum TickStyle
    {
        /// <summary>
        /// Ticks are directed to the top or to the left.
        /// </summary>
        Near,
        /// <summary>
        /// Ticks are directed to the bottom or to the right.
        /// </summary>
        Far,
        /// <summary>
        /// Ticks cross the axis.
        /// </summary>
        Cross,
        /// <summary>
        /// Do not display ticks.
        /// </summary>
        None
    }
    /// <summary>
    /// Enumerates the available labels positions.
    /// </summary>
    public enum LabelsPosition
    {
        /// <summary>
        /// Closer to the lower-right corner.
        /// </summary>
        Near,
        /// <summary>
        /// Closer to the upper-left corner.
        /// </summary>
        Far,
        /// <summary>
        /// Do not display label.
        /// </summary>
        None
    }
    /// <summary>
    /// Enumerates the available alignments.
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// Closer to the lower-right corner.
        /// </summary>
        Near = 0,
        /// <summary>
        /// Aligned to the center
        /// </summary>
        Center = 1,
        /// <summary>
        /// Closer to the upper-left corner.
        /// </summary>
        Far = 2
    }
    /// <summary>
    /// Enumerates the available tick labels rotation.
    /// </summary>
    public enum TicksLabelsRotation
    {
        /// <summary>
        /// Tick labels is paralel to the axis.
        /// </summary>
        Parallel,
        /// <summary>
        /// Tick labels is rotated 45 degrees relative to the axis.
        /// </summary>
        Tilted,
        /// <summary>
        /// Tick labels is perpendicular to the axis.
        /// </summary>
        Perpendicular
    }
    /// <summary>
    /// Enumerates the available grid styles.
    /// </summary>
    public enum GridStyle
    {
        /// <summary>
        /// Do not display any grid.
        /// </summary>
        None,
        /// <summary>
        /// Display only major grid.
        /// </summary>
        Major,
        /// <summary>
        /// Display only minor grid.
        /// </summary>
        Minor,
        /// <summary>
        /// Display all grid.
        /// </summary>
        Both
    }

}

