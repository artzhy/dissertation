using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using GCMSharp.Client;
using Android.Util;

using Newtonsoft.Json;
using ComputeAndroidSDK.Communication;

namespace ComputeAndroidApp {
    [BroadcastReceiver(Permission = GCMConstants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { GCMConstants.INTENT_FROM_GCM_MESSAGE },
        Categories = new string[] { "com.ComputeAndroidApp" })]
    [IntentFilter(new string[] { GCMConstants.INTENT_FROM_GCM_REGISTRATION_CALLBACK },
        Categories = new string[] { "com.ComputeAndroidApp" })]
    [IntentFilter(new string[] { GCMConstants.INTENT_FROM_GCM_LIBRARY_RETRY },
        Categories = new string[] { "com.ComputeAndroidApp" })]
    public class BroadcastReceiver : GCMBroadcastReceiver<GCMIntentService> {
        public const string SENDER_ID = "348279368873";

        public const string TAG = "marcdissertationcomm";
    }

    [Service] //Must use the service tag
    public class GCMIntentService : GCMBaseIntentService {
        public GCMIntentService() : base(BroadcastReceiver.SENDER_ID) {
        }

        protected override void OnRegistered(Context context, string registrationId) {
        //    Log.Verbose(BroadcastReceiver.TAG, "GCM Registered: " + registrationId);

        //    createNotification("PushSharp-GCM Registered...", "The device has been Registered, Tap to View!");

            App.setGCMCode(context, registrationId);

            // Update the device to have the gcm code
            new UserWS.UserSvc().ModifyUserDevice(App.GetAuthToken(context, App.GetUsername(context), App.GetPassword(context), App.GetDeviceId(context)), registrationId, App.GetDeviceId(context), true);
        }

        protected override void OnUnRegistered(Context context, string registrationId) {
            Log.Verbose(BroadcastReceiver.TAG, "GCM Unregistered: " + registrationId);
            //Remove from the web service
            //	var wc = new WebClient();
            //	var result = wc.UploadString("http://your.server.com/api/unregister/", "POST",
            //		"{ 'registrationId' : '" + lastRegistrationId + "' }");

            createNotification("PushSharp-GCM Unregistered...", "The device has been unregistered, Tap to View!");
        }

        public enum UpdateType {
            Result,
            UpdateRequest,
            Error,
            NewWorkOrder
        }

        protected override void OnMessage(Context context, Intent intent) {
            Log.Info(BroadcastReceiver.TAG, "GCM Message Received!");

            UpdateType ut = (UpdateType)int.Parse(intent.Extras.Get("CommunicationType").ToString());
            String workOrderId = intent.Extras.Get("WorkOrderId").ToString();

            if (ut == UpdateType.NewWorkOrder) {
                //TODO: Acnowledge receipt of message
                App.GetServiceBinder().GetService().AddSlaveWorkItem(int.Parse(workOrderId));

            } else if (ut == UpdateType.Result) {
                //TODO: Acnowledge receipt of message
                  App.GetServiceBinder().GetService().ReceiveWorkOrderResult(int.Parse(workOrderId));

                

            } else if (ut == UpdateType.UpdateRequest) {
                //TODO: Handle update request

                // Speak to background portion of UI

            }

            //String workOrderId = intent.Extras.Get("WorkOrderId").ToString();

            //if (workOrderId != null) {
            //    App.GetServiceBinder().GetService().AddWorkItem(int.Parse(workOrderId));
            //}
        }

        protected override bool OnRecoverableError(Context context, string errorId) {
            Log.Warn(BroadcastReceiver.TAG, "Recoverable Error: " + errorId);

            return base.OnRecoverableError(context, errorId);
        }

        protected override void OnError(Context context, string errorId) {
            Log.Error(BroadcastReceiver.TAG, "GCM Error: " + errorId);
        }

        void createNotification(string title, string desc) {
            //Create notification
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            //Create an intent to show ui
            var uiIntent = new Intent(this, typeof(LogonActivity));

            //Create the notification
            var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);

            //Auto cancel will remove the notification once the user touches it
            notification.Flags = NotificationFlags.AutoCancel;

            //Set the notification info
            //we use the pending intent, passing our ui intent over which will get called
            //when the notification is tapped.
            notification.SetLatestEventInfo(this,
                title,
                desc,
                PendingIntent.GetActivity(this, 0, uiIntent, 0));

            //Show the notification
            notificationManager.Notify(1, notification);
        }
    }
}