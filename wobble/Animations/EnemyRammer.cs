using Android.Graphics;

namespace wobble.Animations
{
    public class EnemyRammer : Enemy
    {
        protected override int TopSpeed => 10;
        protected override int Width => 90;
        protected override int Height => 90;

        protected override int SpawnLocationX => 400;

        protected override int SpawnLocationY => 400;

        protected override double SpawnAngle => 0;

        public EnemyRammer(int frameWidth, int frameHeight, Bitmap bitmap, Player player) : base(frameWidth, frameHeight, bitmap, player) { }

        public override void CalculateNextPosition()
        {
            this.Angle = Utils.GetAngleBetweenPoints(GetLocation(), player.GetLocation());

            CalculateNextXLocation(1);
            CalculateNextYLocation(1);
        }
    }
}