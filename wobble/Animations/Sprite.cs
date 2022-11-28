using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wobble.Animations
{
    public class Sprite
    {
        private int x;
        private int y;
        private int width;
        private int height;
        private Bitmap bitmap;

        public Sprite(int width, int height, Bitmap bitmap)
        {
            this.width = width;
            this.height = height;
            this.x = 0;
            this.y = -bitmap.Height;
            //this.bitmap = bitmap;
            this.bitmap = Bitmap.CreateScaledBitmap(bitmap, 100, 100, false);
        }

        public void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(bitmap, x, y, null);
        }

        int xSpeed = 20;
        int ySpeed = 20;

        int xDirection = 1;
        int yDirection = 1;

        public void Move()
        {
            xDirection = getDirection(xDirection, x, x + bitmap.Width, width);
            x += xSpeed * xDirection;

            yDirection = getDirection(yDirection, y, y + bitmap.Height, height);
            y += ySpeed * yDirection;
        }

        private int getDirection(int initDirection, int startLocation, int endLocation, int length)
        {
            if (startLocation < 0)
                return 1;

            if (endLocation >= length)
                return -1;

            return initDirection;
        }
    }
}