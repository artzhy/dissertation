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

using Newtonsoft.Json;

using ComputeAndroidSDK.Communication;

/**
 * The UIIncomingReceiver simply listens for intents with the name com.ComputeApps.{AppName}.Intents.ReceiveResult and then submits this result to the WorkOrderList.
 * 
 **/

namespace com.ComputeApps.MandelbrotApp {
    [BroadcastReceiver(Exported=true, Enabled=true)]
    [IntentFilter(new[] { "com.ComputeApps.MandelbrotApp.Intents.ReceiveResult" }, Categories = new[] { Android.Content.Intent.CategoryHome })]
    public class UIIncomingReceiver : BroadcastReceiver {
    
        public override void OnReceive(Context context, Intent intent) {
            try {
                if (intent.Action == "com.ComputeApps.MandelbrotApp.Intents.ReceiveResult") {
                    new SubmitResultTask().Execute(intent);
                }
            } catch (Exception ex) {
                Log.Error("com.ComputeApps.MandelbrotApp.Intents.ReceiveResult", ex.Message);
            }
        }

        class SubmitResultTask : AsyncTask {
            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params) {
                WorkOrderTrimmed wo = JsonConvert.DeserializeObject<WorkOrderTrimmed>(((Intent)@params[0]).GetStringExtra("WorkOrderTrimmed"));

                // Update work order list

                WorkOrderList.SubmitWorkOrderResult(wo);

                return true;
            }
        }
    }
}