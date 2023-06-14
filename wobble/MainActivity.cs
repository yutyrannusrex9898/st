using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using wobble.Animations;

namespace wobble
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ViewGroup frame;
        MovementView movementView;
        Button btStart;
        Button btQuit;
        Button btPauseResume;
        bool isStarted = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_movement);
            frame = FindViewById<ViewGroup>(Resource.Id.flMainFrame);
            btStart = FindViewById<Button>(Resource.Id.btStart);
            btQuit = FindViewById<Button>(Resource.Id.btQuit);
            btPauseResume = FindViewById<Button>(Resource.Id.btPauseResume);

            SupportActionBar?.Hide();

            btStart.Click += BtStart_Click;
            btQuit.Click += BtQuit_Click;
            btPauseResume.Click += BtPauseResume_Click;
        }
        private void BtStart_Click(object sender, System.EventArgs e)
        {
            System.Console.WriteLine("Start!");

            btPauseResume.Visibility = ViewStates.Visible;
            frame.RemoveAllViews();
            frame.AddView(btPauseResume);
            frame.AddView(movementView);

            movementView.Start();
        }

        private void BtQuit_Click(object sender, System.EventArgs e)
        {
            System.Console.WriteLine("Quit!");
            System.Environment.Exit(0);
        }

        private void BtPauseResume_Click(object sender, System.EventArgs e)
        {
            movementView.IsRunning = !movementView.IsRunning;
            btPauseResume.Text = (movementView.IsRunning) ? "PAUSE" : "RESUME";
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
            {
                movementView = new MovementView(this, frame.Width, frame.Height);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}