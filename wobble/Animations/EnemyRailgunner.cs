using Android.Content.Res;
using Android.Graphics;
using Java.Lang;

namespace wobble.Animations
{
    public class EnemyRailgunner : Enemy
    {
        protected override int Width => 90;
        protected override int Height => 90;
        protected override int TopSpeed => 10;

        private AbilityHandler laserAbility = new AbilityHandler(100, 200);
        private Laser laser;

        public EnemyRailgunner(int frameWidth, int frameHeight, Resources resources, Sprite target, Vector initVector) : base(frameWidth, frameHeight, resources, target, initVector)
        {
            SetBitmap(BitmapFactory.DecodeResource(resources, Resource.Drawable.RailGunner));
            this.Angle = Utils.getRandomAngle();
            laser = new Laser(frameWidth, frameHeight, resources, target, initVector);
        }

        public void CalculateNextMovement()
        {
            CalculateNextSpriteAngle();
            CalculateNextPosition();
            laser.CalculateNextMovement();
        }

        public void CalculateNextSpriteAngle()
        {
            if (laserAbility.IsCoolingdown())
            {
                double angle = Utils.GetAngleBetweenPoints(this.GetCenterPoint(), target.GetCenterPoint());
                float degrees = (Math.Abs(Utils.RadiansToDegrees(angle)) + 90) % 360;
                int bitmapIndex = (int)(degrees / bitmapAngleSize);
                currentBitmap = rotatedBitmaps[bitmapIndex];
            }
        }

        public override void CalculateNextPosition()
        {
            if (laserAbility.IsCoolingdown())
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
            }

            HandleShooting();
        }

        private void HandleShooting()
        {
            if (laserAbility.IsCoolingdown())
            {
                laserAbility.ReduceAbilityTimer();
            }
            else if (laserAbility.IsActive())
            {
                Point centerPoint = this.GetCenterPoint();
                laser.UpdateLocation(centerPoint.X, centerPoint.Y);
                laser.fire();
                laserAbility.ReduceAbilityTimer();
            }
            else
            {
                laserAbility.Reset();
            }
        }

        public new bool IsColliding(Sprite other)
        {
            return base.IsColliding(other) || this.laser.IsColliding(other);
        }

        public override void Draw(Canvas canvas)
        {
            laser.Draw(canvas);
            base.Draw(canvas);
        }

        public new void Reset()
        {
            base.Reset();
            laser.Reset();
            laserAbility.Reset();
        }
    }
}