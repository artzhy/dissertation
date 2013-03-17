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

/**
   * This broadcast receiver simply listens for the intent outlined above (which should be set in the Azure database as the WorkIntent) and creates an AsyncCalculateResult task.
   **/


namespace com.ComputeApps.MandelbrotApp {
    [BroadcastReceiver]
    [IntentFilter(new[] { "com.ComputeApps.MandelbrotApp.Intents.DoWork" }, Categories = new[] { Android.Content.Intent.CategoryHome} )]

  
    public class BackgroundIncomingReceiver : BroadcastReceiver {
    
        public override void OnReceive(Context context, Intent intent) {
                new AsyncCalculateResult(intent).Execute();
        }
    }

    public class ReturnResultTask : AsyncTask {

        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params) {
            Context c = (Context)@params[0];
            Intent intent = (Intent)@params[1];
            Log.Error("com.ComputeApps.MandelbrotApp.Intents.DoWork", "HERE!!");
            CommPackage cp = CommPackage.DeserializeJson(intent.GetStringExtra("CommPackage"));

            return null;
        }
    }

}