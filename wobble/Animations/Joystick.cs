﻿using Android.Graphics;

namespace wobble.Animations
{
    public class Joystick : ControlledSprite
    {
        private static int ringRadius = 150;
        private static int ringThickness = 30;
        public static int joystickWorkingRadius = ringRadius - ringThickness;

        private static int centerPointRadius = 30;
        private static int trackingPointRadius = 25;

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

        protected override int TopSpeed { get => 0; }
        protected override int Width { get => 0; }
        protected override int Height { get => 0; }

        private Paint ringPaint = getRingPaint();
        private Paint centerPointPaint = getCenterPointPaint();
        private Paint trackingPointPaint = getTrackingPointPaint();

        private Point MainLocation { get; }

        public Joystick(int frameWidth, int frameHeight, Vector initVector) : base(frameWidth, frameHeight, null, initVector)
        {
            MainLocation = this.GetInitLocalPoint();
        }

        public override void Draw(Canvas canvas)
        {
            DrawRing(canvas);
            DrawCenterPoint(canvas);
            DrawTrackingPoint(canvas);
        }

        private void DrawRing(Canvas canvas)
        {
            canvas.DrawCircle(MainLocation.X, MainLocation.Y, ringRadius, ringPaint);
        }

        private void DrawCenterPoint(Canvas canvas)
        {
            canvas.DrawCircle(MainLocation.X, MainLocation.Y, centerPointRadius, centerPointPaint);
        }

        private void DrawTrackingPoint(Canvas canvas)
        {
            canvas.DrawCircle(this.x, this.y, trackingPointRadius, trackingPointPaint);
        }

        public override void CalculateNextPosition()
        {
            Point trackingPointLocation = Utils.GetMovedPointByAngleAndDistance(this.GetInitLocalPoint(), Angle, Distance);
            this.x = trackingPointLocation.X;
            this.y = trackingPointLocation.Y;
        }
    }
}