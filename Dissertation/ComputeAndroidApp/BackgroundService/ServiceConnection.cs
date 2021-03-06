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
    class ServiceConnection : Java.Lang.Object, IServiceConnection {
        private ControllerServiceBinder binder;
     //   private IAppConn app;

        public ServiceConnection(App app) {
     //       this.app = app;
        }

        public void OnServiceConnected(ComponentName name, IBinder service) {
            ControllerServiceBinder compSvcBinder = (ControllerServiceBinder)service;

            if (compSvcBinder != null) {
                this.binder = (ControllerServiceBinder)service;
                App.SetServiceBinder(this.binder);
              //  this.app.binderSet = true;

            }
        }

        public void OnServiceDisconnected(ComponentName name) {
            App.SetServiceBinder(null);
           // this.app.binderSet = false;
        }



    }
}