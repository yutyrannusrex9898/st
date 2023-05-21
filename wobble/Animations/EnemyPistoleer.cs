using Android.Content.Res;
using Android.Graphics;

namespace wobble.Animations
{
    public class EnemyPistoleer : Enemy
    {
        protected override int Width => 90;
        protected override int Height => 90;
        protected override int TopSpeed => 10;

        private AbilityHandler shotCountdown = new AbilityHandler(150);
        public PistoleerProjectile projectile;

        public EnemyPistoleer(int frameWidth, int frameHeight, Resources resources, Sprite target, Vector initVector) : base(frameWidth, frameHeight, resources, target, initVector)
        {
            SetBitmap(BitmapFactory.DecodeResource(resources, Resource.Drawable.Pistoleer));
            this.Angle = Utils.getRandomAngle();
            this.projectile = new PistoleerProjectile(frameWidth, frameHeight, resources, target, this.InitVector);
        }

        public void Shoot()
        {
            Bitmap a = BitmapFactory.DecodeResource(resources, Resource.Drawable.Projectile);
            double angle = Utils.GetAngleBetweenPoints(this.GetCurrentLocalPoint(), target.GetCurrentLocalPoint());
            projectile.SetInitLocation(this.x, this.y, angle);
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

        public void CalculateNextMovement()
        {
            base.CalculateNextMovement();
            projectile.CalculateNextMovement();
        }

        private void HandleShooting()
        {
            if (this.isAlive)
            {
                projectile.CalculateNextPosition();

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
        }

        public new bool IsColliding(Sprite other)
        {
            return base.IsColliding(other) || this.projectile.IsColliding(other);
        }

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
            projectile.Draw(canvas);
        }
    }
}