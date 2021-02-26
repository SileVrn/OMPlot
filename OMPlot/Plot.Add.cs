using System;
using System.Collections.Generic;
using System.Linq;
using OMPlot.Data;

namespace OMPlot
{
    public partial class Plot
    {
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
        /// Create instance of <see cref="OMPlot.Data.ScatterSeries"/> and add it to plot.
        /// </summary>
        /// <param name="x">Collection of X values.</param>
        /// <param name="y">Collection of Y values.</param>
        /// <param name="ps">Default style for graph.</param>
        /// <param name="name">Name of created graph.</param>
        /// <returns>Instance of <see cref="OMPlot.Data.ScatterSeries"/></returns>
        public ScatterSeries Add(IEnumerable<double> x, IEnumerable<double> y, PlotStyle ps = PlotStyle.Lines, string name = "")
        {
            ScatterSeries data = new ScatterSeries(x, y, string.IsNullOrEmpty(name) ? "Plot" + Data.Count().ToString() : name);
            int plotIndex = Data.Count();
            PlotStyleApply(data, ps, plotIndex);
            this.Add(data);
            return data;
        }
        /// <summary>
        /// Create instance of <see cref="OMPlot.Data.LineSeries"/> with default dX and X0 and add it to plot.
        /// </summary>
        /// <param name="y">Collection of the values.</param>
        /// <param name="ps">Default style for graph.</param>
        /// <param name="name">Name of created graph.</param>
        /// <returns>Instance of <see cref="OMPlot.Data.LineSeries"/></returns>
        public LineSeries Add(IEnumerable<double> y, PlotStyle ps = PlotStyle.Lines, string name = "")
        {
            return this.Add(y, 1, 0, ps, name);
        }
        /// <summary>
        /// Create instance of <see cref="OMPlot.Data.LineSeries"/> with default X0 and add it to plot.
        /// </summary>
        /// <param name="y">Collection of the values.</param>
        /// <param name="dx">Custom dX for the series.</param>
        /// <param name="ps">Default style for graph.</param>
        /// <param name="name">Name of created graph.</param>
        /// <returns>Instance of <see cref="OMPlot.Data.LineSeries"/></returns>
        public LineSeries Add(IEnumerable<double> y, double dx, PlotStyle ps = PlotStyle.Lines, string name = "")
        {
            return this.Add(y, dx, 0, ps, name);
        }
        /// <summary>
        /// Create instance of <see cref="OMPlot.Data.LineSeries"/> and add it to plot.
        /// </summary>
        /// <param name="y">Collection of the values.</param>
        /// <param name="dx">Custom dX for the series.</param>
        /// <param name="x0">Custom start X value.</param>
        /// <param name="ps">Default style for graph.</param>
        /// <param name="name">Name of created graph.</param>
        /// <returns>Instance of <see cref="OMPlot.Data.LineSeries"/></returns>
        public LineSeries Add(IEnumerable<double> y, double dx, double x0, PlotStyle ps = PlotStyle.Lines, string name = "")
        {
            LineSeries data = new LineSeries(y, dx, x0);
            data.Name = string.IsNullOrEmpty(name) ? "Plot" + Data.Count().ToString() : name;
            int plotIndex = Data.Count();
            PlotStyleApply(data, ps, plotIndex);
            this.Add(data);
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="ps">Default style for graph.</param>
        /// <param name="name">>Name of created graph.</param>
        /// <returns></returns>
        public ScatterSeries Add(Dictionary<string, double> dictionary, PlotStyle ps = PlotStyle.VerticalBars, string name = "")
        {
            var axis = PlotStyle == PlotStyle.HorisontalBars ? GetVerticalAxis() : GetHorizontalAxis();
            if (axis.CustomTicksLabels == null)
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
            PlotStyleApply(data, ps, plotIndex);

            this.Add(data);
            return data;
        }

        private void PlotStyleApply(ScatterSeries data, PlotStyle ps, int plotIndex)
        {
            int colorIndex = plotIndex % defaultPlotColors.Length;

            data.LineColor = defaultPlotColors[colorIndex];
            data.MarkColor = defaultPlotColors[colorIndex];
            data.FillColor = defaultPlotColors[colorIndex];
            data.BarFillColor = defaultPlotColors[colorIndex];

            if (ps == PlotStyle.Lines || ps == PlotStyle.Splines)
            {
                int lineStyleIndex = (plotIndex - colorIndex) / defaultPlotColors.Length % defaultLineStyle.Length;
                data.LineStyle = defaultLineStyle[lineStyleIndex];
                if (ps == PlotStyle.Splines)
                    data.Interpolation = Interpolation.Spline;
            }
            else if (ps == PlotStyle.LinesMarkers || ps == PlotStyle.SplinesMarkers)
            {
                int markerStyleIndex = (plotIndex - colorIndex) / defaultPlotColors.Length % defaultMarkerStyle.Length;
                data.MarkStyle = defaultMarkerStyle[markerStyleIndex];
                if (ps == PlotStyle.SplinesMarkers)
                    data.Interpolation = Interpolation.Spline;
            }
            else if (ps == PlotStyle.Markers)
            {
                int markerStyleIndex = plotIndex % defaultMarkerStyle.Length;
                data.LineStyle = LineStyle.None;
                data.MarkStyle = defaultMarkerStyle[markerStyleIndex];
            }
            else if (ps == PlotStyle.VerticalBars || ps == PlotStyle.HorisontalBars)
            {
                data.LineStyle = LineStyle.None;
                data.BarDuty = 0.7f;
                data.BarGrouping = true;
                data.BarStyle = ps == PlotStyle.HorisontalBars ? BarStyle.Horisontal : BarStyle.Vertical;
            }
        }
    }
}
