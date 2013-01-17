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
    [Service]
    public class ControllerService : Service {
        private ControllerServiceBinder binder;
        // Service variables here.
        


        public override IBinder OnBind(Intent intent) {
           this.binder = new ControllerServiceBinder(this);
           return this.binder;
        }

        public void DoWork() {
            Log.Error("test", "here");
        }

        public void NotifyDeviceActive() {

        }

        public override void OnCreate() {
            base.OnCreate();
            App.HandleException(new Exception("Here!"), this, false);
            // ...
        }

        public override void OnStart(Intent intent, int startId) {
            base.OnStart(intent, startId);
            App.HandleException(new Exception("Here!"), this, false);

            // ...
        }

        public override void OnDestroy() {
            base.OnDestroy();

            // ...
        } 
    }

 
}