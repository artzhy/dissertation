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

using Newtonsoft.Json;
using ComputeAndroidSDK.Communication;
using Android.Util;

namespace com.ComputeApps.MandelbrotApp {
    public class AsyncCalculateResult : AsyncTask {
    private Context _context;
    private CommPackage _cp;
    private CommPackage _resultPackage;

    public  AsyncCalculateResult(Intent i) {
         //  _context = context;
   //        _cp = cp;

           Log.Error("com.ComputeApps.MandelbrotApp.Intents.DoWork", "Calculating result of WO");
           _cp = CommPackage.DeserializeJson(i.GetStringExtra("CommPackage"));
        }

   
        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params) {
            _resultPackage = WorkerClass.DoWork(_cp);

            return true;
        }

        protected override void OnPostExecute(Java.Lang.Object result) {
            base.OnPostExecute(result);
          
            Intent intent = new Intent();
            intent.PutExtra("CommPackage", _resultPackage.SerializeJson());
            intent.SetAction(Constants.RETURN_RESULT_INTENT);
            App.Context.SendBroadcast(intent);
         
        }

     
    }
}
