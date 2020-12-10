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
		float minimum;
		float maximum;

		string title;
		bool logarithmic;
		bool vertical;
		bool reverse;
		bool zoomLocked;
		bool moveLocked;
		TickStyle tickLocation;
		AxisPosition axisPosition;
		float crossValue;
		TicksLabelsLineAlignment ticksLabelsLineAlignment;
		LabelAlignment labelAlignment;
		GridStyle gridStyle;

		Font font;
		Brush brush;
		Pen pen;
		Color color;

		float heightNear;
		float heightFar;
		float overflowNear;
		float overflowFar;

		RectangleExtended drawnRectangle;

		public int TickNumber;
		public int SubTickNumber;


		public float HeightNear { get { return heightNear; } }
		public float HeightFar { get { return heightFar; } }
		public float OverflowNear { get { return overflowNear; } }
		public float OverflowFar { get { return overflowFar; } }
		

		private SizeF titleSize;
		private float res;
		private float offset;
		private float[] tick;
		private string[] tickLabel;
		private float[] subTick;

		public Axis()
		{
			Color = Color.Black;
			TickNumber = 10;
		}


		public float FullScale
		{
			get { return maximum - minimum; }
			set
			{
				float center = Center;
				maximum = center + value / 2.0f;
				minimum = center - value / 2.0f;
			}
		}

		public float Center
		{
			get { return (maximum + minimum) / 2.0f; }
			set
			{
				float delta = value - Center;
				maximum += delta;
				minimum += delta;
			}
		}

		public float Minimum { get { return minimum; } set { minimum = value; } }
		public float Maximum { get { return maximum; } set { maximum = value; } }

		public float Resolution { get { return res; } }

		public string Title { get { return title; } set { title = value; } }
		public bool Logarithmic { get { return logarithmic; } set { logarithmic = value; } }
		public bool Vertical { get { return vertical; } set { vertical = value; } }
		public bool Reverse { get { return reverse; } set { reverse = value; } }
		public bool MoveLocked { get { return moveLocked; } set { moveLocked = value; } }
		public bool ZoomLocked { get { return zoomLocked; } set { zoomLocked = value; } }
		public AxisPosition Position { get { return axisPosition; } set { axisPosition = value; } }
		public float CrossValue { get { return crossValue; } set { crossValue = value; } }
		public TickStyle TickStyle { get { return tickLocation; } set { tickLocation = value; } }
		public TicksLabelsLineAlignment TicksLabelsLineAlignment { get { return ticksLabelsLineAlignment; } set { ticksLabelsLineAlignment = value; } }
		public LabelAlignment LabelAlignment { get { return labelAlignment; } set { labelAlignment = value; } }
		public GridStyle GridStyle { get { return gridStyle; } set { gridStyle = value; } }

		public Font Font { get { return font; } set { font = value; } }
		public Color Color { get { return color; } set { color = value; brush = new SolidBrush(color); pen = new Pen(brush); } }
				
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
					minimum *= (float)Math.Pow(10, length / res);
					maximum *= (float)Math.Pow(10, length / res);
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

		public void Zoom(float zoom, float length)
		{
			if (!zoomLocked)
			{
				if (!logarithmic)
				{
					float halfscale = (maximum - minimum) / 2;
					minimum += (1 - zoom) * (halfscale + length / res);
					maximum -= (1 - zoom) * (halfscale - length / res);
				}
				else
                {
					double halfscale = (Math.Log10(maximum) - Math.Log10(minimum)) / 2;
					minimum = (float)Math.Pow(10, Math.Log10(minimum) + (1 - zoom) * (halfscale + length / res));
					maximum = (float)Math.Pow(10, Math.Log10(maximum) - (1 - zoom) * (halfscale - length / res));
				}
			}
		}

		public void Calculate(Graphics g)
		{
			string tickLabelFormat;

			if (!logarithmic)
			{
				float step = (maximum - minimum) / TickNumber;
				int roundStepSign = Accessories.FirstSignRound(step);
				step = Accessories.Round(step, roundStepSign);

				int maxDegree = int.MinValue;


				List<float> tickList = new List<float>();
				List<float> subTickList = new List<float>();
				float mark = (float)Math.Ceiling(minimum / step) * step;
				float subStep = step / SubTickNumber;
				float subMark = mark - subStep;

				float nextmark = mark + step;

				if (mark - nextmark == 0) //step smaller than floating point resolution
				{
					tick = new float[] { minimum, maximum };
					subTick = new float[] { };
					maxDegree = Accessories.Degree(minimum);
					if (maxDegree < Accessories.Degree(maximum))
						maxDegree = Accessories.Degree(maximum);
				}
				else
				{
					if (SubTickNumber != 0)
					{
						for (int i = 0; i < SubTickNumber - 1 && subMark > Minimum; i++)
						{
							subTickList.Add(subMark);
							subMark -= subStep;
						}
					}
					while (mark <= maximum)
					{
						tickList.Add(mark);
						if (maxDegree < Accessories.Degree(mark))
							maxDegree = Accessories.Degree(mark);
						if (SubTickNumber != 0)
						{
							subMark = mark + step / SubTickNumber;
							for (int i = 0; i < SubTickNumber - 1 && subMark < Maximum; i++)
							{
								subTickList.Add(subMark);
								subMark += subStep;
							}
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
				tick = new float[(int)(Math.Log10(maximum) - Math.Log10(minimum) + 1)];
				subTick = new float[0];

				for (int i = 0; i < tick.Length; i++)
					tick[i] = Accessories.Pow10(i) * minimum;

				tickLabelFormat = "###";
			}

			tickLabel = new string[tick.Length];
			for (int i = 0; i < tick.Length; i++)
				tickLabel[i] = Accessories.ToSI(tick[i], tickLabelFormat);

			titleSize = new SizeF();
			if (!string.IsNullOrEmpty(title))
				titleSize = g.MeasureString(title, this.font);

			heightNear = 6;
			heightFar = 6;
			if (tickLabel.Any())
			{
				if (vertical)
				{
					if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Near)
					{
						heightNear += g.MeasureString('-' + tickLabelFormat, this.font).Width; // + '#'
						heightNear += titleSize.Height;
					}
					else if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Far)
					{
						heightFar += g.MeasureString('-' + tickLabelFormat, this.font).Width; // + '#'
						heightFar += titleSize.Height;
					}
					overflowFar = g.MeasureString(tickLabel.Last(), this.font).Height / 2.0f;
					overflowNear = g.MeasureString(tickLabel.First(), this.font).Height / 2.0f;
				}
				else
				{
					if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Near)
					{
						heightNear += g.MeasureString('-' + tickLabelFormat, this.font).Height; // + '#'
						heightNear += titleSize.Height;
					}
					else if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Far)
					{
						heightFar += g.MeasureString('-' + tickLabelFormat, this.font).Height; // + '#'
						heightFar += titleSize.Height;
					}
					overflowFar = g.MeasureString('-' + tickLabelFormat, this.font).Width / 2.0f; // + '#'
					overflowNear = overflowFar;
				}
			}
		}

		public void DrawVertical(Graphics g, float x, float y, RectangleExtended rect)
		{
			drawnRectangle = new RectangleExtended(x, y, 0, rect.Height);
			if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Near)
				drawnRectangle.Left -= heightNear;
			else if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Far)
				drawnRectangle.Right += heightFar;
			else
				drawnRectangle.FullScaleX = 10;
			StringFormat stringFormat = new StringFormat();
			if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Near)
			{
				stringFormat.Alignment = StringAlignment.Far;
				stringFormat.LineAlignment = StringAlignment.Center;
			}
			else if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Far)
			{
				stringFormat.Alignment = StringAlignment.Near;
				stringFormat.LineAlignment = StringAlignment.Center;
			}
			for (int i = 0; i < tickLabel.Length; i++)
			{
				float ticky = Transform(tick[i]);
				if (gridStyle == GridStyle.Both || gridStyle == GridStyle.Major)
					g.DrawLine(Pens.Gray, rect.Left, ticky, rect.Right, ticky);
				switch (tickLocation)
				{
					case TickStyle.Near: g.DrawLine(pen, x, ticky, x - 5, ticky); drawnRectangle.Left -= 5; break;
					case TickStyle.Far: g.DrawLine(pen, x, ticky, x + 5, ticky); drawnRectangle.Right += 5; break;
					case TickStyle.Cross: g.DrawLine(pen, x + 5, ticky, x - 5, ticky); drawnRectangle.FullScaleX += 10; break;
					case TickStyle.None: break;
				}
				if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Near)
					g.DrawString(tickLabel[i], this.font, brush, x - 6, ticky, stringFormat);
				else if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Far)
					g.DrawString(tickLabel[i], this.font, brush, x + 6, ticky, stringFormat);
			}
			for (int i = 0; i < subTick.Length; i++)
			{
				float ticky = Transform(subTick[i]);
				if (gridStyle == GridStyle.Both || gridStyle == GridStyle.Minor)
					g.DrawLine(Pens.Silver, rect.Left, ticky, rect.Right, ticky);
				switch (tickLocation)
				{
					case TickStyle.Near: g.DrawLine(pen, x, ticky, x - 2, ticky); break;
					case TickStyle.Far: g.DrawLine(pen, x, ticky, x + 2, ticky); break;
					case TickStyle.Cross: g.DrawLine(pen, x + 2, ticky, x - 2, ticky); break;
					case TickStyle.None: break;
				}
			}
			g.DrawLine(pen, x, Transform(minimum), x, Transform(maximum));
			if (!string.IsNullOrEmpty(title))
			{
				stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
				if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Near)
				{
					stringFormat.LineAlignment = StringAlignment.Near;
					switch (labelAlignment)
					{
						case LabelAlignment.Center: g.DrawString(title, this.font, brush, x - HeightNear, y + rect.Height / 2, stringFormat); break;
						case LabelAlignment.Near:
							{
								stringFormat.Alignment = StringAlignment.Far;
								g.DrawString(title, this.font, brush, x - HeightNear, y + rect.Height, stringFormat);
								break;
							}
						case LabelAlignment.Far:
							{
								stringFormat.Alignment = StringAlignment.Near;
								g.DrawString(title, this.font, brush, x - HeightNear, y, stringFormat);
								break;
							}
					}
				}
				else if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Far)
				{
					stringFormat.LineAlignment = StringAlignment.Far;
					switch (labelAlignment)
					{
						case LabelAlignment.Center:
							g.DrawString(title, this.font, brush, x + HeightFar, y + rect.Height / 2, stringFormat);
							break;
						case LabelAlignment.Near:
							{
								stringFormat.Alignment = StringAlignment.Far;
								g.DrawString(title, this.font, brush, x + HeightFar, y + rect.Height, stringFormat);
								break;
							}
						case LabelAlignment.Far:
							{
								stringFormat.Alignment = StringAlignment.Near;
								g.DrawString(title, this.font, brush, x + HeightFar, y, stringFormat);
								break;
							}
					}
				}
			}
		}
		public void DrawHorisontal(Graphics g, float x, float y, RectangleExtended rect)
		{
			drawnRectangle = new RectangleExtended(x, y, rect.Width, 0);
			if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Near)
				drawnRectangle.Bottom += heightNear;
			else if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Far)
				drawnRectangle.Top -= heightFar;
			else
				drawnRectangle.FullScaleY = 10;
			StringFormat stringFormat = new StringFormat { Alignment = StringAlignment.Center };
			if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Far)
				stringFormat.LineAlignment = StringAlignment.Far;
			for (int i = 0; i < tickLabel.Length; i++)
			{
				float tickx = Transform(tick[i]);
				if (gridStyle == GridStyle.Both || gridStyle == GridStyle.Major)
					g.DrawLine(Pens.Gray, tickx, rect.Top, tickx, rect.Bottom);

				switch (tickLocation)
				{
					case TickStyle.Near: g.DrawLine(pen, tickx, y, tickx, y + 5); break;
					case TickStyle.Far: g.DrawLine(pen, tickx, y, tickx, y - 5); break;
					case TickStyle.Cross: g.DrawLine(pen, tickx, y + 5, tickx, y - 5); break;
					case TickStyle.None: break;
				}
				if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Near)
					g.DrawString(tickLabel[i], this.font, brush, tickx, y + 6, stringFormat);
				else if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Far)
					g.DrawString(tickLabel[i], this.font, brush, tickx, y - 6, stringFormat);
			}
			for (int i = 0; i < subTick.Length; i++)
			{
				float tickx = Transform(subTick[i]);
				if (gridStyle == GridStyle.Both || gridStyle == GridStyle.Minor)
					g.DrawLine(Pens.Silver, tickx, rect.Top, tickx, rect.Bottom);

				switch (tickLocation)
				{
					case TickStyle.Near: g.DrawLine(pen, tickx, y, tickx, y + 2); break;
					case TickStyle.Far: g.DrawLine(pen, tickx, y, tickx, y - 2); break;
					case TickStyle.Cross: g.DrawLine(pen, tickx, y + 2, tickx, y - 2); break;
					case TickStyle.None: break;
				}
			}
			g.DrawLine(pen, Transform(minimum), y, Transform(maximum), y);
			if (!string.IsNullOrEmpty(title))
			{
				if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Near)
				{
					stringFormat.LineAlignment = StringAlignment.Far;
					switch (labelAlignment)
					{
						case LabelAlignment.Center:
							g.DrawString(title, this.font, brush, x + rect.Width / 2, y + HeightNear, stringFormat);
							break;
						case LabelAlignment.Near:
							{
								stringFormat.Alignment = StringAlignment.Near;
								g.DrawString(title, this.font, brush, x, y + HeightNear, stringFormat);
								break;
							}
						case LabelAlignment.Far:
							{
								stringFormat.Alignment = StringAlignment.Far;
								g.DrawString(title, this.font, brush, x + rect.Width, y + HeightNear, stringFormat);
								break;
							}
					}
				}
				else if (ticksLabelsLineAlignment == TicksLabelsLineAlignment.Far)
				{
					stringFormat.LineAlignment = StringAlignment.Near;
					switch (labelAlignment)
					{
						case LabelAlignment.Center:
							g.DrawString(title, this.font, brush, x + rect.Width / 2, y - HeightFar, stringFormat);
							break;
						case LabelAlignment.Near:
							{
								stringFormat.Alignment = StringAlignment.Near;
								g.DrawString(title, this.font, brush, x, y - HeightFar, stringFormat);
								break;
							}
						case LabelAlignment.Far:
							{
								stringFormat.Alignment = StringAlignment.Far;
								g.DrawString(title, this.font, brush, x + rect.Width, y - HeightFar, stringFormat);
								break;
							}
					}
				}
			}
		}
		public void Draw(Graphics g, float x, float y, RectangleExtended rect)
		{
			if (vertical)
				DrawVertical(g, x, y, rect);
			else //Horisontal
				DrawHorisontal(g, x, y, rect);
		}
		
		public void CalculateTranform(float minimum, float maximum)
		{
			if (!logarithmic)
			{
				if (vertical ^ reverse)
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
				if (vertical ^ reverse)
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

		float TransformBack(float value)
		{
			if(!logarithmic)
				return (value - offset) / res;
			return (float)Math.Pow(10, (value - offset) / res);
		}

		public float Transform(float value)
		{
			float transformed = offset + (logarithmic ? (float)Math.Log10(value) : value) * res;
			if (transformed < -100)
				return -100;
			if (transformed > 10000)
				return 10000;
			return transformed;
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
