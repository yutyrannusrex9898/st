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
        private int directionY = 20;

        public Sprite(int width, int height, Bitmap bitmap)
        {
            this.width = width;
            this.height = height;
            this.x = 0;
            this.y = -bitmap.Height;
            this.bitmap = bitmap;
        }

        public void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(bitmap, x, y, null);
        }

        public void move()
        {
            y += 20;
            if (y >= height)
            {
                y = -bitmap.Height;
            }
        }
        //public void Move()
        //{
        //    y += directionY;
        //    if (y >= bitmap.Height || y<= -height - bitmap.Height)
        //    {
        //        directionY *= -1;
        //    }
        //}
    }
}