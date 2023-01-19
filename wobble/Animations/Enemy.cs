using Android.Graphics;

namespace wobble.Animations
{
    public abstract class Enemy : Sprite
    {
        protected readonly Player player;
        protected readonly bool isAlive;
        protected abstract int SpawnLocationX { get; }
        protected abstract int SpawnLocationY { get; }

        protected abstract double SpawnAngle { get; }


        public Enemy(int frameWidth, int frameHeight, Bitmap bitmap, Player player) : base(frameWidth, frameHeight, bitmap)
        {
            this.player = player;
            this.isAlive = true;
            this.x = SpawnLocationX;
            this.y = SpawnLocationY;
            this.Angle = SpawnAngle;
        }

        public override void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(currentBitmap, x, y, null);
        }
    }
}