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

namespace com.ComputeApps.TestApp {
    [BroadcastReceiver(Exported=true, Enabled=true)]
    [IntentFilter(new[] { "com.ComputeApps.TestApp.Intents.DoWork" })]
    public class IncomingReceiver : BroadcastReceiver {
    
        public override void OnReceive(Context context, Intent intent) {
            try {
                CommPackage cp = CommPackage.DeserializeJson(intent.GetStringExtra("CommPackage"));
                Log.Error("com.ComputeApps.TestApp.Intents.DoWork", "HERE!!");
            

                App1.WorkList.SetAppContext(context);
                App1.WorkList.AddWorkItem(cp);

                int test = 0;
            } catch (Exception ex) {
                Log.Error("App1.Receiver.OnReceive", ex.Message);
            }

            

          
        }
    }
}