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
            this.y = 0;
            this.bitmap = Bitmap.CreateScaledBitmap(bitmap, 100, 100, false);
        }

        public void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(bitmap, x, y, null);
        }

        Point p1 = new Point(0, 0);
        Point p2 = new Point(2, 3);
        int speed = 20;
        double xRate = 0.0;
        double yRate = 0.0;

        public void Move()
        {
            //int a = ((DateTime.Now.Second / 10) - 3);
            //int b = 3 * ((DateTime.Now.Second % 10) - 5) / 5;
            //Point p2 = new Point(a, b);
            double angle = getAngleBetweenPoints(p1, p2);

            int seconds = DateTime.Now.Second % 10;
            Random rnd = new Random(seconds);
            angle = rnd.NextDouble() * Math.PI * 2;

            xRate = getXRate(angle);
            yRate = getYRate(angle);

            int jumpX = ((int)Math.Round(speed * xRate));
            if (x + jumpX + bitmap.Width >= width)
                x = width - bitmap.Width;
            else if (x + jumpX < 0)
                x = 0;
            else
                x += jumpX;

            int jumpY = ((int)Math.Round(speed * yRate));
            if (y + jumpY + bitmap.Height >= height)
                y = height - bitmap.Height;
            else if (y + jumpY < 0)
                y = 0;
            else
                y += jumpY;
        }

        private double getAngleBetweenPoints(Point p1, Point p2)
        {
            return Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }

        private double getXRate(double angle)
        {
            return Math.Cos(angle);
        }

        private double getYRate(double angle)
        {
            return Math.Sin(angle);
        }
    }
}