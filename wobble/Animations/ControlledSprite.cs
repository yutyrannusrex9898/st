using Android.Content.Res;

namespace wobble.Animations
{
    public abstract class ControlledSprite : Sprite
    {
        protected ControlledSprite(int frameWidth, int frameHeight, Resources resources, Vector InitVector) : base(frameWidth, frameHeight, resources, InitVector) { }

        public void CalculateNextControlledMovement(double angle, double distance)
        {
            UpdateAngleAndDistance(angle, distance);
            CalculateNextMovement();
        }

        private void UpdateAngleAndDistance(double angle, double distance)
        {
            this.Angle = angle;
            this.Distance = distance;
        }
    }
}