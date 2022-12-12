using Android.Graphics;

namespace wobble.Animations
{
    public abstract class ControlledSprite : Sprite
    {
        protected int Distance { get; set; }

        protected ControlledSprite(int frameWidth, int frameHeight, Bitmap bitmap) : base(frameWidth, frameHeight, bitmap) { }

        public abstract void CalculateNextPosition(double angle, double distance);
        public abstract void CalculateNextControlledMovement(double angle, double distance);
    }
}