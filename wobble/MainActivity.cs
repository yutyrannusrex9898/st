using Android.App;
using Android.Content;
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
        Button btPauseQuit;
        public TextView tvDeaths;
        private bool isRunning;
        private const string PreferenceKey = "CurrentDeaths";
        private ISharedPreferences sharedPreferences;
        private int savedValue;
        public static int currentDeaths;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_movement);
            frame = FindViewById<ViewGroup>(Resource.Id.flMainFrame);
            btStart = FindViewById<Button>(Resource.Id.btStart);
            btQuit = FindViewById<Button>(Resource.Id.btQuit);
            btPauseResume = FindViewById<Button>(Resource.Id.btPauseResume);
            btPauseQuit = FindViewById<Button>(Resource.Id.btPauseQuit);
            tvDeaths = FindViewById<TextView>(Resource.Id.tvDeaths);

            SupportActionBar?.Hide();
            isRunning = true;

            btStart.Click += BtStart_Click;
            btQuit.Click += BtQuit_Click;
            btPauseResume.Click += BtPauseResume_Click;
            btPauseQuit.Click += BtPauseQuit_Click;

            sharedPreferences = GetSharedPreferences("MyPreferences", FileCreationMode.Private);
            LoadSavedValue();
            currentDeaths = int.Parse(tvDeaths.Text);
        }

        private void LoadSavedValue()
        {
            savedValue = sharedPreferences.GetInt(PreferenceKey, 0);
            tvDeaths.Text = savedValue.ToString();
        }

        private void BtStart_Click(object sender, System.EventArgs e)
        {
            System.Console.WriteLine("Start!");
            System.Threading.Thread thread = new System.Threading.Thread(BackgroundThreadCode);
            thread.Start();
            btPauseResume.Visibility = ViewStates.Visible;
            tvDeaths.Visibility = ViewStates.Visible;
            frame.RemoveAllViews();
            frame.AddView(btPauseResume);
            frame.AddView(movementView);
            frame.AddView(tvDeaths);
            frame.AddView(btPauseQuit);
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
            if (btPauseQuit.Visibility == ViewStates.Visible)
            {
                btPauseQuit.Visibility = ViewStates.Invisible;
            }
            else
            {
                btPauseQuit.Visibility = ViewStates.Visible;
            }
        }

        private void BtPauseQuit_Click(object sender, System.EventArgs e)
        {
            int valueToSave = movementView.getDeaths();

            // Save the value using SharedPreferences.Editor
            ISharedPreferencesEditor editor = sharedPreferences.Edit();
            editor.PutInt(PreferenceKey, valueToSave);
            editor.Apply();

            savedValue = valueToSave;
            tvDeaths.Text = savedValue.ToString();
            System.Console.WriteLine(savedValue);
            System.Environment.Exit(0);
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
            {
                movementView = new MovementView(this, frame.Width, frame.Height);
            }
        }

        void BackgroundThreadCode()
        {
            while (isRunning)
            {
                RunOnUiThread(() =>
                {
                    currentDeaths = movementView.getDeaths();
                    tvDeaths.Text = $"{currentDeaths}";
                });
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}