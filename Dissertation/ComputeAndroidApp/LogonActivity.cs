using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Util;
using ComputeAndroidSDK.Communication;
using System.Collections.Generic;

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

            //CommPackage pkg = new CommPackage();
            //pkg.ComputationRequestId = 1;
            //List<CommPackage.ParamListItem> list = new List<CommPackage.ParamListItem>();
            //list.Add(new CommPackage.ParamListItem("Param1", "VAlue1"));

            //pkg.ParameterList = list;
            //pkg.IntentAction = "intent.action";


            //string test = pkg.SerializeJson();

        }

        void buttonLogin_Click(object sender, EventArgs e) {
            try {
                String username = FindViewById<EditText>(Resource.Id.txtUsername).Text;
                String password = FindViewById<EditText>(Resource.Id.txtPassword).Text;


         //       string authtoken = App.GetAuthToken(username, password);


               Boolean authResult, authResultSet = false;
               new AuthWS.AuthSvc().AuthenticateUser(username, password, out authResult, out authResultSet);

               App.setGCMCode(this, "");

                if (authResult) {

                    if (username != App.GetUsername(this) || password != App.GetPassword(this)) {
                        App.setUsername(this, username);
                        App.setPassword(this, password);
                        App.setDeviceId(this, -1);

                        App.RegisterDeviceToUserNoGCM(this, username);
                    }

                    if (App.GetDeviceId(this) == -1) {
                        App.RegisterDeviceToUserNoGCM(this, username);
                    } else if (App.GetGCMCode(this) == "") {
                        App.RegisterDeviceGCM(this);
                    }


                    // TODO: Check what transactions are on the device, by searching the prefix com.ComputeApps.* and compare this to what the Database thinks.

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

