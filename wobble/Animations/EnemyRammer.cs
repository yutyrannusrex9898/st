using Android.Graphics;

namespace wobble.Animations
{
    public class EnemyRammer : Enemy
    {
        protected override int TopSpeed => 10;
        protected override int Width => 90;
        protected override int Height => 90;

        public EnemyRammer(int frameWidth, int frameHeight, Bitmap bitmap, Player player, Vector initVector) : base(frameWidth, frameHeight, bitmap, player, initVector) { }

        public override void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(currentBitmap, x, y, null);
        }

        public override void CalculateNextPosition()
        {
            this.Angle = Utils.GetAngleBetweenPoints(GetLocation(), player.GetLocation());

            CalculateNextXLocation(1);
            CalculateNextYLocation(1);
        }
    }
}