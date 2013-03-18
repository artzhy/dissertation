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

namespace AppTemplate.ComputeFunction {
    class ServiceConnection : Java.Lang.Object, IServiceConnection {
        private ComputeServiceBinder binder;
        private IComputeActivity activity;

        public ServiceConnection(IComputeActivity act) {
            this.activity = act;
        }

        public ComputeServiceBinder Binder {
            get {
                return this.binder;
            }
        }

        public void OnServiceConnected(ComponentName name, IBinder service) {
            ComputeServiceBinder compSvcBinder = (ComputeServiceBinder) service;

            if (compSvcBinder != null) {
                this.binder = (ComputeServiceBinder)service;
                this.activity.binder = this.binder;
                this.activity.binderSet = true;
                
            }
        }

        public void OnServiceDisconnected(ComponentName name) {
            throw new NotImplementedException();
        }

       
       
    }
}