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

namespace ComputeAndroidApp.BackgroundService {
    [Service]
    public class ControllerService : Service {
        public override IBinder OnBind(Intent intent) {
            return null;
        }

        public override void OnCreate() {
            base.OnCreate();
          // App.ShowDialog("test", "Backgrund service started", this);
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