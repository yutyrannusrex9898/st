using Android.Graphics;

namespace wobble.Animations
{
    public class Player : ControlledSprite
    {
        private static int DashMaxTimer { get => 7; }
        protected override int TopSpeed { get => 20; }
        private int DashMultiplier { get => 3; }
        private int DashTimer { set; get; }
        protected override int Width { get => 100; }
        protected override int Height { get => 100; }

        public Player(int frameWidth, int frameHeight, Bitmap bitmap) : base(frameWidth, frameHeight, bitmap) { }

        public override void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(currentBitmap, x, y, null);
        }

        public override void CalculateNextPosition()
        {
            double speedMultiplier = CalculateSpeedMultiplier();
            CalculateNextXLocation(speedMultiplier);
            CalculateNextYLocation(speedMultiplier);
            if (DashTimer > 0)
                DashTimer--;
        }

        private double CalculateSpeedMultiplier()
        {
            if (DashTimer > 0)
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
            DashTimer = DashMaxTimer;
        }
    }
}