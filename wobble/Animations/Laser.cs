using Android.Content.Res;
using Android.Graphics;

namespace wobble.Animations
{
    public class Laser : Enemy
    {
        private static Paint getLaserPathPaint()
        {
            Paint paint = new Paint();
            paint.StrokeWidth = 30;
            paint.Color = Color.DeepSkyBlue;
            paint.Alpha = 128;
            paint.SetStyle(Paint.Style.Stroke);

            return paint;
        }

        private static Paint getActiveLaserPaint()
        {
            Paint paint = new Paint();
            paint.StrokeWidth = 20;
            paint.Color = Color.Red;
            paint.Alpha = 128;
            paint.SetStyle(Paint.Style.Stroke);
            return paint;
        }

        protected override int Width => 0;
        protected override int Height => 0;
        protected override int TopSpeed => 0;

        private Paint laserPathPaint = getLaserPathPaint();
        private Paint activeLaserPaint = getActiveLaserPaint();

        private AbilityHandler shotAbility = new AbilityHandler(50, 50);
        private Point borderPoint;

        public Laser(int frameWidth, int frameHeight, Resources resources, Sprite target, Vector initVector) : base(frameWidth, frameHeight, resources, target, initVector) { }

        public new bool IsColliding(Sprite other)
        {
            return this.isAlive && base.IsColliding(other);
        }

        public override void Draw(Canvas canvas)
        {
            if (shotAbility.IsCoolingdown())
                drawLaser(canvas, laserPathPaint);
            else if (shotAbility.IsActive())
                drawLaser(canvas, activeLaserPaint);
            else
                borderPoint = null;
        }

        private void drawLaser(Canvas canvas, Paint paint)
        {
            if (borderPoint != null)
                canvas.DrawLine(this.GetCenterPoint().X, this.GetCenterPoint().Y, borderPoint.X, borderPoint.Y, paint);
        }

        public void fire(int initX, int initY)
        {
            if (borderPoint == null)
            {
                this.x = initX;
                this.y = initY;
                borderPoint = Utils.GetBorderPoint(this.GetCenterPoint(), target.GetCenterPoint(), frameWidth, frameHeight);
                shotAbility.ResetCooldownTimer();
                shotAbility.ResetAbilityTimer();
            }
        }

        public override void CalculateNextPosition()
        {
            if (borderPoint != null)
            {
                if (shotAbility.IsCoolingdown())
                {
                    System.Console.WriteLine($"LASER - warmup");
                    shotAbility.ReduceCooldownTimer();
                }
                else if (shotAbility.IsActive())
                {
                    System.Console.WriteLine($"LASER - firing");
                    shotAbility.ReduceAbilityTimer();
                }
            }
        }

        public new void Reset()
        {
            base.Reset();
            //projectile.Reset();
        }
    }
}