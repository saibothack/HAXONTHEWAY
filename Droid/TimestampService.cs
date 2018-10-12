
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Location;
using Android.Gms.Common.Apis;
using Android.Locations;
using Android.Support.V4.App;
using Android;
using Android.Content.PM;
using System;
using Android.Gms.Common;
using static Android.Gms.Common.Apis.GoogleApiClient;
using Android.Runtime;
using Android.Util;
using Android.Content;
using System.Threading;
using HaxOnTheWay.Droid;
using Firebase.Iid;
using System.Collections.Generic;
using HaxOnTheWay.Models;
using HaxOnTheWay;
using Java.Lang;

namespace ServicesDemo3
{

	/// <summary>
	/// This is a sample started service. When the service is started, it will log a string that details how long 
	/// the service has been running (using Android.Util.Log). This service displays a notification in the notification
	/// tray while the service is active.
	/// </summary>
	[Service]
    public class TimestampService : Service,IConnectionCallbacks, IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
	{
		static readonly string TAG = typeof(TimestampService).FullName;

        private const int MY_PERMISSION_REQUEST_CODE = 7171;
        private const int PLAY_SERVICES_RESOLUTION_REQUEST = 7172;
        private TextView txtCoordinates;
        private Button btnGetCoordinates, btnTracking;
        private bool mRequestingLocationUpdates = false;
        private LocationRequest mLocationRequest;
        private GoogleApiClient mGoogleApiClient;
        private Android.Locations.Location mLastLocation;

        private static string ANDROID_CHANNEL_ID = "channel_01";

        private static int UPDATE_INTERVAL = 10; // SEC
        private static int FATEST_INTERVAL = 10; // SEC
        private static int DISPLACEMENT = 1; // METERS

        public bool estatus_ind;
        public int idConductor;
		//UtcTimestamper timestamper;
		bool isStarted;
        bool isStartedF;
		Handler handler;
		Action runnable;
        Handler handler1;
        Action runnableL;
		public override void OnCreate()
		{
			base.OnCreate();
			Log.Info(TAG, "OnCreate: the service is initializing.");

			//timestamper = new UtcTimestamper();
			handler = new Handler();

			// This Action is only for demonstration purposes.
			runnable = new Action(() =>
							{
                System.Diagnostics.Debug.WriteLine("entra runnable");
								//if (timestamper == null)
								//{
									//Log.Wtf(TAG, "Why isn't there a Timestamper initialized?");
								//}
								//else
								//{
									//string msg = timestamper.GetFormattedTimestamp();
									//Log.Debug(TAG, msg);
									//Intent i = new Intent(Constants.NOTIFICATION_BROADCAST_ACTION);
									//i.PutExtra(Constants.BROADCAST_MESSAGE_KEY, msg);
									//Android.Support.V4.Content.LocalBroadcastManager.GetInstance(this).SendBroadcast(i);
									//handler.PostDelayed(runnable, Constants.DELAY_BETWEEN_LOG_MESSAGES);
								//}
							});
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			if (intent.Action.Equals(Constants.ACTION_START_SERVICE))
			{
				if (isStarted)
				{
					Log.Info(TAG, "OnStartCommand: The service is already running.");
				}
				else 
				{
					Log.Info(TAG, "OnStartCommand: The service is starting.");
					//RegisterForegroundService();
					handler.PostDelayed(runnable, Constants.DELAY_BETWEEN_LOG_MESSAGES);
					isStarted = true;

                    //App_Foreground.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
                        
                        //Log.Debug(logTag, "ServiceConnected Event Raised");
                        // notifies us of location changes from the system
                        //App_Foreground.Current.LocationService.LocationChanged += HandleLocationChanged;

                        //notifies us of user changes to the location provider (ie the user disables or enables GPS)
                        //App_Foreground.Current.LocationService.ProviderDisabled += HandleProviderDisabled;

                        //App_Foreground.Current.LocationService.ProviderEnabled += HandleProviderEnabled;

                        // notifies us of the changing status of a provider (ie GPS no longer available)
                        //App_Foreground.Current.LocationService.StatusChanged += HandleStatusChanged;

                    //};
                    //App_Foreground.StartLocationService();

                    System.Diagnostics.Debug.WriteLine("before api google");
                    mGoogleApiClient = new GoogleApiClient.Builder(this)
                        .AddConnectionCallbacks(this)
                        .AddOnConnectionFailedListener(this)
                        .AddApi(LocationServices.API).Build();
                            mGoogleApiClient.Connect();
                    CreateLocationRequest();
				}
			}
			else if (intent.Action.Equals(Constants.ACTION_STOP_SERVICE))
			{
				Log.Info(TAG, "OnStartCommand: The service is stopping.");
				//timestamper = null;
				StopForeground(true);
				StopSelf();
				isStarted = false;

			}
			else if (intent.Action.Equals(Constants.ACTION_RESTART_TIMER))
			{
				Log.Info(TAG, "OnStartCommand: Restarting the timer.");
				//timestamper.Restart();

			}

			// This tells Android not to restart the service if it is killed to reclaim resources.
			return StartCommandResult.Sticky;
		}


