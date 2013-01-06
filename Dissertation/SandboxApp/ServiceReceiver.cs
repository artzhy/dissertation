using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.SandboxApp {
    [BroadcastReceiver]
    public class ServiceReceiver : BroadcastReceiver {
        public override void OnReceive(Context context, Intent intent) {
      //      Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();
         //   Log.Info("ComputeAndroidApp.BackgroundService.ServiceReciever", "Message rec'd");

            int test = 0;

            context.StartActivity(typeof(Activity1));
            /*
             * C:\Users\Marc.COOPERSOFTWARE\AppData\Local\Android\android-sdk\platform-tools>ad
 b.exe shell am broadcast -a android.intent.action.BOOT_COMPLETED -c android.inte
 nt.category.HOME -n com.ComputeAndroidApp/.BackgroundService.ServiceReceiver
             */

            //  Intent startServiceIntent = new Intent(context, MyService.class);
      //  context.StartService(startServiceIntent);
        }
    }
}