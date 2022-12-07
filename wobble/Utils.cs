using Java.Lang;
using Android.Graphics;

namespace wobble
{
    public static class Utils
    {
        public static double GetAngleBetweenPoints(Point p1, Point p2)
        {
            return Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }
        internal static double GetDistanceBetweenPoints(Point p1, Point p2)
        {
            int a = p1.X - p2.X;
            int b = p1.Y - p2.Y;
            return Math.Sqrt(a * a + b * b);
        }

        public static double GetXRate(double angle)
        {
            return Math.Cos(angle);
        }

        public static double GetYRate(double angle)
        {
            return Math.Sin(angle);
        }

        internal static Point GetMovedPointByAngleAndDistance(Point point, double angle, double distance)
        {
            int x = (int)(GetXRate(angle) * distance);
            int y = (int)(GetYRate(angle) * distance);
            return new Point(point.X + x, point.Y + y);
        }
    }
}