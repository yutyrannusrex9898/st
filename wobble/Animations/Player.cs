using Android.Graphics;
using System;

namespace wobble.Animations
{
    public class Player : Sprite
    {
        protected override int TopSpeed { get => 30; }

        public Player(int frameWidth, int frameHeight, Bitmap bitmap) : base(frameWidth, frameHeight, bitmap) { }

        public override void Move(double angle, double distance)
        {
            double joystickSpeedMultiplier = distance / Joystick.joystickWorkingRadius;

            xSpeed = Utils.GetXRate(angle);
            int jumpX = (int)Math.Round(TopSpeed * xSpeed * joystickSpeedMultiplier);
            if (x + jumpX + bitmap.Width >= frameWidth)
                x = frameWidth - bitmap.Width;
            else if (x + jumpX < 0)
                x = 0;
            else
                x += jumpX;

            ySpeed = Utils.GetYRate(angle);
            int jumpY = (int)Math.Round(TopSpeed * ySpeed * joystickSpeedMultiplier);
            if (y + jumpY + bitmap.Height >= frameHeight)
                y = frameHeight - bitmap.Height;
            else if (y + jumpY < 0)
                y = 0;
            else
                y += jumpY;
        }
    }
}