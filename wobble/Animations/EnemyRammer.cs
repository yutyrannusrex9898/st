using Android.Content.Res;
using Android.Graphics;

namespace wobble.Animations
{
    public class EnemyRammer : Enemy
    {
        protected override int TopSpeed => 10;
        protected override int Width => 90;
        protected override int Height => 90;

        public EnemyRammer(int frameWidth, int frameHeight, Resources resources, Sprite target, Vector initVector) : base(frameWidth, frameHeight, resources, target, initVector)
        {
            SetBitmap(BitmapFactory.DecodeResource(resources, Resource.Drawable.Rammer));
        }

        public override void CalculateNextPosition()
        {
            this.Angle = Utils.GetAngleBetweenPoints(this.GetCenterPoint(), target.GetCenterPoint());
            CalculateNextLocation();
        }
    }
}