		public override IBinder OnBind(Intent intent)
		{
			// Return null because this is a pure started service. A hybrid service would return a binder that would
			// allow access to the GetFormattedStamp() method.
			return null;
		}


		public override void OnDestroy()
		{
			// We need to shut things down.
			//Log.Debug(TAG, GetFormattedTimestamp() ?? "The TimeStamper has been disposed.");
			Log.Info(TAG, "OnDestroy: The started service is shutting down.");

			// Stop the handler.
			handler.RemoveCallbacks(runnable);

			// Remove the notification from the status bar.
			var notificationManager = (NotificationManager)GetSystemService(NotificationService);
			notificationManager.Cancel(Constants.SERVICE_RUNNING_NOTIFICATION_ID);

			//timestamper = null;
			isStarted = false;
			base.OnDestroy();
		}

		/// <summary>
		/// This method will return a formatted timestamp to the client.
		/// </summary>
		/// <returns>A string that details what time the service started and how long it has been running.</returns>
		//string GetFormattedTimestamp()
		//{
			
			//return timestamper?.GetFormattedTimestamp();
		//}

		void RegisterForegroundService()
		{

			

            if (Build.VERSION.SdkInt > Build.VERSION_CODES.N)
            {
                NotificationCompat.Builder notification = new NotificationCompat.Builder(this)
               //.SetContentTitle(Resources.GetString(HaxOnTheWay.Droid.Resource.String.app_name))
               .SetContentTitle("Rastreo HaxOnTheWay")

                //.SetContentText(Resources.GetString(HaxOnTheWay.Droid.Resource.String.notification_text))
                .SetContentText("Rastreo activo")
                .SetSmallIcon(HaxOnTheWay.Droid.Resource.Drawable.Hax_on)
                //.SetSmallIcon(HaxOnTheWay.Droid.Resource.Drawable.Hax_on)
                .SetContentIntent(BuildIntentToShowMainActivity())
                                                   .SetOngoing(true);
                //.AddAction(BuildRestartTimerAction())
                //.AddAction(BuildStopServiceAction())
                //.Build(); 
                notification.SetChannelId(ANDROID_CHANNEL_ID);
                Notification builder = notification.Build();
                StartForeground(12345678, builder);
            }
            else
            {
                var notification = new Notification.Builder(this)
               //.SetContentTitle(Resources.GetString(HaxOnTheWay.Droid.Resource.String.app_name))
               .SetContentTitle("Rastreo HaxOnTheWay")

                //.SetContentText(Resources.GetString(HaxOnTheWay.Droid.Resource.String.notification_text))
                .SetContentText("Rastreo activo")
                .SetSmallIcon(HaxOnTheWay.Droid.Resource.Drawable.Hax_on)
                //.SetSmallIcon(HaxOnTheWay.Droid.Resource.Drawable.Hax_on)
                .SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                //.AddAction(BuildRestartTimerAction())
                //.AddAction(BuildStopServiceAction())
                .Build();
                StartForeground(Constants.SERVICE_RUNNING_NOTIFICATION_ID, notification);
            }
			// Enlist this instance of the service as a foreground service

		}

