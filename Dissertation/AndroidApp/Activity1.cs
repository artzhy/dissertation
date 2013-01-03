using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AndroidApp {
    [Activity(Label = "AndroidApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity {
    


        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            Button btnRegister = FindViewById<Button>(Resource.Id.btnRegister);
            btnRegister.Click += btnRegister_Click;
        }

        void btnRegister_Click(object sender, EventArgs e) {
            SetContentView(Resource.Layout.Register);
     
        }
    }
}

