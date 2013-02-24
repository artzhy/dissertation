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

namespace com.ComputeApps.MandelbrotApp {
    public class App : Application {

        public override void OnCreate() {
            base.OnCreate();

            WorkOrderList.SetApplicationContext(ApplicationContext);
        }

    }
}