using Java.Lang;
using Android.Graphics;
using Xamarin.Essentials;

namespace wobble.Animations
{
    public class Joystick : ControlledSprite
    {
        public static Point mainLocation = new Point(300, 300);

        public static int ringRadius = 150;
        public static int ringThickness = 30;
        public static int joystickWorkingRadius = ringRadius - ringThickness;
        public static Paint ringPaint = getRingPaint();

        public static int centerPointRadius = 30;
        public static Paint centerPointPaint = getCenterPointPaint();

        public static int trackingPointRadius = 25;
        public static Paint trackingPointPaint = getTrackingPointPaint();

        protected override int TopSpeed { get => 0; }
        protected override int Width { get => 0; }
        protected override int Height { get => 0; }

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

        public Joystick(int frameWidth, int frameHeight) : base(frameWidth, frameWidth, null) { }

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
            canvas.DrawCircle(this.x, this.y, trackingPointRadius, trackingPointPaint);
        }

        public override void CalculateNextControlledMovement(double angle, double distance)
        {
            CalculateNextPosition(angle, distance);
        }

        public override void CalculateNextPosition(double angle, double distance)
        {
            Point trackingPointLocation = Utils.GetMovedPointByAngleAndDistance(mainLocation, angle, distance);
            this.x = trackingPointLocation.X;
            this.y = trackingPointLocation.Y;
        }
    }
}