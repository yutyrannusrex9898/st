using Android.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace wobble.Animations
{
    public class EnemyPistoleer : Enemy
    {
        protected override int Width => 90;
        protected override int Height => 90;
        protected override int TopSpeed => 10;

        private AbilityHandler shotCountdown = new AbilityHandler(150);
        private LinkedList<PistoleerProjectile> projectiles = new LinkedList<PistoleerProjectile>();

        public EnemyPistoleer(int frameWidth, int frameHeight, Bitmap bitmap, Sprite target, Vector initVector) : base(frameWidth, frameHeight, bitmap, target, initVector)
        {
            this.Angle = Utils.getRandomAngle();
        }

        public void shoot()
        {
            double angle = Utils.GetAngleBetweenPoints(this.GetCurrentLocalPoint(), target.GetCurrentLocalPoint());
            PistoleerProjectile projectile = new PistoleerProjectile(frameWidth, frameHeight, originlBitmap, target, this.InitVector);
            projectile.SetInitLocation(this.x, this.y, angle);
            projectiles.AddLast(projectile);
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
            // TODO: split into 2 functions (check projectiles and check countdown).
            HashSet<PistoleerProjectile> projectiles = this.projectiles.ToHashSet();

            foreach (PistoleerProjectile projectile in projectiles)
            {
                if (projectile.IsOutsideBorder())
                    this.projectiles.Remove(projectile);
                else
                    projectile.CalculateNextPosition();
            }

            if (shotCountdown.HasAbilityTimeLeft())
            {
                shotCountdown.ReduceTimer();
            }
            else
            {
                shoot();
                shotCountdown.ResetAbilityTimer();
            }
        }

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
            foreach (PistoleerProjectile projectile in this.projectiles)
            {
                projectile.Draw(canvas);
            }
        }
    }
}