using Android.Content;
using Android.Graphics;
using Android.Views;
using AndroidX.Core.Content;
using Java.Lang;
using System;
using System.Numerics;
using Xamarin.Essentials;

namespace wobble.Animations
{
    internal class MovementView : SurfaceView, IRunnable
    {
        int frameWidth;
        int frameHeight;
        double angle = 0;
        double distance = 0;

        private Sprite sprite;
        private Joystick joystick;
        private Canvas canvas;
        private Thread thread;

        public MovementView(Context context, int frameWidth, int frameHeight) : base(context)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            Bitmap bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Player);
            sprite = new Sprite(frameWidth, frameHeight, bitmap);
            joystick = new Joystick(frameWidth, frameHeight);

            thread = new Thread(this);
            thread.Start();
        }

        public void drawSurface()
        {
            if (Holder.Surface.IsValid)
            {
                canvas = Holder.LockCanvas();
                canvas.DrawColor(Color.Wheat);
                sprite.Draw(canvas);
                joystick.Draw(canvas);
                Holder.UnlockCanvasAndPost(canvas);
            }
        }

        public void Run()
        {
            while (true)
            {
                drawSurface();
                sprite.Move(this.angle, this.distance);
                joystick.Move(this.angle, this.distance);
            }
        }

        public override bool OnTouchEvent(MotionEvent args)
        {
            int pointerIndex = args.ActionIndex;
            Point fingerLocation = new Point((int)args.GetX(pointerIndex), (int)args.GetY(pointerIndex));
            this.angle = Utils.GetAngleBetweenPoints(Joystick.mainLocation, fingerLocation);
            this.distance = Utils.GetDistanceBetweenPoints(Joystick.mainLocation, fingerLocation);

            return true;
        }
    }
}