using Android.Graphics;
using Java.Lang;

namespace wobble.Animations
{
    public abstract class Sprite
    {
        protected static readonly int bitmapAngles = 360;
        protected static readonly int bitmapAngleSize = 360 / bitmapAngles;

        public Vector InitVector { get; set; }
        protected abstract int Width { get; }
        protected abstract int Height { get; }
        protected double Angle { get; set; }
        protected double Distance { get; set; }
        protected abstract int TopSpeed { get; }

        protected int x;
        protected int y;

        protected int frameWidth;
        protected int frameHeight;

        protected double xSpeed = 0.0;
        protected double ySpeed = 0.0;

        protected readonly Bitmap originlBitmap;
        protected readonly Bitmap[] rotatedBitmaps;
        protected Bitmap currentBitmap;

        public Sprite(int frameWidth, int frameHeight, Bitmap bitmap, Vector initVector)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.InitVector = initVector;
            this.x = (int)(initVector.X * frameWidth);
            this.y = (int)(initVector.Y * frameHeight);

            if (bitmap != null)
            {
                this.originlBitmap = Bitmap.CreateScaledBitmap(bitmap, this.Width, this.Height, false);
                this.rotatedBitmaps = Utils.CalculateBitmaps(originlBitmap, bitmapAngles, 100, 100);
                this.currentBitmap = rotatedBitmaps[0];
            }
        }

        public abstract void Draw(Canvas canvas);

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

        public void CalculateNextMovement()
        {
            CalculateNextSpriteAngle();
            CalculateNextPosition();
        }

        public Point GetLocation()
        {
            int x = this.x + (Width / 2);
            int y = this.y + (Height / 2);
            return new Point(x, y);
        }

        protected bool IsTouchingLeftBorder(int jumpX)
        {
            return (x + jumpX < 0);
        }

        protected bool IsTouchingRightBorder(int jumpX)
        {
            return (x + this.Width + jumpX >= frameWidth);
        }

        protected bool IsTouchingTopBorder(int jumpY)
        {
            return (y + jumpY < 0);
        }

        protected bool IsTouchingBottomBorder(int jumpY)
        {
            return (y + this.Height + jumpY >= frameHeight);
        }

        protected int CalculateXJump(double speedMultiplier = 1)
        {
            xSpeed = Utils.GetXRate(Angle);
            return (int)Math.Round(TopSpeed * xSpeed * speedMultiplier);
        }

        private void CalculateNextXLocation(double speedMultiplier)
        {
            int jumpX = CalculateXJump(speedMultiplier);

            if (IsTouchingLeftBorder(jumpX))
                x = 0;

            else if (IsTouchingRightBorder(jumpX))
                x = frameWidth - this.Width;

            else
                x += jumpX;
        }

        protected int CalculateYJump(double speedMultiplier = 1)
        {
            ySpeed = Utils.GetYRate(Angle);
            return (int)Math.Round(TopSpeed * ySpeed * speedMultiplier);
        }

        private void CalculateNextYLocation(double speedMultiplier)
        {
            int jumpY = CalculateYJump(speedMultiplier);

            if (IsTouchingBottomBorder(jumpY))
                y = frameHeight - this.Height;

            else if (IsTouchingTopBorder(jumpY))
                y = 0;

            else
                y += jumpY;
        }

        protected void CalculateNextLocation(double speedMultiplier = 1)
        {
            CalculateNextXLocation(speedMultiplier);
            CalculateNextYLocation(speedMultiplier);
        }

        public Point GetInitLocalPoint()
        {
            return new Point((int)(this.InitVector.X * frameWidth), (int)(this.InitVector.Y * frameHeight));
        }

        public Point GetCurrentLocalPoint()
        {
            return new Point(this.x, this.y);
        }

        public bool IsColliding(Sprite other)
        {
            int thisLeftX = this.x;
            int thisRightX = this.x + this.Width;
            int thisTopY = this.y;
            int thisBottomY = this.y + this.Height;

            int otherLeftX = other.x;
            int otherRightX = other.x + other.Width;
            int otherTopY = other.y;
            int otherBottomY = other.y + other.Height;


            bool xCollision = (otherLeftX >= thisLeftX && otherLeftX <= thisRightX) || (otherRightX >= thisLeftX && otherRightX <= thisRightX);
            bool yCollision = (otherTopY >= thisTopY && otherTopY <= thisBottomY) || (otherBottomY >= thisTopY && otherBottomY <= thisBottomY);

            return xCollision && yCollision;
        }

        public void ResetLocation()
        {
            Point initLocationPoint = this.GetInitLocalPoint();
            this.x = initLocationPoint.X;
            this.y = initLocationPoint.Y;
            this.Angle = this.InitVector.Angle;
        }
    }
}