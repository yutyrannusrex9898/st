using Android.Content.Res;
using Android.Graphics;

namespace wobble.Animations
{
    public class PistoleerProjectile : Enemy
    {
        protected override int TopSpeed => 10;

        protected override int Width => 40;

        protected override int Height => 40;

        public PistoleerProjectile(int frameWidth, int frameHeight, Resources resources, Sprite target, Vector initVector) : base(frameWidth, frameHeight, resources, target, initVector)
        {
            SetBitmap(BitmapFactory.DecodeResource(resources, Resource.Drawable.Projectile));
            SetInitLocation(-1000, -1000, 0);
        }

        public void SetInitLocation(int x, int y, double angle)
        {
            this.x = x;
            this.y = y;
            this.Angle = angle;
        }

        public override void CalculateNextPosition()
        {
            CalculateNextLocation(1, false);
        }

        public bool IsOutsideBorder()
        {
            int jumpX = CalculateXJump();
            int jumpY = CalculateYJump();

            return IsTouchingLeftBorder(jumpX) || IsTouchingRightBorder(jumpX) || IsTouchingTopBorder(jumpY) || IsTouchingBottomBorder(jumpY);
        }
    }
}

