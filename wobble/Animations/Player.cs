using Android.Graphics;
using System;

namespace wobble.Animations
{
    public class Player : ControlledSprite
    {
        private static int DashMaxTimer { get => 7; }

        protected override int TopSpeed { get => 20; }
        private int DashMultiplier { get => 3; }
        private int DashTimer { set; get; }
        protected override int Width { get => 100; }
        protected override int Height { get => 100; }

        public Player(int frameWidth, int frameHeight, Bitmap bitmap) : base(frameWidth, frameHeight, bitmap) { }

        public override void CalculateNextControlledMovement(double angle, double distance)
        {
            CalculateNextAngle(angle);
            CalculateNextPosition(angle, distance);
        }

        public override void CalculateNextPosition(double angle, double distance)
        {
            double speedMultiplier = CalculateSpeedMultiplier(distance);
            CalculateNextXLocation(angle, distance, speedMultiplier);
            CalculateNextYLocation(angle, distance, speedMultiplier);
            if (DashTimer > 0) DashTimer--;
        }

        private double CalculateSpeedMultiplier(double distance)
        {
            if (DashTimer > 0)
                return DashMultiplier;
            else
                return CalculateJoystickMultiplier(distance);
        }

        private double CalculateJoystickMultiplier(double distance)
        {
            return distance / Joystick.joystickWorkingRadius;
        }

        private void CalculateNextXLocation(double angle, double distance, double speedMultiplier)
        {
            xSpeed = Utils.GetXRate(angle);
            int jumpX = (int)Math.Round(TopSpeed * xSpeed * speedMultiplier);
            if (x + jumpX + this.Width >= frameWidth)
                x = frameWidth - this.Width;
            else if (x + jumpX < 0)
                x = 0;
            else
                x += jumpX;
        }

        private void CalculateNextYLocation(double angle, double distance, double speedMultiplier)
        {
            ySpeed = Utils.GetYRate(angle);
            int jumpY = (int)Math.Round(TopSpeed * ySpeed * speedMultiplier);
            if (y + jumpY + this.Height >= frameHeight)
                y = frameHeight - this.Height;
            else if (y + jumpY < 0)
                y = 0;
            else
                y += jumpY;
        }

        public void InitDash()
        {
            DashTimer = DashMaxTimer;
        }
    }
}