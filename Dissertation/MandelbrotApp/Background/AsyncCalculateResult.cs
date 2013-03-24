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
using System.IO;

namespace com.ComputeApps.MandelbrotApp {
    /**
     * This task can be called asynchronously - it runs in its own thread (i.e. not the UI thread).
     * The task takes an intent containing a Communication Package and uses the parameters within this to calculate the result of the WO by calling the WorkerClass.DoWork function.
     * 
     * When the result has been computed, the task sends a RETURN_RESULT_INTENT to the Main App, which in turn returns the result to the Cloud Service.
     * 
     **/

    public class AsyncCalculateResult : AsyncTask {
    private Context _context;
    private CommPackage _cp;
    private CommPackage _resultPackage;
    private DateTime deserialisaitonStart;
    private DateTime deserialisationEnd;

    public AsyncCalculateResult(Intent i) {
           Log.Info("com.ComputeApps.MandelbrotApp.Intents.DoWork", "Calculating result of WO");
           deserialisaitonStart = DateTime.Now;
           _cp = CommPackage.DeserializeJson(i.GetStringExtra("CommPackage"));
           deserialisationEnd = DateTime.Now;
        }

   
        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params) {
            Log.Error("COMPUTING", "STARTING TO COMPUTE RESULT FOR WO: " + _cp.ComputationRequestId);
            _resultPackage = WorkerClass.DoWork(_cp);

            _resultPackage.DeserialisationTime = (Decimal)deserialisationEnd.Subtract(deserialisaitonStart).TotalSeconds;

            return true;
        }

        protected override void OnPostExecute(Java.Lang.Object result) {
            base.OnPostExecute(result);
            
            string fileName = Android.OS.Environment.ExternalStorageDirectory +
Java.IO.File.Separator + _cp.ComputationRequestId + "-Result.txt";

            string serialisedRes = _resultPackage.SerializeJson();

            using (StreamWriter fs = new StreamWriter(fileName, false)) {
                fs.Write(serialisedRes);
                fs.Close();
            }
          
            Intent intent = new Intent();
            intent.PutExtra("FileLocation", fileName);

           // intent.PutExtra("CommPackage", _resultPackage.SerializeJson());
            intent.SetAction(Constants.RETURN_RESULT_INTENT);
            App.Context.SendBroadcast(intent);

            this.Dispose();
        }
    }
}
