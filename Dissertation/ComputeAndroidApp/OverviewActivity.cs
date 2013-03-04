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

namespace ComputeAndroidApp {
    [Activity(Label = "Overview")]
    public class OverviewActivity : Activity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Overview);

            Button btnDeregisterDevice = FindViewById<Button>(Resource.Id.btnDeregisterDevice);
            TextView tv = FindViewById<TextView>(Resource.Id.txtDeviceId);

            tv.Text = "Device ID: " + App.GetDeviceId(this);


            btnDeregisterDevice.Click += btnDeregisterDevice_Click;

        }

        void btnDeregisterDevice_Click(object sender, EventArgs e) {
            App.DeregisterDevice(this);
        }
    }
}