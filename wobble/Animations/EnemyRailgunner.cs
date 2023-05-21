using Android.Content.Res;
using Android.Graphics;

namespace wobble.Animations
{
    public class EnemyRailgunner : Enemy
    {
        protected override int Width => 90;
        protected override int Height => 90;
        protected override int TopSpeed => 10;

        private AbilityHandler shotCountdown = new AbilityHandler(150);

        public EnemyRailgunner(int frameWidth, int frameHeight, Resources resources, Sprite target, Vector initVector) : base(frameWidth, frameHeight, resources, target, initVector)
        {
            SetBitmap(BitmapFactory.DecodeResource(resources, Resource.Drawable.Rammer));
            this.Angle = Utils.getRandomAngle();
        }

        public void Shoot()
        {
            double angle = Utils.GetAngleBetweenPoints(this.GetCurrentLocalPoint(), target.GetCurrentLocalPoint());
        }

        public override void CalculateNextPosition()
        {
            int jumpX = CalculateXJump();
            if (IsTouchingLeftBorder(jumpX) || IsTouchingRightBorder(jumpX))
            {
                Angle = Utils.MirrorAngleHorizontally(Angle);
            }

            int jumpY = CalculateYJump();
            if (IsTouchingTopBorder(jumpY) || IsTouchingBottomBorder(jumpY))
            {
                Angle = Utils.MirrorAngleVertically(Angle);
            }

            CalculateNextLocation();

            HandleShooting();
        }

        private void HandleShooting()
        {
            if (shotCountdown.HasAbilityTimeLeft())
            {
                shotCountdown.ReduceTimer();
            }
            else
            {
                Shoot();
                shotCountdown.ResetAbilityTimer();
            }
        }

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
        }
    }
}