using System;

using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using Android.Locations;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;


using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using HaxOnTheWay.Services;
using System.Net.Http.Headers;
using HaxOnTheWay;


using HaxOnTheWay.Helpers;
using HaxOnTheWay.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using HaxOnTheWay.Models;
using Plugin.FirebasePushNotification;
using Firebase.Iid;

namespace Location.Droid.Services
{
	[Service]
	public class LocationService : Service, ILocationListener
	{
		public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
		public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
		public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
		public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };
        HttpClient client;
        public bool estatus_ind;
        public int idConductor;
		public LocationService() 
		{
		}

		// Set our location manager as the system location service
		protected LocationManager LocMgr = Android.App.Application.Context.GetSystemService ("location") as LocationManager;

		readonly string logTag = "LocationService";
		IBinder binder;

		public override void OnCreate ()
		{
			base.OnCreate ();
			Log.Debug (logTag, "OnCreate called in the Location Service");
		}

		// This gets called when StartService is called in our App class
		[Obsolete("deprecated in base class")]
		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			Log.Debug (logTag, "LocationService started");

			return StartCommandResult.Sticky;
		}

		// This gets called once, the first time any client bind to the Service
		// and returns an instance of the LocationServiceBinder. All future clients will
		// reuse the same instance of the binder
		public override IBinder OnBind (Intent intent)
		{
			Log.Debug (logTag, "Client now bound to service");

			binder = new LocationServiceBinder (this);
			return binder;
		}

		// Handle location updates from the location manager
		public void StartLocationUpdates () 
		{
			//we can set different location criteria based on requirements for our app -
			//for example, we might want to preserve power, or get extreme accuracy
			var locationCriteria = new Criteria();
			
			locationCriteria.Accuracy = Accuracy.NoRequirement;
			locationCriteria.PowerRequirement = Power.NoRequirement;

			// get provider: GPS, Network, etc.
			var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);
			Log.Debug (logTag, string.Format ("You are about to get location updates via {0}", locationProvider));

			// Get an initial fix on location
			LocMgr.RequestLocationUpdates(locationProvider, 2000, 0, this);

			Log.Debug (logTag, "Now sending location updates");
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			Log.Debug (logTag, "Service has been terminated");

            // Stop getting updates from the location manager:
            LocMgr.RemoveUpdates(this);
		}

		
        #region ILocationListener implementation
        // ILocationListener is a way for the Service to subscribe for updates
        // from the System location Service

        public async void OnLocationChanged (Android.Locations.Location location)
		{
                System.Diagnostics.Debug.WriteLine("location");
			this.LocationChanged (this, new LocationChangedEventArgs (location));

            DateTime now = DateTime.Now.ToLocalTime();
            string currentTime = (string.Format("{0}", now));

            // This should be updating every time we request new location updates
            // both when teh app is in the background, and in the foreground
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
            if (Settings.IsLoggedIn){
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
                try{
                    await Services.SendLocation(Longitud);
                }catch (Exception e){
                    System.Diagnostics.Debug.WriteLine("error location : " + e.Message.ToString());
                }
            }else{
                if(estatus_ind){
                    Longitud.idConductor = idConductor;
                    Longitud.indConnect = 0;
                    estatus_ind = false;
                    try{
                        await Services.SendLocation(Longitud);
                    }catch (Exception e){
                        System.Diagnostics.Debug.WriteLine(e.Message.ToString());
                    }
                }
            }

			Log.Debug (logTag, String.Format ("Latitude is {0}", location.Latitude));
			Log.Debug (logTag, String.Format ("Longitude is {0}", location.Longitude));
			Log.Debug (logTag, String.Format ("Altitude is {0}", location.Altitude));
			Log.Debug (logTag, String.Format ("Speed is {0}", location.Speed));
			Log.Debug (logTag, String.Format ("Accuracy is {0}", location.Accuracy));
			Log.Debug (logTag, String.Format ("Bearing is {0}", location.Bearing));
		}

		public void OnProviderDisabled (string provider)
		{
			this.ProviderDisabled (this, new ProviderDisabledEventArgs (provider));
		}

		public void OnProviderEnabled (string provider)
		{
			this.ProviderEnabled (this, new ProviderEnabledEventArgs (provider));
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
			this.StatusChanged (this, new StatusChangedEventArgs (provider, status, extras));
		}
       
        #endregion

	}

}

