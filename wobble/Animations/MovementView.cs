﻿using Android.Content;
using Android.Graphics;
using Android.Views;
using Java.Lang;

namespace wobble.Animations
{
    internal class MovementView : SurfaceView, IRunnable
    {

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

            Bitmap playerBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Player);
            Bitmap enemyRammerBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Cube);
            Bitmap enemyPistoleerBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Cube);

            player = new Player(frameWidth, frameHeight, playerBitmap);
            joystick = new Joystick(frameWidth, frameHeight);
            enemyRammer = new EnemyRammer(frameWidth, frameHeight, enemyRammerBitmap, player);
            enemyPistoleer = new EnemyPistoleer(frameWidth, frameHeight, enemyRammerBitmap, player);

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
                player.CalculateNextControlledMovement(this.angle, this.distance);
                joystick.CalculateNextControlledMovement(this.angle, this.distance);
                enemyRammer.CalculateNextPosition();
                enemyPistoleer.CalculateNextPosition();
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
            this.angle = Utils.GetAngleBetweenPoints(joystick.MainLocation, fingerLocation);
            this.actualDistance = Utils.GetDistanceBetweenPoints(joystick.MainLocation, fingerLocation);
            this.distance = Math.Min(actualDistance, Joystick.joystickWorkingRadius);
        }
        private void HandleDashTouch(Point fingerLocation)
        {
            this.player.InitDash();
            System.Console.WriteLine($"pew pew pew!!! ({fingerLocation.X},{fingerLocation.Y})");
        }
    }
}