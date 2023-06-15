using Android.Content;
using Android.Graphics;
using Android.Views;
using Java.Lang;

namespace wobble.Animations
{
    internal class MovementView : SurfaceView, IRunnable
    {
        private static int joystickFingerIndex = 0;

        private readonly int frameWidth;
        private readonly int frameHeight;

        public bool IsRunning { get; set; }
        double angle = 0;
        double distance = 0;
        double actualDistance = 0;

        private int lives = 10;
        private Player player;
        private Joystick joystick;
        private EnemyRammer enemyRammer;
        private EnemyPistoleer enemyPistoleer;
        private EnemyRailgunner enemyRailgunner;

        private Canvas canvas;
        private Thread thread;

        private Bitmap bg;
        private int deaths;

        public MovementView(Context context, int frameWidth, int frameHeight) : base(context)
        {
            this.bg = BitmapFactory.DecodeResource(Resources, Resource.Drawable.BG2);
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.deaths = MainActivity.currentDeaths;

            IsRunning = true;
            player = new Player(frameWidth, frameHeight, Resources, Constants.LEVEL_A.initPlayerVector);
            joystick = new Joystick(frameWidth, frameHeight, Constants.joystickVector);
            enemyRammer = new EnemyRammer(frameWidth, frameHeight, Resources, player, Constants.LEVEL_A.initRammerVector);
            enemyPistoleer = new EnemyPistoleer(frameWidth, frameHeight, Resources, player, Constants.LEVEL_A.initPistoleerVector);
            enemyRailgunner = new EnemyRailgunner(frameWidth, frameHeight, Resources, player, Constants.LEVEL_A.initRailGunnerVector);

            thread = new Thread(this);
        }

        private void drawSurface()
        {
            if (Holder.Surface.IsValid)
            {
                canvas = Holder.LockCanvas();
                if (canvas != null)
                {
                    canvas.DrawBitmap(bg, 0, 0, null);
                    player.Draw(canvas);
                    joystick.Draw(canvas);
                    if (enemyRammer.isAlive) enemyRammer.Draw(canvas);
                    if (enemyPistoleer.isAlive) enemyPistoleer.Draw(canvas);
                    if (enemyRailgunner.isAlive) enemyRailgunner.Draw(canvas);

                    Holder.UnlockCanvasAndPost(canvas);
                }
            }
        }

        public void Start()
        {
            this.thread.Start();
        }

        public void Run()
        {
            while (true)
            {
                while (IsRunning)
                {

                    drawSurface();
                    bool hasAbilityTimeLeft = player.IsDashing();

                    if (enemyRammer.isAlive)
                    {
                        bool RammerShouldDie = enemyRammer.isAlive && hasAbilityTimeLeft && player.IsColliding(enemyRammer);
                        enemyRammer.isAlive = !RammerShouldDie;
                    }

                    if (enemyPistoleer.isAlive)
                    {
                        bool pistoleerShouldDie = enemyPistoleer.isAlive && hasAbilityTimeLeft && player.IsColliding(enemyPistoleer);
                        enemyPistoleer.isAlive = !pistoleerShouldDie;
                    }

                    if (enemyRailgunner.isAlive)
                    {
                        bool railGunnerShouldDie = enemyRailgunner.isAlive && hasAbilityTimeLeft && player.IsColliding(enemyRailgunner);
                        enemyRailgunner.isAlive = !railGunnerShouldDie;
                    }

                    if (player.isAlive)
                    {
                        bool playerWasHit = !hasAbilityTimeLeft && (enemyRammer.IsColliding(player) || enemyPistoleer.IsColliding(player) || enemyRailgunner.IsColliding(player));
                        player.isAlive = !playerWasHit;

                        if (playerWasHit)
                        {
                            if (lives > -1000)
                            {
                                lives--;
                                System.Console.WriteLine($"Down to {lives} lives.");

                                player.Reset();
                                enemyRammer.Reset();
                                enemyPistoleer.Reset();
                                enemyRailgunner.Reset();
                                deaths++;
                            }
                            else
                            {
                                System.Console.WriteLine("Game Over!");
                                this.angle = 0;
                                this.distance = 0;
                            }
                        }
                        else
                        {
                            player.CalculateNextControlledMovement(this.angle, this.distance);
                            joystick.CalculateNextControlledMovement(this.angle, this.distance);
                            enemyRammer.CalculateNextMovement();
                            enemyPistoleer.CalculateNextMovement();
                            enemyRailgunner.CalculateNextMovement();
                        }
                    }
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent args)
        {
            int fingerIndex = args.ActionIndex;
            MotionEventActions action = args.ActionMasked;

            switch (action)
            {
                case MotionEventActions.Move:
                    if (fingerIndex == joystickFingerIndex)
                    {
                        Point fingerLocation = new Point((int)args.GetX(fingerIndex), (int)args.GetY(fingerIndex));
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
                    HandleDashTouch();
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

        private void HandleDashTouch()
        {
            this.player.InitDash();
        }

        public int getDeaths()
        {
            return deaths;
        }
    }
}