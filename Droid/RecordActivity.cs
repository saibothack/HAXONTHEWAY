using System;
using Android.Content;
using HaxOnTheWay.Droid;
using Xamarin.Forms;
using static HaxOnTheWay.Views.CommandsPage;

[assembly: Xamarin.Forms.Dependency(typeof(RecordActivity))]
namespace HaxOnTheWay.Droid
{
    public class RecordActivity: IRecordVideoPage
    {
        public void StartNativeIntentOrActivity()
        {
            var intent = new Intent(Forms.Context, typeof(MapWithMarkersActivity));
            Forms.Context.StartActivity(intent);
        }
    }
}
