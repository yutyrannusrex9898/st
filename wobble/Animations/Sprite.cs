using Android.Graphics;
using System;

namespace wobble.Animations
{
    public class Sprite
    {
        private int x;
        private int y;
        private int frameWidth;
        private int frameHeight;

        int speed = 20;
        double xRate = 0.0;
        double yRate = 0.0;

        private Bitmap bitmap;

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

        public void Move(double angle, double distance)
        {
            xRate = Utils.GetXRate(angle);
            yRate = Utils.GetYRate(angle);
            Console.WriteLine(Math.Min(distance/120,1));

            int jumpX = ((int)Math.Round((speed * xRate) * (Math.Min(distance / 120, 1))));
            if (x + jumpX + bitmap.Width >= frameWidth)
                x = frameWidth - bitmap.Width;
            else if (x + jumpX < 0)
                x = 0;
            else
                x += jumpX;

            int jumpY = ((int)Math.Round((speed * yRate) * (Math.Min(distance / 120, 1))));
            if (y + jumpY + bitmap.Height >= frameHeight)
                y = frameHeight - bitmap.Height;
            else if (y + jumpY < 0)
                y = 0;
            else
                y += jumpY;
        }
    }
}