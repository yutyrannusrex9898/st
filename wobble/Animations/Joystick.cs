using Java.Lang;
using Android.Graphics;

namespace wobble.Animations
{
    public class Joystick
    {
        public static Point mainLocation = new Point(300, 300);

        private static int ringRadius = 150;
        private static int ringThickness = 30;
        private static Paint ringPaint = getRingPaint();

        private static int centerPointRadius = 30;
        private static Paint centerPointPaint = getCenterPointPaint();

        private static int trackingPointRadius = 25;
        private static Paint trackingPointPaint = getTrackingPointPaint();

        private static Paint getRingPaint()
        {
            Paint paint = new Paint();
            paint.StrokeWidth = ringThickness;
            paint.Color = Color.Black;
            paint.SetStyle(Paint.Style.Stroke);
            paint.Alpha = 32;
            return paint;
        }

        private static Paint getCenterPointPaint()
        {
            Paint paint = new Paint();
            paint.Color = Color.White;
            paint.SetStyle(Paint.Style.Fill);
            paint.Alpha = 64;
            return paint;
        }

        private static Paint getTrackingPointPaint()
        {
            Paint paint = new Paint();
            paint.Color = Color.Gray;
            paint.SetStyle(Paint.Style.Fill);
            paint.Alpha = 128;
            return paint;
        }

        private int frameWidth;
        private int frameHeight;
        private Point trackingPointLocation;

        public Joystick(int frameWidth, int frameHeight)
        {
            this.trackingPointLocation = new Point(mainLocation);
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
        }

        public void Draw(Canvas canvas)
        {
            drawRing(canvas);
            drawCenterPoint(canvas);
            drawTrackingPoint(canvas);
        }

        private void drawRing(Canvas canvas)
        {
            canvas.DrawCircle(mainLocation.X, mainLocation.Y, ringRadius, ringPaint);
        }
        private void drawCenterPoint(Canvas canvas)
        {
            canvas.DrawCircle(mainLocation.X, mainLocation.Y, centerPointRadius, centerPointPaint);
        }
        private void drawTrackingPoint(Canvas canvas)
        {
            canvas.DrawCircle(trackingPointLocation.X, trackingPointLocation.Y, trackingPointRadius, trackingPointPaint);
        }

        public void Move(double angle, double distance)
        {
            distance = Math.Min(distance, ringRadius - ringThickness);
            trackingPointLocation = Utils.GetMovedPointByAngleAndDistance(mainLocation, angle, distance);
        }
    }
}