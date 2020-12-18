using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Diagnostics;
using OMPlot.Data;

namespace OMPlot
{
	public partial class Plot: UserControl
	{
		int mouseX, mouseY;
		int currentmouseX = -1;
		int currentmouseY = -1;

		RectangleExtended plotRectangle;

		string title;
		bool showLegend;

		public List<IData> Data;

		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		Dictionary<string, Axis> vertical;
		Dictionary<string, Axis> Horizontal;

		bool selectHorizontal, selectVertical;
		string selectedAxisName;

		public Plot()
		{
			InitializeComponent();
			Data = new List<IData>();
			vertical = new Dictionary<string, Axis>();
			Horizontal = new Dictionary<string, Axis>();
			this.MouseWheel += Analog_MouseWheel;
		}

		public void AddVerticalAxis(Axis axis)
		{
			if (axis != null)
			{
				if (string.IsNullOrEmpty(axis.Title))
					axis.Title = axis.GetHashCode().ToString();
				if(axis.Font == null)
					axis.Font = this.Font;
				axis.Vertical = true;
				vertical.Add(axis.Title, axis);
			}
		}

		public void AddHorizontalAxis(Axis axis)
		{
			if (axis != null)
			{
				if (string.IsNullOrEmpty(axis.Title))
					axis.Title = axis.GetHashCode().ToString();
				if (axis.Font == null)
					axis.Font = this.Font;
				Horizontal.Add(axis.Title, axis);
			}
		}

		public Axis GetVerticalAxis(string name = "")
		{
			if (vertical.Any())
			{
				if (string.IsNullOrEmpty(name))
					return vertical.First().Value;
				return vertical[name];
			}
			return null;
		}

		public Axis GetHorizontalAxis(string name = "")
		{
			if (Horizontal.Any())
			{
				if (string.IsNullOrEmpty(name))
					return Horizontal.First().Value;
				return Horizontal[name];
			}
			return null;
		}

		private void Analog_MouseWheel(object sender, MouseEventArgs e)
		{
			float zoom = 100 / (float)e.Delta;
			if (zoom < 0)
				zoom = -1 / zoom;
			if(plotRectangle.InRectangle(e.X, e.Y))
			{
				foreach(var axis in Horizontal)
					axis.Value.Zoom(zoom, e.X - plotRectangle.CenterX);
				foreach (var axis in vertical)
					axis.Value.Zoom(zoom, e.Y - plotRectangle.CenterY);
			}
			else
			{
				foreach (var axis in Horizontal.Where(axis => axis.Value.ActionOnAxis(e.X, e.Y)))
					axis.Value.Zoom(zoom, e.X - plotRectangle.CenterX);
				foreach (var axis in vertical.Where(axis => axis.Value.ActionOnAxis(e.X, e.Y)))
					axis.Value.Zoom(zoom, e.Y - plotRectangle.CenterY);
			}
			this.Refresh();
		}

		private void Analog_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
			{
				selectHorizontal = false;
				selectVertical = false;
				if (plotRectangle.InRectangle(e.X, e.Y))
				{
					selectHorizontal = true;
					selectVertical = true;
				}
				else
				{
					var axisSelected = Horizontal.Where(axis => axis.Value.ActionOnAxis(e.X, e.Y));
					selectHorizontal = axisSelected.Any();
					if(selectHorizontal)
						selectedAxisName = axisSelected.First().Key;
					axisSelected = vertical.Where(axis => axis.Value.ActionOnAxis(e.X, e.Y));
					selectVertical = axisSelected.Any();
					if (selectVertical)
						selectedAxisName = axisSelected.First().Key;
				}
				mouseX = e.X;
				mouseY = e.Y;
			}
		}

