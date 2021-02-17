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
        Brush brush;
        Pen pen;
        Color color;

        RectangleExtended drawnRectangle;

        public int TickNumber;
        public int SubTickNumber;

        private SizeF titleSize;
        private double res;
        private double offset;
        private double[] tick;
        private string[] tickLabel;
        private float[] tickLabelLocation;
        private SizeF[] tickLabelSize;
        private double[] subTicks;
        private string tickLabelFormat;

        public Axis()
        {
            Color = Color.Black;
            TickNumber = 10;
        }

        public double FullScale
        {
            get { return Maximum - Minimum; }
            set
            {
                double center = Center;
                Maximum = center + value / 2.0f;
                Minimum = center - value / 2.0f;
            }
        }
        public double Center
        {
            get { return (Maximum + Minimum) / 2.0f; }
            set
            {
                double delta = value - Center;
                Maximum += delta;
                Minimum += delta;
            }
        }

        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public string Title { get; set; }
        public bool Logarithmic { get; set; }
        public bool Vertical { get; set; }
        public bool Reverse { get; set; }
        public bool MoveLocked { get; set; }
        public bool ZoomLocked { get; set; }

        public AxisPosition Position { get; set; }
        public double CrossValue { get; set; }

        public TickStyle MajorTickStyle { get; set; }
        public TickStyle MinorTickStyle { get; set; }
        public LabelsPosition TicksLabelsPosition { get; set; }
        public Alignment TicksLabelsAlignment { get; set; }
        public Alignment TicksLabelsLineAlignment { get; set; }
        public TicksLabelsRotation TicksLabelsRotation { get; set; }

        public double[] CustomTicks { get; set; }
        public string[] CustomTicksLabels { get; set; }

        public Alignment TitleAlignment { get; set; }
        public LabelsPosition TitlePosition { get; set; }
        public GridStyle GridStyle { get; set; }

        public Font Font { get; set; }
        public Color Color { get { return color; } set { color = value; brush = new SolidBrush(color); pen = new Pen(brush); } }
        
        public float SizeNear { get; private set; }
        public float SizeFar { get; private set; }
        public float OverflowNear { get; private set; }
        public float OverflowFar { get; private set; }

        public void CalculateTranformVertical(float minimum, float maximum)
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
        public void CalculateTranformHorizontal(float minimum, float maximum)
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
        public double TransformBack(double value) { return Logarithmic ? Math.Pow(10, (value - offset) / res) : (value - offset) / res; }
        public double Transform(double value)
        {
            double ret = offset + (Logarithmic ? Math.Log10(value) : value) * res;
            if (ret < -1000)
                return -1000;
            if (ret > 10000)
                return 10000;
            return ret;
        }

        public bool ActionOnAxis(float x, float y) { return drawnRectangle.InRectangle(x, y); }
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
                        for (int j = 0; j < SubTickNumber; j++)
                        {
                            subTick = CustomTicks[i] + (j + 1) * (CustomTicks[i + 1] - CustomTicks[i]) / (SubTickNumber + 1);
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

                    double subStep = step / SubTickNumber;
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
                        for (int i = 0; i < SubTickNumber - 1 && subMark < Maximum; i++)
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


                for (int i = 0; i < SubTickNumber - 1 && subMark >= Minimum; i++)
                {
                    subTickList.Add(subMark);
                    subMark -= 0.1 * mark;
                }
                while (mark < Maximum)
                {
                    mark = Accessories.Pow10(Accessories.FirstSignRound(mark));
                    tickList.Add(mark);
                    subMark = mark + mark;
                    for (int i = 0; i < SubTickNumber - 1 && subMark < Maximum; i++)
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
        public void MeasureVertical(Graphics g, Size plotSize)
        {
            if (TicksLabelsLineAlignment == Alignment.Center && (TicksLabelsRotation == TicksLabelsRotation.Parallel || TicksLabelsRotation == TicksLabelsRotation.Tilted))
                throw new Exception("Central line alignment is not possible for parallel rotation of the tick`s labels.");

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
            while (decreaseTickNumber) ;
           

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
        public void MeasureHorizontal(Graphics g, Size plotSize)
        {
            if (TicksLabelsAlignment == Alignment.Center && TicksLabelsRotation != TicksLabelsRotation.Parallel)
                    throw new Exception("Central alignment is only possible for parallel rotation of the tick`s labels.");

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
        public void DrawVertical(Graphics g, float x, float y, RectangleExtended rect)
        {
            drawnRectangle = new RectangleExtended(x, y, 0, rect.Height);

            if (TicksLabelsPosition == LabelsPosition.Near)
                drawnRectangle.Left -= SizeNear;
            else if (TicksLabelsPosition == LabelsPosition.Far)
                drawnRectangle.Right += SizeFar;
            else
                drawnRectangle.FullScaleX = 10;

            StringFormat stringFormat = new StringFormat();
            int rotationSign = 1;


            if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
            {
                stringFormat.LineAlignment = Alignment2StringAlignment(TicksLabelsLineAlignment);
                stringFormat.Alignment = Alignment2StringAlignment(TicksLabelsAlignment);
                if ((TicksLabelsPosition == LabelsPosition.Near && TicksLabelsLineAlignment == Alignment.Near) ||
                    (TicksLabelsPosition == LabelsPosition.Far && TicksLabelsLineAlignment == Alignment.Far))
                    rotationSign = 1;
                else
                    rotationSign = -1;
            }
            else if(TicksLabelsRotation == TicksLabelsRotation.Perpendicular)
            {
                stringFormat.LineAlignment = Alignment2StringAlignment(TicksLabelsLineAlignment);
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
            //end of Horizontal drawing
        }
        public void DrawHorizontal(Graphics g, float x, float y, RectangleExtended rect)
        {
            drawnRectangle = new RectangleExtended(x, y, rect.Width, 0);

            if (TicksLabelsPosition == LabelsPosition.Far)
                drawnRectangle.Bottom += SizeFar;
            else if (TicksLabelsPosition == LabelsPosition.Near)
                drawnRectangle.Top -= SizeNear;
            else
                drawnRectangle.FullScaleY = 10;

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = Alignment2StringAlignment(TicksLabelsAlignment);

            if (TicksLabelsRotation == TicksLabelsRotation.Parallel)
            {
                if (TicksLabelsPosition == LabelsPosition.Far)
                    stringFormat.LineAlignment = StringAlignment.Near;
                else
                    stringFormat.LineAlignment = StringAlignment.Far;
            }
            else if(TicksLabelsRotation == TicksLabelsRotation.Perpendicular)
                stringFormat.LineAlignment = Alignment2StringAlignment(TicksLabelsLineAlignment);
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

