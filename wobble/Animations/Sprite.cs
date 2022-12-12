using Android.Graphics;
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
        protected int Angle { get; set; }

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

        public void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(currentBitmap, x, y, null);
        }

        public void CalculateNextAngle(double radians)
        {
            float degrees = (Math.Abs(Utils.RadiansToDegrees(radians)) + 90) % 360;
            int bitmapIndex = (int)(degrees / bitmapAngleSize);
            System.Console.WriteLine(radians + " - " + degrees + " - " + bitmapIndex);
            currentBitmap = rotatedBitmaps[bitmapIndex];
        }
    }
}