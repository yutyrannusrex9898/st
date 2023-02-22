using Android.Graphics;
using Java.Lang;
using Java.Util;

namespace wobble
{
    public static class Utils
    {
        static Random rnd = new Random();

        public static double getRandomAngle()
        {
            return rnd.NextDouble() * 2 * Math.Pi;
        }

        public static double GetAngleBetweenPoints(Point p1, Point p2)
        {
            return Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }

        public static double GetDistanceBetweenPoints(Point p1, Point p2)
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

        public static Point GetMovedPointByAngleAndDistance(Point point, double angle, double distance)
        {
            int x = (int)(GetXRate(angle) * distance);
            int y = (int)(GetYRate(angle) * distance);
            return new Point(point.X + x, point.Y + y);
        }

        public static Bitmap[] CalculateBitmaps(Bitmap originlBitmap, int bitmapAngles, int width, int height)
        {
            Bitmap[] rotatedBitmaps = new Bitmap[bitmapAngles];
            for (int i = 0; i < bitmapAngles; i++)
            {
                float degree = 360 / bitmapAngles * i;
                Matrix matrix = new Matrix();
                matrix.PostRotate(degree);
                Bitmap scaledBitmap = Bitmap.CreateScaledBitmap(originlBitmap, width, height, true);
                Bitmap rotatedBitmap = Bitmap.CreateBitmap(scaledBitmap, 0, 0, scaledBitmap.Width, scaledBitmap.Height, matrix, true);
                rotatedBitmaps[i] = rotatedBitmap;
            }
            return rotatedBitmaps;
        }

        public static float RadiansToDegrees(double radians)
        {
            radians += 2 * Math.Pi;
            radians %= 2 * Math.Pi;
            return (float)(180 / Math.Pi * radians);
        }

        public static double InvertAngle(double angle)
        {
            return (angle + Math.Pi) % (2 * Math.Pi);
        }

        public static double MirrorAngleHorizontally(double angle)
        {
            return -1 * InvertAngle(angle);
        }

        public static double MirrorAngleVertically(double angle)
        {
            return InvertAngle(Math.Pi - angle);
        }
    }
}