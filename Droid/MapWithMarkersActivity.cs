using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Widget;
using HaxOnTheWay.Droid;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HaxOnTheWay.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using HaxOnTheWay.Models;
using HaxOnTheWay.Views;
using Plugin.FirebasePushNotification;


namespace HaxOnTheWay
{
    using System.Linq;
    using Android.Content;
    using Android.Locations;
    using Android.Runtime;
    using static Android.Util.Xml;

    [Activity(Label = "@string/activity_label_mapwithmarkers")]
    public class MapWithMarkersActivity : Activity, IOnMapReadyCallback
    {
        private static LatLng Passchendaele = new LatLng(50.897778, 3.013333);
        private static LatLng VimyRidge = new LatLng(50.379444, 2.773611);
        private GoogleMap _map;
        private MapFragment _mapFragment;
        int ind = 0;
        LocationManager locationManager;
        public int idConductor;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MapLayout);

            locationManager = (LocationManager)GetSystemService(LocationService);

            InitMapFragment();

            //SetupAnimateToButton();
            //S/etupZoomInButton();
            //SetupZoomOutButton();
        }

        protected override void OnResume(){
            base.OnResume();
            //SetupMapIfNeeded();
        }
        protected override void OnStart()
        {
            base.OnStart();
            locationManager = GetSystemService(LocationService) as LocationManager;
        }
        private void InitMapFragment(){
            _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeNormal)
                    .InvokeZoomControlsEnabled(false)
                    .InvokeCompassEnabled(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "map");
                fragTx.Commit();
            }
			_mapFragment.GetMapAsync(this);
        }

        public void OnMapReady (GoogleMap map){
			_map = map;
            //SetupMapIfNeeded();
            StartTimerAsync();
		}

        public async Task SetupMapIfNeeded(){
            //if (_map == null)
            //{

            if (_map != null)
            {
                var criteria = new Criteria { PowerRequirement = Power.Medium };
                var bestProvider = locationManager.GetBestProvider(criteria, true);
                var location = locationManager.GetLastKnownLocation(bestProvider);
                System.Diagnostics.Debug.WriteLine("start timer maps loc " + location.Latitude);
                MarkerOptions markerOpt1 = new MarkerOptions();

                VimyRidge = new LatLng(location.Latitude, location.Longitude);
                markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(VimyRidge);
                markerOpt1.SetTitle("Mi ubicación");
                markerOpt1.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
                _map.AddMarker(markerOpt1);



                List<Coord> requestCoord = await App.Database.GetAllCoordAsync();
                foreach (Coord item in requestCoord)
                {
                    System.Diagnostics.Debug.WriteLine("entra maps marker");
                    Passchendaele = new LatLng(Double.Parse(item.LATITUD), Double.Parse(item.LONGITUD));
                    markerOpt1 = new MarkerOptions();
                    markerOpt1.SetPosition(Passchendaele);
                    markerOpt1.SetTitle(item.CMADDRESS);
                    //markerOpt1.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
                    _map.AddMarker(markerOpt1);

                    //MarkerOptions markerOpt2 = new MarkerOptions();
                    //markerOpt2.SetPosition(Passchendaele);
                    //markerOpt2.SetTitle("Passchendaele");
                    //_map.AddMarker(markerOpt2);
                }

                // We create an instance of CameraUpdate, and move the map to it.        15

                //actualizacion de la posicion de la camara
                if (ind == 0){
                    CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(VimyRidge, 10);
                    _map.MoveCamera(cameraUpdate);
                    ind = 1;
                }
            }

            //}

        }
        public async void StartTimerAsync(){
            _map.Clear();

            App.Database.dropTablesCoord();
            List<Drivers> requestCommands = await App.Database.GetAllDriversAsync();
            foreach (Drivers item in requestCommands){
                idConductor = item.iDriver;

                List<Coord> requestCoord = await App.oServiceManager.GetCoord(item);
                if (requestCoord.Count() > 0)
                {
                    foreach (Coord itm in requestCoord)
                    {
                        //se guardan las coord de las comandas del conductor
                        await App.Database.SaveCoordAsync(itm);
                    }
                }
            }



            SetupMapIfNeeded();

            //System.Diagnostics.Debug.WriteLine("StartTimer");
            var handler = new Handler(Looper.MainLooper);
            handler.PostDelayed(() =>
            {
                
                StartTimerAsync();
                handler.Dispose();
                handler = null;
            }, (long)10000);
        }
    }
}
