using Android.Graphics;

namespace wobble.Animations
{
    public class EnemyRammer : Enemy
    {
        protected override int TopSpeed => 10;
        protected override int Width => 90;
        protected override int Height => 90;

        public EnemyRammer(int frameWidth, int frameHeight, Bitmap bitmap, Sprite target, Vector initVector) : base(frameWidth, frameHeight, bitmap, target, initVector) { }

        public override void CalculateNextPosition()
        {
            this.Angle = Utils.GetAngleBetweenPoints(GetLocation(), target.GetLocation());
            CalculateNextLocation();
        }
    }
}