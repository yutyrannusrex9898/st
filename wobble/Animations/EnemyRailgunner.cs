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

        private AbilityHandler shotAbility = new AbilityHandler(300, 150);
        private Vector Laser;

        public EnemyRailgunner(int frameWidth, int frameHeight, Resources resources, Sprite target, Vector initVector) : base(frameWidth, frameHeight, resources, target, initVector)
        {
            SetBitmap(BitmapFactory.DecodeResource(resources, Resource.Drawable.RailGunner));
            this.Angle = Utils.getRandomAngle();
            this.Laser = null;
        }

        public void CalculateNextMovement()
        {
            CalculateNextSpriteAngle();
            CalculateNextPosition();
        }

        public void CalculateNextSpriteAngle()
        {
            double angle = Utils.GetAngleBetweenPoints(GetLocation(), target.GetLocation());
            float degrees = (Math.Abs(Utils.RadiansToDegrees(angle)) + 90) % 360;
            int bitmapIndex = (int)(degrees / bitmapAngleSize);
            currentBitmap = rotatedBitmaps[bitmapIndex];
        }

        public override void CalculateNextPosition()
        {
            if (Laser == null)
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
            if (shotAbility.IsCoolingdown())
            {
                shotAbility.ReduceCooldownTimer();
            }
            else if (shotAbility.IsActive())
            {
                SetLaserVector();
                shotAbility.ReduceAbilityTimer();
            }
            else
            {
                Laser = null;
                shotAbility.ResetCooldownTimer();
                shotAbility.ResetAbilityTimer();
            }
        }

        public void SetLaserVector()
        {
            if (Laser == null)
            {
                Point center = GetCenterPoint();
                Laser = new Vector(center.X, center.Y, Utils.GetAngleBetweenPoints(center, target.GetCenterPoint()));
            }
        }

        public override void Draw(Canvas canvas)
        {
            if (shotAbility.AbilityTimeLeft > 100)
                drawLaserPath(canvas);
            else
                drawActiveLaser(canvas);

            base.Draw(canvas);
        }

        private void drawLaserPath(Canvas canvas)
        {
            Paint paint = new Paint();
            paint.StrokeWidth = 30;
            paint.Color = Color.DeepSkyBlue;
            paint.Alpha = 128;
            paint.SetStyle(Paint.Style.Stroke);


            Point a = target.GetCenterPoint();

            if (Laser != null)
            {
                canvas.DrawLine(Laser.X, Laser.Y, a.X, a.Y, paint);
            }
        }

        private void drawActiveLaser(Canvas canvas)
        {
        }
    }
}