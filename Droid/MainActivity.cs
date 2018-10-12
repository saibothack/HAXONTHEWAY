using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.FirebasePushNotification;
using ServicesDemo3;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using Android.Support.Design.Widget;

namespace HaxOnTheWay.Droid
{
    
    [Activity(Label = "HaxOnTheWay.Droid", Icon = "@drawable/icon_new", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback 
    {
        bool reqPer;
        static readonly int REQUEST_CONTACTS = 1;
        Intent startServiceIntent;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
            {
                System.Diagnostics.Debug.WriteLine("permission solicited");
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessCoarseLocation }, REQUEST_CONTACTS);
            }else{
                initial();
            }

            //Intent intent = new Intent(this,MapWithMarkersActivity.class);



        }
        private void initial(){
            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(Intent);
            if (Build.VERSION.SdkInt > Build.VERSION_CODES.N)
            {
                System.Diagnostics.Debug.WriteLine("version Oreo");
                startServiceIntent = new Intent(this, typeof(TimestampService));
                startServiceIntent.SetAction(Constants.ACTION_START_SERVICE);
                StartService(startServiceIntent);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("version Other");
                startServiceIntent = new Intent(this, typeof(TimestampService));
                startServiceIntent.SetAction(Constants.ACTION_START_SERVICE);
                StartService(startServiceIntent);
            }
            //StartActivity(typeof(MapWithMarkersActivity));

        }
        private bool reqPerm(){
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted)
            {
                System.Diagnostics.Debug.WriteLine("permission denied");
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, REQUEST_CONTACTS);
            }
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
            {
                System.Diagnostics.Debug.WriteLine("permission denied");
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessFineLocation }, REQUEST_CONTACTS);
            }

            return true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) == (int)Permission.Granted)
            {
                System.Diagnostics.Debug.WriteLine("permission granted");
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.CallPhone) != (int)Permission.Granted)
                {
                    System.Diagnostics.Debug.WriteLine("permission solicited callphone");
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.CallPhone }, REQUEST_CONTACTS);
                }
                else
                {
                    initial();
                }
            }
        }
    }
}
