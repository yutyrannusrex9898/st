using Android.Graphics;
using Java.Lang;
using Java.Util;
using System.Collections.Generic;
using System.Linq;

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

        public static bool IsLineRectColliding(Point lineStart, Point lineEnd, Rect rect)
        {
            bool left = IsLineLineColliding(lineStart.X, lineStart.Y, lineEnd.X, lineEnd.Y, rect.Left, rect.Top, rect.Left, rect.Bottom);
            bool right = IsLineLineColliding(lineStart.X, lineStart.Y, lineEnd.X, lineEnd.Y, rect.Right, rect.Top, rect.Right, rect.Bottom);
            bool top = IsLineLineColliding(lineStart.X, lineStart.Y, lineEnd.X, lineEnd.Y, rect.Left, rect.Top, rect.Right, rect.Top);
            bool bottom = IsLineLineColliding(lineStart.X, lineStart.Y, lineEnd.X, lineEnd.Y, rect.Left, rect.Bottom, rect.Right, rect.Bottom);

            return left || right || top || bottom;
        }

        public static bool IsLineLineColliding(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            // calculate the direction of the lines
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // if uA and uB are between 0-1, lines are colliding
            return uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1;
        }

        public static Point GetBorderPoint(Point origin, Point target, int frameWidth, int frameHeight)
        {
            LinkedList<Point> interceptingPoints = getInterceptingPoints(origin, target, frameWidth, frameHeight);
            Point[] pointsWithinBorders = filterOutPointsOutsideBorders(interceptingPoints, frameWidth, frameHeight);
            Point closestPointToTarget = getPointAcrossTarget(origin, target, pointsWithinBorders);

            return closestPointToTarget;
        }

        private static LinkedList<Point> getInterceptingPoints(Point origin, Point target, int frameWidth, int frameHeight)
        {
            LinkedList<Point> interceptingPoints = new LinkedList<Point>();

            int deltaY = target.Y - origin.Y;
            int deltaX = target.X - origin.X;

            if (deltaX == 0)
            {
                interceptingPoints.AddLast(new Point(origin.X, 0));
                interceptingPoints.AddLast(new Point(origin.X, frameHeight));
            }
            else if (deltaY == 0)
            {
                interceptingPoints.AddLast(new Point(0, origin.Y));
                interceptingPoints.AddLast(new Point(frameWidth, origin.Y));
            }
            else
            {
                double m = deltaY / (deltaX * 1.0);

                int x = origin.X;
                int y = origin.Y;
                double b = y - m * x;

                int x1 = (int)(-b / (m * 1.0));
                interceptingPoints.AddLast(new Point(x1, 0));

                int x2 = (int)(1.0 * (frameHeight - b) / (m * 1.0));
                interceptingPoints.AddLast(new Point(x2, frameHeight));

                int y1 = (int)b;
                interceptingPoints.AddLast(new Point(0, y1));

                int y2 = (int)(1.0 * m * frameWidth + b);
                interceptingPoints.AddLast(new Point(frameWidth, y2));
            }

            return interceptingPoints;
        }

        private static Point[] filterOutPointsOutsideBorders(LinkedList<Point> uniquePoints, int frameWidth, int frameHeight)
        {
            return uniquePoints.Where(currentPoint => isWithinBorders(currentPoint, frameWidth, frameHeight)).ToArray();
        }

        private static bool isWithinBorders(Point p, int frameWidth, int frameHeight)
        {
            return p.X >= 0 && p.X <= frameWidth && p.Y >= 0 && p.Y <= frameHeight;
        }

        private static Point getPointAcrossTarget(Point origin, Point target, Point[] pointsWithinBorders)
        {
            for (int i = 0; i < pointsWithinBorders.Length; i++)
            {
                Point borderPoint = pointsWithinBorders[i];
                double originToBorder = GetDistanceBetweenPoints(origin, borderPoint);
                double targetToBorder = GetDistanceBetweenPoints(target, borderPoint);
                if (targetToBorder < originToBorder)
                {
                    return pointsWithinBorders[i];
                }
            }
            return null;
        }
    }
}