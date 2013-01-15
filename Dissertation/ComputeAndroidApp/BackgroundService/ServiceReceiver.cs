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
using Android.Util;

namespace ComputeAndroidApp.BackgroundService {
    [BroadcastReceiver]
    [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted },
          Categories = new[] { Android.Content.Intent.CategoryHome }
  )]
    public class ServiceReceiver : BroadcastReceiver {
        public override void OnReceive(Context context, Intent intent) {
           
            Log.Info(context.ApplicationInfo.PackageName + "-" + context.ApplicationInfo.ClassName, "Device started, attempting to start Controller service");
           

            context.StartService(new Intent(context, typeof(ControllerService)));

            App.BindService(context);
           


            /*
             * adb.exe shell am broadcast -a android.intent.action.BOOT_COMPLETED -c android.inte
nt.category.HOME
             * C:\Users\Marc.COOPERSOFTWARE\AppData\Local\Android\android-sdk\platform-tools>ad
 b.exe shell am broadcast -a android.intent.action.BOOT_COMPLETED -c android.inte
 nt.category.HOME -n ComputeAndroidApp/.BackgroundService.ServiceReceiver
             */

            //  Intent startServiceIntent = new Intent(context, MyService.class);
      //  context.StartService(startServiceIntent);
        }
    }
}