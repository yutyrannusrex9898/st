using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wobble.Animations;

namespace wobble.Resources.layout
{
    [Activity(Label = "MovementActivity")]
    public class MovementActivity : AppCompatActivity
    {
        private FrameLayout frame;
        MovementView movementView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_movement);
            frame = FindViewById<FrameLayout>(Resource.Id.flMainFrame);
            Log.Debug("MovementActivity", "OnCreate" + frame.Width);

            // Create your application here
        }


        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
            {
                int fWidth = frame.Width;
                int fHeight = frame.Height;
                movementView = new MovementView(this, fWidth, fHeight);
                frame.AddView(movementView);
            }
        }
    }
}