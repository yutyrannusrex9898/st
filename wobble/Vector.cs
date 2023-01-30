using Android.Graphics;

namespace wobble
{
    public class Vector
    {
        public float X { set; get; }
        public float Y { set; get; }
        public double Angle { set; get; }

        public Vector(float x, float y, double angle)
        {
            this.X = x;
            this.Y = y;
            this.Angle = angle;
        }

        public PointF GetPointF()
        {
            return new PointF(this.X, this.Y);
        }
    }
}