		/// <summary>
		/// Builds a PendingIntent that will display the main activity of the app. This is used when the 
		/// user taps on the notification; it will take them to the main activity of the app.
		/// </summary>
		/// <returns>The content intent.</returns>
		PendingIntent BuildIntentToShowMainActivity()
		{
			var notificationIntent = new Intent(this, typeof(MainActivity));
			notificationIntent.SetAction(Constants.ACTION_MAIN_ACTIVITY);
			notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
			notificationIntent.PutExtra(Constants.SERVICE_STARTED_KEY, true);

			var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
			return pendingIntent;
		}

		/// <summary>
		/// Builds a Notification.Action that will instruct the service to restart the timer.
		/// </summary>
		/// <returns>The restart timer action.</returns>
		Notification.Action BuildRestartTimerAction()
		{
			var restartTimerIntent = new Intent(this, GetType());
			restartTimerIntent.SetAction(Constants.ACTION_RESTART_TIMER);
			var restartTimerPendingIntent = PendingIntent.GetService(this, 0, restartTimerIntent, 0);

			var builder = new Notification.Action.Builder(HaxOnTheWay.Droid.Resource.Drawable.ic_action_restart_timer,
											  GetText(HaxOnTheWay.Droid.Resource.String.restart_timer),
											  restartTimerPendingIntent);

			return builder.Build();
		}

		/// <summary>
		/// Builds the Notification.Action that will allow the user to stop the service via the
		/// notification in the status bar
		/// </summary>
		/// <returns>The stop service action.</returns>
		Notification.Action BuildStopServiceAction()
		{
			var stopServiceIntent = new Intent(this, GetType());
			stopServiceIntent.SetAction(Constants.ACTION_STOP_SERVICE);
			var stopServicePendingIntent = PendingIntent.GetService(this, 0, stopServiceIntent, 0);

			var builder = new Notification.Action.Builder(Android.Resource.Drawable.IcMediaPause,
                                                          GetText(HaxOnTheWay.Droid.Resource.String.stop_service),
														  stopServicePendingIntent);
			return builder.Build();

		}



        public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Hchang");
            Android.Locations.Location location = e.Location;
            //Log.Debug(logTag, "Foreground updating");

            // these events are on a background thread, need to update on the UI thread
            //RunOnUiThread(() => {
                /*latText.Text = String.Format("Latitude: {0}", location.Latitude);
                longText.Text = String.Format("Longitude: {0}", location.Longitude);
                altText.Text = String.Format("Altitude: {0}", location.Altitude);
                speedText.Text = String.Format("Speed: {0}", location.Speed);
                accText.Text = String.Format("Accuracy: {0}", location.Accuracy);
                bearText.Text = String.Format("Bearing: {0}", location.Bearing);
            });*/

        }

