using Android.Graphics;

namespace wobble.Animations
{
    public abstract class Enemy : Sprite
    {
        protected readonly Player player;
        protected readonly bool isAlive;

        public Enemy(int frameWidth, int frameHeight, Bitmap bitmap, Player player, Vector initVector) : base(frameWidth, frameHeight, bitmap, initVector)
        {
            this.player = player;
            this.isAlive = true;
        }

        public override void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(currentBitmap, x, y, null);
        }

        public void CalculateNextMovement()
        {
            CalculateNextSpriteAngle();
            CalculateNextPosition();
        }
    }
}