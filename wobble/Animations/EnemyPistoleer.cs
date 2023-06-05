using Android.Content.Res;
using Android.Graphics;
using Java.Lang;

namespace wobble.Animations
{
    public class EnemyPistoleer : Enemy
    {
        protected override int Width => 90;
        protected override int Height => 90;
        protected override int TopSpeed => 10;

        private AbilityHandler shotAbility = new AbilityHandler(0, 150);
        public PistoleerProjectile projectile;

        public EnemyPistoleer(int frameWidth, int frameHeight, Resources resources, Sprite target, Vector initVector) : base(frameWidth, frameHeight, resources, target, initVector)
        {
            SetBitmap(BitmapFactory.DecodeResource(resources, Resource.Drawable.Pistoleer));
            this.Angle = Utils.getRandomAngle();
            this.projectile = new PistoleerProjectile(frameWidth, frameHeight, resources, target, this.InitVector);
        }

        public void Shoot()
        {
            double angle = Utils.GetAngleBetweenPoints(this.GetCenterPoint(), target.GetCenterPoint());
            projectile.SetInitLocation(this.x, this.y, angle);
        }

        public new void CalculateNextMovement()
        {
            base.CalculateNextMovement();
            projectile.CalculateNextMovement();

            CalculateNextSpriteAngle();
            CalculateNextPosition();
        }

        public void CalculateNextSpriteAngle()
        {
            double angle = Utils.GetAngleBetweenPoints(this.GetCenterPoint(), target.GetCenterPoint());
            float degrees = (Math.Abs(Utils.RadiansToDegrees(angle)) + 90) % 360;
            int bitmapIndex = (int)(degrees / bitmapAngleSize);
            currentBitmap = rotatedBitmaps[bitmapIndex];
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
            if (this.isAlive)
            {
                projectile.CalculateNextPosition();

                if (shotAbility.IsCoolingdown())
                {
                    shotAbility.ReduceAbilityTimer();
                }
                else
                {
                    Shoot();
                    shotAbility.Reset();
                }
            }
        }

        public new bool IsColliding(Sprite other)
        {
            return base.IsColliding(other) || this.projectile.IsColliding(other);
        }

        public override void Draw(Canvas canvas)
        {
            projectile.Draw(canvas);
            base.Draw(canvas);
        }

        public new void Reset()
        {
            base.Reset();
            projectile.Reset();
            shotAbility.Reset();
        }
    }
}