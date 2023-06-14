using Android.Content.Res;
using Android.Graphics;

namespace wobble.Animations
{
    public class Player : ControlledSprite
    {
        protected override int Width { get => 100; }
        protected override int Height { get => 100; }

        protected override int TopSpeed { get => 20; }
        private int DashMultiplier { get => 3; }

        public AbilityHandler dashAbility = new AbilityHandler(15, 50);
        private bool dashActivated = false;

        public Player(int frameWidth, int frameHeight, Resources resources, Vector initVector) : base(frameWidth, frameHeight, resources, initVector)
        {
            SetBitmap(BitmapFactory.DecodeResource(resources, Resource.Drawable.Player));
        }

        public override void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(currentBitmap, x, y, null);
        }

        public override void CalculateNextPosition()
        {
            double speedMultiplier = CalculateSpeedMultiplier();
            CalculateNextLocation(speedMultiplier);

            if (dashAbility.IsCoolingdown() || dashActivated)
                dashAbility.ReduceAbilityTimer();

            if (dashAbility.IsResetable())
            {
                dashAbility.Reset();
                dashActivated = false;
            }
        }

        private double CalculateSpeedMultiplier()
        {
            if (IsDashing())
                return DashMultiplier;
            else
                return CalculateJoystickMultiplier();
        }

        private double CalculateJoystickMultiplier()
        {
            return Distance / Joystick.joystickWorkingRadius;
        }

        public void InitDash()
        {
            if (dashAbility.IsActive())
                dashActivated = true;
        }

        public bool IsDashing()
        {
            return dashActivated;
        }

        public new void Reset()
        {
            base.Reset();
        }
    }
}