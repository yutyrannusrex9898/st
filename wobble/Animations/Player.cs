using Android.Graphics;
using System;

namespace wobble.Animations
{
    public class Player : ControlledSprite
    {
        protected override int TopSpeed { get => 30; }
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
            double joystickSpeedMultiplier = distance / Joystick.joystickWorkingRadius;

            xSpeed = Utils.GetXRate(angle);
            int jumpX = (int)Math.Round(TopSpeed * xSpeed * joystickSpeedMultiplier);
            if (x + jumpX + this.Width >= frameWidth)
                x = frameWidth - this.Width;
            else if (x + jumpX < 0)
                x = 0;
            else
                x += jumpX;

            ySpeed = Utils.GetYRate(angle);
            int jumpY = (int)Math.Round(TopSpeed * ySpeed * joystickSpeedMultiplier);
            if (y + jumpY + this.Height >= frameHeight)
                y = frameHeight - this.Height;
            else if (y + jumpY < 0)
                y = 0;
            else
                y += jumpY;
        }
    }
}