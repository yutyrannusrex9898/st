using Android.Content.Res;
using Android.Graphics;

namespace wobble.Animations
{
    public abstract class Enemy : Sprite
    {
        protected readonly Sprite target;
        public bool isAlive;

        public Enemy(int frameWidth, int frameHeight, Resources resources, Sprite target, Vector initVector) : base(frameWidth, frameHeight, resources, initVector)
        {
            this.target = target;
            this.isAlive = true;
        }

        public new bool IsColliding(Sprite other)
        {
            return this.isAlive && base.IsColliding(other);
        }

        public override void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(currentBitmap, x, y, null);
        }
    }
}