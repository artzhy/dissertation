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
using System.Threading;
using Java.Lang;

namespace ComputeAndroidApp.BackgroundService {
    [BroadcastReceiver]
    [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted, ComputeAndroidSDK.Communication.Constants.RETURN_STATUS_INTENT, ComputeAndroidSDK.Communication.Constants.RETURN_RESULT_INTENT, ComputeAndroidSDK.Communication.Constants.REQUEST_WORK_ORDER_INTENT, ComputeAndroidSDK.Communication.Constants.CANCEL_WORK_ORDER_INTENT },
          Categories = new[] { Android.Content.Intent.CategoryHome }
  )]
    public class ServiceReceiver : BroadcastReceiver {
        public override void OnReceive(Context context, Intent intent) {
           //TODO: Perhaps handle low battery and such like.


            if (intent.Action == Android.Content.Intent.ActionBootCompleted) {
                //TODO: Check apps are installed and notify cloud that device is on

      //          App.GetInstalledApplications();

                new UserWS.UserSvc().MarkDeviceActive(App.GetAuthToken(context), App.GetDeviceId(context), true);
                App.UpdateLastTransmit();
                
            } else if (intent.Action == ComputeAndroidSDK.Communication.Constants.RETURN_RESULT_INTENT) {
                // Handle result

                System.Threading.Thread oThread = new System.Threading.Thread(new ParameterizedThreadStart(App.GetServiceBinder().GetService().ReturnWORequestResult));

                oThread.Start(intent);

            } else if (intent.Action == ComputeAndroidSDK.Communication.Constants.RETURN_STATUS_INTENT) {

                // Get work order from Web Service

                // Send to UI portion of app


            } else if (intent.Action == ComputeAndroidSDK.Communication.Constants.REQUEST_WORK_ORDER_INTENT) {

                System.Threading.Thread oThread = new System.Threading.Thread(new ParameterizedThreadStart(App.GetServiceBinder().GetService().RequestWorkOrderComputation));

                oThread.Start(intent);
            } else if (intent.Action == ComputeAndroidSDK.Communication.Constants.CANCEL_WORK_ORDER_INTENT) {
                App.GetServiceBinder().GetService().CancelWorkOrderComputationRequest(intent);
            }            
        }
    }
}

//Intent ourIntent = new Intent(context, typeof(ControllerService));
//ControllerServiceBinder binder = (ControllerServiceBinder)PeekService(context, ourIntent);
//binder.GetService().DoCommand("com.ComputeApps.TestApp.Intents.DoWork");

/*
 * adb.exe shell am broadcast -a android.intent.action.BOOT_COMPLETED -c android.intent.category.HOME
 * C:\Users\Marc.COOPERSOFTWARE\AppData\Local\Android\android-sdk\platform-tools>ad
b.exe shell am broadcast -a android.intent.action.BOOT_COMPLETED -c android.inte
nt.category.HOME -n ComputeAndroidApp/.BackgroundService.ServiceReceiver
 */

//  Intent startServiceIntent = new Intent(context, MyService.class);
//  context.StartService(startServiceIntent);