namespace HaxOnTheWay
{
    public class Constants
    {
        // URL of REST service
        public static string RestUrl = "http://sysware.online/haexpress";
        //public static string RestUrl = "http://192.168.15.15/dev/haexpress";

        // Credentials that are hard coded into the REST service
        public static string sUserName { get; set; }
        public static string sPassword { get; set; }

        //Variables de estatus
        public const int iPending = 11;
        public const int iConfirm = 12;
        public const int iPickup = 13;
        public const int iDelivery = 14;
        public const int iNoStock = 15;
        public const int iAbsent = 16;
        public const int iRejected = 17;
        public const int iCancelado = 18;

        public const int iGood = 1;
        public const int iRegular = 2;
        public const int iBad = 3;

        public const int DELAY_BETWEEN_LOG_MESSAGES = 5000; // milliseconds
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
        public const string SERVICE_STARTED_KEY = "has_service_been_started";
        public const string BROADCAST_MESSAGE_KEY = "broadcast_message";
        public const string NOTIFICATION_BROADCAST_ACTION = "ServicesDemo3.Notification.Action";

        public const string ACTION_START_SERVICE = "ServicesDemo3.action.START_SERVICE";
        public const string ACTION_STOP_SERVICE = "ServicesDemo3.action.STOP_SERVICE";
        public const string ACTION_RESTART_TIMER = "ServicesDemo3.action.RESTART_TIMER";
        public const string ACTION_MAIN_ACTIVITY = "ServicesDemo3.action.MAIN_ACTIVITY";
    }
}
