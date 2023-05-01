using Android.Content;
using Android.Graphics;
using Android.Views;
using Java.Lang;

namespace wobble.Animations
{
    internal class MovementView : SurfaceView, IRunnable
    {
        int lives = 3;

        static int joystickFingerIndex = 0;

        int frameWidth;
        int frameHeight;

        double angle = 0;
        double distance = 0;
        double actualDistance = 0;


        private Player player;
        private Joystick joystick;
        private EnemyRammer enemyRammer;
        private EnemyPistoleer enemyPistoleer;


        private Canvas canvas;
        private Thread thread;

        public MovementView(Context context, int frameWidth, int frameHeight) : base(context)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            player = new Player(frameWidth, frameHeight, Resources, Constants.LEVEL_A.initPlayerVector);
            joystick = new Joystick(frameWidth, frameHeight, Constants.joystickVector);
            enemyRammer = new EnemyRammer(frameWidth, frameHeight, Resources, player, Constants.LEVEL_A.initRammerVector);
            enemyPistoleer = new EnemyPistoleer(frameWidth, frameHeight, Resources, player, Constants.LEVEL_A.initPistoleerVector);

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
                    enemyRammer.Draw(canvas);
                    enemyPistoleer.Draw(canvas);
                    Holder.UnlockCanvasAndPost(canvas);
                }
            }
        }

        public void Run()
        {
            while (true)
            {
                drawSurface();

                bool isDead = player.IsColliding(enemyRammer) || player.IsColliding(enemyPistoleer) || player.IsColliding(enemyPistoleer.projectile);

                if (isDead)
                {
                    //if (lives > 0)
                    //{
                    lives--;
                    player.ResetLocation();
                    enemyRammer.ResetLocation();
                    enemyPistoleer.ResetLocation();
                    enemyPistoleer.projectile.ResetLocation();
                    System.Console.WriteLine($"Down to {lives} lives.");
                    //}
                    //else
                    //{
                    //    System.Console.WriteLine("Game Over");
                    //    this.angle = 0;
                    //    this.distance = 0;
                    //}
                }
                else
                {
                    player.CalculateNextControlledMovement(this.angle, this.distance);
                    joystick.CalculateNextControlledMovement(this.angle, this.distance);
                    enemyRammer.CalculateNextMovement();
                    enemyPistoleer.CalculateNextMovement();
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent args)
        {
            int fingerIndex = args.ActionIndex;
            Point fingerLocation = new Point((int)args.GetX(fingerIndex), (int)args.GetY(fingerIndex));
            MotionEventActions action = args.ActionMasked;

            switch (action)
            {
                case MotionEventActions.Move:
                    if (fingerIndex == joystickFingerIndex)
                    {
                        HandleJoystickTouch(fingerLocation);
                    }
                    break;

                case MotionEventActions.Up:
                    if (fingerIndex == joystickFingerIndex)
                    {
                        this.distance = 0;
                    }
                    break;

                case MotionEventActions.Pointer1Down:
                    HandleDashTouch(fingerLocation);
                    break;
            }

            Invalidate();
            return true;
        }

        private void HandleJoystickTouch(Point fingerLocation)
        {
            this.angle = Utils.GetAngleBetweenPoints(joystick.GetInitLocalPoint(), fingerLocation);
            this.actualDistance = Utils.GetDistanceBetweenPoints(joystick.GetInitLocalPoint(), fingerLocation);
            this.distance = Math.Min(actualDistance, Joystick.joystickWorkingRadius);
        }

        private void HandleDashTouch(Point fingerLocation)
        {
            this.player.InitDash();
            System.Console.WriteLine($"pew pew pew!!! ({fingerLocation.X},{fingerLocation.Y})");
        }
    }
}