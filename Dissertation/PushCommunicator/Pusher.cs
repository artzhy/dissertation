using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp;
using PushSharp.Android;

namespace PushCommunicator
{
    public class Pusher
    {
        private PushService pushService;

        public Pusher() {
            this.pushService = new PushService();

            this.pushService.StartGoogleCloudMessagingPushService(new GcmPushChannelSettings("348279368873", "AIzaSyAJclLrg4DWUflL67FS3PI1KqIGnKxSrjY", "com.ComputeAndroidApp"));
        }

        public void SendNotification(String deviceReg, String jsonString) {
       
            
           this.pushService.QueueNotification(NotificationFactory.AndroidGcm()
           .ForDeviceRegistrationId(deviceReg)
           .WithCollapseKey("NONE")
           .WithJson(jsonString));

           
        }

        #region Exception Handling


        private void wireExceptions(PushService push) {
            push.Events.OnDeviceSubscriptionExpired += new PushSharp.Common.ChannelEvents.DeviceSubscriptionExpired(Events_OnDeviceSubscriptionExpired);
            push.Events.OnDeviceSubscriptionIdChanged += new PushSharp.Common.ChannelEvents.DeviceSubscriptionIdChanged(Events_OnDeviceSubscriptionIdChanged);
            push.Events.OnChannelException += new PushSharp.Common.ChannelEvents.ChannelExceptionDelegate(Events_OnChannelException);
            push.Events.OnNotificationSendFailure += new PushSharp.Common.ChannelEvents.NotificationSendFailureDelegate(Events_OnNotificationSendFailure);
            push.Events.OnNotificationSent += new PushSharp.Common.ChannelEvents.NotificationSentDelegate(Events_OnNotificationSent);
            push.Events.OnChannelCreated += new PushSharp.Common.ChannelEvents.ChannelCreatedDelegate(Events_OnChannelCreated);
            push.Events.OnChannelDestroyed += new PushSharp.Common.ChannelEvents.ChannelDestroyedDelegate(Events_OnChannelDestroyed);
        }

        static void Events_OnDeviceSubscriptionIdChanged(PushSharp.Common.PlatformType platform, string oldDeviceInfo, string newDeviceInfo, PushSharp.Common.Notification notification) {
            //Currently this event will only ever happen for Android GCM
            Console.WriteLine("Device Registration Changed:  Old-> " + oldDeviceInfo + "  New-> " + newDeviceInfo);
        }

        static void Events_OnNotificationSent(PushSharp.Common.Notification notification) {
            Console.WriteLine("Sent: " + notification.Platform.ToString() + " -> " + notification.ToString());
        }

        static void Events_OnNotificationSendFailure(PushSharp.Common.Notification notification, Exception notificationFailureException) {
            Console.WriteLine("Failure: " + notification.Platform.ToString() + " -> " + notificationFailureException.Message + " -> " + notification.ToString());
        }

        static void Events_OnChannelException(Exception exception, PushSharp.Common.PlatformType platformType, PushSharp.Common.Notification notification) {
            Console.WriteLine("Channel Exception: " + platformType.ToString() + " -> " + exception.ToString());
        }

        static void Events_OnDeviceSubscriptionExpired(PushSharp.Common.PlatformType platform, string deviceInfo, PushSharp.Common.Notification notification) {
            Console.WriteLine("Device Subscription Expired: " + platform.ToString() + " -> " + deviceInfo);
        }

        static void Events_OnChannelDestroyed(PushSharp.Common.PlatformType platformType, int newChannelCount) {
            Console.WriteLine("Channel Destroyed for: " + platformType.ToString() + " Channel Count: " + newChannelCount);
        }

        static void Events_OnChannelCreated(PushSharp.Common.PlatformType platformType, int newChannelCount) {
            Console.WriteLine("Channel Created for: " + platformType.ToString() + " Channel Count: " + newChannelCount);
        }

        #endregion
    }
}
