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
    /// <summary>
    /// Represents the main class of the OMPlot.
    /// </summary>
    public partial class Plot : UserControl
    {
        int mouseX, mouseY;
        int currentmouseX = -1;
        int currentmouseY = -1;

        bool selectHorizontal, selectVertical;
        string selectedAxisName;

        Pen selectionPen, mainPen;
        Brush selectionBrush, legendBoxBrush, backgroungBrush, mainBrush;
        Font titleFont;

        Color[] defaultPlotColors = new Color[] { Color.FromArgb(35, 80, 170), Color.FromArgb(200,30,30), Color.FromArgb(10,160,50), Color.FromArgb(200,100,30), Color.FromArgb(100,30,200), Color.FromArgb(0,0,0)};
        LineStyle[] defaultLineStyle = ((LineStyle[])Enum.GetValues(typeof(LineStyle))).Where(e => e != LineStyle.None).ToArray();
        MarkerStyle[] defaultMarkerStyle = ((MarkerStyle[])Enum.GetValues(typeof(MarkerStyle))).Where(e => e != MarkerStyle.None).ToArray();

        RectangleExtended plotRectangle;

        List<ScatterSeries> Data;
        Dictionary<string, Axis> Vertical;
        Dictionary<string, Axis> Horizontal;

        public const float MouseEventDistance = 3;

        /// <summary>
        /// Plot title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Default style for graph.
        /// </summary>
        public PlotStyle PlotStyle { get; set; }
        /// <summary>
        /// Style of legend box.
        /// </summary>
        public LegendStyle LegendStyle { get; set; }
        /// <summary>
        /// Position of legend box.
        /// </summary>
        public LegendPosition LegendPosition { get; set; }
        /// <summary>
        /// Alignment of legend box.
        /// </summary>
        public LegendAlignment LegendAlignment { get; set; }

        public event PlotMouseEvent PlotClick;
        public event PlotMouseEvent PlotDoubleClick;
        public event PlotMouseEvent PlotMouseUp;
        public event PlotMouseEvent PlotMouseDown;
        public event PlotMouseEvent PlotMouseMove;

        /// <summary>
        /// Initializes a new instance of the <see cref = "OMPlot.Plot" /> class.
        /// </summary>
        public Plot()
        {
            InitializeComponent();

            selectionPen = new Pen(Color.FromArgb(200, 0, 50, 100));
            selectionBrush = new SolidBrush(Color.FromArgb(100, 0, 50, 100));
            titleFont = new Font(this.Font.FontFamily, this.Font.Size + 4, this.Font.Style, this.Font.Unit, this.Font.GdiCharSet, this.Font.GdiVerticalFont);
            mainBrush = new SolidBrush(this.ForeColor);
            mainPen = new Pen(this.ForeColor);
            backgroungBrush = new SolidBrush(this.BackColor);
            legendBoxBrush = new SolidBrush(Color.FromArgb(200, this.BackColor));

            Data = new List<ScatterSeries>();
            Vertical = new Dictionary<string, Axis>();
            Horizontal = new Dictionary<string, Axis>();
            this.MouseWheel += Plot_MouseWheel;

            Axis xAxis = new Axis();
            xAxis.Minimum = -10;
            xAxis.Maximum = 10;
            xAxis.Position = AxisPosition.Far;
            xAxis.TitlePosition = LabelsPosition.Far;
            xAxis.TicksLabelsPosition = LabelsPosition.Far;
            xAxis.TicksLabelsRotation = TicksLabelsRotation.Parallel;
            xAxis.TicksLabelsAlignment = Alignment.Center;
            xAxis.MajorTickStyle = TickStyle.Far;

            Axis yAxis = new Axis();
            yAxis.Minimum = -10;
            yAxis.Maximum = 10;
            yAxis.TicksLabelsRotation = TicksLabelsRotation.Perpendicular;
            yAxis.TicksLabelsAlignment = Alignment.Far;
            yAxis.TicksLabelsLineAlignment = Alignment.Center;
            yAxis.MinorTickStyle = TickStyle.Near;

            AddHorizontalAxis(xAxis);
            AddVerticalAxis(yAxis);
        }
        /// <summary>
        /// Add graph to plot
        /// </summary>
        /// <param name="data">Instance of <see cref="OMPlot.Data.ScatterSeries"/> or its child classes.</param>
        public void Add(ScatterSeries data)
        {
            if (data.VerticalAxis == null)
                data.VerticalAxis = GetVerticalAxis();
            if (data.HorizontalAxis == null)
                data.HorizontalAxis = GetHorizontalAxis();

            if (Data.Count == 0)
            {
                data.HorizontalAxis.Minimum = data.MinimumX;
                data.VerticalAxis.Minimum = data.MinimumY;
                data.HorizontalAxis.Maximum = data.MaximumX;
                data.VerticalAxis.Maximum = data.MaximumY;
            }
            else
            {
                data.HorizontalAxis.Minimum = data.HorizontalAxis.Minimum > data.MinimumX ? data.MinimumX : data.HorizontalAxis.Minimum;
                data.VerticalAxis.Minimum = data.VerticalAxis.Minimum > data.MinimumY ? data.MinimumY : data.VerticalAxis.Minimum;
                data.HorizontalAxis.Maximum = data.HorizontalAxis.Maximum < data.MaximumX ? data.MaximumX : data.HorizontalAxis.Maximum;
                data.VerticalAxis.Maximum = data.VerticalAxis.Maximum < data.MaximumY ? data.MaximumY : data.VerticalAxis.Maximum;
            }
            Data.Add(data);
        }
        /// <summary>
        /// Create instance of <see cref="OMPlot.Data.ScatterSeries"/> with default name and add it to plot.
        /// </summary>
        /// <param name="x">Collection of X values.</param>
        /// <param name="y">Collection of Y values.</param>
        /// <returns>Instance of <see cref="OMPlot.Data.ScatterSeries"/></returns>
        public ScatterSeries Add(IEnumerable<double> x, IEnumerable<double> y)
        {
            return this.Add(x, y, "Plot" + Data.Count().ToString());
        }
        /// <summary>
        /// Create instance of <see cref="OMPlot.Data.ScatterSeries"/> and add it to plot.
        /// </summary>
        /// <param name="x">Collection of X values.</param>
        /// <param name="y">Collection of Y values.</param>
        /// <param name="name">Name of created graph.</param>
        /// <returns>Instance of <see cref="OMPlot.Data.ScatterSeries"/></returns>
        public ScatterSeries Add(IEnumerable<double> x, IEnumerable<double> y, string name)
        {
            ScatterSeries data = new ScatterSeries(x, y, name);
            int plotIndex = Data.Count();

            if(PlotStyle == PlotStyle.Lines || PlotStyle == PlotStyle.Splines)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                int lineStyleIndex = (plotIndex - colorIndex) / defaultPlotColors.Length % defaultLineStyle.Length;
                data.LineColor = defaultPlotColors[colorIndex];
                data.LineStyle = defaultLineStyle[lineStyleIndex];
                if (PlotStyle == PlotStyle.Splines)
                    data.Interpolation = Interpolation.Spline;
            }
            else if(PlotStyle == PlotStyle.LinesMarkers || PlotStyle == PlotStyle.SplinesMarkers)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                int markerStyleIndex = (plotIndex - colorIndex) / defaultPlotColors.Length % defaultMarkerStyle.Length;
                data.LineColor = defaultPlotColors[colorIndex];
                data.MarkColor = defaultPlotColors[colorIndex];
                data.MarkStyle = defaultMarkerStyle[markerStyleIndex];
                if (PlotStyle == PlotStyle.SplinesMarkers)
                    data.Interpolation = Interpolation.Spline;
            }
            else if(PlotStyle == PlotStyle.Markers)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                int markerStyleIndex = plotIndex % defaultMarkerStyle.Length;
                data.LineStyle = LineStyle.None;
                data.MarkColor = defaultPlotColors[colorIndex];
                data.MarkStyle = defaultMarkerStyle[markerStyleIndex];
                if (PlotStyle == PlotStyle.SplinesMarkers)
                    data.Interpolation = Interpolation.Spline;
            }
            else if(PlotStyle == PlotStyle.VerticalBars || PlotStyle == PlotStyle.HorisontalBars)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                data.LineStyle = LineStyle.None;
                data.BarDuty = 0.7f;
                data.BarFillColor = defaultPlotColors[colorIndex];
                data.BarGrouping = true;
                data.BarStyle = PlotStyle == PlotStyle.HorisontalBars ? BarStyle.Horisontal : BarStyle.Vertical;
            }

            this.Add(data);
            return data;
        }
        /// <summary>
        /// Create instance of <see cref="OMPlot.Data.LineSeries"/> with default name dX and X0 and add it to plot.
        /// </summary>
        /// <param name="y">Collection of the values.</param>
        /// <returns>Instance of <see cref="OMPlot.Data.LineSeries"/></returns>
        public LineSeries Add(IEnumerable<double> y)
        {
            return this.Add(y, 1, 0, "Plot" + Data.Count().ToString());
        }
        /// <summary>
        /// Create instance of <see cref="OMPlot.Data.LineSeries"/> with default name and X0 and add it to plot.
        /// </summary>
        /// <param name="y">Collection of the values.</param>
        /// <param name="dx">Custom dX for the series.</param>
        /// <returns>Instance of <see cref="OMPlot.Data.LineSeries"/></returns>
        public LineSeries Add(IEnumerable<double> y, double dx)
        {
            return this.Add(y, dx, 0, "Plot" + Data.Count().ToString());
        }
        /// <summary>
        /// Create instance of <see cref="OMPlot.Data.LineSeries"/> with default name and add it to plot.
        /// </summary>
        /// <param name="y">Collection of the values.</param>
        /// <param name="dx">Custom dX for the series.</param>
        /// <param name="x0">Custom start X value.</param>
        /// <returns></returns>
        public LineSeries Add(IEnumerable<double> y, double dx, double x0)
        {
            return this.Add(y, dx, x0, "Plot" + Data.Count().ToString());
        }
        /// <summary>
        /// Create instance of <see cref="OMPlot.Data.LineSeries"/> with default dX and X0 and add it to plot.
        /// </summary>
        /// <param name="y">Collection of the values.</param>
        /// <param name="name">Name of created graph.</param>
        /// <returns>Instance of <see cref="OMPlot.Data.LineSeries"/></returns>
        public LineSeries Add(IEnumerable<double> y, string name)
        {
            return this.Add(y, 1, 0, name);
        }
        /// <summary>
        /// Create instance of <see cref="OMPlot.Data.LineSeries"/> with default X0 and add it to plot.
        /// </summary>
        /// <param name="y">Collection of the values.</param>
        /// <param name="name">Name of created graph.</param>
        /// <param name="dx">Custom dX for the series.</param>
        /// <returns>Instance of <see cref="OMPlot.Data.LineSeries"/></returns>
        public LineSeries Add(IEnumerable<double> y, double dx, string name)
        {
            return this.Add(y, dx, 0, name);
        }
        /// <summary>
        /// Create instance of <see cref="OMPlot.Data.LineSeries"/> and add it to plot.
        /// </summary>
        /// <param name="y">Collection of the values.</param>
        /// <param name="name">Name of created graph.</param>
        /// <param name="dx">Custom dX for the series.</param>
        /// <param name="x0">Custom start X value.</param>
        /// <returns>Instance of <see cref="OMPlot.Data.LineSeries"/></returns>
        public LineSeries Add(IEnumerable<double> y, double dx, double x0, string name)
        {
            LineSeries data = new LineSeries(y, dx, x0);
            data.Name = name;
            int plotIndex = Data.Count();

            if (PlotStyle == PlotStyle.Lines || PlotStyle == PlotStyle.Splines)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                int lineStyleIndex = (plotIndex - colorIndex) / defaultPlotColors.Length % defaultLineStyle.Length;
                data.LineColor = defaultPlotColors[colorIndex];
                data.LineStyle = defaultLineStyle[lineStyleIndex];
                if (PlotStyle == PlotStyle.Splines)
                    data.Interpolation = Interpolation.Spline;
            }
            else if (PlotStyle == PlotStyle.LinesMarkers || PlotStyle == PlotStyle.SplinesMarkers)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                int markerStyleIndex = (plotIndex - colorIndex) / defaultPlotColors.Length % defaultMarkerStyle.Length;
                data.LineColor = defaultPlotColors[colorIndex];
                data.MarkColor = defaultPlotColors[colorIndex];
                data.MarkStyle = defaultMarkerStyle[markerStyleIndex];
                if (PlotStyle == PlotStyle.SplinesMarkers)
                    data.Interpolation = Interpolation.Spline;
            }
            else if (PlotStyle == PlotStyle.Markers)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                int markerStyleIndex = plotIndex % defaultMarkerStyle.Length;
                data.LineStyle = LineStyle.None;
                data.MarkColor = defaultPlotColors[colorIndex];
                data.MarkStyle = defaultMarkerStyle[markerStyleIndex];
                if (PlotStyle == PlotStyle.SplinesMarkers)
                    data.Interpolation = Interpolation.Spline;
            }
            else if (PlotStyle == PlotStyle.VerticalBars || PlotStyle == PlotStyle.HorisontalBars)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                data.LineStyle = LineStyle.None;
                data.BarDuty = 0.7f;
                data.BarFillColor = defaultPlotColors[colorIndex];
                data.BarGrouping = true;
                data.BarStyle = PlotStyle == PlotStyle.HorisontalBars ? BarStyle.Horisontal : BarStyle.Vertical;
            }

            this.Add(data);
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ScatterSeries Add(Dictionary<string, double> dictionary, string name)
        {
            var axis = PlotStyle == PlotStyle.HorisontalBars ? GetVerticalAxis() : GetHorizontalAxis();
            if(axis.CustomTicksLabels == null)
                axis.CustomTicksLabels = dictionary.Keys.ToArray();
            else
                axis.CustomTicksLabels = axis.CustomTicksLabels.Concat(dictionary.Keys).Distinct().ToArray();
            axis.CustomTicks = axis.CustomTicksLabels.Select((key, i) => (double)i).ToArray();

            axis.MinorTickNumber = 1;
            axis.MinorTickStyle = PlotStyle == PlotStyle.HorisontalBars ? TickStyle.Near : TickStyle.Far;
            axis.MajorTickStyle = TickStyle.None;

            ScatterSeries data = PlotStyle == PlotStyle.HorisontalBars ?
                new ScatterSeries(dictionary.Values, dictionary.Keys.Select(key => (double)Array.FindIndex(axis.CustomTicksLabels, e => e == key))) :
                new ScatterSeries(dictionary.Keys.Select(key => (double)Array.FindIndex(axis.CustomTicksLabels, e => e == key)), dictionary.Values);
            data.Name = name;
            

            int plotIndex = Data.Count();

            if (PlotStyle == PlotStyle.Lines || PlotStyle == PlotStyle.Splines)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                int lineStyleIndex = (plotIndex - colorIndex) / defaultPlotColors.Length % defaultLineStyle.Length;
                data.LineColor = defaultPlotColors[colorIndex];
                data.LineStyle = defaultLineStyle[lineStyleIndex];
                if (PlotStyle == PlotStyle.Splines)
                    data.Interpolation = Interpolation.Spline;
            }
            else if (PlotStyle == PlotStyle.LinesMarkers || PlotStyle == PlotStyle.SplinesMarkers)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                int markerStyleIndex = (plotIndex - colorIndex) / defaultPlotColors.Length % defaultMarkerStyle.Length;
                data.LineColor = defaultPlotColors[colorIndex];
                data.MarkColor = defaultPlotColors[colorIndex];
                data.MarkStyle = defaultMarkerStyle[markerStyleIndex];
                if (PlotStyle == PlotStyle.SplinesMarkers)
                    data.Interpolation = Interpolation.Spline;
            }
            else if (PlotStyle == PlotStyle.Markers)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                int markerStyleIndex = plotIndex % defaultMarkerStyle.Length;
                data.LineStyle = LineStyle.None;
                data.MarkColor = defaultPlotColors[colorIndex];
                data.MarkStyle = defaultMarkerStyle[markerStyleIndex];
                if (PlotStyle == PlotStyle.SplinesMarkers)
                    data.Interpolation = Interpolation.Spline;
            }
            else if (PlotStyle == PlotStyle.VerticalBars || PlotStyle == PlotStyle.HorisontalBars)
            {
                int colorIndex = plotIndex % defaultPlotColors.Length;
                data.LineStyle = LineStyle.None;
                data.BarDuty = 0.7f;
                data.BarFillColor = defaultPlotColors[colorIndex];
                data.BarGrouping = true;
                data.BarStyle = PlotStyle == PlotStyle.HorisontalBars ? BarStyle.Horisontal : BarStyle.Vertical;
            }

            this.Add(data);
            return data;
        }

        /// <summary>
        /// Remove all graphs.
        /// </summary>
        public void Clear() { Data.Clear(); }

        /// <summary>
        /// Add aditional vertical axis.
        /// </summary>
        /// <param name="axis">Instance of <see cref="Axis"/>.</param>
        public void AddVerticalAxis(Axis axis)
        {
            if (axis != null)
            {
                axis.Vertical = true;
                Vertical.Add(axis.GetHashCode().ToString(), axis);
            }
        }
        /// <summary>
        /// Add aditional horizontal axis.
        /// </summary>
        /// <param name="axis">Instance of <see cref="Axis"/></param>
        public void AddHorizontalAxis(Axis axis)
        {
            if (axis != null)
            {
                Horizontal.Add(axis.GetHashCode().ToString(), axis);
            }
        }
        /// <summary>
        /// Get vertical axis.
        /// </summary>
        /// <param name="name">Name of requared axis.</param>
        /// <returns>Instance of <see cref="Axis"/></returns>
        public Axis GetVerticalAxis(string name = "")
        {
            if (string.IsNullOrEmpty(name))
                return Vertical.First().Value;
            return Vertical[name];
        }
        /// <summary>
        /// Get horizontal axis.
        /// </summary>
        /// <param name="name">Name of requared axis.</param>
        /// <returns>Instance of <see cref="Axis"/></returns>
        public Axis GetHorizontalAxis(string name = "")
        {
            if (string.IsNullOrEmpty(name))
                return Horizontal.First().Value;
            return Horizontal[name];
        }

        private void Plot_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoom = 100 / (float)e.Delta;
            if (zoom < 0)
                zoom = -1 / zoom;
            if (plotRectangle.InRectangle(e.X, e.Y))
            {
                foreach (var axis in Horizontal)
                    axis.Value.Zoom(zoom, e.X - plotRectangle.CenterX);
                foreach (var axis in Vertical)
                    axis.Value.Zoom(zoom, e.Y - plotRectangle.CenterY);
            }
            else
            {
                foreach (var axis in Horizontal.Where(axis => axis.Value.ActionOnAxis(e.X, e.Y)))
                    axis.Value.Zoom(zoom, e.X - plotRectangle.CenterX);
                foreach (var axis in Vertical.Where(axis => axis.Value.ActionOnAxis(e.X, e.Y)))
                    axis.Value.Zoom(zoom, e.Y - plotRectangle.CenterY);
            }
            this.Refresh();
        }
        private void Plot_MouseDown(object sender, MouseEventArgs e)
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
                    if (selectHorizontal)
                        selectedAxisName = axisSelected.First().Key;
                    axisSelected = Vertical.Where(axis => axis.Value.ActionOnAxis(e.X, e.Y));
                    selectVertical = axisSelected.Any();
                    if (selectVertical)
                        selectedAxisName = axisSelected.First().Key;
                }
                mouseX = e.X;
                mouseY = e.Y;
            }
            if (PlotMouseDown != null && plotRectangle.InRectangle(e.X, e.Y))
            {
                var plotPoint = CalculateMinDistance(e.X, e.Y);
                if (plotPoint.Item1 >= 0)
                    PlotMouseDown(this, new PlotMouseEventArgs(e, Data[plotPoint.Item1], plotPoint.Item2.Point));
                else
                    PlotMouseDown(this, new PlotMouseEventArgs(e));
            }
        }
        private void Plot_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (selectHorizontal && selectVertical)
                {
                    foreach (var axis in Horizontal)
                        axis.Value.Move(mouseX - e.X);
                    foreach (var axis in Vertical)
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
            if (PlotMouseMove != null && plotRectangle.InRectangle(e.X, e.Y))
            {
                var plotPoint = CalculateMinDistance(e.X, e.Y);
                if (plotPoint.Item1 >= 0)
                    PlotMouseMove(this, new PlotMouseEventArgs(e, Data[plotPoint.Item1], plotPoint.Item2.Point));
                else
                    PlotMouseMove(this, new PlotMouseEventArgs(e));
            }
        }
        private void Plot_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (selectHorizontal && selectVertical)
                {
                    foreach (var axis in Horizontal)
                        axis.Value.Select((int)Math.Min(mouseX, currentmouseX), (int)Math.Max(mouseX, currentmouseX));
                    foreach (var axis in Vertical)
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
            if (PlotMouseUp != null && plotRectangle.InRectangle(e.X, e.Y))
            {
                var plotPoint = CalculateMinDistance(e.X, e.Y);
                if (plotPoint.Item1 >= 0)
                    PlotMouseUp(this, new PlotMouseEventArgs(e, Data[plotPoint.Item1], plotPoint.Item2.Point));
                else
                    PlotMouseUp(this, new PlotMouseEventArgs(e));
            }
        }
        private void Plot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (PlotDoubleClick != null && plotRectangle.InRectangle(e.X, e.Y))
            {
                var plotPoint = CalculateMinDistance(e.X, e.Y);
                if (plotPoint.Item1 >= 0)
                    PlotDoubleClick(this, new PlotMouseEventArgs(e, Data[plotPoint.Item1], plotPoint.Item2.Point));
                else
                    PlotDoubleClick(this, new PlotMouseEventArgs(e));
            }
        }
        private void Plot_MouseCaptureChanged(object sender, EventArgs e)
        {
            currentmouseX = -1;
            currentmouseY = -1;
            this.Refresh();
        }
        private void Plot_MouseClick(object sender, MouseEventArgs e)
        {
            if (PlotClick != null && plotRectangle.InRectangle(e.X, e.Y))
            {
                var plotPoint = CalculateMinDistance(e.X, e.Y);
                if (plotPoint.Item1 >= 0)
                    PlotClick(this, new PlotMouseEventArgs(e, Data[plotPoint.Item1], plotPoint.Item2.Point));
                else
                    PlotClick(this, new PlotMouseEventArgs(e));
            }
        }
        private void Plot_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint(e.Graphics);
        }
        private void Control_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }
        public void ControlPaint(Graphics g)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
                        
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(this.BackColor);

            var paddingRectangle = new RectangleExtended(6, 6, this.Width - 13, this.Height - 13);
            plotRectangle = paddingRectangle;

            var titleSize = new SizeF(0, 0);
            if (!string.IsNullOrEmpty(Title))
                titleSize = g.MeasureString(Title, titleFont);
            plotRectangle.Top += titleSize.Height;

            var legendRectangles = new RectangleF[Data.Count];
            var legendRectangle = new RectangleF();
            if (LegendStyle == LegendStyle.Outside && Data.Any())
            {
                var legendsMeas = Data.Select(data => g.MeasureString(data.Name, this.Font)).ToArray();

                if (LegendPosition == LegendPosition.Top || LegendPosition == LegendPosition.Bottom)
                {
                    float limit = paddingRectangle.Width;
                    float legendx = 0;
                    float legendy = 0;
                    for (int i = 0; i < Data.Count; i++)
                    {
                        if (legendx + legendsMeas[i].Width + legendsMeas[i].Height * 2 > limit)
                        {
                            legendy += legendsMeas[0].Height + 5;
                            legendx = 0;
                        }
                        legendRectangles[i] = new RectangleF(legendx, legendy, legendsMeas[i].Height * 2, legendsMeas[i].Height);
                        legendx += legendsMeas[i].Width + 5 + legendsMeas[i].Height * 2;
                    }
                    legendRectangle.Width = limit;
                    legendRectangle.Height = legendRectangles.Last().Y + legendsMeas.Last().Height + 5;
                }
                else
                {
                    float limit = paddingRectangle.Height - titleSize.Height;
                    float legendx = 0;
                    float legendy = 0;
                    int rowFirstElement = 0;
                    for (int i = 0; i < Data.Count; i++)
                    {
                        if (legendy + legendsMeas[i].Height > limit)
                        {
                            legendx += legendsMeas.Skip(rowFirstElement).Take(i - rowFirstElement).Max(meas => meas.Width) + 5 + legendsMeas[i].Height * 2;
                            legendy = 0;
                            rowFirstElement = i;
                        }
                        legendRectangles[i] = new RectangleF(legendx, legendy, legendsMeas[i].Height * 2, legendsMeas[i].Height);
                        legendy += legendsMeas[i].Height + 5;
                    }
                    legendRectangle.Width = legendRectangles.Last().X + legendRectangles.Last().Width + legendsMeas.Skip(rowFirstElement).Max(meas => meas.Width) + 5;
                    legendRectangle.Height = limit;
                }

                legendRectangle.X = LegendPosition == LegendPosition.Right ? paddingRectangle.Right - legendRectangle.Width + 5 : paddingRectangle.Left;
                legendRectangle.Y = LegendPosition == LegendPosition.Bottom ? paddingRectangle.Bottom - legendRectangle.Height : paddingRectangle.Top + titleSize.Height;

                if (LegendPosition == LegendPosition.Top)
                    plotRectangle.Top += legendRectangle.Height;
                else if (LegendPosition == LegendPosition.Bottom)
                    plotRectangle.Bottom -= legendRectangle.Height;
                else if (LegendPosition == LegendPosition.Left)
                    plotRectangle.Left += legendRectangle.Width;
                else
                    plotRectangle.Right -= legendRectangle.Width;
            }

            foreach (var axis in Vertical)
            {
                axis.Value.Font = this.Font;
                axis.Value.CalculateTranformVertical(plotRectangle.Top, plotRectangle.Bottom); //preliminary calculation
                axis.Value.MeasureVertical(g);
                if (axis.Value.Position == AxisPosition.Near || axis.Value.Position == AxisPosition.CrossValue)
                    plotRectangle.Left += axis.Value.SizeNear;
                if (axis.Value.Position == AxisPosition.Far || axis.Value.Position == AxisPosition.CrossValue)
                    plotRectangle.Right -= axis.Value.SizeFar;
            }
            foreach (var axis in Horizontal)
            {
                axis.Value.Font = this.Font;
                axis.Value.CalculateTranformHorizontal(plotRectangle.Left, plotRectangle.Right); //preliminary calculation
                axis.Value.MeasureHorizontal(g);
                if (axis.Value.Position == AxisPosition.Near || axis.Value.Position == AxisPosition.CrossValue)
                    plotRectangle.Top += axis.Value.SizeNear;
                if (axis.Value.Position == AxisPosition.Far || axis.Value.Position == AxisPosition.CrossValue)
                    plotRectangle.Bottom -= axis.Value.SizeFar;
            }

            foreach (var axis in Vertical)
            {
                plotRectangle.Top = plotRectangle.Top > axis.Value.OverflowNear ? plotRectangle.Top : axis.Value.OverflowNear;
                plotRectangle.Bottom = (this.Height - plotRectangle.Bottom) > axis.Value.OverflowFar ? plotRectangle.Bottom : (this.Height - axis.Value.OverflowFar);
            }
            foreach (var axis in Horizontal)
            {
                plotRectangle.Left = plotRectangle.Left > axis.Value.OverflowNear ? plotRectangle.Left : axis.Value.OverflowNear;
                plotRectangle.Right = (this.Width - plotRectangle.Right) > axis.Value.OverflowFar ? plotRectangle.Right : (this.Width - axis.Value.OverflowFar);
            }

            if (LegendStyle == LegendStyle.Inside && Data.Any())
            {
                var legendsMeas = Data.Select(data => g.MeasureString(data.Name, this.Font)).ToArray();

                if (LegendPosition == LegendPosition.Top || LegendPosition == LegendPosition.Bottom)
                {
                    float limit = plotRectangle.Width / 2;
                    float legendx = 0;
                    float legendy = 0;
                    float legendWidth = 0;
                    for (int i = 0; i < Data.Count; i++)
                    {
                        if (legendx + legendsMeas[i].Width + legendsMeas[i].Height * 2 > limit)
                        {
                            legendy += legendsMeas[0].Height + 5;
                            legendx = 0;
                        }
                        legendRectangles[i] = new RectangleF(legendx, legendy, legendsMeas[i].Height * 2, legendsMeas[i].Height);
                        legendx += legendsMeas[i].Width + 5 + legendsMeas[i].Height * 2;
                        if (legendWidth < legendx)
                            legendWidth = legendx;
                    }
                    legendRectangle.Width = legendWidth;
                    legendRectangle.Height = legendRectangles.Last().Y + legendsMeas.Last().Height + 10;
                }
                else
                {
                    float limit = plotRectangle.Height / 2;
                    float legendx = 0;
                    float legendy = 0;
                    int rowFirstElement = 0;
                    for (int i = 0; i < Data.Count; i++)
                    {
                        if (legendy + legendsMeas[i].Height > limit)
                        {
                            legendx += legendsMeas.Skip(rowFirstElement).Take(i - rowFirstElement).Max(meas => meas.Width) + 5 + legendsMeas[i].Height * 2;
                            legendy = 0;
                            rowFirstElement = i;
                        }
                        legendRectangles[i] = new RectangleF(legendx, legendy, legendsMeas[i].Height * 2, legendsMeas[i].Height);
                        legendy += legendsMeas[i].Height + 5;
                    }
                    legendRectangle.Width = legendRectangles.Last().X + legendRectangles.Last().Width + legendsMeas.Skip(rowFirstElement).Max(meas => meas.Width) + 10;
                    legendRectangle.Height = legendRectangles.Max(r => r.Y + r.Height) + 10;
                }

                
                if (LegendAlignment == LegendAlignment.Near)
                {
                    legendRectangle.X = LegendPosition == LegendPosition.Right ? plotRectangle.Right - legendRectangle.Width - 5 : plotRectangle.Left + 5;
                    legendRectangle.Y = LegendPosition == LegendPosition.Bottom ? plotRectangle.Bottom - legendRectangle.Height - 5 : plotRectangle.Top + 5;
                }
                else if (LegendAlignment == LegendAlignment.Far)
                {
                    legendRectangle.X = LegendPosition == LegendPosition.Left ? plotRectangle.Left + 5 : plotRectangle.Right - legendRectangle.Width - 5;
                    legendRectangle.Y = LegendPosition == LegendPosition.Top ? plotRectangle.Top + 5 : plotRectangle.Bottom - legendRectangle.Height - 5;                    
                }
                else
                {
                    if (LegendPosition == LegendPosition.Top)
                    {
                        legendRectangle.X = plotRectangle.CenterX - legendRectangle.Width / 2f;
                        legendRectangle.Y = plotRectangle.Top + 5;
                    }
                    else if (LegendPosition == LegendPosition.Bottom)
                    {
                        legendRectangle.X = plotRectangle.CenterX - legendRectangle.Width / 2f;
                        legendRectangle.Y = plotRectangle.Bottom - legendRectangle.Height - 5;
                    }
                    else if (LegendPosition == LegendPosition.Left)
                    {
                        legendRectangle.X = plotRectangle.Left + 5;
                        legendRectangle.Y = plotRectangle.CenterY - legendRectangle.Height / 2f;
                    }
                    else
                    {
                        legendRectangle.X = plotRectangle.Right - legendRectangle.Width - 5;
                        legendRectangle.Y = plotRectangle.CenterY - legendRectangle.Height / 2f;
                    }
                }
            }

            if (LegendStyle != LegendStyle.None && Data.Any())
            {
                for (int i = 0; i < legendRectangles.Length; i++)
                {
                    legendRectangles[i].X += legendRectangle.X + (LegendStyle == LegendStyle.Inside ? 5 : 0);
                    legendRectangles[i].Y += legendRectangle.Y + (LegendStyle == LegendStyle.Inside ? 5 : 0);
                }
            }

            foreach (var axis in Vertical)
                axis.Value.CalculateTranformVertical(plotRectangle.Top, plotRectangle.Bottom);
            foreach (var axis in Horizontal)
                axis.Value.CalculateTranformHorizontal(plotRectangle.Left, plotRectangle.Right);


            if (Vertical.Any() && Horizontal.Any())
            {
                //BarStyle.Vertical
                var barData = Data.Where(data => data.BarStyle == BarStyle.Vertical && data.BarGrouping);
                int barCount = barData.Count();
                int barIndex = 0;
                foreach (var bar in barData)
                {
                    bar.BarIndex = barIndex;
                    bar.BarCount = barCount;
                    barIndex++;
                }
                //BarStyle.Horisontal
                barData = Data.Where(data => data.BarStyle == BarStyle.Horisontal && data.BarGrouping);
                barCount = barData.Count();
                barIndex = 0;
                foreach (var bar in barData)
                {
                    bar.BarIndex = barIndex;
                    bar.BarCount = barCount;
                    barIndex++;
                }

                //foreach (var data in Data)
                Parallel.ForEach(Data, data => data.CalculateGraphics(plotRectangle));
                int plotIndex = 0;
                foreach (var data in Data)
                    data.Draw(g, plotRectangle, plotIndex++);
            }
            g.ResetTransform();

            g.FillRectangle(backgroungBrush, -1, -1, this.Width + 2, plotRectangle.Top);
            g.FillRectangle(backgroungBrush, -1, plotRectangle.Top - 2, plotRectangle.Left + 2, plotRectangle.Height + 4);
            g.FillRectangle(backgroungBrush, plotRectangle.Right + 1, plotRectangle.Top - 2, this.Width - plotRectangle.Right, plotRectangle.Height + 4);
            g.FillRectangle(backgroungBrush, -1, plotRectangle.Bottom + 1, this.Width + 2, this.Height - plotRectangle.Bottom);
            //if()
            //g.DrawRectangle(mainPen, plotRectangle.X, plotRectangle.Y, plotRectangle.Width, plotRectangle.Height);

            if (!string.IsNullOrEmpty(Title))
            {
                StringFormat stringFormat = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString(Title, titleFont, mainBrush, this.Width / 2.0f, paddingRectangle.Top, stringFormat);
            }

            bool drawBorderFar = true;
            bool drawBorderNear = true;
            float rightAxisPosition = plotRectangle.Right;
            float leftAxisPosition = plotRectangle.Left;
            foreach (var axis in Vertical)
            {
                switch (axis.Value.Position)
                {
                    case AxisPosition.Near:
                        {
                            axis.Value.DrawVertical(g, leftAxisPosition, plotRectangle.Top, plotRectangle);
                            leftAxisPosition -= axis.Value.SizeNear;
                            drawBorderNear = false;
                            break;
                        }
                    case AxisPosition.Far:
                        {
                            axis.Value.DrawVertical(g, rightAxisPosition, plotRectangle.Top, plotRectangle);
                            rightAxisPosition += axis.Value.SizeFar;
                            drawBorderFar = false;
                            break;
                        }
                    case AxisPosition.Center:
                        axis.Value.DrawVertical(g, plotRectangle.CenterX, plotRectangle.Top, plotRectangle);
                        break;
                    case AxisPosition.CrossValue:
                        {
                            if (axis.Value.CrossValue < Horizontal.First().Value.Minimum)
                            {
                                axis.Value.DrawVertical(g, leftAxisPosition, plotRectangle.Top, plotRectangle);
                                leftAxisPosition -= axis.Value.SizeNear;
                                drawBorderNear = false;
                            }
                            else if (axis.Value.CrossValue > Horizontal.First().Value.Maximum)
                            {
                                axis.Value.DrawVertical(g, rightAxisPosition, plotRectangle.Top, plotRectangle);
                                rightAxisPosition += axis.Value.SizeFar;
                                drawBorderFar = false;
                            }
                            else
                                axis.Value.DrawVertical(g, (float)Horizontal.First().Value.Transform(axis.Value.CrossValue), plotRectangle.Top, plotRectangle);
                            break;
                        }
                }
            }
            if (drawBorderFar)
                g.DrawLine(mainPen, plotRectangle.Right, plotRectangle.Top, plotRectangle.Right, plotRectangle.Bottom);
            if (drawBorderNear)
                g.DrawLine(mainPen, plotRectangle.Left, plotRectangle.Top, plotRectangle.Left, plotRectangle.Bottom);

            drawBorderFar = true;
            drawBorderNear = true;
            float topAxisPosition = plotRectangle.Top;
            float bottomAxisPosition = plotRectangle.Bottom;
            foreach (var axis in Horizontal)
            {
                switch (axis.Value.Position)
                {
                    case AxisPosition.Far:
                        {
                            axis.Value.DrawHorizontal(g, plotRectangle.Left, bottomAxisPosition, plotRectangle);
                            bottomAxisPosition += axis.Value.SizeFar;
                            drawBorderFar = false;
                            break;
                        }
                    case AxisPosition.Near:
                        {
                            axis.Value.DrawHorizontal(g, plotRectangle.Left, topAxisPosition, plotRectangle);
                            topAxisPosition -= axis.Value.SizeNear;
                            drawBorderNear = false;
                            break;
                        }
                    case AxisPosition.Center:
                        axis.Value.DrawHorizontal(g, plotRectangle.Left, plotRectangle.CenterY, plotRectangle);
                        break;
                    case AxisPosition.CrossValue:
                        {
                            if (axis.Value.CrossValue < Vertical.First().Value.Minimum)
                            {
                                axis.Value.DrawHorizontal(g, plotRectangle.Left, bottomAxisPosition, plotRectangle);
                                bottomAxisPosition += axis.Value.SizeNear;
                                drawBorderFar = false;
                            }
                            else if (axis.Value.CrossValue > Vertical.First().Value.Maximum)
                            {
                                axis.Value.DrawHorizontal(g, plotRectangle.Left, topAxisPosition, plotRectangle);
                                topAxisPosition -= axis.Value.SizeFar;
                                drawBorderNear = false;
                            }
                            else
                                axis.Value.DrawHorizontal(g, plotRectangle.Left, (float)Vertical.First().Value.Transform(axis.Value.CrossValue), plotRectangle);
                            break;
                        }
                }
            }
            if (drawBorderFar)
                g.DrawLine(mainPen, plotRectangle.Left, plotRectangle.Bottom, plotRectangle.Right, plotRectangle.Bottom);
            if (drawBorderNear)
                g.DrawLine(mainPen, plotRectangle.Left, plotRectangle.Top, plotRectangle.Right, plotRectangle.Top);

            if (LegendStyle != LegendStyle.None)
            {
                if (LegendStyle == LegendStyle.Inside)
                {
                    g.FillRectangle(legendBoxBrush, legendRectangle);
                    g.DrawRectangle(mainPen, legendRectangle.X, legendRectangle.Y, legendRectangle.Width, legendRectangle.Height);
                }
                for (int i = 0; i < Data.Count; i++)
                {
                    g.DrawString(Data[i].Name, this.Font, mainBrush, legendRectangles[i].Right, legendRectangles[i].Top);
                    Data[i].DrawLegend(g, legendRectangles[i]);
                }
            }

            if (currentmouseX >= 0 && currentmouseY >= 0 && (selectVertical || selectHorizontal))
            {
                Rectangle selectionRec = new Rectangle();
                if (selectVertical && selectHorizontal)
                    selectionRec = new Rectangle(Math.Min(mouseX, currentmouseX), Math.Min(mouseY, currentmouseY), Math.Abs(mouseX - currentmouseX), Math.Abs(mouseY - currentmouseY));
                else if (selectVertical)
                    selectionRec = new Rectangle((int)plotRectangle.Left, Math.Min(mouseY, currentmouseY), (int)plotRectangle.Width, Math.Abs(mouseY - currentmouseY));
                else if (selectHorizontal)
                    selectionRec = new Rectangle(Math.Min(mouseX, currentmouseX), (int)plotRectangle.Top, Math.Abs(mouseX - currentmouseX), (int)plotRectangle.Height);
                g.DrawRectangle(selectionPen, selectionRec);
                g.FillRectangle(selectionBrush, selectionRec);
            }

            sw.Stop();

            if (ElapsedMilliseconds.Count > 100)
                ElapsedMilliseconds.Dequeue();
            ElapsedMilliseconds.Enqueue(sw.ElapsedMilliseconds);
            double ElapsedMillisecondsAvg = ElapsedMilliseconds.Average();
            g.DrawString((1000.0 / (ElapsedMillisecondsAvg > 0 ? ElapsedMillisecondsAvg : 1)).ToString("#.###"), this.Font, mainBrush, 0, 0);

        }

        private void Plot_FontChanged(object sender, EventArgs e)
        {
            titleFont = new Font(this.Font.FontFamily, this.Font.Size + 4, this.Font.Style, this.Font.Unit, this.Font.GdiCharSet, this.Font.GdiVerticalFont);
        }
        private void Plot_ForeColorChanged(object sender, EventArgs e)
        {
            mainBrush = new SolidBrush(this.ForeColor);
            mainPen = new Pen(this.ForeColor);
        }
        private void Plot_BackColorChanged(object sender, EventArgs e)
        {
            backgroungBrush = new SolidBrush(this.BackColor);
            legendBoxBrush = new SolidBrush(Color.FromArgb(200, this.BackColor));
        }

        /// <summary>
        /// Create <see cref="Image"/> from current plot.
        /// </summary>
        /// <returns>Instance of <see cref="Image"/>.</returns>
        /// <example>
        /// OMPlot.Plot plot = new Plot { Height = 500, Width = 500 };
        /// plot.Add(new double[]{ 1.0, 1.2, 1.7, 2.5, 3.5 });
        /// plot.ToImage().Save("plot.png");
        /// </example>
        public Image ToImage()
        {
            Image img = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(img);
            ControlPaint(g);
            return img;
        }

        private Tuple<int, PointDistance> CalculateMinDistance(int X, int Y)
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            var dist = Data.AsParallel().Select((data, i) => new { Index = i, PointDistance = data.DistanceToPoint(X, Y) }).ToArray();

            double mindistance = Height * Height + Width * Width;
            int index = -1;
            PointDistance pd = new PointDistance();
            for (int i = 0; i < dist.Length; i++)
            {
                if (dist[i].PointDistance.Distance < mindistance)
                {
                    mindistance = dist[i].PointDistance.Distance;
                    pd = dist[i].PointDistance;
                    index = dist[i].Index;
                    //test0 = new PointF(e.X, e.Y);
                    //test1 = dist[i].Distance.Point;
                }
            }
            //sw.Stop();
            //time = sw.ElapsedMilliseconds;
            //this.Refresh();
            return new Tuple<int, PointDistance>(index, pd);
        }

        public void Autoscale()
        {
            foreach (var axis in Horizontal)
            {
                axis.Value.Minimum = double.MaxValue;
                axis.Value.Maximum = double.MinValue;
            }
            foreach (var axis in Vertical)
            {
                axis.Value.Minimum = double.MaxValue;
                axis.Value.Maximum = double.MinValue;
            }
            foreach (var data in Data)
            {
                data.HorizontalAxis.Minimum = data.HorizontalAxis.Minimum > data.MinimumX ? data.MinimumX : data.HorizontalAxis.Minimum;
                data.VerticalAxis.Minimum = data.VerticalAxis.Minimum > data.MinimumY ? data.MinimumY : data.VerticalAxis.Minimum;
                data.HorizontalAxis.Maximum = data.HorizontalAxis.Maximum < data.MaximumX ? data.MaximumX : data.HorizontalAxis.Maximum;
                data.VerticalAxis.Maximum = data.VerticalAxis.Maximum < data.MaximumY ? data.MaximumY : data.VerticalAxis.Maximum;
            }
        }

        Queue<long> ElapsedMilliseconds = new Queue<long>(100);



    }

    public delegate void PlotMouseEvent(object sender, PlotMouseEventArgs e);

    /// <summary>
    /// Enumerates the available legend style.
    /// </summary>
    public enum LegendStyle
    {
        /// <summary>
        /// Do not display legend.
        /// </summary>
        None,
        Inside, Outside
    }
    /// <summary>
    /// Enumerates the available legend position.
    /// </summary>
    public enum LegendPosition { Top, Bottom, Left, Right }
    /// <summary>
    /// Enumerates the available legend alignment.
    /// </summary>
    public enum LegendAlignment { Far, Center, Near }
    /// <summary>
    /// Enumerates the available plot styles.
    /// </summary>
    public enum PlotStyle { Lines, Splines, LinesMarkers, SplinesMarkers, Markers, VerticalBars, HorisontalBars, Custom }
}
