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
    public class ComputeServiceBinder : Binder {
        private ComputeService service;

        public ComputeServiceBinder(ComputeService svc) {
            this.service = svc;
        }

        public ComputeService GetService() {
            return this.service;
        }
    }
}