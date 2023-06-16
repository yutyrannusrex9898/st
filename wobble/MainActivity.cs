using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using SQLite;
using System.IO;
using System.Timers;
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
        public TextView tvKills;
        public TextView tvTimer;
        private bool isThreadRunning;

        private int kills;
        private int deaths;

        private Timer timer;
        private int secondsRemaining = 1800;

        private SQLiteConnection dbCon;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_movement);
            frame = FindViewById<ViewGroup>(Resource.Id.flMainFrame);
            btStart = FindViewById<Button>(Resource.Id.btStart);
            btQuit = FindViewById<Button>(Resource.Id.btQuit);
            btPauseResume = FindViewById<Button>(Resource.Id.btPauseResume);
            btPauseQuit = FindViewById<Button>(Resource.Id.btPauseQuit);
            tvKills = FindViewById<TextView>(Resource.Id.tvKills);
            tvDeaths = FindViewById<TextView>(Resource.Id.tvDeaths);
            tvTimer = FindViewById<TextView>(Resource.Id.tvTimer);

            SupportActionBar?.Hide();
            isThreadRunning = true;

            SetupButtons();
            SetupDBConnection();
            LoadUserDataFromDb();
        }

        private void SetupButtons()
        {
            btStart.Click += BtStart_Click;
            btQuit.Click += BtQuit_Click;
            btPauseResume.Click += BtPauseResume_Click;
            btPauseQuit.Click += BtPauseQuit_Click;
        }

        private void SetupDBConnection()
        {
            this.dbCon = new SQLiteConnection(GetDbPath());
            this.dbCon.CreateTable<UserData>();
        }

        private string GetDbPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "wobble.DB");
        }

        private void LoadUserDataFromDb()
        {
            UserData userData = getUserData();
            this.kills = userData.Kills;
            this.deaths = userData.Deaths;
        }

        private UserData getUserData()
        {
            string deviceId = GetDeviceId();

            UserData userData;
            try { userData = this.dbCon.Get<UserData>(deviceId); }
            catch { userData = new UserData(); }
            return userData;
        }

        private string GetDeviceId()
        {
            return Android.Provider.Settings.Secure.GetString(Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
        }


        private void BtStart_Click(object sender, System.EventArgs e)
        {
            System.Console.WriteLine("Start!");
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            // Create a new timer
            timer = new Timer();
            timer.Interval = 1000; // Timer interval in milliseconds
            timer.Elapsed += Timer_Elapsed;

            // Start the timer
            timer.Start();

            // Update the UI immediately
            UpdateTimerText();

            System.Threading.Thread thread = new System.Threading.Thread(BackgroundThreadCode);
            thread.Start();
            btPauseResume.Visibility = ViewStates.Visible;
            tvKills.Visibility = ViewStates.Visible;
            tvDeaths.Visibility = ViewStates.Visible;
            tvTimer.Visibility = ViewStates.Visible;
            frame.RemoveAllViews();
            frame.AddView(btPauseResume);
            frame.AddView(movementView);
            frame.AddView(tvKills);
            frame.AddView(tvDeaths);
            frame.AddView(btPauseQuit);
            frame.AddView(tvTimer);
            movementView.Start(this.kills, this.deaths);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            secondsRemaining--;

            if (secondsRemaining <= 1)
            {
                timer.Stop();
                timer.Dispose();

                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, "You've been playing for too long!", ToastLength.Long).Show();
                });
            }
            else
            {
                RunOnUiThread(() =>
                {
                    UpdateTimerText();
                });
            }
        }

        private void UpdateTimerText()
        {
            int hours = secondsRemaining / 3600;
            int minutes = (secondsRemaining % 3600) / 60;
            int seconds = secondsRemaining % 60;

            tvTimer.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
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
            this.deaths = movementView.GetDeaths();
            this.kills = movementView.GetKills();
            SaveUserDataToDb();
            System.Environment.Exit(0);
        }

        private void SaveUserDataToDb()
        {
            UserData ud = new UserData()
            {
                DeviceId = GetDeviceId(),
                Kills = this.kills,
                Deaths = this.deaths
            };
            dbCon.InsertOrReplace(ud);
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
            while (isThreadRunning)
            {
                RunOnUiThread(() =>
                {
                    this.kills = movementView.GetKills();
                    this.deaths = movementView.GetDeaths();
                    tvKills.Text = $"Kills: {kills}";
                    tvDeaths.Text = $"Deaths: {deaths}";
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