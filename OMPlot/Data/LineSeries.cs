using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    /// <summary>
    /// Represents a series for line plots.
    /// </summary>
    public class LineSeries : ScatterSeries
    {
        double[] Y;
        double dX, X0;

        public override double GetX(int index) { return dX * index + X0; }
        public override double GetY(int index) { return Y[index]; }

        public override void SetX(int index, double value) {  }
        public override void SetY(int index, double value) { Y[index] = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref = "LineSeries" /> class.
        /// </summary>
        /// <param name="y">Collection of ghraph values.</param>
        /// <param name="dx">Custom dX value.</param>
        /// <param name="x0">Custom start x value.</param>
        public LineSeries (IEnumerable<double> y, double dx = 1, double x0 = 0)
        {
            Y = y.ToArray();
            dX = dx;
            X0 = x0;
            minX = dX > 0 ? X0 : X0 + dX * Y.Length;
            maxX = dX < 0 ? X0 : X0 + dX * Y.Length;
            int minYIndex = -1;
            int maxYIndex = -1;
            minXPre = minX + dX;
            maxXPre = maxX - dX;
            for (int i = 0; i < Y.Length; i++)
            {
                if (minY > Y[i])
                {
                    minY = Y[i];
                    minYIndex = i;
                }
                if (maxY < Y[i])
                {
                    maxY = Y[i];
                    maxYIndex = i;
                }
            }
            for (int i = 0; i < Y.Length; i++)
            {
                if (minYPre > Y[i] && i != minYIndex)
                    minYPre = Y[i];
                if (maxYPre < Y[i] && i != maxYIndex)
                    maxYPre = Y[i];
            }
        }
        internal override int NearestPointIndex(double x, double y)
        {
            double minDistance = double.MaxValue;
            double distance;
            int index = -1;
            for (int i = 0; i < Y.Length; i++)
            {
                distance = (dX * i + X0 - x) * (dX * i + X0 - x) + (Y[i] - y) * (Y[i] - y);
                if (minDistance > distance)
                {
                    index = i;
                    minDistance = distance;
                }
            }           
            return index;
        }
        protected override PointF[] CalculatePoints()
        {            
            double maxX = HorizontalAxis.Transform(HorizontalAxis.Maximum);
            double DX = HorizontalAxis.Transform(HorizontalAxis.Minimum + dX) - HorizontalAxis.Transform(HorizontalAxis.Minimum);
            int start = 0;
            if (minX < HorizontalAxis.Minimum)
                start = (int)Math.Floor((HorizontalAxis.Minimum - minX) / dX);
            if (start >= Y.Length)
                start = Y.Length - 1;
            List<PointF> pointList = new List<PointF>();
            double prevX = HorizontalAxis.Transform(X0 + start * dX);
            double prevY = VerticalAxis.Transform(Y[start]);
            pointList.Add(new PointF((float)prevX, (float)prevY));
            double curX = prevX;
            double curY;
            for (int i = start + 1; i < Y.Length && ((DX > 0 && curX <= maxX) || (DX < 0 && curX >= maxX)); i++)
            {
                curX += DX;
                curY = VerticalAxis.Transform(Y[i]);
                if (curX - prevX > 1.5 || curY - prevY > 1.5 || prevX - curX > 1.5 || prevY - curY > 1.5)
                {
                    prevX = curX;
                    prevY = curY;
                    pointList.Add(new PointF((float)prevX, (float)prevY));
                }
            }
            return pointList.ToArray();
        }
    }
}