        public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Hdisa");
            //Log.Debug(logTag, "Location provider disabled event raised");
        }

        public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Henab");
            //Log.Debug(logTag, "Location provider enabled event raised");
        }

        public void HandleStatusChanged(object sender, StatusChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Hchang");
            //Log.Debug(logTag, "Location status changed, event raised");
        }

        private void CreateLocationRequest()
        {
            mLocationRequest = new LocationRequest();
            mLocationRequest.SetInterval(UPDATE_INTERVAL);
            mLocationRequest.SetFastestInterval(FATEST_INTERVAL);
            mLocationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            mLocationRequest.SetSmallestDisplacement(DISPLACEMENT);
        }
        private void StartLocationUpdates()
        {
            //f (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted
              //&& ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
            //{
                //return;
            //}
            LocationServices.FusedLocationApi.RequestLocationUpdates(mGoogleApiClient, mLocationRequest, this);
        }

        public void OnConnected(Bundle connectionHint)
        {
            System.Diagnostics.Debug.WriteLine("Connected");
            StartLocationUpdates();
            mLastLocation = LocationServices.FusedLocationApi.GetLastLocation(mGoogleApiClient);
            if (Build.VERSION.SdkInt <= Build.VERSION_CODES.N)
            {
                StartTimer();
            }
            //sendLocation(mLastLocation);
            //OnLocationChanged(mLastLocation);
            //throw new NotImplementedException();
        }

        public void OnConnectionSuspended(int cause)
        {
            System.Diagnostics.Debug.WriteLine("suspended");
            mGoogleApiClient.Connect();
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            System.Diagnostics.Debug.WriteLine("failed");
            mGoogleApiClient.Connect();
        }
        public void StartTimer()
        {
            //System.Diagnostics.Debug.WriteLine("StartTimer");
            var handler = new Handler(Looper.MainLooper);
            handler.PostDelayed(() =>
            {
                OnLocationChanged(mLastLocation);
                StartTimer();

                handler.Dispose();
                handler = null;
            }, (long)5000);
        }

        public double conAlt(Android.Locations.Location location){
            return location.Altitude;
        }
        public double conLon(Android.Locations.Location location){
            return location.Longitude;
        }

       public async void OnLocationChanged(Android.Locations.Location location)
        {
            
            //mLastLocation = location;
            System.Diagnostics.Debug.WriteLine("location");
            DateTime now = DateTime.Now.ToLocalTime();
            string currentTime = (string.Format("{0}", now));

            HaxOnTheWay.Models.Location Longitud = new HaxOnTheWay.Models.Location();
            HaxOnTheWay.Services.RestService Services = new HaxOnTheWay.Services.RestService();

            Longitud.Long = (float)location.Longitude;
            Longitud.Lat = (float)location.Latitude;
            Longitud.time = currentTime;

            var instanceID = FirebaseInstanceId.Instance;
            //instanceID.DeleteInstanceId();
            var iid1 = instanceID.Token;
            //var iid2 = instanceID.GetToken("haxontheway", Firebase.Messaging.FirebaseMessaging.InstanceIdScope);
            //System.Diagnostics.Debug.WriteLine("token ss" + iid1);

            Longitud.sToken = iid1;
            if (HaxOnTheWay.Helpers.Settings.IsLoggedIn)
            {
                if(!isStartedF){
                    isStartedF = true;
                    RegisterForegroundService();
                }
                    List<Drivers> requestCommands = await App.Database.GetAllDriversAsync();
                    //List<dataSourceCommands> reqestSourceCommand = new List<dataSourceCommands>();

                    foreach (Drivers item in requestCommands)
                    {
                        //dataSourceCommands items = new dataSourceCommands();
                        Longitud.idConductor = item.iDriver;
                        idConductor = item.iDriver;

                    }

                Longitud.indConnect = 1;
                estatus_ind = true;
                //SendLocation(Longitud);
                try
                {
                    await Services.SendLocation(Longitud);
                }
                catch (Java.Lang.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("error location : " + e.Message.ToString());
                }
            }
            else
            {
                if (estatus_ind)
                {
                    Longitud.idConductor = idConductor;
                    Longitud.indConnect = 0;
                    estatus_ind = false;
                    try
                    {
                        await Services.SendLocation(Longitud);
                    }
                    catch (Java.Lang.Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message.ToString());
                    }
                    StopForeground(true);
                    //StopSelf();
                    //isStarted = false;
                    isStartedF = false;
                }
            }

        }
	}
}
