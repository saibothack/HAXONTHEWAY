using System;
using System.Collections.Generic;
using Android.App;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using HaxOnTheWay.Models;
using HaxOnTheWay.Views;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;

namespace HaxOnTheWay.Droid
{
	//You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {

        static dbLogic database;
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            //System.Diagnostics.Debug.WriteLine("Entra mainapp");
            //A great place to initialize Xamarin.Insights and Dependency Services!

            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
            FirebasePushNotificationManager.Initialize(this,false);
#endif

            //Handle notification when app is closed here
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {


            };
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
            Toast.MakeText(this, "Created", ToastLength.Long).Show();
            //startServiceIntent = new Intent(this, typeof(TimestampService));
            //startServiceIntent.SetAction(Constants.ACTION_START_SERVICE);
            //StartService(startServiceIntent);

        }

        public void OnActivityDestroyed(Activity activity)
        {
            //Toast.MakeText(this, "Destroyed", ToastLength.Long).Show();
        }

        public void OnActivityPaused(Activity activity)
        {
            //Toast.MakeText(this, "Paused", ToastLength.Long).Show();
        }

        public void OnActivityResumed(Activity activity)
        {
            //up/dateDataN();
            //Toast.MakeText(this, "Resumed", ToastLength.Long).Show();
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
            //Toast.MakeText(this, "State", ToastLength.Long).Show();
        }

        public void OnActivityStarted(Activity activity)
        {
            ///Toast.MakeText(this, "Started", ToastLength.Long).Show();
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
            //Toast.MakeText(this, "Stop", ToastLength.Long).Show();
        }


    }
}