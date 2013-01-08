using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Util;

namespace ComputeAndroidApp {
    [Activity(Label = "ComputeAndroidApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class LogonActivity : Activity {
        
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button buttonRegister = FindViewById<Button>(Resource.Id.btnRegister);
            Context context = this;
            buttonRegister.Click += delegate {
                StartActivity(typeof(RegisterActivity));
                
            };

            Button buttonLogin = FindViewById<Button>(Resource.Id.btnLogin);
            buttonLogin.Click += buttonLogin_Click;


        }

        void buttonLogin_Click(object sender, EventArgs e) {
            try {
                String username = FindViewById<EditText>(Resource.Id.txtUsername).Text;
                String password = FindViewById<EditText>(Resource.Id.txtPassword).Text;

                Boolean success, success1;

                new AuthWS.AuthSvc().AuthenticateUser(username, password, out success, out  success1);

                if (success) {
                    if (username != App.GetUsername(this) || password != App.GetPassword(this)) {
                        App.setUsername(this, username);
                        App.setPassword(this, password);
                        App.setDeviceId(this, -1);
                        App.RegisterDevice(this);
                    } else if (App.GetGCMCode(this) == "") {
                        App.setUsername(this, username);
                        App.setPassword(this, password);
                        App.RegisterDevice(this);
                    } else if (App.GetDeviceId(this) == -1) {
                        App.setUsername(this, username);
                        App.setPassword(this, password);
                        App.RegisterDevice(this);
                    }

                    StartActivity(typeof(OverviewActivity));
                    StartService(new Intent(this, typeof(BackgroundService.ControllerService)));

                } else {
                    App.ShowDialog("Invalid login", "Invalid login details.", this);
                }

            } catch (Exception ex) {
                App.HandleException(ex, this);
                
            }
           
        }
    }
}

