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
    public class ControllerServiceBinder : Binder {
        private ControllerService service;

        public ControllerServiceBinder(ControllerService svc) {
            this.service = svc;
        }

        public ControllerService GetService() {
            return this.service;
        }
    }
 
}