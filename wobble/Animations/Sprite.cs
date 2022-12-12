using Android.Graphics;
using System;

namespace wobble.Animations
{
    public abstract class Sprite
    {
        protected abstract int TopSpeed { get; }

        protected int x;
        protected int y;
        protected int frameWidth;
        protected int frameHeight;

        protected double xSpeed = 0.0;
        protected double ySpeed = 0.0;

        protected readonly Bitmap bitmap;

        public Sprite(int frameWidth, int frameHeight, Bitmap bitmap)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.x = 0;
            this.y = 0;
            this.bitmap = Bitmap.CreateScaledBitmap(bitmap, 100, 100, false);
        }

        public void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(bitmap, x, y, null);
        }

        public abstract void Move(double angle, double distance);
    }
}