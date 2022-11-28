using Android.Content;
using Android.Graphics;
using Android.Views;
using AndroidX.Core.Content;
using Java.Lang;

namespace wobble.Animations
{
    internal class MovementView : SurfaceView, IRunnable
    {
        int width;
        int height;
        private Sprite sprite;
        private Canvas canvas;
        private Thread thread;

        public MovementView(Context context, int width, int height) : base(context)
        {
            this.width = width;
            this.height = height;
            Bitmap bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Player);
            sprite = new Sprite(width, height, bitmap);

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

                Holder.UnlockCanvasAndPost(canvas);
            }
        }

        public void Run()
        {
            while (true)
            {
                drawSurface();
                sprite.Move();
            }
        }
    }
}