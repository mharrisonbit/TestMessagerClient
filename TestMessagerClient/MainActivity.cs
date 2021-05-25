using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using TestMessagerCore;

namespace TestMessagerClient
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
		static readonly string TAG = typeof(MainActivity).FullName;
		internal TextView timestampMessageTextView;
		internal Button sayHelloButton;
		internal Button askForTimestampButton;
		internal bool isStarting;
		TimestampServiceConnection serviceConnection;
		Messenger activityMessenger;
		Intent intentToBindService;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
			timestampMessageTextView = FindViewById<TextView>(Resource.Id.message_textview);

			sayHelloButton = FindViewById<Button>(Resource.Id.say_hello_to_service_button);
			sayHelloButton.Click += SayHelloToService_Click;

			askForTimestampButton = FindViewById<Button>(Resource.Id.get_timestamp_button);
			askForTimestampButton.Click += AskForTimestampButton_Click;
		}

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			switch (requestCode)
			{
				case Constants.TIMESTAMP_SERVICE_REQUEST_PERMISSION:
					if ((grantResults.Length > 0) && (grantResults[0] == Permission.Granted))
					{
						Log.Info(TAG, "Permission was granted.");
					}
					else
					{
						Log.Warn(TAG, "Permission was not granted.");
						timestampMessageTextView.SetText(Resource.String.permission_not_granted);
						sayHelloButton.Enabled = false;
						sayHelloButton.Click -= SayHelloToService_Click;
						askForTimestampButton.Enabled = false;
						askForTimestampButton.Click -= AskForTimestampButton_Click;
					}
					break;

				default:
					Log.Wtf(TAG, $"Don't know what this requestCode is: {requestCode}.");
					break;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			StartService();
		}

		protected override void OnResume()
		{
			base.OnResume();
			if (isStarting)
			{
				timestampMessageTextView.SetText(Resource.String.service_starting);
			}
		}

		void StartService()
		{
			isStarting = false;
			if (!this.IsTimestampServicePackageInstalled())
			{
                return;
            }

			if (this.HasPermissionToRunTimestampService())
			{
				if (activityMessenger == null)
				{
					activityMessenger = new Messenger(new MainActivityMessageHandler(this));
				}
				if (serviceConnection == null)
				{
					serviceConnection = new TimestampServiceConnection(this);
				}
				if (intentToBindService == null)
				{
					intentToBindService = TimestampServiceHelpers.CreateIntentToBindService(this);
				}

				BindService(intentToBindService, serviceConnection, Bind.AutoCreate);
				isStarting = true;
				Log.Debug(TAG, "BindService has been called.");
			}
			else
			{
				Log.Error(TAG, $"No permissions to use the service {Constants.REMOTE_SERVICE_PACKAGE_NAME}.");
				serviceConnection = null;
				activityMessenger = null;

				timestampMessageTextView.SetText(Resource.String.permission_not_granted);
				sayHelloButton.Enabled = false;
				sayHelloButton.Click -= SayHelloToService_Click;
				askForTimestampButton.Enabled = false;
				askForTimestampButton.Click -= AskForTimestampButton_Click;

			}
		}

		protected override void OnPause()
		{
			base.OnPause();
		}

		void SayHelloToService_Click(object sender, EventArgs e)
		{
			if (serviceConnection.Messenger != null)
			{
				Message msg = Message.Obtain(null, Constants.SAY_HELLO_TO_TIMESTAMP_SERVICE);
				try
				{
					serviceConnection.Messenger.Send(msg);
				}
				catch (RemoteException ex)
				{
					Log.Error(TAG, ex, "There was a error trying to send the message.");
				}
			}
			else
			{
				Toast.MakeText(this, "Seems like we're not connected to the service - nobody around to say hello to.", ToastLength.Short).Show();
			}
		}

		void AskForTimestampButton_Click(object sender, EventArgs e)
		{
			if (!serviceConnection.IsConnected)
			{
				Log.Warn(TAG, "Not connected to the service, so can't ask for the timestamp.");
				return;
			}

			Message msg = Message.Obtain(null, Constants.GET_UTC_TIMESTAMP);
			msg.ReplyTo = activityMessenger;

			try
			{
				serviceConnection.Messenger.Send(msg);
				Log.Debug(TAG, "Requested the timestamp from the Service.");
			}
			catch (RemoteException ex)
			{
				Log.Error(TAG, ex, "There was a problem sending the message.");
			}
		}

		protected override void OnStop()
		{
			if (serviceConnection != null)
			{
				UnbindService(serviceConnection);
			}
			base.OnStop();
		}
	}
}