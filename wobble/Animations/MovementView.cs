using Android.Content;
using Android.Graphics;
using Android.Views;
using Java.Lang;

namespace wobble.Animations
{
    internal class MovementView : SurfaceView, IRunnable
    {
        int frameWidth;
        int frameHeight;
        double angle = 0;
        double distance = 0;
        double actualDistance = 0;

        private Player player;
        private Joystick joystick;
        private Canvas canvas;
        private Thread thread;

        public MovementView(Context context, int frameWidth, int frameHeight) : base(context)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            Bitmap bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Player);
            player = new Player(frameWidth, frameHeight, bitmap);
            joystick = new Joystick(frameWidth, frameHeight);

            thread = new Thread(this);
            thread.Start();
        }

        private void drawSurface()
        {
            if (Holder.Surface.IsValid)
            {
                canvas = Holder.LockCanvas();
                if (canvas != null)
                {
                    canvas.DrawColor(Color.Wheat);
                    player.Draw(canvas);
                    joystick.Draw(canvas);
                    Holder.UnlockCanvasAndPost(canvas);
                }
            }
        }

        public void Run()
        {
            while (true)
            {
                drawSurface();
                player.CalculateNextControlledMovement(this.angle, this.distance);
                joystick.CalculateNextControlledMovement(this.angle, this.distance);
            }
        }

        public override bool OnTouchEvent(MotionEvent args)
        {
            int pointerIndex = args.ActionIndex;
            MotionEventActions action = args.Action & MotionEventActions.Mask;
            Point fingerLocation = new Point((int)args.GetX(pointerIndex), (int)args.GetY(pointerIndex));

            this.angle = Utils.GetAngleBetweenPoints(joystick.MainLocation, fingerLocation);
            this.actualDistance = Utils.GetDistanceBetweenPoints(joystick.MainLocation, fingerLocation);
            this.distance = Math.Min(actualDistance, Joystick.joystickWorkingRadius);

            if (action == MotionEventActions.Up)
            {
                fingerLocation = joystick.MainLocation;
                this.distance = 0;
            }

            return true;
        }
    }
}