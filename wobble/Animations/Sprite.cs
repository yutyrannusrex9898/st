﻿using Android.Graphics;
using Java.Lang;

namespace wobble.Animations
{
    public abstract class Sprite
    {
        protected static readonly int bitmapAngles = 360;
        protected static readonly int bitmapAngleSize = 360 / bitmapAngles;

        protected abstract int TopSpeed { get; }
        protected abstract int Width { get; }
        protected abstract int Height { get; }
        protected double Angle { get; set; }
        protected double Distance { get; set; }


        protected int x = 0;
        protected int y = 0;

        protected int frameWidth;
        protected int frameHeight;

        protected double xSpeed = 0.0;
        protected double ySpeed = 0.0;

        protected readonly Bitmap originlBitmap;
        protected readonly Bitmap[] rotatedBitmaps;
        protected Bitmap currentBitmap;

        public Sprite(int frameWidth, int frameHeight, Bitmap bitmap)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.x = 0;
            this.y = 0;

            if (bitmap != null)
            {
                this.originlBitmap = Bitmap.CreateScaledBitmap(bitmap, this.Width, this.Height, false);
                this.rotatedBitmaps = Utils.CalculateBitmaps(originlBitmap, bitmapAngles, 100, 100);
                this.currentBitmap = rotatedBitmaps[0];
            }
        }

        public abstract void Draw(Canvas canvas);

        public void UpdateAngleAndDistance(double angle, double distance)
        {
            this.Angle = angle;
            this.Distance = distance;
        }

        public void CalculateNextSpriteAngle()
        {
            if (this.currentBitmap != null)
            {
                float degrees = (Math.Abs(Utils.RadiansToDegrees(Angle)) + 90) % 360;
                int bitmapIndex = (int)(degrees / bitmapAngleSize);
                currentBitmap = rotatedBitmaps[bitmapIndex];
            }
        }

        public abstract void CalculateNextPosition();

        public Point GetLocation()
        {
            int x = this.x + (Width / 2);
            int y = this.y + (Height / 2);
            return new Point(x, y);
        }

        protected void CalculateNextXLocation(double speedMultiplier)
        {
            xSpeed = Utils.GetXRate(Angle);
            int jumpX = (int)Math.Round(TopSpeed * xSpeed * speedMultiplier);
            if (x + jumpX + this.Width >= frameWidth)
                x = frameWidth - this.Width;
            else if (x + jumpX < 0)
                x = 0;
            else
                x += jumpX;
        }

        protected void CalculateNextYLocation(double speedMultiplier)
        {
            ySpeed = Utils.GetYRate(Angle);
            int jumpY = (int)Math.Round(TopSpeed * ySpeed * speedMultiplier);
            if (y + jumpY + this.Height >= frameHeight)
                y = frameHeight - this.Height;
            else if (y + jumpY < 0)
                y = 0;
            else
                y += jumpY;
        }
    }
}