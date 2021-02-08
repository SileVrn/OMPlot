using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMPlot.Data
{
    public class LineSeries : XYSeries
    {
        double[] Y;
        double dX, X0;
        public LineSeries (IEnumerable<double> y, double dx, double x0 = 0)
        {
            Y = y.ToArray();
            dX = dx;
            X0 = x0;
            MinimumX = dX > 0 ? X0 : X0 + dX * Y.Length;
            MaximumX = dX < 0 ? X0 : X0 + dX * Y.Length;
            for (int i = 0; i < Y.Length; i++)
            {
                MinimumY = MinimumY > Y[i] ? Y[i] : MinimumY;
                MaximumY = MaximumY < Y[i] ? Y[i] : MaximumY;
            }
        }
        protected override PointF[] CalculatePoints()
        {
            double minx = HorizontalAxis.TransformBack(HorizontalAxis.Minimum);
            double maxX = HorizontalAxis.Transform(HorizontalAxis.Maximum);
            double DX = HorizontalAxis.Transform(minx + dX) - HorizontalAxis.Minimum;
            int start = 0;
            if (MinimumX < HorizontalAxis.Minimum)
                start = (int)Math.Floor((HorizontalAxis.Minimum - MinimumX) / dX);
            List<PointF> pointList = new List<PointF>();
            double prevX = HorizontalAxis.Transform(X0 + start * dX);
            double prevY = VerticalAxis.Transform(Y[start]);
            pointList.Add(new PointF((float)prevX, (float)prevY));
            double curX = prevX;
            double curY;
            for (int i = start + 1; i < Y.Length && curX <= maxX; i++)
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
