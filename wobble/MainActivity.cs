using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using wobble.Animations;

namespace wobble
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private FrameLayout frame;
        MovementView movementView;
        Button btPause;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_movement);
            frame = FindViewById<FrameLayout>(Resource.Id.flMainFrame);
            btPause = FindViewById<Button>(Resource.Id.btPause);
            SupportActionBar?.Hide();

            btPause.Click += BtPause_Click;
            updatePauseResumeButton();
        }

        private void BtPause_Click(object sender, System.EventArgs e)
        {
            if (movementView != null)
            {
                movementView.IsRunning = !movementView.IsRunning;
                updatePauseResumeButton();
            }

        }

        private void updatePauseResumeButton()
        {
            if (btPause != null && movementView != null)
            {
                btPause.Text = (movementView.IsRunning) ? "PAUSE" : "RESUME";
            }
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
            {
                movementView = new MovementView(this, frame.Width, frame.Height);
                frame.AddView(movementView);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}