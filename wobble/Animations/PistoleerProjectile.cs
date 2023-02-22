using Android.Graphics;

namespace wobble.Animations
{
    public class PistoleerProjectile : Enemy
    {
        protected override int TopSpeed => 15;

        protected override int Width => 20;

        protected override int Height => 20;


        public PistoleerProjectile(int frameWidth, int frameHeight, Bitmap bitmap, Sprite target, Vector initVector) : base(frameWidth, frameHeight, bitmap, target, initVector) { }

        public void SetInitLocation(int x, int y, double angle)
        {
            this.x = x;
            this.y = y;
            this.Angle = angle;
        }

        public override void CalculateNextPosition()
        {
            CalculateNextLocation();
        }

        public bool IsOutsideBorder()
        {
            int jumpX = CalculateXJump();
            int jumpY = CalculateYJump();

            return IsTouchingLeftBorder(jumpX) || IsTouchingRightBorder(jumpX) || IsTouchingTopBorder(jumpY) || IsTouchingBottomBorder(jumpY);
        }
    }
}

