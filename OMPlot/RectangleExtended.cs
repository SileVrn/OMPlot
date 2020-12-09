using System;

namespace OMPlot
{
    public struct RectangleExtended
    {
        float x, y, width, height;

        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public float Width
        {
            get { return width; }
            set { width = value; }
        }
        public float Height
        {
            get { return height; }
            set { height = value; }
        }
        public float Left
        {
            get { return x; }
            set { width -= value - x; x = value;  }
        }
        public float Top
        {
            get { return y; }
            set { height -= value - y; y = value; }
        }
        public float Right
        {
            get { return x + width; }
            set { width = value - x; }
        }
        public float Bottom
        {
            get { return y + height; }
            set { height = value - y; }
        }
        public float CenterX
        {
            get { return x + (float)Math.Round(width / 2.0); }
            set { x = value - (float)Math.Round(width / 2.0); }
        }
        public float CenterY
        {
            get { return y + (float)Math.Round(height / 2.0); }
            set{ y = value - (float)Math.Round(height / 2.0); }
        }
        public float FullScaleX
        {
            get { return Math.Abs(width); }
            set
            {
                float center = CenterX;
                if (width < 0)
                    width = -value;
                else
                    width = value;
                CenterX = center;
            }
        }
        public float FullScaleY
        {
            get { return Math.Abs(height); }
            set
            {
                float center = CenterY;
                if (height < 0)
                    height = -value;
                else
                    height = value;
                CenterY = center;
            }
        }

        public bool InRectangle(float pointX, float pointY)
        {
            return pointX >= Math.Min(Left, Right) && pointX <= Math.Max(Left, Right) && pointY >= Math.Min(Top, Bottom) && pointY <= Math.Max(Top, Bottom);
        }

        public RectangleExtended(float X, float Y, float Width, float Height)
        {
            x = X;
            y = Y;
            width = Width;
            height = Height;
        }
    }
}
