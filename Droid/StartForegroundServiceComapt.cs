﻿using System;
namespace HaxOnTheWay.Droid
{
    public class StartForegroundServiceComapt
    {
        public static void StartForegroundServiceComapt<T>(this Context context, Bundle args = null) where T : Service
        {
            var intent = new Intent(context, typeof(T));
            if (args != null) 
            {
                intent.PutExtras(args);
            }

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                context.StartForegroundService(intent);
            }
            else
            {
                context.StartService(intent);
            }
        }
    }
}