		private void Analog_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (selectHorizontal && selectVertical)
				{
					foreach (var axis in Horizontal)
						axis.Value.Move(mouseX - e.X);
					foreach (var axis in vertical)
						axis.Value.Move(mouseY - e.Y);
				}
				else
				{
					if (selectHorizontal)
					{
						var axis = GetHorizontalAxis(selectedAxisName);
						if (axis != null)
							axis.Move(mouseX - e.X);
					}
					if (selectVertical)
					{
						var axis = GetVerticalAxis(selectedAxisName);
						if (axis != null)
							axis.Move(mouseY - e.Y);
					}
				}
				mouseX = e.X;
				mouseY = e.Y;
				this.Refresh();
			}
			else if (e.Button == MouseButtons.Middle)
			{
				currentmouseX = e.X;
				currentmouseY = e.Y;							
				this.Refresh();
			}
		}

		private void Analog_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				if (selectHorizontal && selectVertical)
				{
					foreach (var axis in Horizontal)
						axis.Value.Select((int)Math.Min(mouseX, currentmouseX), (int)Math.Max(mouseX, currentmouseX));
					foreach (var axis in vertical)
						axis.Value.Select((int)Math.Max(mouseY, currentmouseY), (int)Math.Min(mouseY, currentmouseY));
				}
				else
				{
					if (selectHorizontal)
					{
						var axis = GetHorizontalAxis(selectedAxisName);
						if (axis != null)
							axis.Select((int)Math.Min(mouseX, currentmouseX), (int)Math.Max(mouseX, currentmouseX));
					}
					if (selectVertical)
					{
						var axis = GetVerticalAxis(selectedAxisName);
						if (axis != null)
							axis.Select((int)Math.Max(mouseY, currentmouseY), (int)Math.Min(mouseY, currentmouseY));
					}
				}
				currentmouseX = -1;
				currentmouseY = -1;
				this.Refresh();
			}
		}

		public void ControlPaint(Graphics g)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			Font titleFont = new Font(this.Font.FontFamily, this.Font.Size + 4, this.Font.Style, this.Font.Unit, this.Font.GdiCharSet, this.Font.GdiVerticalFont);

			Brush backgroungBrush = new SolidBrush(this.BackColor);
			Brush mainBrush = new SolidBrush(this.ForeColor);
			Pen mainPen = new Pen(this.ForeColor);
			
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			g.Clear(this.BackColor);

			plotRectangle = new RectangleExtended(6, 6, this.Width - 13, this.Height - 13);
			
			if(!string.IsNullOrEmpty(title))
				plotRectangle.Top += g.MeasureString(title, titleFont).Height;

			foreach(var axis in vertical)
			{				
				axis.Value.MeasureVertical(g, this.Size);
				if(axis.Value.Position == AxisPosition.Near)
					plotRectangle.Left += axis.Value.SizeNear;
				if (axis.Value.Position == AxisPosition.Far)
					plotRectangle.Right -= axis.Value.SizeFar;
			}
			foreach (var axis in Horizontal)
			{
				axis.Value.MeasureHorizontal(g, this.Size);
				if (axis.Value.Position == AxisPosition.Near)
					plotRectangle.Top += axis.Value.SizeNear;
				if (axis.Value.Position == AxisPosition.Far)
					plotRectangle.Bottom -= axis.Value.SizeFar;
			}

			foreach (var axis in vertical)
			{
				plotRectangle.Top = plotRectangle.Top > axis.Value.OverflowNear ? plotRectangle.Top : axis.Value.OverflowNear;
				plotRectangle.Bottom = (this.Height - plotRectangle.Bottom) > axis.Value.OverflowFar ? plotRectangle.Bottom : (this.Height - axis.Value.OverflowFar);
			}
			foreach (var axis in Horizontal)
			{
				plotRectangle.Left = plotRectangle.Left > axis.Value.OverflowNear ? plotRectangle.Left : axis.Value.OverflowNear;
				plotRectangle.Right = (this.Width - plotRectangle.Right) > axis.Value.OverflowFar ? plotRectangle.Right : (this.Width - axis.Value.OverflowFar);
			}

			foreach (var axis in vertical)
			{
				axis.Value.CalculateTranformVertical(plotRectangle.Top, plotRectangle.Bottom);
			}
			foreach (var axis in Horizontal)
			{
				axis.Value.CalculateTranformHorizontal(plotRectangle.Left, plotRectangle.Right);
			}

			if (vertical.Any() && Horizontal.Any())
			{
				int plotIndex = 0;
				foreach (var data in Data)
					data.Draw(g, GetVerticalAxis(data.AxisVerticalName), GetHorizontalAxis(data.AxisHorizontalName), plotRectangle, plotIndex++);
			}
			g.ResetTransform();

			g.FillRectangle(backgroungBrush, -1, -1, this.Width + 2, plotRectangle.Top);
			g.FillRectangle(backgroungBrush, -1, plotRectangle.Top, plotRectangle.Left, plotRectangle.Height + 2);
			g.FillRectangle(backgroungBrush, plotRectangle.Right + 1, plotRectangle.Top, this.Width - plotRectangle.Right, plotRectangle.Height + 2);
			g.FillRectangle(backgroungBrush, -1, plotRectangle.Bottom + 1, this.Width + 2, this.Height - plotRectangle.Bottom);
			//if()
			g.DrawRectangle(mainPen, plotRectangle.X, plotRectangle.Y, plotRectangle.Width, plotRectangle.Height);

			if (!string.IsNullOrEmpty(title))
			{
				StringFormat stringFormat = new StringFormat { Alignment = StringAlignment.Center };
				g.DrawString(title, titleFont, mainBrush, this.Width / 2.0f, 0, stringFormat);
			}

			float rightAxisPosition = plotRectangle.Right;
			float leftAxisPosition = plotRectangle.Left;
			foreach (var axis in vertical)
			{
				switch(axis.Value.Position)
				{
					case AxisPosition.Near:
						{
							axis.Value.DrawVertical(g, leftAxisPosition, plotRectangle.Top, plotRectangle);
							leftAxisPosition -= axis.Value.SizeNear;
							break;
						}
					case AxisPosition.Far:
						{
							axis.Value.DrawVertical(g, rightAxisPosition, plotRectangle.Top, plotRectangle);
							rightAxisPosition += axis.Value.SizeFar;
							break;
						}
					case AxisPosition.Center:
						axis.Value.DrawVertical(g, plotRectangle.CenterX, plotRectangle.Top, plotRectangle);
						break;
					case AxisPosition.CrossValue: //Name of Horizontal add
						{
							if (axis.Value.CrossValue < Horizontal.First().Value.Minimum)
							{
								axis.Value.DrawVertical(g, leftAxisPosition, plotRectangle.Top, plotRectangle);
								leftAxisPosition -= axis.Value.SizeNear;
							}
							else if (axis.Value.CrossValue > Horizontal.First().Value.Maximum)
							{
								axis.Value.DrawVertical(g, rightAxisPosition, plotRectangle.Top, plotRectangle);
								rightAxisPosition += axis.Value.SizeFar;
							}
							else
								axis.Value.DrawVertical(g, (int)Horizontal.First().Value.Transform(axis.Value.CrossValue), plotRectangle.Top, plotRectangle);
							break;
						}
				}                
			}
			float topAxisPosition = plotRectangle.Top;
			float bottomAxisPosition = plotRectangle.Bottom;
			foreach (var axis in Horizontal)
			{
				switch (axis.Value.Position)
				{
					case AxisPosition.Far:
						{
							axis.Value.DrawHorizontal(g, plotRectangle.Left, bottomAxisPosition, plotRectangle);
							bottomAxisPosition += axis.Value.SizeNear;
							break;
						}
					case AxisPosition.Near:
						{
							axis.Value.DrawHorizontal(g, plotRectangle.Left, topAxisPosition, plotRectangle);
							topAxisPosition -= axis.Value.SizeFar;
							break;
						}
					case AxisPosition.Center:
						axis.Value.DrawHorizontal(g, plotRectangle.Left, plotRectangle.CenterY, plotRectangle);
						break;
					case AxisPosition.CrossValue:
						{
							if (axis.Value.CrossValue < vertical.First().Value.Minimum)
							{
								axis.Value.DrawHorizontal(g, plotRectangle.Left, bottomAxisPosition, plotRectangle);
								bottomAxisPosition += axis.Value.SizeNear;
							}
							else if (axis.Value.CrossValue > vertical.First().Value.Maximum)
							{ 
								axis.Value.DrawHorizontal(g, plotRectangle.Left, topAxisPosition, plotRectangle);
								topAxisPosition -= axis.Value.SizeFar;
							}
							else
								axis.Value.DrawHorizontal(g, plotRectangle.Left, (int)vertical.First().Value.Transform(axis.Value.CrossValue), plotRectangle);
							break;
						}
				}
			}

			if (currentmouseX >= 0 && currentmouseY >= 0 && (selectVertical || selectHorizontal))
			{
				Rectangle selectionRec = new Rectangle();
				if (selectVertical && selectHorizontal)
					selectionRec = new Rectangle(Math.Min(mouseX, currentmouseX), Math.Min(mouseY, currentmouseY), Math.Abs(mouseX - currentmouseX), Math.Abs(mouseY - currentmouseY));
				else
				{
					if (selectVertical)
						selectionRec = new Rectangle((int)plotRectangle.Left, Math.Min(mouseY, currentmouseY), (int)plotRectangle.Width, Math.Abs(mouseY - currentmouseY));
					if (selectHorizontal)
						selectionRec = new Rectangle(Math.Min(mouseX, currentmouseX), (int)plotRectangle.Top, Math.Abs(mouseX - currentmouseX), (int)plotRectangle.Height);
				}
				Pen selectionPen = new Pen(Color.FromArgb(200, 0, 50, 100));
				Brush selectionBrush = new SolidBrush(Color.FromArgb(100, 0, 50, 100));
				g.DrawRectangle(selectionPen, selectionRec);
				g.FillRectangle(selectionBrush, selectionRec);
			}

			sw.Stop();

			g.DrawString((1000.0 / (double)(sw.ElapsedMilliseconds > 0 ? sw.ElapsedMilliseconds : 1)).ToString("#.###"), this.Font, mainBrush, 0, 0);
		}
		
		private void Analog_Paint(object sender, PaintEventArgs e)
		{
			ControlPaint(e.Graphics);
		}

		private void Control_Resize(object sender, EventArgs e)
		{
			this.Refresh();
		}


	}
}
