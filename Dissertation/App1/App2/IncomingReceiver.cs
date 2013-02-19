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

namespace com.ComputeApps.TestApp {
    [BroadcastReceiver(Exported=true, Enabled=true)]
    [IntentFilter(new[] { "com.ComputeApps.TestApp.Intents.ReceiveResult" })]
    public class IncomingReceiver : BroadcastReceiver {
    
        public override void OnReceive(Context context, Intent intent) {
            try {
                if (intent.Action == "com.ComputeApps.TestApp.Intents.ReceiveResult") {            
                    WorkOrderTrimmed wo = JsonConvert.DeserializeObject<WorkOrderTrimmed>(intent.GetStringExtra("WorkOrderTrimmed"));

                    // Update work order list

                    WorkOrderList.SubmitWorkOrderResult(wo);
                }

                //CommPackage cp = CommPackage.DeserializeJson(intent.GetStringExtra("CommPackage"));
                //Log.Error("com.ComputeApps.TestApp.Intents.DoWork", "HERE!!");
            

                //ComputeApps.TestApp.WorkList.SetAppContext(context);
                //ComputeApps.TestApp.WorkList.AddWorkItem(cp);

                //int test = 0;
            } catch (Exception ex) {
                Log.Error("com.ComputeApps.TestApp.Intents.ReceiveResult", ex.Message);
            }

            

          
        }
    }
}