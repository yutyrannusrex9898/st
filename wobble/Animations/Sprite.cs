using Android.Content.Res;
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
        protected Resources resources;

        protected double xSpeed = 0.0;
        protected double ySpeed = 0.0;
        public bool isAlive = true;


        protected Bitmap originlBitmap;
        protected Bitmap[] rotatedBitmaps;
        protected Bitmap currentBitmap;

        public Sprite(int frameWidth, int frameHeight, Resources resources, Vector initVector)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.resources = resources;
            this.InitVector = initVector;
            this.x = (int)(initVector.X * frameWidth);
            this.y = (int)(initVector.Y * frameHeight);
        }

        protected void SetBitmap(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                this.originlBitmap = bitmap;
                this.rotatedBitmaps = Utils.CalculateBitmaps(originlBitmap, bitmapAngles, this.Width, this.Height);
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

        private void CalculateNextXLocation(double speedMultiplier, bool limitToFrame = true)
        {
            int jumpX = CalculateXJump(speedMultiplier);

            x += jumpX;

            if (limitToFrame)
            {
                if (IsTouchingLeftBorder(jumpX))
                    x = 0;

                else if (IsTouchingRightBorder(jumpX))
                    x = frameWidth - this.Width;
            }
        }

        protected int CalculateYJump(double speedMultiplier = 1)
        {
            ySpeed = Utils.GetYRate(Angle);
            return (int)Math.Round(TopSpeed * ySpeed * speedMultiplier);
        }

        private void CalculateNextYLocation(double speedMultiplier, bool limitToFrame = true)
        {
            int jumpY = CalculateYJump(speedMultiplier);
            y += jumpY;

            if (limitToFrame)
            {

                if (IsTouchingBottomBorder(jumpY))
                    y = frameHeight - this.Height;

                else if (IsTouchingTopBorder(jumpY))
                    y = 0;
            }
        }

        protected void CalculateNextLocation(double speedMultiplier = 1, bool limitToFrame = true)
        {
            CalculateNextXLocation(speedMultiplier, limitToFrame);
            CalculateNextYLocation(speedMultiplier, limitToFrame);
        }

        public Point GetInitLocalPoint()
        {
            return new Point((int)(this.InitVector.X * frameWidth), (int)(this.InitVector.Y * frameHeight));
        }

        public Point GetCenterPoint()
        {
            return new Point(this.x + this.Width / 2, this.y + this.Height / 2);
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
            this.Angle = Utils.getRandomAngle();
        }

        public void Reset()
        {
            isAlive = true;
            ResetLocation();
        }
    }
}