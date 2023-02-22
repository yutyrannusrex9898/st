using Android.Graphics;

namespace wobble.Animations
{
    public abstract class Enemy : Sprite
    {
        protected readonly Sprite target;
        protected readonly bool isAlive;

        public Enemy(int frameWidth, int frameHeight, Bitmap bitmap, Sprite target, Vector initVector) : base(frameWidth, frameHeight, bitmap, initVector)
        {
            this.target = target;
            this.isAlive = true;
        }

        public override void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(currentBitmap, x, y, null);
        }
    }
}