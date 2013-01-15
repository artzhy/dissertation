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

namespace com.ComputeApps.TestApp {
    [BroadcastReceiver(Exported=true, Enabled=true)]
    [IntentFilter(new[] { "com.ComputeApps.TestApp.Intents.DoWork" })]
    public class IncomingReceiver : BroadcastReceiver {
    
        public override void OnReceive(Context context, Intent intent) {
            if (intent.Action == "com.ComputeApps.TestApp.Intents.DoWork") {

            }

            Log.Error("com.App1.IncomingReciever", "HERE");

          
        }
    }
}