using Android.Graphics;

namespace wobble.Animations
{
    public abstract class ControlledSprite : Sprite
    {
        protected ControlledSprite(int frameWidth, int frameHeight, Bitmap bitmap, Vector InitVector) : base(frameWidth, frameHeight, bitmap, InitVector) { }

        public void CalculateNextControlledMovement(double angle, double distance)
        {
            UpdateAngleAndDistance(angle, distance);
            CalculateNextSpriteAngle();
            CalculateNextPosition();
        }
    }
}