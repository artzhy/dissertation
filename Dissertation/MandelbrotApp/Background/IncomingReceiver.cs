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

using ComputeAndroidSDK.Communication;

namespace com.ComputeApps.MandelbrotApp {
    [BroadcastReceiver]
    [IntentFilter(new[] { "com.ComputeApps.MandelbrotApp.Intents.DoWork" }, Categories = new[] { Android.Content.Intent.CategoryHome} )]

    public class BackgroundIncomingReceiver : BroadcastReceiver {
    
        public override void OnReceive(Context context, Intent intent) {
            try {
                CommPackage cp = CommPackage.DeserializeJson(intent.GetStringExtra("CommPackage"));
                Log.Error("com.ComputeApps.MandelbrotApp.Intents.DoWork", "HERE!!");
            

                ComputeApps.MandelbrotApp.WorkList.SetAppContext(context);
                ComputeApps.MandelbrotApp.WorkList.AddWorkItem(cp);

                int test = 0;
            } catch (Exception ex) {
                Log.Error("App1.Receiver.OnReceive", ex.Message);
            }

            

          
        }
    }
}