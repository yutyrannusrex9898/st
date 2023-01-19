using Android.Graphics;

namespace wobble.Animations
{
    public class EnemyPistoleer : Enemy
    {
        protected override int TopSpeed => 10;
        protected override int Width => 90;
        protected override int Height => 90;

        protected override int SpawnLocationX => 200;
        protected override int SpawnLocationY => 200;

        protected override double SpawnAngle => 0.785398163;

        public EnemyPistoleer(int frameWidth, int frameHeight, Bitmap bitmap, Player player) : base(frameWidth, frameHeight, bitmap, player) { }

        public override void CalculateNextPosition()
        {
            if (x >= frameWidth - Width || x <= 0)
            {
                Angle = -1 * Utils.InvertAngle(Angle);
            }
            if (y >= frameHeight - Height || y <= 0)
            {
                Angle = Utils.InvertAngle(3.14159 - Angle);
            }

            CalculateNextXLocation(1);
            CalculateNextYLocation(1);
        }
    }
}