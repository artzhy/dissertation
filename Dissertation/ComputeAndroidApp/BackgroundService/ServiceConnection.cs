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
      //  private IComputeActivity activity;

      //  public ServiceConnection(IComputeActivity act) {
      //      this.activity = act;
      //  }

      //  public ComputeServiceBinder Binder {
      //      get {
     //           return this.binder;
     //       }
     //   }

        public void OnServiceConnected(ComponentName name, IBinder service) {
            ControllerServiceBinder compSvcBinder = (ControllerServiceBinder)service;

            if (compSvcBinder != null) {
                this.binder = (ControllerServiceBinder)service;
             //   this.activity.binder = this.binder;
              //  this.activity.binderSet = true;

            }
        }

        public void OnServiceDisconnected(ComponentName name) {
            throw new NotImplementedException();
        }



    }
}